
using Fertilitycare.Share.Comon;
using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.UseCase.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FertilityCare.UseCase.Implements
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IAppointmentRepository _appointmentRepository;

        private readonly IUserProfileRepository _userProfileRepository;

        public DoctorService(IDoctorRepository doctorRepository, IOrderRepository orderRepository, IAppointmentRepository appointmentRepository, IUserProfileRepository userProfileRepository)
        {
            _doctorRepository = doctorRepository;
            _orderRepository = orderRepository;
            _appointmentRepository = appointmentRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task ChangeAvatarDoctorByIdAsync(Guid guid, string secureUrl)
        {
            var doctor = await _doctorRepository.FindByIdAsync(guid);
            doctor.UserProfile.AvatarUrl = secureUrl;
            doctor.UserProfile.UpdatedAt = DateTime.UtcNow;
            await _doctorRepository.SaveChangeAsync();
        }

        public async Task<IEnumerable<RecentPatientAppointmentDTO>> FindTop5RecentPatientsAsync(Guid doctorId)
        {
            return await _appointmentRepository.FindTop5RecentPatientsAsync(doctorId);
        }

        public async Task<IEnumerable<DoctorDTO>> GetAllDoctorsAsync()
        {
            var result = await _doctorRepository.FindAllAsync();
            return result.Select(x => x.MapToDoctorDTO()).ToList();
        }

        public async Task<DoctorDTO?> GetDoctorByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid doctorId))
                return null;

            var result = await _doctorRepository.FindByIdAsync(doctorId);
            return result?.MapToDoctorDTO();
        }

        public async Task<DoctorDTO> GetDoctorByProfileIdAsync(string id)
        {
            var doctor = await _doctorRepository.FindByProfileIdAsync(id);
            return doctor.MapToDoctorDTO();
        }

        public async Task<IEnumerable<DoctorDTO>> GetDoctorsPagedAsync(PaginationRequestDTO request)
        {
            var result = await _doctorRepository.GetPagedAsync(request.Page, request.PageSize);

            return result.Select(x => x.MapToDoctorDTO()).ToList();

        }

        public async Task<IEnumerable<PatientDashboard>> GetPatientsByDoctorIdAsync(Guid id)
        {
            var order = await _orderRepository.FindAllByDoctorIdAsync(id);

            return order.ToList().Select(x => new PatientDashboard
            {
                PatientId = x.PatientId.ToString(),
                PatientName = $"{x.Patient.UserProfile.FirstName} {x.Patient.UserProfile.MiddleName} {x.Patient.UserProfile.LastName}",
                TreatmentName = x.TreatmentService.Name,
                Email = "",
                Phone = "",
                OrderId = x.Id.ToString(),
                StartDate = x.StartDate.ToString("dd/MM/yyyy"),
                EndDate = x.EndDate != null ? x.EndDate?.ToString("dd/MM/yyyy") : "",
                Status = x.Status.ToString(),
                TotalEggs = x.TotalEgg,
                TotalEmbryos = x.EmbryoGaineds?.Count > 0 ? x.EmbryoGaineds.Count : 0,
                IsFrozen = x.IsFrozen
            });
        }

        public async Task<bool> UpdateDoctorAsync(Guid doctorId, UpdateDoctorRequestDTO request)
        {
            var doctor = await _doctorRepository.FindByIdAsync(doctorId);
            if (doctor == null) return false;

            // Update Doctor fields
            doctor.Degree = request.Degree;
            doctor.Specialization = request.Specialization;
            doctor.YearsOfExperience = request.YearsOfExperience;
            doctor.Biography = request.Biography;
            doctor.UpdatedAt = DateTime.UtcNow;

            // Update UserProfile
            var profile = doctor.UserProfile;
            if (profile != null)
            {
                profile.FirstName = request.FirstName;
                profile.MiddleName = request.MiddleName;
                profile.LastName = request.LastName;
                profile.Address = request.Address;

                if (Enum.TryParse<Gender>(request.Gender, true, out var gender))
                    profile.Gender = gender;

                if (DateOnly.TryParse(request.DateOfBirth, out var dob))
                    profile.DateOfBirth = dob;

                profile.UpdatedAt = DateTime.UtcNow;

                await _userProfileRepository.UpdateAsync(profile);
            }

            await _doctorRepository.UpdateAsync(doctor);

            return true;
        }
    }
}

