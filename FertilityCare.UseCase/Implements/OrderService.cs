using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.DTOs.Orders;
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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        private readonly IAppointmentRepository _appointmentRepository;

        private readonly IOrderStepRepository _stepRepository;

        private readonly IPatientRepository _patientRepository;

        private readonly IDoctorRepository _doctorRepository;

        private readonly IDoctorScheduleRepository _scheduleRepository;

        private readonly IUserProfileRepository _profileRepository;

        private readonly ITreatmentServiceRepository _treatmentSRepository;

        private readonly IAppointmentService _appointmentService;

        public OrderService(IOrderRepository orderRepository,
            IOrderStepRepository stepRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IDoctorScheduleRepository scheduleRepository,
            IUserProfileRepository userProfileRepository,
            ITreatmentServiceRepository treatmentServiceRepository,
            IAppointmentService appointmentService,
            IAppointmentRepository appointmentRepository)
        {
            _orderRepository = orderRepository;
            _stepRepository = stepRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _scheduleRepository = scheduleRepository;
            _profileRepository = userProfileRepository;
            _treatmentSRepository = treatmentServiceRepository;
            _appointmentService = appointmentService;
            _appointmentRepository = appointmentRepository;
        }

        // None process the scenario of patient is exist before placing order
        public async Task<OrderDTO> PlaceOrderAsync(CreateOrderRequestDTO request)
        {
            var patient = await _patientRepository.FindByIdAsync(Guid.Parse(request.PatientId));
            var treatmentService = await _treatmentSRepository.FindByIdAsync(Guid.Parse(request.TreatmentServiceId));
            var doctor = await _doctorRepository.FindByIdAsync(Guid.Parse(request.DoctorId));
            var schedule = await _scheduleRepository.FindByIdAsync(request.DoctorScheduleId);
            var appointmentAmount = await _appointmentRepository.CountAppointmentByScheduleId(schedule.Id);

            if (appointmentAmount > schedule.MaxAppointments)
            {
                throw new AppointmentSlotLimitExceededException(
                    $"The maximum number of appointments for this schedule has been reached. Please choose another time slot or contact support for assistance.");
            }

            patient = SaveInputInfoOrder(request, patient);

            await _patientRepository.SaveChangeAsync();

            var now = DateTime.Now;
            Order placeOrder = new()
            {
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                TreatmentServiceId = treatmentService.Id,
                Status = OrderStatus.InProgress,
                TotalEgg = 0,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                IsFrozen = false,
                TotalAmount = 0,
                EndDate = null,
                UpdatedAt = now,
            };

            await _orderRepository.SaveAsync(placeOrder);

            var orderSteps = treatmentService.TreatmentSteps?
                    .OrderBy(step => step.StepOrder)
                    .Select(step => new OrderStep
                    {
                        OrderId = placeOrder.Id,
                        TreatmentStepId = step.Id,
                        PaymentStatus = PaymentStatus.Pending,
                        TotalAmount = step.Amount,
                        Status = step.StepOrder == 1 ? StepStatus.InProgress : StepStatus.Planned,
                        StartDate = step.StepOrder == 1 ? DateOnly.FromDateTime(now) : DateOnly.MinValue,
                        EndDate = null,
                    }).ToList() ?? new List<OrderStep>();

            await _stepRepository.SaveAllAsync(orderSteps);

            var firstStep = orderSteps.FirstOrDefault(x => x.Status == StepStatus.InProgress)
                    ?? throw new InvalidOperationException("No first step found for appointment.");

            await _appointmentService.PlaceAppointmentWithStartOrderAsync(new CreateAppointmentRequestDTO
            {
                PatientId = patient.Id.ToString(),
                DoctorId = doctor.Id.ToString(),
                DoctorScheduleId = schedule.Id,
                OrderStepId = firstStep.Id,
                TreatmentServiceId = treatmentService.Id.ToString(),
            });

            placeOrder.OrderSteps = orderSteps;

            return placeOrder.MapToOderDTO();
        }

        public async Task<IEnumerable<OrderDTO>> GetOrderByDoctorIdAsync(Guid doctorId)
        {
            var doctor = await _doctorRepository.FindByIdAsync(doctorId);

            Console.WriteLine(doctorId.ToString());

            var orders = await _orderRepository.FindAllByDoctorIdAsync(doctorId);
            return orders.Select(x => x.MapToOderDTO());
        }

        public async Task<OrderDTO> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _orderRepository.FindByIdAsync(orderId);

            return order.MapToOderDTO();
        }

        public async Task<IEnumerable<OrderInfo>> GetOrdersByPatientIdAsync(Guid patientId)
        {
            var patient = await _patientRepository.FindByIdAsync(patientId);

            var order = await _orderRepository.FindAllByPatientIdAsync(patientId);
            return order.Select(x => x.MapToOrderInfo());
        }

        public async Task<long?> SetTotalEgg(long totalEgg, string orderId)
        {
            var order = await _orderRepository.FindByIdAsync(Guid.Parse(orderId))
                ?? throw new NotFoundException("Order not found!");
            order.TotalEgg = totalEgg;
            order.UpdatedAt = DateTime.Now;
            var orderUpdate = await _orderRepository.UpdateAsync(order);
            return totalEgg;
        }

        private Patient SaveInputInfoOrder(CreateOrderRequestDTO request, Patient patient)
        {
            patient.UserProfile.FirstName = request.FirstName;
            patient.UserProfile.LastName = request.LastName;
            patient.UserProfile.MiddleName = request.MiddleName;
            patient.UserProfile.Address = request.Address;
            patient.UserProfile.Gender = request.Gender.Equals(Gender.Female.ToString())
                ? Gender.Female
                : Gender.Male;
            patient.PartnerPhone = request.PartnerPhone;
            patient.PartnerEmail = request.PartnerEmail;
            patient.PartnerFullName = request.PartnerFullName;
            patient.MedicalHistory = request.MedicalHistory;
            patient.UserProfile.UpdatedAt = DateTime.Now;

            return patient;
        }
    }
}
