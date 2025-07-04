using Fertilitycare.Share.Comon;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDTO>> GetAllDoctorsAsync();
        Task<DoctorDTO?> GetDoctorByIdAsync(string id);
        Task<DoctorDTO> GetDoctorByProfileIdAsync(string id);
        Task<IEnumerable<DoctorDTO>> GetDoctorsPagedAsync(PaginationRequestDTO request);
        Task<IEnumerable<PatientDashboard>> GetPatientsByDoctorIdAsync(Guid id);
        Task<IEnumerable<RecentPatientAppointmentDTO>> FindTop5RecentPatientsAsync(Guid doctorId);
        Task<bool> UpdateDoctorAsync(Guid doctorId, UpdateDoctorRequestDTO request);
        Task ChangeAvatarDoctorByIdAsync(Guid guid, string secureUrl);
    }
}
