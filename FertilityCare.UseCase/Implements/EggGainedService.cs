using FertilityCare.Domain.Entities;
using FertilityCare.Domain.Enums;
using FertilityCare.UseCase.DTOs.EggGained;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.Implements
{
    public class EggGainedService : IEggGainedService
    {
        private readonly IEggGainedRepository _eggGainedRepository;

        private readonly IOrderRepository _orderRepository;

        public EggGainedService(IEggGainedRepository eggGainedRepository, IOrderRepository orderRepository)
        {
            _eggGainedRepository = eggGainedRepository;
            _orderRepository = orderRepository;
        }

        public async Task<CreateEggResponseDTO> AddEggsAsync(Guid orderId, CreateEggGainedListRequestDTO request)
        {
            if (request?.Eggs == null || !request.Eggs.Any())
                throw new ArgumentException("Danh sách trứng không được rỗng");

            var dateGained = DateOnly.FromDateTime(DateTime.UtcNow);

            var entities = request.Eggs.Select(x => new EggGained
            {
                Grade = x.Grade.Equals("M2")?EggGrade.M2:x.Grade.Equals("M1")?EggGrade.M1:EggGrade.GV,
                IsUsable = x.isQualified,
                DateGained = dateGained,
                OrderId = orderId
            }).ToList();

            await _eggGainedRepository.BulkInsertAsync(entities);

            var order = await _orderRepository.FindByIdAsync(orderId);
            var eggGains = await _eggGainedRepository.FindAllByOrderIdAsync(orderId);

            order.TotalEgg = eggGains.Count();
            await _orderRepository.SaveChangeAsync();

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

        public async Task<IEnumerable<EggReportResponse>> GetEggReportByOrderId(Guid orderId)
        {
            var eggs = await _eggGainedRepository.FindAllByOrderIdAsync(orderId);

            return eggs.Select(x => new EggReportResponse
            {
                Id = x.Id,
                Grade = x.Grade.ToString(),
                DateGain = x.DateGained.ToString("dd/MM/yyyy"),
                IsUsable = x.IsUsable,
                OrderId = x.OrderId.ToString()
            }).ToList();
        }

        public async Task<IEnumerable<EggDataStatistic>> GetStatisticEggGradeAndViableAsync(Guid orderId)
        {
            var order = await _orderRepository.FindByIdAsync(orderId);

            var result = from e in order.EggGaineds
                         group e by e.Grade into t
                         let q = t.Count()
                         let vc = t.Count(x => x.IsUsable == true)
                         select new EggDataStatistic
                         {
                             Grade = t.Key.ToString(),
                             Quantity = q,
                             ViableCount = vc
                         };

            return result.ToList();
        }

        public async Task<IEnumerable<EmbryoDropdownEggDTO>> GetUsableEggsByOrderIdAsync(Guid orderId)
        {
            var eggs = await _eggGainedRepository.GetUsableEggsByOrderIdAsync(orderId);

            return eggs.Select(e => new EmbryoDropdownEggDTO
            {
                Id = e.Id,
                Grade = e.Grade.ToString()
            }).ToList();
        }
    }

}
