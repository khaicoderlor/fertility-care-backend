using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Feedbacks;

namespace FertilityCare.UseCase.Mappers
{
    public static class FeedbackMapper
    {
        public static FeedbackDTO MapToFeedbackDTO(this Feedback feedback)
        {
            return new FeedbackDTO()
            {
                Id = feedback.Id.ToString(),
                PatientId = feedback.PatientId.ToString(),
                PatientName = feedback.Patient?.UserProfile?.FirstName + " " + feedback.Patient?.UserProfile?.MiddleName + " " + feedback.Patient?.UserProfile?.LastName,
                DoctorId = feedback.DoctorId.ToString(),
                DoctorName = feedback.Doctor?.UserProfile?.FirstName + " " + feedback.Doctor?.UserProfile?.MiddleName + " " + feedback.Doctor?.UserProfile?.LastName,
                TreatmentServiceId = feedback.TreatmentServiceId.ToString(),
                TreatmentServiceName = feedback.TreatmentService?.Name,
                Status = feedback.Status,
                Rating = feedback.Rating,
                Comment = feedback.Comment,
                CreatedAt = feedback.CreatedAt,
                UpdatedAt = feedback.UpdatedAt
            };
        }
        public static Feedback MapToFeedback(this FeedbackDTO feedbackDTO)
        {
            return new Feedback()
            {
                Id = Guid.Parse(feedbackDTO.Id),
                PatientId = Guid.Parse(feedbackDTO.PatientId),
                DoctorId = string.IsNullOrEmpty(feedbackDTO.DoctorId) ? Guid.Empty : Guid.Parse(feedbackDTO.DoctorId),
                TreatmentServiceId = string.IsNullOrEmpty(feedbackDTO.TreatmentServiceId) ? Guid.Empty : Guid.Parse(feedbackDTO.TreatmentServiceId),
                Status = feedbackDTO.Status,
                Rating = feedbackDTO.Rating,
                Comment = feedbackDTO.Comment,
                CreatedAt = feedbackDTO.CreatedAt,
                UpdatedAt = feedbackDTO.UpdatedAt
            };
        }
    }
}
