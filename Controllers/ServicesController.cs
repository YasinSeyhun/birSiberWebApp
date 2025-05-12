using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BirSiberDanismanlik.Models;
using BirSiberDanismanlik.Data;
using Microsoft.AspNetCore.Authorization;

namespace BirSiberDanismanlik.Controllers
{
    [Authorize(Roles = "Admin,Çalışan")]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.Services.ToListAsync();
            return View(services);
        }

        [HttpGet]
        public async Task<IActionResult> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();
            return Json(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateService(Service service)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "Geçersiz veri." });

            try
            {
                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Hizmet eklenirken bir hata oluştu." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(Service service)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, error = "Geçersiz veri." });

            try
            {
                _context.Services.Update(service);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Hizmet güncellenirken bir hata oluştu." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                var service = await _context.Services.FindAsync(id);
                if (service == null)
                    return Json(new { success = false, error = "Hizmet bulunamadı." });

                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Hizmet silinirken bir hata oluştu." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Management(string? searchString, string? sortOrder)
        {
            var query = _context.Appointments.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(a => a.ServiceType.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder == "date")
                    query = query.OrderByDescending(a => a.AppointmentDate);
                else if (sortOrder == "status")
                    query = query.OrderBy(a => a.Status);
            }
            var appointments = await query.ToListAsync();
            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAppointment(Appointment model)
        {
            if (!ModelState.IsValid)
            {
                var appointments = await _context.Appointments.ToListAsync();
                return View("Management", appointments);
            }
            model.CreatedAt = DateTime.UtcNow;
            _context.Appointments.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Management");
        }

        [HttpGet]
        public async Task<IActionResult> EditAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAppointment(int id, Appointment model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();
            appointment.UserId = model.UserId;
            appointment.ServiceType = model.ServiceType;
            appointment.AppointmentDate = model.AppointmentDate;
            appointment.Status = model.Status;
            appointment.Notes = model.Notes;
            appointment.InstructorId = model.InstructorId;
            await _context.SaveChangesAsync();
            return RedirectToAction("Management");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Management");
        }
    }
} 