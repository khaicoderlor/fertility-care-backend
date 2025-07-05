using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.DTOs.Patients;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IPatientService 
    {
        Task<IEnumerable<PatientDTO>> FindAllAsync();

        Task<PatientDTO> FindPatientByPatientIdAsync(string patientId);
        Task<IEnumerable<AppointmentDataTable>> GetAppointmentsDataByPatientIdAsync(Guid guid);
        Task<bool> UpdateInfoPatientByIdAsync(string patientId, UpdatePatientInfoDTO request);
    }
}
