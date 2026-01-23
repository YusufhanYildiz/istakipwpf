using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IsTakipWpf.Models;

namespace IsTakipWpf.Repositories
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public async Task<int> AddAsync(Customer customer)
        {
            customer.CreatedDate = DateTime.Now;
            customer.UpdatedDate = DateTime.Now;
            customer.IsDeleted = false;

            const string sql = @"
                INSERT INTO Customers (FirstName, LastName, PhoneNumber, City, District, Address, CreatedDate, UpdatedDate, IsDeleted)
                VALUES (@FirstName, @LastName, @PhoneNumber, @City, @District, @Address, @CreatedDate, @UpdatedDate, @IsDeleted);
                SELECT last_insert_rowid();";

            using (var connection = CreateConnection())
            {
                var id = await connection.ExecuteScalarAsync<int>(sql, customer);
                customer.Id = id;
                return id;
            }
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(bool includeDeleted = false)
        {
            string sql = "SELECT * FROM Customers" + (includeDeleted ? "" : " WHERE IsDeleted = 0");

            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Customer>(sql);
            }
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM Customers WHERE Id = @Id";

            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Customer>(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm, string city = null, string district = null)
        {
            var sql = @"
                SELECT * FROM Customers 
                WHERE IsDeleted = 0";
            
            var parameters = new DynamicParameters();
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                sql += " AND (FirstName LIKE @Search OR LastName LIKE @Search OR (FirstName || ' ' || LastName) LIKE @Search OR PhoneNumber LIKE @Search OR Address LIKE @Search)";
                parameters.Add("Search", $"%{searchTerm}%");
            }
            
            if (!string.IsNullOrWhiteSpace(city))
            {
                sql += " AND City = @City";
                parameters.Add("City", city);
            }
            
            if (!string.IsNullOrWhiteSpace(district))
            {
                sql += " AND District = @District";
                parameters.Add("District", district);
            }

            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Customer>(sql, parameters);
            }
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            const string sql = "UPDATE Customers SET IsDeleted = 1, UpdatedDate = @UpdatedDate WHERE Id = @Id";

            using (var connection = CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id, UpdatedDate = DateTime.Now });
                return affectedRows > 0;
            }
        }

        public async Task<int> AddMultipleAsync(IEnumerable<Customer> customers)
        {
            const string sql = @"
                INSERT INTO Customers (FirstName, LastName, PhoneNumber, City, District, Address, CreatedDate, UpdatedDate, IsDeleted)
                VALUES (@FirstName, @LastName, @PhoneNumber, @City, @District, @Address, @CreatedDate, @UpdatedDate, @IsDeleted);";

            using (var connection = CreateConnection())
            {
                if (connection.State != ConnectionState.Open) connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    int count = 0;
                    foreach (var customer in customers)
                    {
                        customer.CreatedDate = DateTime.Now;
                        customer.UpdatedDate = DateTime.Now;
                        customer.IsDeleted = false;
                        count += await connection.ExecuteAsync(sql, customer, transaction);
                    }
                    transaction.Commit();
                    return count;
                }
            }
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            customer.UpdatedDate = DateTime.Now;

            const string sql = @"
                UPDATE Customers 
                SET FirstName = @FirstName, 
                    LastName = @LastName, 
                    PhoneNumber = @PhoneNumber,
                    City = @City,
                    District = @District,
                    Address = @Address, 
                    UpdatedDate = @UpdatedDate 
                WHERE Id = @Id";

            using (var connection = CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync(sql, customer);
                return affectedRows > 0;
            }
        }
    }
}
