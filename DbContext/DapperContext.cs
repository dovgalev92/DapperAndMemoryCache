using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperMemoryCache.DbContext
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _dapperConnect;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _dapperConnect = _configuration.GetConnectionString("ConnectDapper");
        }
        public IDbConnection CreateConnection() => new SqlConnection(_dapperConnect);
    }
}
