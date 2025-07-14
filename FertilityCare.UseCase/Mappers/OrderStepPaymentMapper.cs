using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.OrderSteps;
using FertilityCare.UseCase.DTOs.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Mappers
{
    public static class OrderStepPaymentMapper
    {

        public static OrderStepPaymentTuple MapToOrderStepPaymentTuple(this OrderStepPayment orderStepPayment)
        {
            var profileDoctor = orderStepPayment.OrderStep.Order.Doctor.UserProfile;

            return new OrderStepPaymentTuple
            {
                TreatmentName = orderStepPayment.OrderStep.Order.TreatmentService.Name,
                DoctorName = $"Dr.{profileDoctor.LastName}",
                OrderStepName = orderStepPayment.OrderStep.TreatmentStep.StepName,
                StepOrder = orderStepPayment.OrderStep.TreatmentStep.StepOrder,
                PaymentCode = orderStepPayment.PaymentCode,
                TotalAmount = orderStepPayment.TotalAmount,
                PaymentMethod = orderStepPayment.PaymentMethod.ToString(),
                PaymentDate = orderStepPayment.PaymentDate.ToString("dd/MM/yyyy"),
                Status = orderStepPayment.Status.ToString(),
                GatewayMessage = orderStepPayment.GatewayMessage
            };
        }

        public static OrderStepPaymentDTO MapToOrderStepPaymentDTO(this OrderStepPayment orderStepPayment)
        {
            return new OrderStepPaymentDTO
            {
                Id = orderStepPayment.Id.ToString(),
                Patient = orderStepPayment.Patient.MapToPatientDTO(),
                TreatmentServiceName = orderStepPayment.OrderStep.Order.TreatmentService.Name,
                OrderStep = orderStepPayment.OrderStep.MapToStepDTO(),
                PaymentCode = orderStepPayment.PaymentCode ?? string.Empty,
                TotalAmount = orderStepPayment.TotalAmount,
                PaymentMethod = orderStepPayment.PaymentMethod.ToString(),
                TransactionCode = orderStepPayment.TransactionCode ?? string.Empty,
                PaymentDate = orderStepPayment.PaymentDate.ToString("dd/MM/yyyy"),
                PaymentStatus = orderStepPayment.Status.ToString(),
                GatewayResponseCode = orderStepPayment.GatewayResponseCode ?? string.Empty,
                GatewayMessage = orderStepPayment.GatewayMessage ?? string.Empty
            };
        }

    }
}
