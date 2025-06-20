using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities;

public class OrderStep
{
    public long Id { get; set; }

    public Guid OrderId { get; set; }

    public virtual Order Order { get; set; }

    public long TreatmentStepId { get; set; }

    public virtual TreatmentStep TreatmentStep { get; set; }

    public StepStatus Status { get; set; } = StepStatus.Planned;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public decimal? TotalAmount { get; set; } = 0;

    public virtual List<Appointment>? Appointments { get; set; }


}
