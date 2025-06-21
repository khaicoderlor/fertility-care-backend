using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Patients
{
    public class PatientDTO
    {
        public string? Id { get; set; }

        public ProfileDTO Profile { get; set; }

        public string? MedicalHistory { get; set; }

        public string? Note { get; set; }

        public string? PartnerFullName { get; set; }    

        public string? PartnerEmail { get; set; }

        public string? PartnerPhone { get; set; }

    }
}
