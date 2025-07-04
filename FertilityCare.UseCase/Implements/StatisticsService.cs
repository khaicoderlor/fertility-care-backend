using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.DTOs.Statistics;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;

namespace FertilityCare.UseCase.Implements
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IFeedbackRepository _feedbackRepository;

        private readonly IDoctorRepository _doctorRepository;

        public StatisticsService(IAppointmentRepository appointmentRepository, IOrderRepository orderRepository, IFeedbackRepository feedbackRepository, IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _orderRepository = orderRepository;
            _feedbackRepository = feedbackRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<DoctorOverallStatistics> GetDoctorOverallStatisticInDashboardAsync(string doctorId)
        {
            var now = DateTime.Now;

            var doctor = await _doctorRepository.FindByIdAsync(Guid.Parse(doctorId));

            var order = await _orderRepository.FindAllByDoctorIdAsync(Guid.Parse(doctorId));

            var totalPatients = order.Select(x => x.PatientId).Distinct().Count();

            var totalPatientsCurrentMonth = order.Where(x => x.StartDate.Year == now.Year && x.StartDate.Month == now.Month)
                .Select(x => x.PatientId).Distinct().Count();

            var totalPatientsPreviousMonth = order.Where(x => x.StartDate.Year == now.Year && x.StartDate.Month == now.Month - 1)
                .Select(x => x.PatientId).Distinct().Count();

            var appointments = await _appointmentRepository.FindByDoctorIdAsync(Guid.Parse(doctorId));

            var totalAppointmentCurrentMonth = appointments
                .Where(x => x.AppointmentDate.Year == now.Year && x.AppointmentDate.Month == now.Month)
                .Count();

            var totalAppointmentPreviousMonth = appointments.Where(x => x.AppointmentDate.Year == now.Year && x.AppointmentDate.Month == now.Month - 1)
                .Count();

            var totalRate = doctor.Rating;

            var feedbackCur = await _feedbackRepository.FindAllByDoctorIdAndMonthAsync(Guid.Parse(doctorId), now.Month, now.Year);

            var totalRateCurrentMonth = feedbackCur.Any() ? feedbackCur.Average(x => x.Rating) : 0;

            var feedbackPrev = await _feedbackRepository.FindAllByDoctorIdAndMonthAsync(Guid.Parse(doctorId), now.Month - 1, now.Year);

            var totalRatePreviousMonth = feedbackPrev.Any() ? feedbackPrev.Average(x => x.Rating) : 0;

            var comparingPatientsPreviousMonth = totalPatientsCurrentMonth == 0 ? 0 : (decimal)(totalPatientsCurrentMonth - totalPatientsPreviousMonth) / totalPatientsPreviousMonth * 100;

            var comparingAppointmentsPreviousMonth = totalAppointmentCurrentMonth == 0 ? 0 : (decimal)(totalAppointmentCurrentMonth - totalAppointmentPreviousMonth) / totalAppointmentPreviousMonth * 100;

            var comparingRatePreviousMonth = totalRateCurrentMonth == 0 ? 0 : (decimal)(totalRateCurrentMonth - totalRatePreviousMonth) / totalRatePreviousMonth * 100;
            
            return new DoctorOverallStatistics
            {
                TotalPatients = totalPatients,
                TotalPatientsCurrentMonth = totalPatientsCurrentMonth,
                TotalPatientsPreviousMonth = totalPatientsPreviousMonth,
                TotalAppointmentsPreviousMonth = totalAppointmentPreviousMonth,
                TotalRate = totalRate??0,
                TotalRatePreviousMonth = totalRatePreviousMonth,
                ComparingPatientsPreviousMonth = comparingPatientsPreviousMonth,
                ComparingAppointmentsPreviousMonth = comparingAppointmentsPreviousMonth,
                ComparingRatePreviousMonth = comparingRatePreviousMonth
            };
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
