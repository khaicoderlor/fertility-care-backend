using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.DTOs.EmbryoTransfers;
using FertilityCare.UseCase.DTOs.OrderSteps;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.UseCase.Mappers;

namespace FertilityCare.UseCase.Implements
{
    public class EmbryoTransferService : IEmbryoTransferService
    {
        private readonly IEmbryoTransferRepository _embryoTransferRepository;
        private readonly IOrderStepRepository _orderStepRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IEmbryoGainedRepository _embryoGainedRepository;
        private readonly IAppointmentService _appointmentService;
        public EmbryoTransferService(IEmbryoTransferRepository embryoTransferRepository,
            IOrderStepRepository orderStepRepository,
            IOrderRepository orderRepository,
            IEmbryoGainedRepository embryoGainedRepository,
            IAppointmentService appointmentService)
        {
            _embryoTransferRepository = embryoTransferRepository;
            _orderStepRepository = orderStepRepository;
            _orderRepository = orderRepository;
            _embryoGainedRepository = embryoGainedRepository;
            _appointmentService = appointmentService;
        }
        public async Task<EmbryoTransferDTO> CreateEmbryoTransferAsync(CreateEmbryoTransferRequestDTO request, bool isFrozen)
        {
            var order = await _orderRepository.FindByIdAsync(Guid.Parse(request.OrderId));
            var orderstep = order.OrderSteps.Where(x => x.TreatmentStep.StepOrder == 5).First()
                ?? throw new NotFoundException($"orderStep is not exist!");
            var embryoGained = await _embryoGainedRepository.FindByIdAsync(request.EmbryoGainedId)
                ?? throw new NotFoundException($"Embryo gained with ID {request.EmbryoGainedId} not found.");
            var embryoList = await _embryoGainedRepository.FindAllAsync();
            embryoList = embryoList.Where(x => x.Id != request.EmbryoGainedId);
            var transferType = TransferType.Fresh;
            if (orderstep.Status.Equals(StepStatus.ReTranfer))
            {
                transferType = TransferType.Frozen;
            }

            if (isFrozen)
            {
                embryoList.ToList().ForEach(x => x.IsFrozen = true);
                order.IsFrozen = true;
            }
            else
            {
                embryoList.ToList().ForEach(x => { x.IsFrozen = false; x.EmbryoStatus = EmbryoStatus.Discarded; });
            }

            var embryoTransfer = new EmbryoTransfer()
            {
                EmbryoGainedId = request.EmbryoGainedId,
                TransferDate = DateTime.Now,
                TransferType = transferType,
                AppointmentId = request.AppointmentId != null ? Guid.Parse(request.AppointmentId) : null,
                OrderId = order.Id,
                UpdatedAt = DateTime.Now
            };

            embryoGained.IsTransfered = true;
            embryoGained.EmbryoStatus = EmbryoStatus.Transferred;
            await _embryoTransferRepository.SaveAsync(embryoTransfer);
            return embryoTransfer.MapToEmbryoTranferDTO();
        }

        public async Task<IEnumerable<EmbryoTransferredReportResponse>> GetEmbryoTransferReportByOrderIdAsync(Guid guid)
        {
            var result = await _embryoTransferRepository.FindAllByOrderIdAsync(guid);

            return result.Select(x => new EmbryoTransferredReportResponse
            {
                Id = x.Id,
                EggId = x.EmbryoGained.EggGainedId,
                EmbryoId = x.EmbryoGainedId,
                OrderId = x.OrderId.ToString(),
                EmbryoGrade = x.EmbryoGained.Grade.ToString(),
                TransferDate = x.TransferDate.ToString("dd/MM/yyyy"),
                TransferType = x.TransferType.ToString(),
            });
        }

        public async Task<OrderStepDTO> ReTransferAsync(string orderId)
        {
            var order = await _orderRepository.FindByIdAsync(Guid.Parse(orderId))
                ?? throw new NotFoundException($"Order with ID {orderId} not found.");
            var embryoGained = await _embryoGainedRepository.FindByOrderIdAsync(Guid.Parse(orderId))
                ?? throw new NotFoundException($"Embryo gained with orderID {orderId} not found.");
            if (order.IsFrozen &&
               embryoGained.Any(x => x.IsFrozen
                                        && x.IsViable
                                        && !x.IsTransfered
                                        && x.EmbryoStatus.Equals(EmbryoStatus.Available)))
            {

                var orderStep = order.OrderSteps.Where(x => x.TreatmentStep.StepOrder == 6).FirstOrDefault()
                    ?? throw new NotFoundException($"Order step for embryo transfer not found in order {order.Id}.");
                orderStep.Status = StepStatus.Planned;
                await _orderStepRepository.SaveAsync(orderStep);
                orderStep = order.OrderSteps.Where(x => x.TreatmentStep.StepOrder == 5).FirstOrDefault()
                    ?? throw new NotFoundException($"Order step for embryo transfer not found in order {order.Id}.");
                orderStep.Status = StepStatus.ReTranfer;

                return orderStep.MapToStepDTO();
            }
            else
            {
                var orderStep = order.OrderSteps.Where(x => x.TreatmentStep.StepOrder == 6).FirstOrDefault()
                    ?? throw new NotFoundException($"Order step for embryo transfer not found in order {order.Id}.");
                orderStep.Status = StepStatus.Completed;
                order.Status = OrderStatus.Completed;
                return orderStep.MapToStepDTO();
            }
        }
    }
}
