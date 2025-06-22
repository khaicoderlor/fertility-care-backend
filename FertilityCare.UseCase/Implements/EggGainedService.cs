using FertilityCare.Domain.Entities;
using FertilityCare.UseCase.DTOs.EggGained;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Implements
{
    public class EggGainedService : IEggGainedService
    {
        private readonly IEggGainedRepository _eggGainedRepository;

        public EggGainedService(IEggGainedRepository eggGainedRepository)
        {
            _eggGainedRepository = eggGainedRepository;
        }

        public async Task<CreateEggResponseDTO> AddEggsAsync(Guid orderId, CreateEggGainedListRequestDTO request)
        {
            if (request?.Eggs == null || !request.Eggs.Any())
                throw new ArgumentException("Danh sách trứng không được rỗng");

            var dateGained = DateOnly.FromDateTime(DateTime.UtcNow);

            var entities = request.Eggs.Select(x => new EggGained
            {
                Grade = x.Grade,
                IsUsable = x.IsUsable,
                DateGained = dateGained,
                OrderId = orderId
            }).ToList();

            await _eggGainedRepository.BulkInsertAsync(entities);

            // Lấy lại danh sách vừa lưu
            var usable = entities
                .Where(e => e.IsUsable)
                .Select(e => new EggGainedDropdownDTO { Id = e.Id, Grade = e.Grade, DateGained = e.DateGained })
                .ToList();

            var unusable = entities
                .Where(e => !e.IsUsable)
                .Select(e => new EggGainedDropdownDTO { Id = e.Id, Grade = e.Grade, DateGained = e.DateGained })
                .ToList();

            return new CreateEggResponseDTO
            {
                UsableEggs = usable,
                UnusableEggs = unusable
            };
        }

        public async Task<IEnumerable<EmbryoDropdownEggDTO>> GetUsableEggsByOrderIdAsync(Guid orderId)
        {
            var eggs = await _eggGainedRepository.GetUsableEggsByOrderIdAsync(orderId);

            return eggs.Select(e => new EmbryoDropdownEggDTO
            {
                Id = e.Id,
                Grade = e.Grade
            }).ToList();
        }
    }

}
