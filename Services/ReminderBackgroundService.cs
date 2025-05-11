using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using BirSiberDanismanlik.Services;
using System.Linq;

namespace BirSiberDanismanlik.Services
{
    public class ReminderBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public ReminderBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
                    var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                    var smsService = scope.ServiceProvider.GetRequiredService<SmsService>();

                    var allAppointments = await appointmentService.GetAllAppointmentsAsync();
                    var now = DateTime.UtcNow;
                    var upcoming = allAppointments
                        .Where(a => a.AppointmentDate > now && a.AppointmentDate <= now.AddHours(1) && a.Status == "Onaylandı")
                        .ToList();

                    foreach (var appt in upcoming)
                    {
                        // TODO: Kullanıcı e-posta ve telefonunu çekmek için User tablosu ile ilişki kurmalısınız
                        // Örnek: string userEmail = ...; string userPhone = ...;
                        string userEmail = "user@example.com"; // Dummy
                        string userPhone = "+905xxxxxxxxx"; // Dummy
                        string msg = $"Sayın kullanıcı, {appt.AppointmentDate:dd.MM.yyyy HH:mm} tarihinde '{appt.ServiceType}' randevunuz bulunmaktadır.";
                        await emailService.SendEmailAsync(userEmail, "Randevu Hatırlatma", msg);
                        await smsService.SendSmsAsync(userPhone, msg);
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken); // 10 dakikada bir kontrol
            }
        }
    }
} 