using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.Share.Exceptions;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.OrderSteps;
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
    public class OrderStepService : IOrderStepService
    {

        private readonly IOrderRepository _orderRepository;

        private readonly IOrderStepRepository _stepRepository;

        public OrderStepService(IOrderStepRepository orderStepRepository, IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _stepRepository = orderStepRepository;
        }

        public async Task<IEnumerable<OrderStepDTO>> GetAllStepsByOrderIdAsync(Guid orderId)
        {
            var order = await _orderRepository.FindByIdAsync(orderId)
                ?? throw new NotFoundException("Order not found!");

            var steps = await _stepRepository.FindAllByOrderIdAsync(orderId);

            return steps.OrderBy(x => x.TreatmentStep.StepOrder).Select(step => step.MapToStepDTO()).ToList();
        }

        public async Task<StepStatusUpdateResultDTO> MarkStatusByStepIdAsync(long stepId, string status)
        {
            var step = await _stepRepository.FindByIdAsync(stepId);

            var allSteps = await _stepRepository.FindAllByOrderIdAsync(step.OrderId);
            OrderStep? beforeStep = null;

            if (step.TreatmentStep.StepOrder > 1)
            {
                beforeStep = allSteps.FirstOrDefault(x => x.TreatmentStep.StepOrder == (step.TreatmentStep.StepOrder - 1));
            }

            if (!Enum.TryParse(status, true, out StepStatus stepStatus))
            {
                throw new ArgumentException($"Invalid status: {status}");
            }

            if(beforeStep != null && beforeStep.Status != StepStatus.Completed)
            {
                throw new PreviousNotCompletedExpception("Cannot mark step as completed before the previous step is completed.");
            }

            if (step.Appointments != null && step.Appointments.Any(x => x.Status != AppointmentStatus.Completed))
            {
                throw new AppointmentNotCompleteException("Cannot mark step as completed while there are pending appointments.");
            }

            if (step.PaymentStatus != PaymentStatus.Paid)
            {
                throw new NotPaidOrderStepException("Not paid order step cannot be marked as completed.");
            }

            step.Status = stepStatus;
            if (stepStatus == StepStatus.Completed)
            {
                step.EndDate = DateOnly.FromDateTime(DateTime.Now);
            }

            await _stepRepository.SaveChangeAsync();

            var finalSteps = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                ["IVF"] = 6,
                ["IUI"] = 4
            };

            if (stepStatus == StepStatus.Completed &&
                finalSteps.TryGetValue(step.Order.TreatmentService.Name, out var finalStep) &&
                step.TreatmentStep.StepOrder == finalStep)
            {
                var order = await _orderRepository.FindByIdAsync(step.OrderId);
                order.Status = OrderStatus.Completed;
                order.EndDate = DateOnly.FromDateTime(DateTime.Now);
                await _orderRepository.SaveChangeAsync();
            }

            if (stepStatus == StepStatus.Completed)
            {
                var currentStepOrder = step.TreatmentStep.StepOrder;
                var nextStep = allSteps.FirstOrDefault(x => x.TreatmentStep.StepOrder == (currentStepOrder + 1));
                if (nextStep != null)
                {
                    nextStep.Status = StepStatus.InProgress;
                    nextStep.StartDate = DateOnly.FromDateTime(DateTime.Now);
                    await _stepRepository.SaveChangeAsync();
                }

                return new StepStatusUpdateResultDTO
                {
                    Step = step.MapToStepDTO(),
                    NextStepStatus = StepStatus.InProgress.ToString()
                };
            }
            else
            {
                return new StepStatusUpdateResultDTO
                {
                    Step = step.MapToStepDTO(),
                    NextStepStatus = ""
                };
            }
        }

    }
}
