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
                Note = request.Note,
                PrescriptionItems = null,
            };
            var savedPrescription = await _prescriptionRepository.SaveAsync(prescriprion);
            return savedPrescription.MapToPrescriptionDTO();

        }

        
    }
}
