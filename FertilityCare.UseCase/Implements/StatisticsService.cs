﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Enums;
using FertilityCare.UseCase.DTOs.Patients;
using FertilityCare.UseCase.DTOs.Statistics;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;

namespace FertilityCare.UseCase.Implements
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IFeedbackRepository _feedbackRepository;

        private readonly IDoctorRepository _doctorRepository;

        public StatisticsService(IAppointmentRepository appointmentRepository, IOrderRepository orderRepository, IFeedbackRepository feedbackRepository, IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _orderRepository = orderRepository;
            _feedbackRepository = feedbackRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<DoctorOverallStatistics> GetDoctorOverallStatisticInDashboardAsync(string doctorId)
        {
            var now = DateTime.Now;

            var doctor = await _doctorRepository.FindByIdAsync(Guid.Parse(doctorId));

            var order = await _orderRepository.FindAllByDoctorIdAsync(Guid.Parse(doctorId));

            var totalPatients = order.Select(x => x.PatientId).Distinct().Count();

            var totalPatientsCurrentMonth = order.Where(x => x.StartDate.Year == now.Year && x.StartDate.Month == now.Month)
                .Select(x => x.PatientId).Distinct().Count();

            var totalPatientsPreviousMonth = order.Where(x => x.StartDate.Year == now.Year && x.StartDate.Month == now.Month - 1)
                .Select(x => x.PatientId).Distinct().Count();

            var appointments = await _appointmentRepository.FindByDoctorIdAsync(Guid.Parse(doctorId));

            var totalAppointmentCurrentMonth = appointments
                .Where(x => x.AppointmentDate.Year == now.Year && x.AppointmentDate.Month == now.Month)
                .Count();

            var totalAppointmentPreviousMonth = appointments.Where(x => x.AppointmentDate.Year == now.Year && x.AppointmentDate.Month == now.Month - 1)
                .Count();

            var totalRate = doctor.Rating;

            var feedbackCur = await _feedbackRepository.FindAllByDoctorIdAndMonthAsync(Guid.Parse(doctorId), now.Month, now.Year);

            var totalRateCurrentMonth = feedbackCur.Any() ? feedbackCur.Average(x => x.Rating) : 0;

            var feedbackPrev = await _feedbackRepository.FindAllByDoctorIdAndMonthAsync(Guid.Parse(doctorId), now.Month - 1, now.Year);

            var totalRatePreviousMonth = feedbackPrev.Any() ? feedbackPrev.Average(x => x.Rating) : 0;

            var comparingPatientsPreviousMonth = totalPatientsPreviousMonth == 0
                ? 0
                : (decimal)(totalPatientsCurrentMonth - totalPatientsPreviousMonth) / totalPatientsPreviousMonth * 100;

            var comparingAppointmentsPreviousMonth = totalAppointmentPreviousMonth == 0
                ? 0
                : (decimal)(totalAppointmentCurrentMonth - totalAppointmentPreviousMonth) / totalAppointmentPreviousMonth * 100;

            var comparingRatePreviousMonth = totalRatePreviousMonth == 0
                ? 0
                : Math.Round((decimal)(totalRateCurrentMonth - totalRatePreviousMonth) / totalRatePreviousMonth * 100, 2);

            return new DoctorOverallStatistics
            {
                TotalPatients = totalPatients,
                TotalAppointments = appointments.Count(),
                TotalPatientsCurrentMonth = totalPatientsCurrentMonth,
                TotalPatientsPreviousMonth = totalPatientsPreviousMonth,
                TotalAppointmentsPreviousMonth = totalAppointmentPreviousMonth,
                TotalRate = totalRate ?? 0,
                TotalRatePreviousMonth = totalRatePreviousMonth,
                ComparingPatientsPreviousMonth = comparingPatientsPreviousMonth,
                ComparingAppointmentsPreviousMonth = comparingAppointmentsPreviousMonth,
                ComparingRatePreviousMonth = comparingRatePreviousMonth
            };
        }

        public async Task<IEnumerable<StatusTreatmentPatientOverall>> GetOrderStatusOverallByDoctorIdAsync(Guid guid)
        {
            var orders = await _orderRepository.FindAllByDoctorIdAsync(guid);

            var a = orders
                .GroupBy(x => x.Status)
                .Select(g => new StatusTreatmentPatientOverall
                {
                    Name = g.Key.ToString(),
                    Value = g.Select(x => x.Id).Distinct().Count()
                })
                .ToList();

            return a.Select(x => new StatusTreatmentPatientOverall
            {
                Name = x.Name,
                Value = x.Value,
                Color = x.Name switch
                {
                    nameof(OrderStatus.Completed) => "#10B981",
                    nameof(OrderStatus.InProgress) => "#3B82F6",
                    nameof(OrderStatus.Cancelled) => "#EF4444",
                    nameof(OrderStatus.Closed) => "#F59E0B",
                    _ => "#6C757D"
                }
            });
        }

        public async Task<IEnumerable<PatientMonthlyCountDTO>> GetPatientCountByYearAsync(Guid doctorId, int year)
        {
            var orders = await _orderRepository.FindAllByDoctorIdAsync(doctorId);
            var patients = orders.Where(x => x.StartDate.Year == year)
                .OrderBy(x => x.StartDate.Month)
                .GroupBy(x => x.StartDate.Month)
                .ToDictionary(x => x.Key, x => x.Select(g => g.PatientId).Distinct().Count());

            var appointments = await _appointmentRepository.FindAllByDoctorIdAsync(doctorId);
            var appointmentsMonthly = appointments
                .Where(a => a.AppointmentDate.Year == year)
                .OrderBy(a => a.AppointmentDate.Month)
                .GroupBy(a => a.AppointmentDate.Month)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Id).Count());

            var result = Enumerable.Range(1, 12)
                .Select(month => new PatientMonthlyCountDTO
                {
                    Month = month,
                    Patients = patients.ContainsKey(month) ? patients[month] : 0,
                    Appointments = appointmentsMonthly.ContainsKey(month) ? appointmentsMonthly[month] : 0
                })
                .ToList();

            return result;
        }
    }
}
