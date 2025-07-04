﻿
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

        public async Task<IEnumerable<RecentPatientAppointmentDTO>> FindTop6RecentPatientsAsync(Guid doctorId)
        {
            var appointments = await _appointmentRepository.FindTop6RecentPatientsAsync(doctorId);

            var result = new List<RecentPatientAppointmentDTO>();
            foreach (var appointment in appointments)
            {
                var profile = appointment.Patient.UserProfile;

                var status = appointment.OrderStep.Order.Status;

                result.Add(new RecentPatientAppointmentDTO
                {
                    Name = $"{profile.MiddleName} {profile.LastName}",
                    Age = profile.DateOfBirth != null ? (DateTime.UtcNow.Year - profile.DateOfBirth.Value.Year).ToString() : "N/A",
                    LastVisit = _appointmentRepository.FindAllByDoctorIdAsync(doctorId).Result.OrderByDescending(x => x.AppointmentDate).First().AppointmentDate.ToString("dd/MM/yyyy"),
                    Status = status.ToString(),
                    TreatmentName = appointment.OrderStep.Order.TreatmentService.Name,
                });
            }

            return result;
        }

        public async Task<IEnumerable<DoctorDTO>> GetAllDoctorsAsync()
        {
            var result = await _doctorRepository.FindAllAsync();
            return result.Select(x => x.MapToDoctorDTO()).ToList();
        }

        public async Task<DoctorDTO?> GetDoctorByIdAsync(string id)
        {
            var result = await _doctorRepository.FindByIdAsync(Guid.Parse(id));
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

            doctor.Degree = request.Degree;
            doctor.Specialization = request.Specialization;
            doctor.YearsOfExperience = request.YearsOfExperience;
            doctor.Biography = request.Biography;
            doctor.UpdatedAt = DateTime.UtcNow;

            var profile = doctor.UserProfile;
            if (profile != null)
            {
                profile.FirstName = request.FirstName;
                profile.MiddleName = request.MiddleName;
                profile.LastName = request.LastName;
                profile.Address = request.Address;
                profile.DateOfBirth = request.DateOfBirth;
                profile.UpdatedAt = DateTime.UtcNow;
                profile.Gender = request.Gender.Equals("Male") ? Gender.Male : request.Gender.Equals("Female") ? Gender.Female : Gender.Unknown;
            }

            await _doctorRepository.SaveChangeAsync();
            return true;
        }
    }
}

