using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities;

public class MedicalExamination
{

    public long Id { get; set; }

    public Guid AppointmentId { get; set; }
    
    public virtual Appointment Appointment { get; set; }

    public string? Symptoms { get; set; } = "";

    public string? Diagnosis { get; set; } = "";

    public string? Indications { get; set; } = "";

    public string? Note { get; set; } = "";

}
