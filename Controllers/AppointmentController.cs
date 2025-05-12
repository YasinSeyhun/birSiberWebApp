using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using BirSiberDanismanlik.Data;
using BirSiberDanismanlik.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BirSiberDanismanlik.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var appointments = await _context.Appointments
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
            return View(appointments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var appt = await _context.Appointments.FindAsync(id);
            if (appt == null || appt.UserId != _userManager.GetUserId(User)) return NotFound();
            return View(appt);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Instructors = await _userManager.GetUsersInRoleAsync("Çalışan");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment model)
        {
            if (!ModelState.IsValid) return View(model);
            model.UserId = _userManager.GetUserId(User);
            model.Status = "Beklemede";
            model.CreatedAt = DateTime.UtcNow;
            await _context.Appointments.AddAsync(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var appt = await _context.Appointments.FindAsync(id);
            var isAdmin = User.IsInRole("Admin");
            if (appt == null || (!isAdmin && appt.UserId != _userManager.GetUserId(User))) return NotFound();
            ViewBag.Instructors = await _userManager.GetUsersInRoleAsync("Çalışan");
            return View(appt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment model)
        {
            ViewBag.Instructors = await _userManager.GetUsersInRoleAsync("Çalışan");
            var appt = await _context.Appointments.FindAsync(id);
            var isAdmin = User.IsInRole("Admin");
            if (appt == null || (!isAdmin && appt.UserId != _userManager.GetUserId(User))) return NotFound();
            if (!ModelState.IsValid) {
                return View(model);
            }
            appt.ServiceType = model.ServiceType;
            appt.AppointmentDate = model.AppointmentDate;
            appt.Status = model.Status;
            appt.Notes = model.Notes;
            appt.InstructorId = model.InstructorId;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var appt = await _context.Appointments.FindAsync(id);
            if (appt == null || appt.UserId != _userManager.GetUserId(User)) return NotFound();
            return View(appt);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appt = await _context.Appointments.FindAsync(id);
            if (appt != null && appt.UserId == _userManager.GetUserId(User))
            {
                _context.Appointments.Remove(appt);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
} 