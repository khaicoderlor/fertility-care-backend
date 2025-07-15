using FertilityCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Repositories
{
    public interface IOrderStepPaymentRepository : IBaseRepository<OrderStepPayment, Guid>
    {

        Task<IEnumerable<OrderStepPayment>> FindAllByPatientIdAsync(Guid patientId); 
        
       
    }
}
