using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.DTOs.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Mappers
{
    public static class PatientMapper
    {

        public static PatientDTO MapToPatientDTO(this Patient patient)
        {
            return new PatientDTO
            {
                Id = patient.Id.ToString(),
                MedicalHistory = patient.MedicalHistory,
                PartnerFullName = patient.PartnerFullName,
                PartnerEmail = patient.PartnerEmail,
                PartnerPhone = patient.PartnerPhone,
                Profile = patient.UserProfile.MapToProfileDTO()
            };
        }
    }
}