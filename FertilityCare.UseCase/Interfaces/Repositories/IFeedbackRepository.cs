using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Feedbacks;

namespace FertilityCare.UseCase.Interfaces.Repositories
{
    public interface IFeedbackRepository : IBaseRepository<Feedback, Guid>
    {
        Task<List<Feedback>> FindAllByDoctorIdAndMonthAsync(Guid guid, int month, int year);
        Task<List<Feedback>> FindDoctorByIdAsync(Guid doctorId);
        Task<List<Feedback>> FindTreatmentServiceByIdAsync(Guid treatmentId);
        Task<List<Feedback>> GetAllFeedbacksAsync(int pageNumber, int pageSize);
        Task<List<Feedback>> GetFeedbackByAllIdAsync(FeedbackQueryDTO query, int pageNumber, int pageSize);

        Task<List<Feedback>> GetFeedbackByDoctorIdAsync(Guid doctorId);
    }
}
