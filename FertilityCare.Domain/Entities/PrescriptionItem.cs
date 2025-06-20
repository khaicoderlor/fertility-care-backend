using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities;

public class PrescriptionItem
{
    public long Id { get; set; }

    public Guid PrescriptionId { get; set; }

    public virtual Prescription Prescription { get; set; } 

    public string? MedicationName { get; set; }

    public int Quantity { get; set; }

    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public DateOnly? EndDate { get; set; }

    public string? SpecialInstructions { get; set; }


}
