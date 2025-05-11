using System;

namespace BirSiberDanismanlik.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? InstructorId { get; set; }
    }
} 