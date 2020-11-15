using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace TradeHub.Database
{
    public static class DatabaseHelper
    {
        /// <summary>
        /// Gets or sets the absolute path to the database file.
        /// </summary>
        public static string DatabaseFile { get; set; }

        public static async Task<DataTable> ExecuteQuery(string sql)
        {
            var command = new SqliteCommand { CommandText = sql, CommandType = CommandType.Text };

            using SqliteConnection connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            command.Connection = connection;

            using SqliteDataReader reader = await command.ExecuteReaderAsync();
            DataTable result = new DataTable();
            result.Load(reader);
            return result;
        }
        
        public static async Task ExecuteNonQuery(string sql)
        {
            var command = new SqliteCommand { CommandText = sql, CommandType = CommandType.Text };

            using SqliteConnection connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            command.Connection = connection;
            await command.ExecuteNonQueryAsync();
        }

        public static Task<object> ExecuteScalar(string sql)
        {
            var command = new SqliteCommand { CommandText = sql, CommandType = CommandType.Text };

            using SqliteConnection connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            command.Connection = connection;

            return command.ExecuteScalarAsync();
        }

        private static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(DatabaseFile))
            {
                DatabaseFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "tradehub.sqlite");
            }

            return $"Data Source={DatabaseFile}";
        }
    }
}
