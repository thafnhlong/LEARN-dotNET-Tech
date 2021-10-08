using Domain.Entities.Roles;
using Domain.Entities.Users;
using Domain.Exceptions;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;

namespace Api
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment env;

        public HttpGlobalExceptionFilter(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            if (exception is BaseDomainException ex) {
                context.HttpContext.Response.StatusCode = ex.status;
                context.HttpContext.Response.ContentType = "application/json";
                context.Result = new ObjectResult(ex.GetMessage());
            }
            else {
                var error = new GeneralError();
                if (env.IsDevelopment()) {
                    error.message = exception.Message;
                }
                context.Result = new InternalServerErrorObjectResult(error);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
        }

        private class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object error) : base(error)
            {
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureExceptionHandler(app);

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // DB
            services.AddDbContext<DemoContext>(opt => opt.UseSqlite("Data source=demo.db", b => b.MigrationsAssembly("Api")));

            services.AddScoped<DemoContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddControllers(options => {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddNewtonsoftJson(options => {
                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new DefaultContractResolver() {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });
        }

        protected void ConfigureExceptionHandler(IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp => {
                errorApp.Run(async context => {
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;
                    if (exception is BaseDomainException ex) {
                        context.Response.StatusCode = ex.status;
                        await context.Response.WriteAsync(ex.ToString()).ConfigureAwait(false);
                    }
                    else {
                        context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new { code = "internal_server_error", message = exception.Message })).ConfigureAwait(false);
                    }
                });
            });
        }
    }
}