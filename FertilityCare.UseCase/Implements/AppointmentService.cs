using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Appointments;
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
    public class AppointmentService : IAppointmentService
    {
        private readonly IOrderStepRepository _stepRepository;

        private readonly IDoctorScheduleRepository _scheduleRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository,
            IOrderStepRepository orderStepRepository,
            IOrderRepository orderRepository,
            IDoctorScheduleRepository doctorScheduleRepository
           )
        {
            _appointmentRepository = appointmentRepository;
            _stepRepository = orderStepRepository;
            _orderRepository = orderRepository;
            _scheduleRepository = doctorScheduleRepository;
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAppointmentsByStepIdAsync(Guid orderId, long stepId)
        {
            var order = await _orderRepository.FindByIdAsync(orderId)
               ?? throw new NotFoundException("Order not found!");

            var step = await _stepRepository.FindByIdAsync(stepId)
                ?? throw new NotFoundException("Order step not found!");

            var appointments = await _appointmentRepository.FindAllByStepIdAsync(stepId);
            return appointments.Select(a => a.MapToAppointmentDTO()).ToList();
        }

        public async Task<List<AppointmentDTO>> GetPagedAppointmentsAsync(AppointmentQueryDTO query)
        {
            query.PageSize = 8;
            var result = await _appointmentRepository.GetPageAsync(query);
            return result.Select(x => x.MapToAppointmentDTO()).ToList();
        }

        public async Task<AppointmentDTO> MarkStatusAppointmentAsync(Guid appointmentId, string status)
        {
            var appointment = await _appointmentRepository.FindByIdAsync(appointmentId)
                ?? throw new NotFoundException("Appointment not found!");

            if (!Enum.TryParse<AppointmentStatus>(status, true, out var appointmentStatus))
            {
                throw new ArgumentException("Invalid appointment status provided.");
            }

            appointment.Status = appointmentStatus;
            appointment.UpdatedAt = DateTime.Now;
            await _appointmentRepository.UpdateAsync(appointment);

            return appointment.MapToAppointmentDTO();
        }

        public async Task<AppointmentDTO> PlaceAppointmentByStepIdAsync(Guid orderId, CreateAppointmentDailyRequestDTO request)
        {
            var order = await _orderRepository.FindByIdAsync(orderId);

            var step = await _stepRepository.FindByIdAsync(request.OrderStepId);

            var schedule = await _scheduleRepository.FindByIdAsync(request.DoctorScheduleId);

            var appointmentCount = await _appointmentRepository.CountAppointmentByScheduleId(schedule.Id);
            if (appointmentCount > schedule.MaxAppointments)
            {
                throw new AppointmentSlotLimitExceededException("This schedule is fully booked, please choose another one.");
            }

            Appointment appointment = new Appointment
            {
                PatientId = Guid.Parse(request.PatientId),
                DoctorId = Guid.Parse(request.DoctorId),
                DoctorScheduleId = request.DoctorScheduleId,
                TreatmentServiceId = order.TreatmentServiceId,
                OrderStepId = request.OrderStepId,
                AppointmentDate = schedule.WorkDate,
                StartTime = schedule.Slot.StartTime,
                EndTime = schedule.Slot.EndTime,
                Status = AppointmentStatus.Booked,
                Type = DetermineAppointmentType(request.Type),
                CancellationReason = "",
                Note = "",
                ExtraFee = request.Extrafee,
                PaymentStatus = PaymentStatus.Pending,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            step.TotalAmount += appointment.ExtraFee;

            await _appointmentRepository.SaveAsync(appointment);

            await _stepRepository.SaveChangeAsync();
            return appointment.MapToAppointmentDTO();
        }

        // none process the scenario of content email html css
        public async Task<AppointmentDTO> PlaceAppointmentWithStartOrderAsync(CreateAppointmentRequestDTO request)
        {
            var step = await _stepRepository.FindByIdAsync(request.OrderStepId);
            var schedule = await _scheduleRepository.FindByIdAsync(request.DoctorScheduleId);

            Appointment appointment = new Appointment
            {
                PatientId = Guid.Parse(request.PatientId),
                DoctorId = Guid.Parse(request.DoctorId),
                DoctorScheduleId = request.DoctorScheduleId,
                TreatmentServiceId = Guid.Parse(request.TreatmentServiceId),
                OrderStepId = request.OrderStepId,
                AppointmentDate = schedule.WorkDate,
                StartTime = schedule.Slot.StartTime,
                EndTime = schedule.Slot.EndTime,
                Status = AppointmentStatus.Booked,
                Type = AppointmentType.InitialConsultation,
                CancellationReason = "",
                Note = "",
                ExtraFee = 0,
                PaymentStatus = PaymentStatus.Pending,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await _appointmentRepository.SaveAsync(appointment);

            var profilePatient = appointment.Patient.UserProfile;
            var profileDoctor = appointment.Doctor.UserProfile;
            var patientFullName = $"{profilePatient.FirstName} {profilePatient.MiddleName} {profilePatient.LastName}";
            var doctorFullName = $"{profileDoctor.FirstName} {profileDoctor.MiddleName} {profileDoctor.LastName}";

            return appointment.MapToAppointmentDTO();
        }

        private AppointmentType DetermineAppointmentType(string request)
        {
            switch (request)
            {
                case "InitialConsultation":
                    return AppointmentType.InitialConsultation;
                case "Check":
                    return AppointmentType.Check;
                case "FollowUp":
                    return AppointmentType.FollowUp;
                case "Treatment":
                    return AppointmentType.Treatment;
                default:
                    return AppointmentType.Other;
            }

        }
    }
}
