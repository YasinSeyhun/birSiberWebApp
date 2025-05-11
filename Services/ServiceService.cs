using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BirSiberDanismanlik.Models;

namespace BirSiberDanismanlik.Services
{
    public interface IServiceService
    {
        Task<IEnumerable<Service>> GetAllServicesAsync();
        Task<Service?> GetServiceByIdAsync(int id);
        Task<bool> CreateServiceAsync(Service service);
        Task<bool> UpdateServiceAsync(Service service);
        Task<bool> DeleteServiceAsync(int id);
    }

    public class ServiceService : IServiceService
    {
        private readonly string _connectionString;

        public ServiceService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
        }

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand("SELECT * FROM Services WHERE IsActive = 1", connection);
                using var reader = await command.ExecuteReaderAsync();
                var services = new List<Service>();
                while (await reader.ReadAsync())
                {
                    services.Add(new Service
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                    });
                }
                return services;
            }
        }

        public async Task<Service?> GetServiceByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand("SELECT * FROM Services WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Service
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                    };
                }
                return null;
            }
        }

        public async Task<bool> CreateServiceAsync(Service service)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(
                    @"INSERT INTO Services (Name, Description, Price, IsActive)
                      VALUES (@Name, @Description, @Price, @IsActive)",
                    connection);

                command.Parameters.AddWithValue("@Name", service.Name);
                command.Parameters.AddWithValue("@Description", (object)service.Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@Price", service.Price);
                command.Parameters.AddWithValue("@IsActive", service.IsActive);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> UpdateServiceAsync(Service service)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(
                    @"UPDATE Services 
                      SET Name = @Name,
                          Description = @Description,
                          Price = @Price,
                          IsActive = @IsActive
                      WHERE Id = @Id",
                    connection);

                command.Parameters.AddWithValue("@Id", service.Id);
                command.Parameters.AddWithValue("@Name", service.Name);
                command.Parameters.AddWithValue("@Description", (object)service.Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@Price", service.Price);
                command.Parameters.AddWithValue("@IsActive", service.IsActive);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(
                    @"UPDATE Services 
                      SET IsActive = 0
                      WHERE Id = @Id",
                    connection);

                command.Parameters.AddWithValue("@Id", id);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
} 