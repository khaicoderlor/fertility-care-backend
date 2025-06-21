using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Patients
{
    public class PatientSecretInfo
    {

        public string PatientId { get; set; } = string.Empty;

        public string UserProfileId { get; set; } = string.Empty;

        public List<string> OrderIds { get; set; } = new List<string>();

    }
}
