using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Enums;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Feedbacks;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.DTOs.Statistics;
using FertilityCare.UseCase.DTOs.TreatmentServices;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.UseCase.Mappers;

namespace FertilityCare.UseCase.Implements
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IFeedbackRepository _feedbackRepository;

        private readonly IDoctorRepository _doctorRepository;

        private readonly IEggGainedRepository _eggGainedRepository;

        private readonly IEmbryoGainedRepository _embryoGainedRepository;

        private readonly IEmbryoTransferRepository _embryoTransferRepository;

        private readonly IOrderStepRepository _orderStepRepository;

        public StatisticsService(IAppointmentRepository appointmentRepository, IOrderStepRepository orderStepRepository, IEmbryoTransferRepository embryoTransferRepository, IEmbryoGainedRepository embryoGainedRepository, IEggGainedRepository eggGainedRepository, IOrderRepository orderRepository, IFeedbackRepository feedbackRepository, IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _orderRepository = orderRepository;
            _feedbackRepository = feedbackRepository;
            _doctorRepository = doctorRepository;
            _eggGainedRepository = eggGainedRepository;
            _embryoGainedRepository = embryoGainedRepository;
            _embryoTransferRepository = embryoTransferRepository;
            _orderStepRepository = orderStepRepository;
        }
        public async Task<string> GetRevenueByTreatmentServiceAsync(string treatmentName)
        {
            var revenue = await _orderRepository.GetRevenueByTreatmentServiceAsync(treatmentName);
            return revenue.ToString("N0"); // Format kiểu "100,000"
        }

        public async Task<string> GetTotalEmbryoTransfersAsync()
        {
            var count = await _embryoTransferRepository.CountTotalEmbryoTransfersAsync();
            return count.ToString();
        }

        public async Task<string> CountTotalOrdersAsync()
        {
            return (await _orderRepository.CountAllOrdersAsync()).ToString();
        }

        public async Task<string> FindTotalPatientAsync()
        {
            var count = await _orderRepository.CountDistinctActivePatientsAsync();
            return count.ToString();
        }

        public async Task<string> GetCurrentActiveDoctorCountAsync()
        {
            var count = await _orderRepository.CountDistinctDoctorsAsync();
            return count.ToString();
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

            var comparingPatientsPreviousMonth = totalPatientsPreviousMonth == 0
                ? 0
                : (decimal)(totalPatientsCurrentMonth - totalPatientsPreviousMonth) / totalPatientsPreviousMonth * 100;

            var comparingAppointmentsPreviousMonth = totalAppointmentPreviousMonth == 0
                ? 0
                : (decimal)(totalAppointmentCurrentMonth - totalAppointmentPreviousMonth) / totalAppointmentPreviousMonth * 100;

            var comparingRatePreviousMonth = totalRatePreviousMonth == 0
                ? 0
                : Math.Round((decimal)(totalRateCurrentMonth - totalRatePreviousMonth) / totalRatePreviousMonth * 100, 2);

            return new DoctorOverallStatistics
            {
                TotalPatients = totalPatients,
                TotalAppointments = appointments.Count(),
                TotalPatientsCurrentMonth = totalPatientsCurrentMonth,
                TotalPatientsPreviousMonth = totalPatientsPreviousMonth,
                TotalAppointmentsPreviousMonth = totalAppointmentPreviousMonth,
                TotalRate = totalRate ?? 0,
                TotalRatePreviousMonth = totalRatePreviousMonth,
                ComparingPatientsPreviousMonth = comparingPatientsPreviousMonth,
                ComparingAppointmentsPreviousMonth = comparingAppointmentsPreviousMonth,
                ComparingRatePreviousMonth = comparingRatePreviousMonth
            };
        }

        public async Task<IEnumerable<StatusTreatmentPatientOverall>> GetOrderStatusOverallByDoctorIdAsync(Guid guid)
        {
            var orders = await _orderRepository.FindAllByDoctorIdAsync(guid);

            var a = orders
                .GroupBy(x => x.Status)
                .Select(g => new StatusTreatmentPatientOverall
                {
                    Name = g.Key.ToString(),
                    Value = g.Select(x => x.Id).Distinct().Count()
                })
                .ToList();

            return a.Select(x => new StatusTreatmentPatientOverall
            {
                Name = x.Name,
                Value = x.Value,
                Color = x.Name switch
                {
                    nameof(OrderStatus.Completed) => "#10B981",
                    nameof(OrderStatus.InProgress) => "#3B82F6",
                    nameof(OrderStatus.Cancelled) => "#EF4444",
                    nameof(OrderStatus.Closed) => "#F59E0B",
                    _ => "#6C757D"
                }
            });
        }

        public async Task<IEnumerable<PatientMonthlyCountDTO>> GetPatientCountByYearAsync(Guid doctorId, int year)
        {
            var orders = await _orderRepository.FindAllByDoctorIdAsync(doctorId);
            var patients = orders.Where(x => x.StartDate.Year == year)
                .OrderBy(x => x.StartDate.Month)
                .GroupBy(x => x.StartDate.Month)
                .ToDictionary(x => x.Key, x => x.Select(g => g.PatientId).Distinct().Count());

            var appointments = await _appointmentRepository.FindAllByDoctorIdAsync(doctorId);
            var appointmentsMonthly = appointments
                .Where(a => a.AppointmentDate.Year == year)
                .OrderBy(a => a.AppointmentDate.Month)
                .GroupBy(a => a.AppointmentDate.Month)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Id).Count());

            var result = Enumerable.Range(1, 12)
                .Select(month => new PatientMonthlyCountDTO
                {
                    Month = month,
                    Patients = patients.ContainsKey(month) ? patients[month] : 0,
                    Appointments = appointmentsMonthly.ContainsKey(month) ? appointmentsMonthly[month] : 0
                })
                .ToList();

            return result;
        }

        public async Task<List<AverageRateMonthly>> GetStatisticAverageRateMonthlyDoctor(Guid doctorId)
        {
            var feedbackDotor = await _feedbackRepository.GetFeedbackByDoctorIdAsync(doctorId);
            var avgRateMonth = new List<AverageRateMonthly>();
            var year = DateTime.Now.Year;
            for (int i = 1; i <= 12; i++)
            {
                var avgRating = feedbackDotor.Where(x => x.CreatedAt.Year == year && x.CreatedAt.Month == i).Average(x => x.Rating);

                avgRateMonth.Add(new AverageRateMonthly
                {
                    Rating = avgRating,
                    Monthly = i,
                    IsData = avgRating > 0 ? true : false
                });
            }
            return avgRateMonth;
        }

        public async Task<List<DoctorDTO>> GetTop5DoctorMostApointmentAsync()
        {
            var listDoctors = _appointmentRepository.FindAllAsync().Result
                .GroupBy(x => x.DoctorId)
                .Select(g => new
                {
                    DoctorId = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();
            List<DoctorDTO> topDoctors = new List<DoctorDTO>();
            foreach (var doctor in listDoctors)
            {
                var doctorEntity = await _doctorRepository.FindByIdAsync(doctor.DoctorId);
                topDoctors.Add(doctorEntity.MapToDoctorDTO());
            }
            return topDoctors;
        }
        public async Task<string> GetTotalEggsByMonthAsync(int month)
        {
            var total = await _eggGainedRepository.CountEggGainedByMonthAsync(month);
            return total.ToString();
        }

        public async Task<List<TurnoverTreatmentDTO>> GetTurnoverByTreatmentName()
        {
            var result = await _orderRepository.FindAllAsync();
            return result.GroupBy(x => x.TreatmentService.Name)
            .Select(g => new TurnoverTreatmentDTO
            {
                TreatmentName = g.Key,
                TotalTurnover = g.Sum(x => x.TotalAmount ?? 0)
            })
            .ToList();
        }
        public async Task<string> CountAppointmentsTodayAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return (await _appointmentRepository.CountAppointmentsByDateAsync(today)).ToString();
        }
        public async Task<string> GetTotalEmbryosAsync()
        {
            var count = await _embryoGainedRepository.CountTotalEmbryosAsync();
            return count.ToString();
        }

        public async Task<RecentStatistics> GetRecentStatisticsAsync()
        {
            return new RecentStatistics
            {
                totalPatients = (await _orderRepository.CountDistinctActivePatientsAsync()).ToString(),
                totalDoctor = (await _orderRepository.CountDistinctDoctorsAsync()).ToString(),
                totalOrders = (await _orderRepository.CountAllOrdersAsync()).ToString(),
                totalRevenue = (await _appointmentRepository.GetTodayRevenueAsync()),
                totalAppointments = (await _appointmentRepository.CountAppointmentsByDateAsync(DateOnly.FromDateTime(DateTime.Today))).ToString(),
                totalEggsByMonth = (await _eggGainedRepository.CountEggGainedByMonthAsync(DateTime.Now.Month)).ToString(),
                totalEmbryosByMonth = (await _embryoGainedRepository.CountTotalEmbryosAsync()).ToString(),
                totalEmryoTransfersByMonth = (await _embryoTransferRepository.CountTotalEmbryoTransfersAsync()).ToString(),
                totalRevenueByIVF = (await _orderRepository.GetRevenueByTreatmentServiceAsync("IVF")).ToString("N0"),
                totalRevenueByIUI = (await _orderRepository.GetRevenueByTreatmentServiceAsync("IUI")).ToString("N0")
            };
        }

        public async Task<ManagerSideStatistics> GetManagerSideStatisticsAsync()
        {
            var totalPatients = await _orderRepository.CountDistinctActivePatientsAsync();

            var inProgressOrders = await _orderRepository.CountOrderInProgressAsync();

            var completedOrders = await _orderRepository.CountOrderCompletedAsync();

            var plannedSteps = await _orderStepRepository.CountOrderStepPlannedAsync();

            return new ManagerSideStatistics
            {
                TotalPatients = totalPatients,
                TotalInProgressOrder = inProgressOrders,
                TotalCompleteOrder = completedOrders,
                TotalPlannedOrder = plannedSteps
            };
        }

        public async Task<StatisticsFeedbackDTO> GetStatisticFeedbackAsync()
        {
            var allFeedback = await _feedbackRepository.FindAllAsync();
            int total5Start = allFeedback.Count(x => x.Rating == 5);
            int total4Start = allFeedback.Count(x => x.Rating == 4);
            int total3Start = allFeedback.Count(x => x.Rating == 3);
            int total2Start = allFeedback.Count(x => x.Rating == 2);
            int total1Start = allFeedback.Count(x => x.Rating == 1);
            return new StatisticsFeedbackDTO
            {
                NumberOf5Start = total5Start,
                NumberOf4Start = total4Start,
                NumberOf3Start = total3Start,
                NumberOf2Start = total2Start,
                NumberOf1Start = total1Start,
                TotalFeedbacks = allFeedback.Count()
            };
        }
    }
}
