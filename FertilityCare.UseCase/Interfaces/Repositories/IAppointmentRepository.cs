using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.DTOs.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Repositories
{
    public interface IAppointmentRepository : IBaseRepository<Appointment, Guid>
    {

        Task<int> CountAppointmentByScheduleId(long scheduleId);

        Task<IEnumerable<Appointment>> FindAllByStepIdAsync(long stepId);

        Task<List<Appointment>> GetPageAsync(AppointmentQueryDTO query);

        Task SaveChangesAsync();

        Task<IEnumerable<RecentPatientAppointmentDTO>> FindTop5RecentPatientsAsync(Guid doctorId);

        Task<IEnumerable<Appointment>> FindByDoctorIdAsync(Guid doctorId);

        Task<IEnumerable<Appointment>> FindAllByDoctorIdAsync(Guid doctorId);
    }
}
