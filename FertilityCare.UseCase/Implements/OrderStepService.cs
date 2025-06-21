using FertilityCare.Domain.Enums;
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

        public async Task<(OrderStepDTO, string)> MarkStatusByStepIdAsync(long stepId, string status)
        {
            var step = await _stepRepository.FindByIdAsync(stepId);

            if (!Enum.TryParse(status, true, out StepStatus stepStatus))
            {
                throw new ArgumentException($"Invalid status: {status}");
            }

            step.Status = stepStatus;
            await _stepRepository.UpdateAsync(step);

            if (stepStatus == StepStatus.Completed)
            {
                var orderId = step.OrderId;
                var steps = await _stepRepository.FindAllByOrderIdAsync(orderId);
                var currentStepOrder = step.TreatmentStep.StepOrder;
                steps.Where(x => x.TreatmentStep.StepOrder == (currentStepOrder + 1)).First().Status = StepStatus.InProgress;
                await _stepRepository.SaveChangeAsync();

                return new(step.MapToStepDTO(), StepStatus.InProgress.ToString());
            }
            else
            {
                return new(step.MapToStepDTO(), "");
            }

        }
    }
}
