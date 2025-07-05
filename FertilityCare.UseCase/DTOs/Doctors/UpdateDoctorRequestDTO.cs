using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Doctors
{
    public class UpdateDoctorRequestDTO
    {
        public string Degree { get; set; }
        public string Specialization { get; set; }
        public int YearsOfExperience { get; set; }
        public string Biography { get; set; }
            
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
    }
}
