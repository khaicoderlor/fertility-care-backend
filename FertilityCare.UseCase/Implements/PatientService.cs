using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
