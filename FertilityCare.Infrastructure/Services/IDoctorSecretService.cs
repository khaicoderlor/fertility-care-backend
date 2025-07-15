using FertilityCare.Infrastructure.Identity;
using FertilityCare.Infrastructure.Repositories;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Feedbacks;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.DTOs.TreatmentServices;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Mappers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Services
{
    public interface IDoctorSecretService
    {

        Task<IEnumerable<DoctorSideAdminPage>> GetDoctorSideAdminPages();

        Task<IEnumerable<FeedbackSideDoctor>> GetFeedbacksOfDoctorSide(Guid doctorId);

    }

    public class DoctorSecretService : IDoctorSecretService
    {
        private readonly IDoctorRepository _doctorRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IFeedbackRepository _feedbackRepository;

        private readonly IOrderRepository _orderRepository;
        public DoctorSecretService(IDoctorRepository doctorRepository, IOrderRepository orderRepository, UserManager<ApplicationUser> userManager, IFeedbackRepository feedbackRepository)
        {
            _doctorRepository = doctorRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _feedbackRepository = feedbackRepository;
        }
        public async Task<IEnumerable<DoctorSideAdminPage>> GetDoctorSideAdminPages()
        {
            var doctors = await _doctorRepository.FindAllAsync();
            var doctorSideAdminPages = new List<DoctorSideAdminPage>();
            foreach (var doctor in doctors)
            {
                var user = await _userManager.FindByProfileIdAsync(doctor.UserProfileId);
                var orders = await _orderRepository.FindAllByDoctorIdAsync(doctor.Id);
                doctorSideAdminPages.Add(new DoctorSideAdminPage
                {
                    DoctorEmail = user.Email,
                    DoctorPhone = user.PhoneNumber,
                    Doctor = doctor.MapToDoctorDTO(),
                    Orders = orders.Select(o => o.MapToOderDTO())
                });
            }

            return doctorSideAdminPages;
        }

        public async Task<IEnumerable<FeedbackSideDoctor>> GetFeedbacksOfDoctorSide(Guid doctorId)
        {
            var feedbacks = await _feedbackRepository.FindDoctorByIdAsync(doctorId);
            var res = new List<FeedbackSideDoctor>();
            foreach (var feedback in feedbacks)
            {
                var user = await _userManager.FindByProfileIdAsync(feedback.Patient.UserProfileId);
                res.Add(new FeedbackSideDoctor
                {
                    Id = feedback.Id.ToString(),
                    Comment = feedback.Comment,
                    Rating = feedback.Rating,
                    Status = feedback.Status,
                    CreatedAt = feedback.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss"),
                    UpdatedAt = feedback.UpdatedAt?.ToString("dd/MM/yyyy hh:mm:ss"),
                    PatientEmail = user.Email,
                    PatientPhone = user.PhoneNumber,
                    TreatmentService = feedback.TreatmentService?.MapToTreatmentServiceDTO(),
                    Patient = feedback.Patient.MapToPatientDTO(),
                    Doctor = feedback.Doctor?.MapToDoctorDTO()
                });
            }

            return res;
        }
    }
}
