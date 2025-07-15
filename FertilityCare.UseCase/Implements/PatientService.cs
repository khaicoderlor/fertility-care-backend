using FertilityCare.Domain.Enums;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.DTOs.Orders;
using FertilityCare.UseCase.DTOs.Patients;
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
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IAppointmentRepository _appointmentRepository;

        public PatientService(IPatientRepository patientRepository, IAppointmentRepository appointmentRepository, IOrderRepository orderRepository)
        {
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<PatientDTO>> FindAllAsync()
        {
            var patients = await _patientRepository.FindAllAsync();
            return patients.Select(p => p.MapToPatientDTO());
        }

        public async Task<PatientDTO> FindPatientByPatientIdAsync(string patientId)
        {
            var loadedPatient = await _patientRepository.FindByIdAsync(Guid.Parse(patientId));
            return loadedPatient.MapToPatientDTO();
        }

        public async Task<IEnumerable<AppointmentDataTable>> GetAppointmentsDataByPatientIdAsync(Guid guid)
        {
            var appointments = await _appointmentRepository.FindByPatientIdAsync(guid);

            return appointments.Select(a => new AppointmentDataTable
            {
                Id = a.Id.ToString(),
                AppointmentDate = a.AppointmentDate.ToString("dd/MM/yyyy"),
                AppointmentStatus = a.Status.ToString(),
                TreatmentServiceName = a.OrderStep.Order.TreatmentService.Name,
                DoctorName = $"{a.Doctor.UserProfile.MiddleName} {a.Doctor.UserProfile.LastName}",
                ExtraFee = a.ExtraFee ?? 0,
                EndTime = a.EndTime.ToString("HH:mm") ?? "-",
                StartTime = a.StartTime.ToString("HH:mm") ?? "-",
                Target = a.Type.ToString(),
                Note = a.Note ?? "-",
                Specialization = a.Doctor.Specialization.ToString(),
                TreatmentStepName = a.OrderStep.TreatmentStep.StepName ?? "-",
            }).ToList();
        }

        public async Task<IEnumerable<PatientProgress>> GetPatientProgressSideManager()
        {
            var orders = await _orderRepository.FindAllAsync();
            var ordersSorted = orders.OrderByDescending(o => o.StartDate);

            return ordersSorted.Select(o => new PatientProgress
            {
                Patient = o.Patient.MapToPatientDTO(),
                Doctor = o.Doctor.MapToDoctorDTO(),
                Order = o.MapToOderDTO(),
                ServiceName = o.TreatmentService.Name,
                CurrentStep = o.OrderSteps.FirstOrDefault(x => x.Status == StepStatus.InProgress)?.TreatmentStep.StepOrder ?? 0,
                TotalSteps = o.TreatmentService.TreatmentSteps.Count(),
                StartDate = o.StartDate.ToString("dd/MM/yyyy"),
                EndDate = o.EndDate?.ToString("dd/MM/yyyy"),
                Status = o.Status.ToString()
            }).ToList();
        }

        public async Task<bool> UpdateInfoPatientByIdAsync(string patientId, UpdatePatientInfoDTO request)
        {
            try
            {
                var patient = await _patientRepository.FindByIdAsync(Guid.Parse(patientId));

                patient.PartnerEmail = request.PartnerEmail;
                patient.PartnerPhone = request.PartnerPhone;
                patient.PartnerFullName = request.PartnerFullName;
                patient.MedicalHistory = request.MedicalHistory;

                patient.UserProfile.FirstName = request.FirstName;
                patient.UserProfile.LastName = request.LastName;
                patient.UserProfile.MiddleName = request.MiddleName;
                patient.UserProfile.Address = request.Address;
                patient.UserProfile.Gender = request.Gender.Equals("Female") ? Gender.Female : Gender.Male;
                patient.UserProfile.DateOfBirth = request.DateOfBirth;

                await _patientRepository.SaveChangeAsync();

                return true;
            } 
            catch(Exception ex) 
            {
                return false;
            }
        }
    }
}
