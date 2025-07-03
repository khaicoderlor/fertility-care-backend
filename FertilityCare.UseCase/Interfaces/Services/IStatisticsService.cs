using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.Patients;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IStatisticsService
    {
        Task<IEnumerable<PatientMonthlyCountDTO>> GetPatientCountByYearAsync(int year);
    }
}
