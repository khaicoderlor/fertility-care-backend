using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.PrescriptionItem;
using FertilityCare.UseCase.DTOs.Prescriptions;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.UseCase.Mappers;

namespace FertilityCare.UseCase.Implements
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        public PrescriptionService(IPrescriptionRepository prescriptionRepository, IOrderRepository orderRepository, IUserProfileRepository userProfileRepository)
        {
            _prescriptionRepository = prescriptionRepository;
            _orderRepository = orderRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<PrescriptionDTO> AddPrescriptionItemToPrescriptionAsync(PrescriptionItemDTO prescriptionItem, string prescriptionId)
        {
            var loadedPrescription = await _prescriptionRepository.FindByIdAsync(Guid.Parse(prescriptionId));
            var existingItem = loadedPrescription.PrescriptionItems
       .FirstOrDefault(x => x.Id == prescriptionItem.Id);
            var item = prescriptionItem.MapToPrescriptionItem();
            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {

                loadedPrescription.PrescriptionItems.Add(item);
            }
            return item.Prescription.MapToPrescriptionDTO();

        }

        public async Task<PrescriptionDTO> CreatePrescriptionAsync(CreatePrecriptionRequestDTO request)
        {
            var loadedOrder = await _orderRepository.FindByIdAsync(Guid.Parse(request.OrderId));
            if (loadedOrder == null)
                throw new NotFoundException("Order not found");
            var prescriprion = new Prescription()
            {
                OrderId = loadedOrder.Id,
                PrescriptionDate = DateTime.Now,
                PrescriptionItems = null,
            };
            var savedPrescription = await _prescriptionRepository.SaveAsync(prescriprion);
            return savedPrescription.MapToPrescriptionDTO();

        }

        public async Task<IEnumerable<PrescriptionDTO>> FindPrescriptionByOrderIdAsync(string orderId)
        {
            var prescription = await _prescriptionRepository.FindPrescriptionsByOrderIdAsync(Guid.Parse(orderId));
            if (prescription is null)
                throw new NotFoundException("Prescription not found for the given order ID");
            return prescription.Select(p => p.MapToPrescriptionDTO()).ToList();
        }

        public async Task<IEnumerable<PrescriptionDetailDTO>> GetPrescriptionByPatientId(string patientId)
        {
            var loadedPrescriptions = await _prescriptionRepository.FindPrescriptionsByPatientIdAsync(Guid.Parse(patientId));
            if (loadedPrescriptions is null)
                throw new NotFoundException("No prescriptions found for the given patient ID");
            var patientProfile = await _userProfileRepository.FindByIdAsync(Guid.Parse(patientId));
            var doctorProfile = await _userProfileRepository.FindByIdAsync(loadedPrescriptions.FirstOrDefault()?.Order.DoctorId ?? Guid.Empty);
            return loadedPrescriptions.Select(p => new PrescriptionDetailDTO
            {
                Id = p.Id.ToString(),
                OrderId = p.OrderId.ToString(),
                PatientId = p.Order.PatientId.ToString(),
                PatientFullName = $"{patientProfile.FirstName} {patientProfile.MiddleName} {patientProfile.LastName}",
                DoctorId = p.Order.DoctorId.ToString(),
                DoctorFullName = $"{doctorProfile.FirstName} {doctorProfile.MiddleName} {doctorProfile.LastName}",
                TreatmentServiceName = p.Order.TreatmentService?.Name ?? "N/A",
                PrescriptionDate = p.PrescriptionDate.ToString("yyyy-MM-dd"),
                PrescriptionItems = p.PrescriptionItems?.Select(item => item.MapToPrescriptionItemDTO()).ToList() ?? new List<PrescriptionItemDTO>(),
            }).ToList();
        }

        public async Task<IEnumerable< PrescriptionDetailDTO>> GetPrescriptionDetailByOrderIdAsync(string orderId)
        {
            var prescription = await _prescriptionRepository.FindPrescriptionsByOrderIdAsync(Guid.Parse(orderId));
            var order = await _orderRepository.FindByIdAsync(Guid.Parse(orderId));
            if (prescription is null)
                throw new NotFoundException("Prescription not found for the given order ID");
            var patientProfile = await _userProfileRepository.FindByIdAsync(order.PatientId);
            var doctorProfile = await _userProfileRepository.FindByIdAsync(order.DoctorId);
            return prescription.Select(p => new PrescriptionDetailDTO
            {
                Id = p.Id.ToString(),
                OrderId = p.OrderId.ToString(),
                PatientId = p.Order.PatientId.ToString(),
                PatientFullName = $"{patientProfile.FirstName} {patientProfile.MiddleName} {patientProfile.LastName}",
                DoctorId = p.Order.DoctorId.ToString(),
                DoctorFullName = $"{doctorProfile.FirstName} {doctorProfile.MiddleName} {doctorProfile.LastName}",
                TreatmentServiceName = p.Order.TreatmentService?.Name ?? "N/A",
                PrescriptionDate = p.PrescriptionDate.ToString("yyyy-MM-dd"),
                PrescriptionItems = p.PrescriptionItems?.Select(item => item.MapToPrescriptionItemDTO()).ToList() ?? new List<PrescriptionItemDTO>(),
            }).ToList();
        }
    }
}
