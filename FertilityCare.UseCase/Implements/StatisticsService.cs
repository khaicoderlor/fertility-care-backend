using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;

namespace FertilityCare.UseCase.Implements
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public StatisticsService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IEnumerable<PatientMonthlyCountDTO>> GetPatientCountByYearAsync(int year)
        {
            var appointments = await _appointmentRepository.FindAllAsync();
            var countByMonth = appointments
                .Where(a => a.AppointmentDate.Year == year)
                .GroupBy(a => a.AppointmentDate.Month)
                .ToDictionary(g => g.Key, g => g.Select(x => x.PatientId).Distinct().Count());

            var result = Enumerable.Range(1, 12)
                .Select(month => new PatientMonthlyCountDTO
                {
                    Month = month,
                    PatientCount = countByMonth.ContainsKey(month) ? countByMonth[month] : 0
                })
                .ToList();
            return result;
        }
    }
}
