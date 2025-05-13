using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BirSiberDanismanlik.Data;

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
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

                    var now = DateTime.Now;
                    var upcoming = await db.Appointments
                        .Where(a => a.AppointmentDate <= now.AddMinutes(30)
                                 && a.AppointmentDate > now
                                 && !a.IsReminderSent)
                        .ToListAsync();

                    foreach (var appt in upcoming)
                    {
                        var user = await db.Users.FindAsync(appt.UserId);
                        if (user != null && !string.IsNullOrEmpty(user.Email))
                        {
                            var instructor = appt.InstructorId != null ? await db.Users.FindAsync(appt.InstructorId) : null;
                            var body = $@"
                            <div style='font-family:Arial,sans-serif;background:#f8f9fa;padding:24px;border-radius:12px;'>
                                <h2 style='color:#2b2b2b;'>Randevu Hatırlatma</h2>
                                <p>Merhaba <b>{user.FullName ?? user.UserName}</b>,</p>
                                <p><b>{appt.AppointmentDate:dd.MM.yyyy HH:mm}</b> tarihinde <b>{appt.ServiceType}</b> randevunuz bulunmaktadır.</p>
                                {(instructor != null ? $"<p><b>Eğitmen:</b> {instructor.FullName ?? instructor.UserName}</p>" : "")}
                                {(string.IsNullOrWhiteSpace(appt.Notes) ? "" : $"<p><b>Not:</b> {appt.Notes}</p>")}
                                <hr style='margin:16px 0;'>
                                <p style='color:#888;font-size:13px;'>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
                            </div>";
                            await emailService.SendEmailAsync(user.Email, "Randevu Hatırlatma", body, true);

                            appt.IsReminderSent = true;
                        }
                    }
                    await db.SaveChangesAsync();
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
} 