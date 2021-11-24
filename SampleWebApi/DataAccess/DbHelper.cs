using System.Data.SqlClient;

namespace SampleWebApi.DataAccess
{
    public static class DbHelper
    {
        public static string ConnectionString { get; set; } = string.Empty;
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString); ;
        }

    }
}
