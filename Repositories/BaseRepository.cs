using System.Data;
using System.Data.SQLite;
using IsTakipWpf.Infrastructure;

namespace IsTakipWpf.Repositories
{
    public abstract class BaseRepository
    {
        /// <summary>
        /// Creates and opens a new SQLite database connection.
        /// </summary>
        /// <returns>An open IDbConnection instance.</returns>
        protected IDbConnection CreateConnection()
        {
            var connection = new SQLiteConnection(DatabaseBootstrap.ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
