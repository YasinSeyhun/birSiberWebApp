using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BirSiberDanismanlik.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using BirSiberDanismanlik.Data;
using System.Linq;

namespace BirSiberDanismanlik.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly string _connectionString;
        private readonly IMemoryCache _cache;
        private readonly IDistributedCache _distributedCache;
        private readonly ApplicationDbContext _dbContext;

        public AppointmentService(IConfiguration configuration, IMemoryCache cache, IDistributedCache distributedCache, ApplicationDbContext dbContext)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
            _cache = cache;
            _distributedCache = distributedCache;
            _dbContext = dbContext;
        }

        public async Task<int> GetTotalAppointmentsCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand("SELECT COUNT(*) FROM Appointments", connection);
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
        }

        public async Task<int> GetTodayAppointmentsCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(
                    "SELECT COUNT(*) FROM Appointments WHERE CAST(AppointmentDate AS DATE) = CAST(GETDATE() AS DATE)",
                    connection);
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
        }

        public async Task<IEnumerable<Appointment>> GetRecentAppointmentsAsync(int count = 5)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(
                    "SELECT TOP (@Count) * FROM Appointments ORDER BY AppointmentDate DESC",
                    connection);
                command.Parameters.AddWithValue("@Count", count);
                using var reader = await command.ExecuteReaderAsync();
                return ConvertDataTableToAppointments(reader);
            }
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            var cacheKey = "all_appointments";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<Appointment> cachedAppointments))
            {
                return cachedAppointments;
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand("SELECT * FROM Appointments", connection);
                using var reader = await command.ExecuteReaderAsync();
                var appointments = ConvertDataTableToAppointments(reader);
                _cache.Set(cacheKey, appointments, TimeSpan.FromMinutes(1));
                return appointments;
            }
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand("SELECT * FROM Appointments WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return ConvertDataRowToAppointment(reader);
                }
                return null;
            }
        }

        public async Task<bool> CreateAppointmentAsync(Appointment appointment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(
                    @"INSERT INTO Appointments (UserId, ServiceType, AppointmentDate, Status, Notes, CreatedAt)
                      VALUES (@UserId, @ServiceType, @AppointmentDate, @Status, @Notes, @CreatedAt)",
                    connection);

                command.Parameters.AddWithValue("@UserId", appointment.UserId);
                command.Parameters.AddWithValue("@ServiceType", appointment.ServiceType);
                command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                command.Parameters.AddWithValue("@Status", appointment.Status);
                command.Parameters.AddWithValue("@Notes", (object)appointment.Notes! ?? DBNull.Value);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(
                    @"UPDATE Appointments 
                      SET ServiceType = @ServiceType,
                          AppointmentDate = @AppointmentDate,
                          Status = @Status,
                          Notes = @Notes
                      WHERE Id = @Id",
                    connection);

                command.Parameters.AddWithValue("@Id", appointment.Id);
                command.Parameters.AddWithValue("@ServiceType", appointment.ServiceType);
                command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                command.Parameters.AddWithValue("@Status", appointment.Status);
                command.Parameters.AddWithValue("@Notes", (object)appointment.Notes! ?? DBNull.Value);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand("DELETE FROM Appointments WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<IEnumerable<Appointment>> GetPopularAppointmentsAsync(int count = 5)
        {
            var cacheKey = $"popular_appointments_{count}";
            var cached = await _distributedCache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                return JsonSerializer.Deserialize<List<Appointment>>(cached) ?? new List<Appointment>();
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(
                    "SELECT TOP (@Count) * FROM Appointments ORDER BY AppointmentDate DESC", connection);
                command.Parameters.AddWithValue("@Count", count);
                using var reader = await command.ExecuteReaderAsync();
                var appointments = ConvertDataTableToAppointments(reader);
                var json = JsonSerializer.Serialize(appointments);
                await _distributedCache.SetStringAsync(cacheKey, json, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
                return appointments;
            }
        }

        private List<Appointment> ConvertDataTableToAppointments(SqlDataReader reader)
        {
            var appointments = new List<Appointment>();
            while (reader.Read())
            {
                appointments.Add(ConvertDataRowToAppointment(reader));
            }
            return appointments;
        }

        private Appointment ConvertDataRowToAppointment(SqlDataReader reader)
        {
            return new Appointment
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                UserId = reader.GetString(reader.GetOrdinal("UserId")),
                ServiceType = reader.GetString(reader.GetOrdinal("ServiceType")),
                AppointmentDate = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            };
        }
    }
} 