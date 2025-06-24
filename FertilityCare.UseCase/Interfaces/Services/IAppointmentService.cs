using Fertilitycare.Share.Comon;
using FertilityCare.UseCase.DTOs.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IAppointmentService
    {

        Task<AppointmentDTO> MarkStatusAppointmentAsync(Guid appointmentId, string status);

        Task<AppointmentDTO> PlaceAppointmentWithStartOrderAsync(CreateAppointmentRequestDTO request);

        Task<IEnumerable<AppointmentDTO>> GetAppointmentsByStepIdAsync(Guid orderId, long stepId);

        Task<AppointmentDTO> PlaceAppointmentByStepIdAsync(Guid stepId, CreateAppointmentDailyRequestDTO request);

        Task<AppointmentDTO> UpdateInfoAppointmentByAppointmentIdAsync(
            Guid appointmentId, UpdateInfoAppointmentRequestDTO request);
        Task<List<AppointmentDTO>> GetPagedAppointmentsAsync(AppointmentQueryDTO query);
        Task PlaceAppointmentToEmbryoTransferAsync(Guid guid, CreateAppointmentEmbryoTransferRequest request);
    }
}
