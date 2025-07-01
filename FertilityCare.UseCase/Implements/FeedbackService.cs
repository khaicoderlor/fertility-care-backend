using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var feedbackDTO = new FeedbackDTO()
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                TreatmentServiceId = request.TreatmentServiceId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.Now,
                Status = true // Default status is true
            };
            await _feedbackRepository.SaveAsync(feedbackDTO.MapToFeedback());
 
            if(!string.IsNullOrWhiteSpace(feedbackDTO.DoctorId) && !string.IsNullOrWhiteSpace(feedbackDTO.TreatmentServiceId))
            {
                var feedbackDoctor = await _feedbackRepository
                    .FindDoctorByIdAsync(Guid.Parse(feedbackDTO.DoctorId))
                    ?? throw new NotFoundException($"Feedback with doctorId {feedbackDTO.DoctorId} not found.");
                
                var feedbackTreatmentService = await _feedbackRepository
                    .FindTreatmentServiceByIdAsync(Guid.Parse(feedbackDTO.TreatmentServiceId))
                    ?? throw new NotFoundException($"Feedback with treatmentServiceId {feedbackDTO.TreatmentServiceId} not found.");

                var doctor = await _doctorRepository.FindByIdAsync(Guid.Parse(feedbackDTO.DoctorId));
                var treatmentService = await _treatmentServiceRepository
                    .FindByIdAsync(Guid.Parse(feedbackDTO.TreatmentServiceId));
                doctor.Rating = feedbackDoctor.Average(f => f.Rating);
                treatmentService.SuccessRate = feedbackTreatmentService.Average(f => f.Rating); // Update treatment service's rating based on feedback
                treatmentService.UpdatedAt = DateTime.Now;
                await _treatmentServiceRepository.UpdateAsync(treatmentService);
                await _doctorRepository.UpdateAsync(doctor);
            }
            else if (!string.IsNullOrWhiteSpace(feedbackDTO.DoctorId) && string.IsNullOrWhiteSpace(feedbackDTO.TreatmentServiceId))
            {
                var feedbackDoctor = await _feedbackRepository
                    .FindDoctorByIdAsync(Guid.Parse(feedbackDTO.DoctorId))
                    ?? throw new NotFoundException($"Feedback with doctorId {feedbackDTO.DoctorId} not found.");

                var doctor = await _doctorRepository.FindByIdAsync(Guid.Parse(feedbackDTO.DoctorId));
                doctor.Rating = feedbackDoctor.Average(f => f.Rating); // Update doctor's rating based on feedback
                doctor.UpdatedAt = DateTime.Now;
                await _doctorRepository.UpdateAsync(doctor);
            }
            else if (!string.IsNullOrWhiteSpace(feedbackDTO.TreatmentServiceId) && string.IsNullOrWhiteSpace(feedbackDTO.DoctorId))
            {
                var feedbackTreatmentService = await _feedbackRepository
                    .FindTreatmentServiceByIdAsync(Guid.Parse(feedbackDTO.TreatmentServiceId))
                    ?? throw new NotFoundException($"Feedback with treatmentServiceId {feedbackDTO.TreatmentServiceId} not found.");

                var treatmentService = await _treatmentServiceRepository
                    .FindByIdAsync(Guid.Parse(feedbackDTO.TreatmentServiceId));
                treatmentService.SuccessRate = feedbackTreatmentService.Average(f => f.Rating); // Update treatment service's rating based on feedback
                treatmentService.UpdatedAt = DateTime.Now;
                await _treatmentServiceRepository.UpdateAsync(treatmentService);
            }
            return feedbackDTO;
        }

    }
}
