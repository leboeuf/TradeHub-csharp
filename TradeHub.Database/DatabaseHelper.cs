using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace TradeHub.Database
{
    public static class DatabaseHelper
    {
        private const string _connectionString = @"Data Source=D:\tradehub.sqlite;Version=3;";

        public static DataTable ExecuteQuery(string sql)
        {
            var command = new SQLiteCommand { CommandText = sql, CommandType = CommandType.Text };

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                command.Connection = connection;

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    DataTable result = new DataTable();
                    result.Load(reader);
                    return result;
                }
            }
        }
        
        public static void ExecuteNonQuery(string sql)
        {
            var command = new SQLiteCommand { CommandText = sql, CommandType = CommandType.Text };

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
        }
    }
}
