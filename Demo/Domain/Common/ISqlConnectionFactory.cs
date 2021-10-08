using System.Data;

namespace Domain.Common
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}