using birSiberDanismanlik.ViewModels;

namespace birSiberDanismanlik.Services
{
    public interface IServiceService
    {
        Task<List<ServiceViewModel>> GetAllServicesAsync();
        Task<ServiceViewModel?> GetServiceByIdAsync(int id);
        Task<bool> CreateServiceAsync(ServiceViewModel service);
        Task<bool> UpdateServiceAsync(ServiceViewModel service);
        Task<bool> DeleteServiceAsync(int id);
        Task<List<ServiceViewModel>> GetActiveServicesAsync();
    }
} 