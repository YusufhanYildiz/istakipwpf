using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
using Dapper;
using IsTakipWpf.Infrastructure;
using IsTakipWpf.Models;

namespace IsTakipWpf.Repositories
{
    public class NoteRepository : BaseRepository, INoteRepository
    {
        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            using (var connection = CreateConnection())
            {
                // Order by Pinned first, then by UpdatedDate descending
                return await connection.QueryAsync<Note>(
                    "SELECT * FROM Notes ORDER BY IsPinned DESC, UpdatedDate DESC"
                );
            }
        }

        public async Task<Note> GetByIdAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Note>(
                    "SELECT * FROM Notes WHERE Id = @Id", new { Id = id }
                );
            }
        }

        public async Task<int> AddAsync(Note note)
        {
            using (var connection = CreateConnection())
            {
                note.CreatedDate = DateTime.Now;
                note.UpdatedDate = DateTime.Now;
                
                var sql = @"
                    INSERT INTO Notes (Title, Content, Color, IsPinned, CreatedDate, UpdatedDate)
                    VALUES (@Title, @Content, @Color, @IsPinned, @CreatedDate, @UpdatedDate);
                    SELECT last_insert_rowid();";

                return await connection.ExecuteScalarAsync<int>(sql, note);
            }
        }

        public async Task<bool> UpdateAsync(Note note)
        {
            using (var connection = CreateConnection())
            {
                note.UpdatedDate = DateTime.Now;
                var sql = @"
                    UPDATE Notes 
                    SET Title = @Title, 
                        Content = @Content, 
                        Color = @Color, 
                        IsPinned = @IsPinned,
                        UpdatedDate = @UpdatedDate
                    WHERE Id = @Id";

                var rows = await connection.ExecuteAsync(sql, note);
                return rows > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                var rows = await connection.ExecuteAsync("DELETE FROM Notes WHERE Id = @Id", new { Id = id });
                return rows > 0;
            }
        }
    }
}
