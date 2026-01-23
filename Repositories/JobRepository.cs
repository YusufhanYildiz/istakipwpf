using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using IsTakipWpf.Models;

namespace IsTakipWpf.Repositories
{
    public class JobRepository : BaseRepository, IJobRepository
    {
        private const string SelectBase = @"
            SELECT j.*, (c.FirstName || ' ' || c.LastName) as CustomerFullName, 
                   c.City as CustomerCity, c.District as CustomerDistrict 
            FROM Jobs j
            JOIN Customers c ON j.CustomerId = c.Id";

        public async Task<int> AddAsync(Job job)
        {
            job.CreatedDate = DateTime.Now;
            job.UpdatedDate = DateTime.Now;
            job.IsDeleted = false;

            const string sql = @"
                INSERT INTO Jobs (CustomerId, JobTitle, Description, Status, StartDate, EndDate, CreatedDate, UpdatedDate, IsDeleted)
                VALUES (@CustomerId, @JobTitle, @Description, @Status, @StartDate, @EndDate, @CreatedDate, @UpdatedDate, @IsDeleted);
                SELECT last_insert_rowid();";

            using (var connection = CreateConnection())
            {
                var id = await connection.ExecuteScalarAsync<int>(sql, job);
                job.Id = id;
                return id;
            }
        }

        public async Task<IEnumerable<Job>> GetAllAsync(bool includeDeleted = false)
        {
            string sql = SelectBase;
            
            if (!includeDeleted)
            {
                sql += " WHERE j.IsDeleted = 0";
            }

            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Job>(sql);
            }
        }

        public async Task<IEnumerable<Job>> GetByCustomerIdAsync(int customerId)
        {
            string sql = $"{SelectBase} WHERE j.CustomerId = @CustomerId AND j.IsDeleted = 0";

            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Job>(sql, new { CustomerId = customerId });
            }
        }

        public async Task<Job> GetByIdAsync(int id)
        {
            string sql = $"{SelectBase} WHERE j.Id = @Id";

            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Job>(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<Job>> SearchAsync(string searchTerm, int? customerId = null, JobStatus? status = null, string city = null, string district = null)
        {
            var sql = new StringBuilder($"{SelectBase} WHERE j.IsDeleted = 0");

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                sql.Append(" AND (j.JobTitle LIKE @Search OR j.Description LIKE @Search OR c.FirstName LIKE @Search OR c.LastName LIKE @Search)");
                parameters.Add("Search", $"%{searchTerm}%");
            }

            if (customerId.HasValue)
            {
                sql.Append(" AND j.CustomerId = @CustomerId");
                parameters.Add("CustomerId", customerId.Value);
            }

            if (status.HasValue)
            {
                sql.Append(" AND j.Status = @Status");
                parameters.Add("Status", (int)status.Value);
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                sql.Append(" AND c.City = @City");
                parameters.Add("City", city);
            }

            if (!string.IsNullOrWhiteSpace(district))
            {
                sql.Append(" AND c.District = @District");
                parameters.Add("District", district);
            }

            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Job>(sql.ToString(), parameters);
            }
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            const string sql = "UPDATE Jobs SET IsDeleted = 1, UpdatedDate = @UpdatedDate WHERE Id = @Id";

            using (var connection = CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id, UpdatedDate = DateTime.Now });
                return affectedRows > 0;
            }
        }

        public async Task<int> AddMultipleAsync(IEnumerable<Job> jobs)
        {
            const string sql = @"
                INSERT INTO Jobs (CustomerId, JobTitle, Description, Status, StartDate, EndDate, CreatedDate, UpdatedDate, IsDeleted)
                VALUES (@CustomerId, @JobTitle, @Description, @Status, @StartDate, @EndDate, @CreatedDate, @UpdatedDate, @IsDeleted);";

            using (var connection = CreateConnection())
            {
                if (connection.State != ConnectionState.Open) connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    int count = 0;
                    foreach (var job in jobs)
                    {
                        job.CreatedDate = DateTime.Now;
                        job.UpdatedDate = DateTime.Now;
                        job.IsDeleted = false;
                        count += await connection.ExecuteAsync(sql, job, transaction);
                    }
                    transaction.Commit();
                    return count;
                }
            }
        }

        public async Task<bool> UpdateAsync(Job job)
        {
            job.UpdatedDate = DateTime.Now;

            const string sql = @"
                UPDATE Jobs 
                SET CustomerId = @CustomerId,
                    JobTitle = @JobTitle, 
                    Description = @Description, 
                    Status = @Status, 
                    StartDate = @StartDate, 
                    EndDate = @EndDate,
                    UpdatedDate = @UpdatedDate 
                WHERE Id = @Id";

            using (var connection = CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync(sql, job);
                return affectedRows > 0;
            }
        }
    }
}