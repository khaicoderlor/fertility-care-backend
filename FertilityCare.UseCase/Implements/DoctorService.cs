
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

        public DoctorService(IDoctorRepository doctorRepository, IOrderRepository orderRepository)
        {
            _doctorRepository = doctorRepository;
            _orderRepository = orderRepository;
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

        public async Task<IEnumerable<PatientInfoTable>> GetPatientsByDoctorIdAsync(Guid id)
        {
            var order = await _orderRepository.FindAllByDoctorIdAsync(id);

            return order.ToList().Select(x => new PatientInfoTable
            {
                PatientId = x.PatientId.ToString(),
                PatientName = $"{x.Patient.UserProfile.FirstName} {x.Patient.UserProfile.MiddleName} {x.Patient.UserProfile.LastName}",
                OrderId = x.Id.ToString(),
                TotalEggs = x.TotalEgg,
                StartDate = x.StartDate.ToString("dd/MM/yyyy"),
                EndDate = x.EndDate != null ? x.EndDate?.ToString("dd/MM/yyyy") : "",
                Status = x.Status.ToString(),
            });
        }
    }
}

