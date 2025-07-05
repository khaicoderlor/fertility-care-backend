using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Feedbacks;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.UseCase.Mappers;

namespace FertilityCare.UseCase.Implements
{
    public class FeedbackService : IFeedbackService
    {
        private IFeedbackRepository _feedbackRepository;
        private IDoctorRepository _doctorRepository;
        private ITreatmentServiceRepository _treatmentServiceRepository;
        public FeedbackService(IFeedbackRepository feedbackRepository, 
            IDoctorRepository doctorRepository,
            ITreatmentServiceRepository treatmentServiceRepository)
        {
            _feedbackRepository = feedbackRepository;
            _doctorRepository = doctorRepository;
            _treatmentServiceRepository = treatmentServiceRepository;
        }

        //có thể sử dụng hàm này cho cả feedback cho doctor và treatment service hoặc là cả 2
        public async Task<FeedbackDTO> CreateFeedbackAsync(CreateFeedbackRequestDTO request)
        {
            //có thể thêm validation ở đây nếu như patient không có trong bất cứ order nào thì không cho phép feedback
            //nếu order đó không có bác sĩ mà patient muốn feedback thì không cho phép feedback
            // nếu order đó không có dịch vụ điều trị mà patient muốn feedback thì không cho phép feedback

            var feedback = new Feedback()
            {
                PatientId = Guid.Parse(request.PatientId),
                DoctorId = string.IsNullOrWhiteSpace(request.DoctorId) ? Guid.Empty : Guid.Parse(request.DoctorId),
                TreatmentServiceId = string.IsNullOrWhiteSpace(request.TreatmentServiceId) ? null : Guid.Parse(request.TreatmentServiceId),
                Status = true,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.Now,
                UpdatedAt = null

            };
            await _feedbackRepository.SaveAsync(feedback);
 
            if(!string.IsNullOrWhiteSpace(feedback.DoctorId.ToString()) && !string.IsNullOrWhiteSpace(feedback.TreatmentServiceId.ToString()))
            {
                await RatingDoctor(feedback.DoctorId);
                await RatingTreatmentService(feedback.TreatmentServiceId ?? Guid.Empty);
            }
            else if (!string.IsNullOrWhiteSpace(feedback.DoctorId.ToString()) && string.IsNullOrWhiteSpace(feedback.TreatmentServiceId.ToString()))
            {
                await RatingDoctor(feedback.DoctorId);

            }
            else if (!string.IsNullOrWhiteSpace(feedback.TreatmentServiceId.ToString()) && string.IsNullOrWhiteSpace(feedback.DoctorId.ToString()))
            {
                await RatingTreatmentService(feedback.TreatmentServiceId ?? Guid.Empty);
            }
            return feedback.MapToFeedbackDTO();
        }

        public async Task<List<FeedbackDTO>> GetAllFeedbacksAsync(FeedbackQueryDTO query)
        {
            var loadedFeedbacks = await _feedbackRepository.GetFeedbackByAllIdAsync(query, query.PageNumber, query.PageSize);
            return loadedFeedbacks.Select(feedback => feedback.MapToFeedbackDTO()).ToList();
        }
        public async Task<FeedbackDTO> UpdateFeedbackAsync(string feedbackId, UpdateFeedbackDTO request)
        {
            var loadedFeedback = await _feedbackRepository.FindByIdAsync(Guid.Parse(feedbackId))
                ?? throw new NotFoundException($"Feedback with id {feedbackId} not found.");
            loadedFeedback.Comment = request.Comment;
            loadedFeedback.Rating = request.Rating;
            loadedFeedback.UpdatedAt = DateTime.Now;
            await _feedbackRepository.UpdateAsync(loadedFeedback);
            if(loadedFeedback.DoctorId != null)
            {
                await RatingDoctor(loadedFeedback.DoctorId);
            }
            if (loadedFeedback.TreatmentServiceId != null)
            {
                await RatingTreatmentService(loadedFeedback.TreatmentServiceId ?? Guid.Empty);
            }
            return loadedFeedback.MapToFeedbackDTO();
        }

        public async Task<FeedbackDTO> UpdateStatusFeedbackAsync(string feedbackId, bool status)
        {
            var loadedFeedback = await _feedbackRepository.FindByIdAsync(Guid.Parse(feedbackId))
                ?? throw new NotFoundException($"Feedback with id {feedbackId} not found.");

            loadedFeedback.Status = status;
            loadedFeedback.UpdatedAt = DateTime.Now;
            await _feedbackRepository.UpdateAsync(loadedFeedback);

            await RatingDoctor(loadedFeedback.DoctorId);
            await RatingTreatmentService(loadedFeedback.TreatmentServiceId ?? Guid.Empty);

            return loadedFeedback.MapToFeedbackDTO();
        }
        private async Task RatingTreatmentService(Guid treatmentServicesId)
        {
            var feedbackTreatmentService = await _feedbackRepository
                        .FindTreatmentServiceByIdAsync(treatmentServicesId);
            var treatmentService = await _treatmentServiceRepository
                .FindByIdAsync(treatmentServicesId);
            treatmentService.SuccessRate = feedbackTreatmentService.Where(x => x.Status).Average(f => f.Rating); // Update treatment service's rating based on feedback
            treatmentService.UpdatedAt = DateTime.Now;
            await _treatmentServiceRepository.UpdateAsync(treatmentService);
        }
        private async Task RatingDoctor(Guid doctorId)
        {
            var feedbackDoctor = await _feedbackRepository
                    .FindDoctorByIdAsync(doctorId)
                    ?? throw new NotFoundException($"Feedback with doctorId {doctorId} not found.");

            var doctor = await _doctorRepository.FindByIdAsync(doctorId);
            doctor.Rating = feedbackDoctor.Where(x => x.Status).Average(f => f.Rating); // Update doctor's rating based on feedback
            doctor.UpdatedAt = DateTime.Now;
            await _doctorRepository.UpdateAsync(doctor);
        }
    }
}
