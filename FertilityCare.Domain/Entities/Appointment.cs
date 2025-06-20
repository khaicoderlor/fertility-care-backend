using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities;

public class Appointment
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public virtual Patient Patient { get; set; }

    public Guid DoctorId { get; set; }

    public virtual Doctor Doctor { get; set; }

    public long DoctorScheduleId { get; set; }

    public virtual DoctorSchedule DoctorSchedule { get; set; }

    public Guid? TreatmentServiceId { get; set; }

    public virtual TreatmentService TreatmentService { get; set; }

    public AppointmentType Type { get; set; }

    public long? OrderStepId { get; set; }

    public virtual OrderStep? OrderStep { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Booked;

    public string? CancellationReason { get; set; }

    public string? Note { get; set; }

    public decimal? ExtraFee { get; set; }

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
