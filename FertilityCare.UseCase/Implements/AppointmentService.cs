﻿using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.Share.Exceptions;
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
            var appointment = await _appointmentRepository.FindByIdAsync(appointmentId);

            //if (!Enum.TryParse<AppointmentStatus>(status, true, out var appointmentStatus))
            //{
            //    throw new ArgumentException("Invalid appointment status provided.");
            //}

            var orderStep = await _stepRepository.FindByIdAsync(appointment.OrderStepId ?? 0);
            if (orderStep is not null && orderStep.Appointments is not null)
            {
                bool s = orderStep.Appointments
                    .Where(x => x.AppointmentDate < appointment.AppointmentDate)
                    .ToList()
                    .All(x => x.Status == AppointmentStatus.Completed);

                if (!s)
                {
                    throw new PreviousNotCompletedExpception("Cannot mark this appointment as completed because there are previous appointments that are not completed yet.");
                }
            }

            appointment.Status = AppointmentStatus.Completed;
            appointment.UpdatedAt = DateTime.Now;
            await _appointmentRepository.SaveChangesAsync();

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
                Note = request.Note,
                ExtraFee = request.ExtraFee,
                PaymentStatus = PaymentStatus.Pending,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            step.TotalAmount += appointment.ExtraFee;
            await _stepRepository.SaveChangeAsync();
            await _appointmentRepository.SaveAsync(appointment);

            appointment.Patient = order.Patient;
            appointment.Doctor = order.Doctor;
            return appointment.MapToAppointmentDTO();
        }

        public async Task PlaceAppointmentToEmbryoTransferAsync(Guid guid, CreateAppointmentEmbryoTransferRequest request)
        {
            var loadedOrder = await _orderRepository.FindByIdAsync(guid);
            var loadedSchedule = await _scheduleRepository.FindByIdAsync(request.DoctorScheduleId);
            var appointmentCount = await _appointmentRepository.CountAppointmentByScheduleId(request.DoctorScheduleId);
            
            if (appointmentCount > loadedSchedule.MaxAppointments)
            {
                throw new AppointmentSlotLimitExceededException("This schedule is fully booked, please choose another one.");
            }

            var appointment = new Appointment
            {
                PatientId = Guid.Parse(request.PatientId),
                DoctorId = Guid.Parse(request.DoctorId),
                DoctorScheduleId = request.DoctorScheduleId,
                TreatmentServiceId = loadedOrder.TreatmentServiceId,
                Type = AppointmentType.Treatment,
                OrderStepId = request.OrderStepId,
                AppointmentDate = loadedSchedule.WorkDate,
                StartTime = loadedSchedule.Slot.StartTime,
                EndTime = loadedSchedule.Slot.EndTime,
                Status = AppointmentStatus.Booked,
                Note = request.Note,
                CancellationReason = "",
                ExtraFee = 0,
                PaymentStatus = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow,
            };

            await _appointmentRepository.SaveAsync(appointment);
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

        public async Task<AppointmentDTO> UpdateInfoAppointmentByAppointmentIdAsync(Guid appointmentId, UpdateInfoAppointmentRequestDTO request)
        {
            throw new NotImplementedException();
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
