using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.Feedbacks;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IFeedbackService
    {
        Task<List<FeedbackDTO>> GetAllFeedbacksAsync(FeedbackQueryDTO query);//retrun list paginated
        Task<FeedbackDTO> CreateFeedbackAsync(CreateFeedbackRequestDTO request);
        Task<FeedbackDTO> UpdateFeedbackAsync(string feedbackId, UpdateFeedbackDTO request);
        Task<FeedbackDTO> UpdateStatusFeedbackAsync(string feedbackId, bool status);// for admin 
        Task<List<FeedbackDTO>> GetAllFeedbacksByDoctorIdAsync(string doctorId);

        Task<IEnumerable<AllFeedbackDTO>> GetAllFeedbackAsync();
    }
}
