using Microsoft.AspNetCore.Mvc;
using BirSiberDanismanlik.Services;
using BirSiberDanismanlik.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace BirSiberDanismanlik.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "Admin,Çalışan,Calisan")] // KALDIRILDI
    public class AppointmentApiController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppointmentApiController(IAppointmentService appointmentService, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
        }

        private IActionResult AccessDeniedIfNotAuthorized()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated || !(user.IsInRole("Admin") || user.IsInRole("Çalışan") || user.IsInRole("Calisan")))
            {
                Response.StatusCode = 403;
                return Content(
                    "<html><body style='background:#181818;color:#fff;text-align:center;padding:60px;'><h2>Erisim Reddedildi</h2><p>Bu sayfaya erisim yetkiniz yok.</p><a href='/' style='color:#6cf;'>Ana Sayfa</a></body></html>",
                    "text/html"
                );
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var denied = AccessDeniedIfNotAuthorized();
            if (denied != null) return denied;
            var result = await _appointmentService.GetAllAppointmentsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var denied = AccessDeniedIfNotAuthorized();
            if (denied != null) return denied;
            var result = await _appointmentService.GetAppointmentByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Appointment appointment)
        {
            var denied = AccessDeniedIfNotAuthorized();
            if (denied != null) return denied;
            var success = await _appointmentService.CreateAppointmentAsync(appointment);
            if (!success) return BadRequest();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Appointment appointment)
        {
            var denied = AccessDeniedIfNotAuthorized();
            if (denied != null) return denied;
            if (id != appointment.Id) return BadRequest();
            var success = await _appointmentService.UpdateAppointmentAsync(appointment);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var denied = AccessDeniedIfNotAuthorized();
            if (denied != null) return denied;
            var success = await _appointmentService.DeleteAppointmentAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
} 