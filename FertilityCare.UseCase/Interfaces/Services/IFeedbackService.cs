using FertilityCare.UseCase.DTOs.Feedbacks;
using FertilityCare.UseCase.DTOs.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IFeedbackService
    {
        Task<List<FeedbackDTO>> GetAllFeedbacksAsync(FeedbackQueryDTO query);//retrun list paginated
        Task<FeedbackDTO> CreateFeedbackAsync(CreateFeedbackRequestDTO request);
        Task<FeedbackDTO> UpdateFeedbackAsync(string feedbackId, UpdateFeedbackDTO request);
        Task<FeedbackDTO> UpdateStatusFeedbackAsync(string feedbackId, bool status);// for admin 
        Task<List<FeedbackDTO>> GetAllFeedbacksByDoctorIdAsync(string doctorId);
        Task<IEnumerable<FeedbackLatestSideManager>> GetSecondFeedbackLatestManagerSide();
        Task<IEnumerable<AllFeedbackDTO>> GetAllFeedbackAsync();
        Task<BestRateDoctor?> GetBestRateDoctor();

        Task<IEnumerable<FeedbackDTO>> GetFeedbacksByPatientIdAsync(Guid patientId);

    }
}
