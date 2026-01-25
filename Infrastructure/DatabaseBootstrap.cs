using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IsTakipWpf.Infrastructure
{
    public static class DatabaseBootstrap
    {
        public static bool IsPortable 
        {
            get
            {
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "portable.txt")))
                    return true;

                // Velopack kontrolü: Eğer "app-x.y.z" klasöründeysek veya üst klasörde Update.exe varsa yüklü moddayızdır.
                string appDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string dirName = Path.GetFileName(appDir);
                if (dirName.StartsWith("app-", StringComparison.OrdinalIgnoreCase))
                    return false;

                try
                {
                    var parent = Directory.GetParent(appDir);
                    if (parent != null && File.Exists(Path.Combine(parent.FullName, "Update.exe")))
                        return false;
                }
                catch { }

                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                bool isInAppData = appDir.StartsWith(localAppData, StringComparison.OrdinalIgnoreCase);
                return !isInAppData;
            }
        }

        private static string GetBaseDataFolder()
        {
            if (IsPortable)
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Veriler");
            }

            // Velopack veya Yüklü Mod: %APPDATA%\IsTakipApp\VeriDeposu
            // Bu konum en güvenlisidir, çünkü uygulama silinse bile (kullanıcı özellikle istemedikçe) silinmez.
            string roamingData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(roamingData, "IsTakipApp", "VeriDeposu");
        }

        private static readonly string AppDataFolder = GetBaseDataFolder();
        public static readonly string DbPath = Path.Combine(AppDataFolder, "database.db");

        private static readonly string OldDbFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private static readonly string OldDbPath = Path.Combine(OldDbFolder, "database.db");

        /// <summary>
        /// Gets the SQLite connection string.
        /// </summary>
        public static string ConnectionString => $"Data Source={DbPath};Version=3;";

        /// <summary>
        /// Initializes the database by creating the folder, file, and necessary tables if they don't exist.
        /// </summary>
        public static void Initialize()
        {
            string dbFolder = Path.GetDirectoryName(DbPath);
            if (!Directory.Exists(dbFolder))
            {
                Directory.CreateDirectory(dbFolder);
            }

            // Geri yükleme dosyası kontrolü (.restore)
            string restorePath = DbPath + ".restore";
            if (File.Exists(restorePath))
            {
                try
                {
                    if (File.Exists(DbPath)) File.Delete(DbPath);
                    File.Move(restorePath, DbPath);
                }
                catch { }
            }

            // Kurtarma Mantığı: Eğer veritabanı yoksa eski olası klasörleri tara
            if (!File.Exists(DbPath))
            {
                TryRecoverData(DbPath);
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
                        Price DECIMAL(10,2) DEFAULT 0,
                        PaidAmount DECIMAL(10,2) DEFAULT 0,
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        IsDeleted INTEGER DEFAULT 0,
                        FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
                    );";

                using (var command = new SQLiteCommand(createJobsTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                string createSettingsTable = @"
                    CREATE TABLE IF NOT EXISTS Settings (
                        Key TEXT PRIMARY KEY,
                        Value TEXT
                    );";

                using (var command = new SQLiteCommand(createSettingsTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                string createNotesTable = @"
                    CREATE TABLE IF NOT EXISTS Notes (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT,
                        Content TEXT,
                        Color TEXT DEFAULT '#FFFFFF',
                        IsPinned INTEGER DEFAULT 0,
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        UpdatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                    );";

                using (var command = new SQLiteCommand(createNotesTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Initialize default admin password if not exists
                string checkAdminPassword = "SELECT COUNT(*) FROM Settings WHERE Key = 'AdminPasswordHash';";
                using (var command = new SQLiteCommand(checkAdminPassword, connection))
                {
                    long count = (long)command.ExecuteScalar();
                    if (count == 0)
                    {
                        // Initial password setup
                        // Önemli: GitHub'a atmadan önce buradaki şifreyi placeholder ile değiştirdik.
                        // İlk kurulumda kullanılacak varsayılan şifre buraya yazılmalıdır.
                        string defaultPassword = "123"; 
                        string salt = Guid.NewGuid().ToString("N");
                        string hashed = HashPassword(defaultPassword, salt);
                        
                        string insertHash = "INSERT OR IGNORE INTO Settings (Key, Value) VALUES ('AdminPasswordHash', @hash);";
                        string insertSalt = "INSERT OR IGNORE INTO Settings (Key, Value) VALUES ('AdminPasswordSalt', @salt);";
                        
                        using (var insertCmd = new SQLiteCommand(insertHash, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@hash", hashed);
                            insertCmd.ExecuteNonQuery();
                        }
                        
                        using (var insertCmd = new SQLiteCommand(insertSalt, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@salt", salt);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }

                // Migrations
                AddColumnIfNotExists(connection, "Customers", "City", "TEXT");
                AddColumnIfNotExists(connection, "Customers", "District", "TEXT");
                AddColumnIfNotExists(connection, "Jobs", "Price", "DECIMAL(10,2) DEFAULT 0");
                AddColumnIfNotExists(connection, "Jobs", "PaidAmount", "DECIMAL(10,2) DEFAULT 0");

                // Performance Indexes
                ExecuteNonQuery(connection, "CREATE INDEX IF NOT EXISTS idx_customers_names ON Customers (FirstName, LastName);");
                ExecuteNonQuery(connection, "CREATE INDEX IF NOT EXISTS idx_customers_city ON Customers (City);");
                ExecuteNonQuery(connection, "CREATE INDEX IF NOT EXISTS idx_jobs_title ON Jobs (JobTitle);");
                ExecuteNonQuery(connection, "CREATE INDEX IF NOT EXISTS idx_jobs_customer ON Jobs (CustomerId);");
            }
        }

        private static void ExecuteNonQuery(SQLiteConnection connection, string sql)
        {
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combined = password + salt;
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private static void AddColumnIfNotExists(SQLiteConnection connection, string tableName, string columnName, string columnType)
        {
            try
            {
                var checkColumn = $"PRAGMA table_info({tableName})";
                bool exists = false;
                using (var cmd = new SQLiteCommand(checkColumn, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader["name"]?.ToString();
                        if (columnName.Equals(name, StringComparison.OrdinalIgnoreCase))
                        {
                            exists = true;
                            break;
                        }
                    }
                }

                if (!exists)
                {
                    var addColumn = $"ALTER TABLE {tableName} ADD COLUMN {columnName} {columnType}";
                    using (var cmd = new SQLiteCommand(addColumn, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("duplicate column name"))
            {
                // Ignore if it somehow already exists
            }
        }

        private static void TryRecoverData(string targetDbPath)
        {
            try
            {
                // Olası eski konumları listele
                var possiblePaths = new List<string>();

                // 1. Konum: Eski VeriDeposu (LocalAppData içinde olabilir)
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                possiblePaths.Add(Path.Combine(localAppData, "IsTakipApp", "VeriDeposu", "database.db"));

                // 2. Konum: Uygulamanın üst klasöründeki VeriDeposu
                string appDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var parent = Directory.GetParent(appDir);
                if (parent != null)
                {
                    possiblePaths.Add(Path.Combine(parent.FullName, "VeriDeposu", "database.db"));

                    // 3. Konum: Eski app-* klasörlerinin içindeki Veriler klasörü
                    var appFolders = Directory.GetDirectories(parent.FullName, "app-*")
                        .OrderByDescending(f => f);

                    foreach (var folder in appFolders)
                    {
                        if (folder.Equals(appDir, StringComparison.OrdinalIgnoreCase)) continue;
                        possiblePaths.Add(Path.Combine(folder, "Veriler", "database.db"));
                        possiblePaths.Add(Path.Combine(folder, "Data", "database.db"));
                    }
                }

                foreach (var oldPath in possiblePaths)
                {
                    if (File.Exists(oldPath) && oldPath != targetDbPath)
                    {
                        string targetFolder = Path.GetDirectoryName(targetDbPath);
                        if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);

                        File.Copy(oldPath, targetDbPath, true);
                        break; 
                    }
                }
            }
            catch { }
        }
    }
}
