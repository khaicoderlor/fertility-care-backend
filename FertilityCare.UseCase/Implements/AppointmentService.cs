using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Implements
{
    public class AppointmentService
    {
        private readonly IOrderStepRepository _stepRepository;

        private readonly IDoctorScheduleRepository _scheduleRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IAppointmentRepository _appointmentRepository;

        private readonly IAppointmentReminderRepository _appointmentReminderRepository;

        private readonly IEmailService _emailService;

        public AppointmentService(IAppointmentRepository appointmentRepository,
            IOrderStepRepository orderStepRepository,
            IOrderRepository orderRepository,
            IDoctorScheduleRepository doctorScheduleRepository,
            IAppointmentReminderRepository appointmentReminderRepository,
            IEmailService emailService)
        {
            _appointmentRepository = appointmentRepository;
            _stepRepository = orderStepRepository;
            _orderRepository = orderRepository;
            _scheduleRepository = doctorScheduleRepository;
            _appointmentReminderRepository = appointmentReminderRepository;
            _emailService = emailService;
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



    }
}
