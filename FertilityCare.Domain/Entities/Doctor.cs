using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities;

public class Doctor
{

    public Guid Id { get; set; }

    public Guid UserProfileId { get; set; }

    public virtual UserProfile UserProfile { get; set; }

    public string? Degree { get; set; }

    public string? Specialization { get; set; }

    public int? YearsOfExperience { get; set; } 

    public string? Biography { get; set; }

    public decimal? Rating { get; set; }

    public int? PatientsServed { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual List<DoctorSchedule>? DoctorSchedules { get; set; }

}
    