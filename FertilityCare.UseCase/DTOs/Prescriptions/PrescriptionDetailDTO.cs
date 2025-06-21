using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.UseCase.DTOs.PrescriptionItem;

namespace FertilityCare.UseCase.DTOs.Prescriptions
{
    public class PrescriptionDetailDTO
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string PatientId { get; set; }
        public string PatientFullName { get; set; }
        public string DoctorId { get; set; }
        public string TreatmentServiceName { get; set; }
        public string? DoctorFullName { get; set; }
        public string? PrescriptionDate { get; set; }
        public string? Note { get; set; }
        public List<PrescriptionItemDTO> PrescriptionItems { get; set; } = new ();
    }
}
