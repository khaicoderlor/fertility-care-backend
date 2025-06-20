using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities;

public class DoctorSchedule
{
    public long Id { get; set; }

    public Guid DoctorId { get; set; }

    public virtual Doctor Doctor { get; set; }

    public DateOnly WorkDate { get; set; }

    public long SlotId { get; set; }

    public virtual Slot Slot { get; set; }

    public int? MaxAppointments { get; set; }

    public bool IsAcceptingPatients { get; set; } = true;

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

}
