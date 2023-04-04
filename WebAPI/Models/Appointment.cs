using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public string? DoctorName { get; set; }

    public string? PatientName { get; set; }

    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    public string? Time { get; set; }
}
