using FertilityCare.Shared.Exceptions;
using FertilityCare.UseCase.DTOs.Orders;
using FertilityCare.UseCase.Interfaces.Services;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderDTO?>>> CreateOrder([FromBody] CreateOrderRequestDTO request)
        {
            try
            {
                var result = await _orderService.PlaceOrderAsync(request);
                return Ok(new ApiResponse<OrderDTO?>
                {
                    StatusCode = 200,
                    Message = "Order created successfully",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    StatusCode = 401,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (AppointmentSlotLimitExceededException ed)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = ed.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse<OrderDTO>>> GetOrderById([FromRoute] string id)
        {
            try
            {
                var result = await _orderService.GetOrderByIdAsync(Guid.Parse(id));
                return Ok(new ApiResponse<OrderDTO>
                {
                    StatusCode = 200,
                    Message = "Order fetched successfully",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpGet]
        [Route("doctors/{doctorId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDTO>>>> GetOrderByDoctorId([FromRoute] string doctorId)
        {
            Console.WriteLine("Doctor " + doctorId);
            try
            {
                var result = await _orderService.GetOrderByDoctorIdAsync(Guid.Parse(doctorId));
                return Ok(new ApiResponse<IEnumerable<OrderDTO>>
                {
                    StatusCode = 200,
                    Message = "Orders fetched successfully",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException e)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpGet]
        [Route("{patientId}/patients")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderInfo>>>> GetOrderByPatientId([FromRoute] string patientId)
        {
            try
            {
                var result = await _orderService.GetOrdersByPatientIdAsync(Guid.Parse(patientId));
                return Ok(new ApiResponse<IEnumerable<OrderInfo>>
                {
                    StatusCode = 200,
                    Message = "Orders fetched successfully",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpPut]
        [Route("{orderid}")]
        public async Task<ActionResult<long?>> SetTotalEgg([FromRoute] string orderid, [FromQuery] long totalEgg)
        {
            try
            {
                var result = await _orderService.SetTotalEgg(totalEgg, orderid);
                return Ok(new ApiResponse<long?>
                {
                    StatusCode = 200,
                    Message = "Total egg count updated successfully",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpPatch]
        [Route("{orderid}/closed")]
        public async Task<ActionResult<ApiResponse<bool>>> ClosedOrder([FromRoute] string orderid)
        {
            try
            {
                var result = await _orderService.MarkClosedOrder(Guid.Parse(orderid));
                return Ok(new ApiResponse<bool?>
                {
                    StatusCode = 200,
                    Message = "Total egg count updated successfully",
                    Data = result,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }
    }
}
