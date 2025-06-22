using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Appointments;
using FertilityCare.UseCase.DTOs.EmbryoTransfers;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebApi.Controllers
{
    [Route("api/v1/embryotransfer")]
    [ApiController]
    public class EmbryoTransferController : ControllerBase
    {
        private readonly IEmbryoTransferService _embryoTransferService;
        public EmbryoTransferController(IEmbryoTransferService embryoTransferService)
        {
            _embryoTransferService = embryoTransferService;
        }
        
        
    }      
}
