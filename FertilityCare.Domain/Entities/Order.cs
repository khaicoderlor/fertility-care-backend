using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public Guid DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }

        public Guid TreatmentServiceId { get; set; }

        public virtual TreatmentService TreatmentService { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.InProgress;

        public decimal? TotalAmount { get; set; }

        public long? TotalEgg  { get; set; }

        public bool IsFrozen { get; set; } = false;

        public DateTime? UpdatedAt { get; set; }

        public virtual List<OrderStep>? OrderSteps { get; set; }

        public virtual List<EggGained>? EggGaineds { get; set; }

        public virtual List<EmbryoTransfer>? EmbryoTransfers { get; set; }

        public virtual List<EmbryoGained>? EmbryoGaineds { get; set; }
    }
}
