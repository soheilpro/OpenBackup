using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace OpenBackup.Extension.SqlServer
{
    internal static class ServerHelper
    {
        public static string GetConnectionString(this IServer server)
        {
            var connectionString = new StringBuilder();
            connectionString.AppendFormat("data source={0}", server.Address);

            if (server.Port != null)
                connectionString.AppendFormat(",{0}", server.Port);

            if (server.Instance != null)
                connectionString.AppendFormat(@"\{0}", server.Instance);

            connectionString.AppendFormat(";user id={0}", server.Username);
            connectionString.AppendFormat(";password={0}", server.Password);

            return connectionString.ToString();
        }

        public static IEnumerable<string> GetAllDatabaseNames(this IServer server)
        {
            using (var connection = new SqlConnection(server.GetConnectionString()))
            {
                connection.Open();

                var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT name FROM sys.databases";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = (string)reader["name"];

                        if (name.Equals("tempdb", StringComparison.OrdinalIgnoreCase))
                            continue;

                        yield return name;
                    }
                }
            }
        }
    }
}
