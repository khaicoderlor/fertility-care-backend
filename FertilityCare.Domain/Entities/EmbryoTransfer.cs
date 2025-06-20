using FertilityCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Domain.Entities;

public class EmbryoTransfer
{
    public long Id { get; set; }

    public long EmbryoGainedId { get; set; }

    public virtual EmbryoGained EmbryoGained { get; set; }

    public DateTime TransferDate { get; set; }

    public TransferType TransferType { get; set; } = TransferType.Fresh;

    public Guid? AppointmentId { get; set; }

    public virtual Appointment? Appointment { get; set; } 

    public Guid OrderId { get; set; }
    
    public virtual Order Order { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
