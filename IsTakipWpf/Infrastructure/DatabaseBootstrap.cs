using System;
using System.Data.SQLite;
using System.IO;

namespace IsTakipWpf.Infrastructure
{
    public static class DatabaseBootstrap
    {
        private static readonly string DbFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IsTakipWpf");
        private static readonly string DbPath = Path.Combine(DbFolder, "database.db");

        /// <summary>
        /// Gets the SQLite connection string.
        /// </summary>
        public static string ConnectionString => $"Data Source={DbPath};Version=3;";

        /// <summary>
        /// Initializes the database by creating the folder, file, and necessary tables if they don't exist.
        /// </summary>
        public static void Initialize()
        {
            if (!Directory.Exists(DbFolder))
            {
                Directory.CreateDirectory(DbFolder);
            }

            if (!File.Exists(DbPath))
            {
                SQLiteConnection.CreateFile(DbPath);
            }

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                
                string createCustomersTable = @"
                    CREATE TABLE IF NOT EXISTS Customers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        PhoneNumber TEXT,
                        Address TEXT,
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        IsDeleted INTEGER DEFAULT 0
                    );";

                using (var command = new SQLiteCommand(createCustomersTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create Jobs table too since it's part of the goal
                string createJobsTable = @"
                    CREATE TABLE IF NOT EXISTS Jobs (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CustomerId INTEGER NOT NULL,
                        JobTitle TEXT NOT NULL,
                        Description TEXT,
                        Status INTEGER NOT NULL DEFAULT 0,
                        StartDate DATETIME,
                        EndDate DATETIME,
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        IsDeleted INTEGER DEFAULT 0,
                        FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
                    );";

                using (var command = new SQLiteCommand(createJobsTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
