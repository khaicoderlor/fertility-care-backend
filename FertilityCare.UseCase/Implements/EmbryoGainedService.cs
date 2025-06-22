using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.UseCase.DTOs.EmbryoGained;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Implements
{
    public class EmbryoGainedService : IEmbryoGainedService
    {
        private readonly IEmbryoGainedRepository _repository;

        public EmbryoGainedService(IEmbryoGainedRepository repository)
        {
            _repository = repository;
        }

        public async Task AddEmbryosAsync(Guid orderId, CreateEmbryoGainedListRequestDTO request)
        {
            if (request == null || request.Embryos == null || !request.Embryos.Any())
                throw new ArgumentException("Danh sách phôi không được để trống");

            var embryos = request.Embryos.Select(e => new EmbryoGained
            {
                EggGainedId = e.EggGainedId,
                Grade = e.Grade,
                IsViable = e.IsViable,
                EmbryoStatus = EmbryoStatus.Available,
                IsFrozen = false,
                IsTransfered = false,
                OrderId = orderId
            }).ToList();

            await _repository.BulkInsertAsync(embryos);
        }
    }

}
