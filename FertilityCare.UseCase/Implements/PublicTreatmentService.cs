using FertilityCare.Domain.Entities;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.TreatmentServices;
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

        public async Task<TreatmentServiceDTO> UpdateAsync(TreatmentServiceDTO treatmentServiceDTO)
        {
            var result = await _treatmentServiceRepository.UpdateAsync(treatmentServiceDTO.MapToTreatmentService());
            return result.MapToTreatmentServiceDTO();
        }

        public async Task<TreatmentStep> UpdateStepAsync(string id, TreatmentStepUpdateDTO dto)
        {
            var stepId = long.Parse(id);
            var treatmentStep = await _treatmentServiceRepository.FindStepByIdAsync(stepId)
                ?? throw new NotFoundException($"TreatmentStep with ID {id} not found!");

            treatmentStep.StepName = dto.StepName;
            treatmentStep.Description = dto.Description;
            treatmentStep.StepOrder = dto.StepOrder;
            treatmentStep.EstimatedDurationDays = dto.EstimatedDurationDays;
            treatmentStep.Amount = dto.Amount;
            treatmentStep.UpdatedAt = DateTime.Now;

            await _treatmentServiceRepository.SaveStepAsync(treatmentStep);
            return treatmentStep;
        }


    }
}
