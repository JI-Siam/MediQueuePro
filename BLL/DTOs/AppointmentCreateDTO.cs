using System.ComponentModel.DataAnnotations;

public class AppointmentCreateDTO
{
    [Required]
    public int DoctorId { get; set; }

    [Required]
    public DateOnly AppointmentDate { get; set; }

    [Required]
    public TimeOnly AppointmentTime { get; set; }

    public bool IsEmergency { get; set; }
}