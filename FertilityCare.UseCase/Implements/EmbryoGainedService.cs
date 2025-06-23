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

        public async Task<List<EmbryoData>> GetEmbryosByOrderIdAsync(string orderId)
        {
            var order = await _orderRepository.FindByIdAsync(Guid.Parse(orderId))
                ?? throw new NotFoundException($"Order id {orderId} not exist!") ;
            var embryos = await _repository.FindByOrderIdAsync(Guid.Parse(orderId))
                ?? throw new NotFoundException($"Embryo with order id {orderId} not exist!");
            return embryos.Select(e => new EmbryoData
            {
                EmbryoId = e.Id.ToString(),
                EmbryoGrade = e.Grade.ToString(),
                Status = e.EmbryoStatus.ToString(),
                IsFrozen = e.IsFrozen,
                EggGrade = e.EggGained?.Grade.ToString() ?? "Unknown"
            }).ToList();

        }
    }

}
