using System;
using System.Collections.Generic;
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
                INSERT INTO Customers (FirstName, LastName, PhoneNumber, Address, CreatedDate, UpdatedDate, IsDeleted)
                VALUES (@FirstName, @LastName, @PhoneNumber, @Address, @CreatedDate, @UpdatedDate, @IsDeleted);
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

        public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm)
        {
            const string sql = @"
                SELECT * FROM Customers 
                WHERE IsDeleted = 0 
                AND (FirstName LIKE @Search OR LastName LIKE @Search OR PhoneNumber LIKE @Search)";

            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Customer>(sql, new { Search = $"%{searchTerm}%" });
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

        public async Task<bool> UpdateAsync(Customer customer)
        {
            customer.UpdatedDate = DateTime.Now;

            const string sql = @"
                UPDATE Customers 
                SET FirstName = @FirstName, 
                    LastName = @LastName, 
                    PhoneNumber = @PhoneNumber, 
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
