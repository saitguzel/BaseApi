using System.Data;
using System.Data.SqlClient;

namespace Base.Service.Helpers
{
    public class DatabaseHelper
    {
        public enum RunTypes
        {
            NonQuery,
            Scalar,
            Reader
        }

        public static string? ConnectionString { get; set; }
        public static int CommandTimeout { get; set; } = 600;

        public static async Task<T> Run<T>(string query, Dictionary<string, object>? parameters = null, CommandType type = CommandType.StoredProcedure, RunTypes rtype = RunTypes.Reader)
        {
            using SqlConnection conn = new(ConnectionString);
            using SqlCommand command = new(query, conn);
            await conn.OpenAsync();
            command.CommandType = type;
            command.CommandTimeout = CommandTimeout;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    command.Parameters.AddWithValue("@" + p.Key, p.Value);
                }
            }
            if (rtype == RunTypes.Reader)
            {
                DataTable _dt = new();
                _dt.Load(await command.ExecuteReaderAsync());
                return (T)Convert.ChangeType(_dt, typeof(T));
            }
            else if (rtype == RunTypes.NonQuery)
            {
                return (T)Convert.ChangeType(await command.ExecuteNonQueryAsync(), typeof(T));
            }
            else
            {
                return (T)Convert.ChangeType(value: await command.ExecuteScalarAsync() ?? default(T), conversionType: typeof(T));
            }
        }

        public static async Task<DataTable> ListFromStoredProcedure(string spName, Dictionary<string, object>? parameters = null)
        {
            using SqlConnection conn = new(ConnectionString);
            using SqlCommand command = new(spName, conn);
            await conn.OpenAsync();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = CommandTimeout;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    command.Parameters.AddWithValue("@" + p.Key, p.Value);
                }
            }
            DataTable x = new();
            x.Load(await command.ExecuteReaderAsync());
            await conn.CloseAsync();
            return x;
        }

        public static async Task<int> RunFromStoredProcedure(string spName, Dictionary<string, object>? parameters = null)
        {
            return await Run<int>(spName, parameters, CommandType.StoredProcedure, RunTypes.NonQuery);
        }

        public static async Task<T> GetFromStoredProcedure<T>(string spName, Dictionary<string, object>? parameters = null)
        {
            return await Run<T>(spName, parameters, CommandType.StoredProcedure, RunTypes.Scalar);
        }

    }
}
