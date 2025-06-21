using Fertilitycare.Share.Comon;
using Fertilitycare.Share.Pagination;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDTO>> GetAllDoctorsAsync();
        Task<DoctorDTO?> GetDoctorByIdAsync(string id);
        Task<IEnumerable<DoctorDTO>> GetDoctorsPagedAsync(PaginationRequestDTO request);

    }
}
