using Microsoft.Data.Sqlite;
using System.Data;

namespace TradeHub.Database
{
    public static class DatabaseHelper
    {
        private const string _connectionString = @"Data Source=D:\tradehub.sqlite;Version=3;";

        public static DataTable ExecuteQuery(string sql)
        {
            var command = new SqliteCommand { CommandText = sql, CommandType = CommandType.Text };

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                command.Connection = connection;

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    DataTable result = new DataTable();
                    result.Load(reader);
                    return result;
                }
            }
        }
        
        public static void ExecuteNonQuery(string sql)
        {
            var command = new SqliteCommand { CommandText = sql, CommandType = CommandType.Text };

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
        }
    }
}
