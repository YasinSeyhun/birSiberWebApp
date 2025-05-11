using System.Threading.Tasks;
using System.Collections.Generic;
using BirSiberDanismanlik.Models;

namespace BirSiberDanismanlik.Services
{
    public interface IAppointmentService
    {
        Task<int> GetTotalAppointmentsCountAsync();
        Task<int> GetTodayAppointmentsCountAsync();
        Task<IEnumerable<Appointment>> GetRecentAppointmentsAsync(int count = 5);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> GetAppointmentByIdAsync(int id);
        Task<bool> CreateAppointmentAsync(Appointment appointment);
        Task<bool> UpdateAppointmentAsync(Appointment appointment);
        Task<bool> DeleteAppointmentAsync(int id);
    }
} 