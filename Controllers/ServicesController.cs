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
    }
} 