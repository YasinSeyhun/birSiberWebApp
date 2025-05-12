using Microsoft.AspNetCore.Mvc;
using BirSiberDanismanlik.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BirSiberDanismanlik.Data;

namespace BirSiberDanismanlik.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                var appts = await _context.Appointments
                    .Where(a => a.UserId == userId)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToListAsync();
                ViewBag.Appointments = appts;
                // Eğitmen isimleri için dictionary oluştur
                var instructorIds = appts.Where(a => !string.IsNullOrEmpty(a.InstructorId)).Select(a => a.InstructorId).Distinct().ToList();
                var instructors = await _userManager.Users.Where(u => instructorIds.Contains(u.Id)).ToListAsync();
                ViewBag.InstructorDict = instructors.ToDictionary(i => i.Id, i => i.FullName ?? i.UserName);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Instructors()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
