using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Feedbacks;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.DTOs.Statistics;
using FertilityCare.UseCase.DTOs.TreatmentServices;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IStatisticsService
    {
        Task<DoctorOverallStatistics> GetDoctorOverallStatisticInDashboardAsync(string doctorId);
        Task<IEnumerable<StatusTreatmentPatientOverall>> GetOrderStatusOverallByDoctorIdAsync(Guid guid);
        Task<IEnumerable<PatientMonthlyCountDTO>> GetPatientCountByYearAsync(Guid doctorId, int year);
        Task<List<AverageRateMonthly>> GetStatisticAverageRateMonthlyDoctor(Guid doctorId);
        Task<string> FindTotalPatientAsync();
        Task<List<DoctorDTO>> GetTop5DoctorMostApointmentAsync();
        Task<string> GetCurrentActiveDoctorCountAsync();
        Task<string> CountTotalOrdersAsync();
        Task<string> CountAppointmentsTodayAsync();
        Task<string> GetTotalEggsByMonthAsync(int month);
        Task<string> GetTotalEmbryosAsync();
        Task<string> GetTotalEmbryoTransfersAsync();
        Task<string> GetRevenueByTreatmentServiceAsync(string treatmentName);
        Task<RecentStatistics> GetRecentStatisticsAsync();
        Task<ManagerSideStatistics> GetManagerSideStatisticsAsync();
        Task<List<TurnoverTreatmentDTO>> GetTurnoverByTreatmentName();
        Task<StatisticsFeedbackDTO> GetStatisticFeedbackAsync();

        Task<ReportProgressSideAdmin> GetReportProgressSideAdminAsync(string orderId);
    }
}
