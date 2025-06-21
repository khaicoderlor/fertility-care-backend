using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.TreatmentServices;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.UseCase.Mappers;

namespace FertilityCare.UseCase.Implements
{
    public class PublicTreatmentService : IPublicTreatmentService
    {
        private readonly ITreatmentServiceRepository _treatmentServiceRepository;
        public PublicTreatmentService(ITreatmentServiceRepository treatmentServiceRepository)
        {
            _treatmentServiceRepository = treatmentServiceRepository;
        }

        public async Task<IEnumerable<TreatmentServiceDTO>> GetAllAsync()
        {
            var result = await _treatmentServiceRepository.FindAllAsync();
            return result.Select(x => x.MapToTreatmentServiceDTO()).ToList();
        }

    }
}
