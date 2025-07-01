using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Enums;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.UseCase.Mappers;

namespace FertilityCare.UseCase.Implements
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<PatientDTO>> FindAllAsync()
        {
            var patients = await _patientRepository.FindAllAsync();
            return patients.Select(p => p.MapToPatientDTO());
        }

        public async Task<PatientDTO> FindPatientByPatientIdAsync(string patientId)
        {
            var loadedPatient = await _patientRepository.FindByIdAsync(Guid.Parse(patientId));
            return loadedPatient.MapToPatientDTO();
        }

        public async Task<bool> UpdateInfoPatientByIdAsync(string patientId, UpdatePatientInfoDTO request)
        {
            try
            {
                var patient = await _patientRepository.FindByIdAsync(Guid.Parse(patientId));

                patient.PartnerEmail = request.PartnerEmail;
                patient.PartnerPhone = request.PartnerPhone;
                patient.PartnerFullName = request.PartnerFullName;
                patient.MedicalHistory = request.MedicalHistory;

                patient.UserProfile.FirstName = request.FirstName;
                patient.UserProfile.LastName = request.LastName;
                patient.UserProfile.MiddleName = request.MiddleName;
                patient.UserProfile.Address = request.Address;
                patient.UserProfile.Gender = request.Gender.Equals("Female") ? Gender.Female : Gender.Male;
                patient.UserProfile.DateOfBirth = request.DateOfBirth;

                await _patientRepository.SaveChangeAsync();

                return true;
            } 
            catch(Exception ex) 
            {
                return false;
            }
        }
    }
}
