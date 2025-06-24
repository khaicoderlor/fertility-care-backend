using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.EmbryoGained;
using FertilityCare.UseCase.DTOs.Embryos;
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
        private readonly IOrderRepository _orderRepository;

        public EmbryoGainedService(IEmbryoGainedRepository repository, IOrderRepository orderRepository)
        {
            _repository = repository;
            _orderRepository = orderRepository;
        }

        public async Task AddEmbryosAsync(Guid orderId, CreateEmbryoGainedListRequestDTO request)
        {
            if (request == null || request.Embryos == null || !request.Embryos.Any())
                throw new ArgumentException("Embryos is not empty!");

            var embryos = new List<EmbryoGained>();

            foreach (var e in request.Embryos)
            {
                if (!Enum.TryParse<EmbryoGrade>(e.Grade, out var parsedGrade))
                    throw new ArgumentException($"Embryo grade not suitable: {e.Grade}");

                embryos.Add(new EmbryoGained
                {
                    EggGainedId = e.EggId,
                    Grade = parsedGrade, 
                    IsViable = e.IsQualified,
                    EmbryoStatus = EmbryoStatus.Available,
                    IsFrozen = false,
                    IsTransfered = false,
                    OrderId = orderId
                });
            }

            await _repository.BulkInsertAsync(embryos);
        }


        public async Task<List<EmbryoData>> GetEmbryosByOrderIdAsync(string orderId)
        {
            var order = await _orderRepository.FindByIdAsync(Guid.Parse(orderId))
                ?? throw new NotFoundException($"Order id {orderId} not exist!") ;
            var embryos = await _repository.FindByOrderIdAsync(Guid.Parse(orderId))
                ?? throw new NotFoundException($"Embryo with order id {orderId} not exist!");
            return embryos.Select(e => new EmbryoData
            {
                Id = e.Id.ToString(),
                EmbryoGrade = e.Grade.ToString(),
                Status = e.EmbryoStatus.ToString(),
                IsFrozen = e.IsFrozen,
                EggGrade = e.EggGained?.Grade.ToString() ?? "Unknown"
            }).ToList();

        }

        public async Task<IEnumerable<EmbryoReportResponse>> GetEmbryosReportByOrderIdAsync(Guid orderId)
        {
            var result = await _repository.FindByOrderIdAsync(orderId);

            return result.Select(x => new EmbryoReportResponse
            {
                Id = x.Id,
                OrderId = x.OrderId.ToString(),
                EggId = x.EggGainedId,
                EggGrade = x.EggGained.Grade.ToString(),
                IsViable = x.IsViable,
                IsFrozen = x.IsFrozen,
                IsTransferred = x.IsTransfered,
                EmbryoStatus = x.EmbryoStatus.ToString(),
                EmbyoGrade = x.Grade.ToString()
            });
        }

        public async Task<IEnumerable<EmbryoData>> GetEmbryosUsableByOrderIdAsync(string orderId)
        {
            var result = await GetEmbryosByOrderIdAsync(orderId);

            return result.Where(x => x.Status.Equals(EmbryoStatus.Available.ToString())).ToList();
        }
    }

}
