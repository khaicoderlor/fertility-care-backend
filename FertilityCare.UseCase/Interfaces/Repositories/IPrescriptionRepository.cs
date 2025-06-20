﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;

namespace FertilityCare.UseCase.Interfaces.Repositories
{
    public interface IPrescriptionRepository : IBaseRepository<Prescription, Guid>
    {
        Task<IEnumerable<Prescription>> FindPrescriptionsByOrderIdAsync(Guid orderId);
        Task<IEnumerable<Prescription>> FindPrescriptionsByPatientIdAsync(Guid patientId);
    }
}
