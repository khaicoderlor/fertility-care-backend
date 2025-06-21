using FertilityCare.Domain.Entities;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FertilityCare.Infrastructure.Services
{
    public interface IPatientSecretService
    {
        Task<PatientSecretInfo> GetPatientByUserIdAsync(string userId);

        Task<PatientSecretInfo> GetPatientByProfileIdAsync(string profileId);
    }

    public class PatientSecretService : IPatientSecretService
    {
        private readonly IPatientRepository _patientRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IUserProfileRepository _profileRepository;

        public PatientSecretService(IPatientRepository patientRepository, 
            IUserProfileRepository profileRepository, 
            UserManager<ApplicationUser> userManager,
            IOrderRepository orderRepository)
        {
            _patientRepository = patientRepository;
            _profileRepository = profileRepository;
            _orderRepository = orderRepository;
        }

        public async Task<PatientSecretInfo> GetPatientByProfileIdAsync(string profileId)
        {
            var result = await _patientRepository.FindByProfileIdAsync(Guid.Parse(profileId));
            var orders = await _orderRepository.FindAllByPatientIdAsync(result.Id);

            return new PatientSecretInfo
            {
                PatientId = result.Id.ToString(),
                UserProfileId = result.UserProfile.Id.ToString(),
                OrderIds = orders.First().Id.ToString()
            };
        }

        public async Task<PatientSecretInfo> GetPatientByUserIdAsync(string userId)
        {
            var profile = await _profileRepository.FindByUserIdAsync(userId);
            if (profile is null) 
            {
                throw new NotFoundException("Profile not found!");
            }

            var patient = await _patientRepository.FindByProfileIdAsync(profile.Id);
            if (patient is null)
            {
                throw new NotFoundException("Patient not found");
            }
            
            var orders = await _orderRepository.FindAllByPatientIdAsync(patient.Id);
            return new PatientSecretInfo
            {
                PatientId = patient.Id.ToString(),
                UserProfileId = profile.Id.ToString(),
                OrderIds = orders.First().Id.ToString()
            };
        }
    }
}
