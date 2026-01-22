using System.Threading.Tasks;
using Dapper;

namespace IsTakipWpf.Repositories
{
    public class SettingsRepository : BaseRepository, ISettingsRepository
    {
        public async Task<string> GetValueAsync(string key)
        {
            const string sql = "SELECT Value FROM Settings WHERE Key = @Key";
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<string>(sql, new { Key = key });
            }
        }

        public async Task<bool> SetValueAsync(string key, string value)
        {
            const string sql = @"
                INSERT INTO Settings (Key, Value) VALUES (@Key, @Value)
                ON CONFLICT(Key) DO UPDATE SET Value = excluded.Value;";

            using (var connection = CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync(sql, new { Key = key, Value = value });
                return affectedRows > 0;
            }
        }
    }
}
