using FertilityCare.Domain.Entities;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.Infrastructure.Repositories;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Services
{
    public interface IPatientSecretService
    {
        Task<PatientSecretInfo> GetPatientByUserIdAsync(string userId);

        Task<PatientSecretInfo> GetPatientByProfileIdAsync(string profileId);

        Task<PatientInfoContactDTO> GetPatientInfoContactByPatientIdAsync(string patientId);
        Task UpdateAvatarAsync(string patientId, string file);
    }

    public class PatientSecretService : IPatientSecretService
    {
        private readonly IPatientRepository _patientRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IUserProfileRepository _profileRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public PatientSecretService(IPatientRepository patientRepository, 
            IUserProfileRepository profileRepository, 
            UserManager<ApplicationUser> userManager,
            IOrderRepository orderRepository)
        {
            _patientRepository = patientRepository;
            _profileRepository = profileRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        public async Task<PatientSecretInfo> GetPatientByProfileIdAsync(string profileId)
        {
            var result = await _patientRepository.FindByProfileIdAsync(Guid.Parse(profileId));
            var orders = await _orderRepository.FindAllByPatientIdAsync(result.Id);

            return new PatientSecretInfo
            {
                PatientId = result.Id.ToString(),
                UserProfileId = result.UserProfile.Id.ToString(),
                OrderIds = new List<string> { (orders.First().Id.ToString()) }
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
                OrderIds = new List<string> { (orders.First().Id.ToString()) }
            };
        }

        public async Task<PatientInfoContactDTO> GetPatientInfoContactByPatientIdAsync(string patientId)
        {
            var loadedPatient = await _patientRepository.FindByIdAsync(Guid.Parse(patientId));

            var loadedUser = await _userManager.FindByProfileIdAsync(loadedPatient.UserProfileId);

            return new PatientInfoContactDTO
            {
                Email = loadedUser.Email,
                PhoneNumber = loadedUser.PhoneNumber
            };
        }

        public async Task UpdateAvatarAsync(string patientId, string secureUrl)
        {
            var patient = await _patientRepository.FindByIdAsync(Guid.Parse(patientId));
            patient.UserProfile.AvatarUrl = secureUrl;
            await _patientRepository.SaveChangeAsync();
        }
    }
}
