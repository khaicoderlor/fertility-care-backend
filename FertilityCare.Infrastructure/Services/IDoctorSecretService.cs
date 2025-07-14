using FertilityCare.Infrastructure.Identity;
using FertilityCare.Infrastructure.Repositories;
using FertilityCare.UseCase.DTOs.Doctors;
using FertilityCare.UseCase.Interfaces.Repositories;
using FertilityCare.UseCase.Mappers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Services
{
    public interface IDoctorSecretService
    {

        Task<IEnumerable<DoctorSideAdminPage>> GetDoctorSideAdminPages();

    }

    public class DoctorSecretService : IDoctorSecretService
    {
        private readonly IDoctorRepository _doctorRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IOrderRepository _orderRepository;
        public DoctorSecretService(IDoctorRepository doctorRepository, IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
        {
            _doctorRepository = doctorRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
        }
        public async Task<IEnumerable<DoctorSideAdminPage>> GetDoctorSideAdminPages()
        {
            var doctors = await _doctorRepository.FindAllAsync();
            var doctorSideAdminPages = new List<DoctorSideAdminPage>();
            foreach (var doctor in doctors)
            {
                var user = await _userManager.FindByProfileIdAsync(doctor.UserProfileId);
                var orders = await _orderRepository.FindAllByDoctorIdAsync(doctor.Id);
                doctorSideAdminPages.Add(new DoctorSideAdminPage
                {
                    DoctorEmail = user.Email,
                    DoctorPhone = user.PhoneNumber,
                    Doctor = doctor.MapToDoctorDTO(),
                    Orders = orders.Select(o => o.MapToOderDTO())
                });
            }

            return doctorSideAdminPages;
        }
    }
}
