
using Fertilitycare.Share.Comon;
using FertilityCare.Domain.Entities;
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

        public DoctorService(IDoctorRepository doctorRepository, IOrderRepository orderRepository, IAppointmentRepository appointmentRepository)
        {
            _doctorRepository = doctorRepository;
            _orderRepository = orderRepository;
            _appointmentRepository = appointmentRepository;
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
    }
}

