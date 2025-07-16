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
        private readonly IPrescriptionItemRepository _prescriptionItemRepository;
        public PrescriptionService(IPrescriptionRepository prescriptionRepository, IOrderRepository orderRepository, IUserProfileRepository userProfileRepository, IPrescriptionItemRepository prescriptionItemRepository)
        {
            _prescriptionRepository = prescriptionRepository;
            _orderRepository = orderRepository;
            _userProfileRepository = userProfileRepository;
            _prescriptionItemRepository = prescriptionItemRepository;
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

            var prescriptionItems = new List<PrescriptionItem>();
            foreach (var item in request.PrescriptionItems)
            {
                var pt = new PrescriptionItem
                {
                    PrescriptionId = savedPrescription.Id,
                    Prescription = savedPrescription,
                    MedicationName = item.MedicationName,
                    Quantity = item.Quantity,
                    StartDate = DateOnly.FromDateTime(new DateTime()),
                    EndDate = null,
                    SpecialInstructions = item.SpecialInstructions
                };
                var savedItem = await _prescriptionItemRepository.SaveAsync(pt);
                prescriptionItems.Add(savedItem);
            }
            savedPrescription.PrescriptionItems = prescriptionItems;
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
            return loadedPrescriptions.Select(p => new PrescriptionDetailDTO
            {
                Id = p.Id.ToString(),
                Order = p.Order.MapToOderDTO(),
                PrescriptionDate = p.PrescriptionDate.ToString("dd/MM/yyyy"),
                PrescriptionItems = p.PrescriptionItems?.Select(item => item.MapToPrescriptionItemDTO()).ToList()
            }).ToList();
        }

        public async Task<IEnumerable< PrescriptionDetailDTO>> GetPrescriptionDetailByOrderIdAsync(string orderId)
        {
            var prescription = await _prescriptionRepository.FindPrescriptionsByOrderIdAsync(Guid.Parse(orderId));
            var order = await _orderRepository.FindByIdAsync(Guid.Parse(orderId));
            if (prescription is null)
                throw new NotFoundException("Prescription not found for the given order ID");
            return prescription.Select(p => new PrescriptionDetailDTO
            {
                Id = p.Id.ToString(),
                Order = order.MapToOderDTO(),
                PrescriptionDate = p.PrescriptionDate.ToString("dd/MM/yyyy"),
                PrescriptionItems = p.PrescriptionItems?.Select(item => item.MapToPrescriptionItemDTO()).ToList() ?? new List<PrescriptionItemDTO>(),
            }).ToList();
        }
    }
}
