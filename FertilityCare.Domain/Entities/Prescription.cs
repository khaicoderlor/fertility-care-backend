using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities;

public class Prescription
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public virtual Order Order { get; set; }

    public DateTime PrescriptionDate { get; set; } = DateTime.Now;

    public virtual List<PrescriptionItem>? PrescriptionItems { get; set; }
}
