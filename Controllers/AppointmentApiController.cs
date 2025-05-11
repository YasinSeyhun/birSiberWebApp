using Microsoft.AspNetCore.Mvc;
using BirSiberDanismanlik.Services;
using BirSiberDanismanlik.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BirSiberDanismanlik.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentApiController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentApiController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _appointmentService.GetAllAppointmentsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _appointmentService.GetAppointmentByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Appointment appointment)
        {
            var success = await _appointmentService.CreateAppointmentAsync(appointment);
            if (!success) return BadRequest();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Appointment appointment)
        {
            if (id != appointment.Id) return BadRequest();
            var success = await _appointmentService.UpdateAppointmentAsync(appointment);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _appointmentService.DeleteAppointmentAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
} 