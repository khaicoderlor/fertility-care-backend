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
            if (!Guid.TryParse(request.UserProfileId, out var userProfileId))
                throw new UnauthorizedAccessException("Invalid or missing user profile ID.");

            if (!Guid.TryParse(request.DoctorId, out var doctorId))
                throw new ArgumentException("Invalid doctor ID.");

            var userProfile = await _profileRepository.FindByIdAsync(userProfileId)
                                ?? throw new UnauthorizedAccessException("User not exist to perform this action!");

            var treatmentService = await _treatmentSRepository.FindByIdAsync(Guid.Parse(request.TreatmentServiceId))
                                     ?? throw new NotFoundException("Treatment service not found!");

            var doctor = await _doctorRepository.FindByIdAsync(doctorId)
                           ?? throw new NotFoundException("Doctor not found!");

            var schedule = await _scheduleRepository.FindByIdAsync(request.DoctorScheduleId)
                             ?? throw new NotFoundException("Schedule not found!");

            var appointmentAmount = await _appointmentRepository.CountAppointmentByScheduleId(schedule.Id);
            if (appointmentAmount > schedule.MaxAppointments)
            {
                throw new AppointmentSlotLimitExceededException(
                    $"The maximum number of appointments for this schedule has been reached. Please choose another time slot or contact support for assistance.");
            }

            Patient savePatient = new Patient
            {
                MedicalHistory = request.MedicalHistory,
                UserProfileId = Guid.Parse(request.UserProfileId ?? ""),
                PartnerFullName = request.PartnerFullName,
                PartnerEmail = request.PartnerEmail,
                PartnerPhone = request.PartnerPhone
            };

            await _patientRepository.SaveAsync(savePatient);

            userProfile.FirstName = request.FirstName;
            userProfile.MiddleName = request.MiddleName;
            userProfile.LastName = request.LastName;
            userProfile.DateOfBirth = request.DateOfBirth;
            userProfile.Gender = request.Gender.Equals(Gender.Female.ToString())
                ? Gender.Female
                : Gender.Male;
            userProfile.Address = request.Address;
            userProfile.UpdatedAt = DateTime.Now;

            await _profileRepository.SaveChangeAsync();

            var now = DateTime.Now;
            Order placeOrder = new()
            {
                PatientId = savePatient.Id,
                DoctorId = doctor.Id,
                TreatmentServiceId = treatmentService.Id,
                Status = OrderStatus.InProgress,
                TotalEgg = 0,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
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

            placeOrder.OrderSteps = orderSteps;
            placeOrder.Doctor = doctor;
            placeOrder.TreatmentService = treatmentService;
            placeOrder.Patient = savePatient;
            await _orderRepository.SaveChangeAsync();

            var firstStep = orderSteps.FirstOrDefault(x => x.Status == StepStatus.InProgress)
                    ?? throw new InvalidOperationException("No first step found for appointment.");


            await _appointmentService.PlaceAppointmentWithStartOrderAsync(new CreateAppointmentRequestDTO
            {
                PatientId = savePatient.Id.ToString(),
                DoctorId = doctor.Id.ToString(),
                DoctorScheduleId = schedule.Id,
                OrderStepId = firstStep.Id,
                TreatmentServiceId = treatmentService.Id.ToString(),
                BookingEmail = request.BookingEmail,
                BookingPhone = request.BookingPhone,
                Note = request.Note
            });

            return placeOrder.MapToOderDTO();
        }

        public async Task<IEnumerable<OrderDTO>> GetOrderByDoctorIdAsync(Guid doctorId)
        {
            var doctor = await _doctorRepository.FindByIdAsync(doctorId);

            Console.WriteLine(doctorId.ToString());

            var orders = await _orderRepository.FindAllByDoctorIdAsync(doctorId);
            return orders.Select(x => x.MapToOderDTO());
        }

        public Task<OrderDTO> GetOrderByIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderDTO>> GetOrderByPatientIdAsync(Guid patientId)
        {
            throw new NotImplementedException();
        }

        public Task<long?> SetTotalEgg(long totalEgg, string orderId)
        {
            throw new NotImplementedException();
        }
    }
}
