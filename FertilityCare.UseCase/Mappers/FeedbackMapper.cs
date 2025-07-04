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
                Patient = feedback.Patient.MapToPatientDTO(),
                Doctor = feedback.Doctor?.MapToDoctorDTO(),
                TreatmentService = feedback.TreatmentService?.MapToTreatmentServiceDTO(),
                Status = feedback.Status,
                Rating = feedback.Rating,
                Comment = feedback.Comment,
                CreatedAt = feedback.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                UpdatedAt = feedback.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }
        
    }
}
