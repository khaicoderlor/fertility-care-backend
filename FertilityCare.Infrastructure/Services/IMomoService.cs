using FertilityCare.Infrastructure.Configurations;
using FertilityCare.UseCase.DTOs.Payments;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Services
{
    public interface IMomoService
    {
        Task<string> CreatePaymentAsync(CreateMomoRequest request);

        bool VerifySignatureFromCallback(IQueryCollection queryParams);

    }

    public class MomoService : IMomoService
    {

        private readonly MomoPaymentConfiguration _momoConfig;

        public MomoService(IOptions<MomoPaymentConfiguration> momoConfig)
        {
            _momoConfig = momoConfig.Value;
        }

        public async Task<string> CreatePaymentAsync(CreateMomoRequest requestdto)
        {
            var request = new MomoPaymentRequest
            {
                PartnerCode = _momoConfig.PartnerCode,
                AccessKey = _momoConfig.AccessKey,
                RequestId = Guid.NewGuid().ToString(),
                Amount = requestdto.Amount,
                OrderId = requestdto.OrderId,
                OrderInfo = requestdto.OrderInfo,
                RedirectUrl = _momoConfig.ReturnUrl,
                NotifyUrl = _momoConfig.NotifyUrl,
                RequestType = _momoConfig.RequestType
            };

            string rawSignature =
                $"accessKey={request.AccessKey}&amount={request.Amount}&extraData={request.ExtraData}" +
                $"&ipnUrl={request.NotifyUrl}&orderId={request.OrderId}&orderInfo={request.OrderInfo}" +
                $"&partnerCode={request.PartnerCode}&redirectUrl={request.RedirectUrl}" +
                $"&requestId={request.RequestId}&requestType={request.RequestType}";

            using var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(_momoConfig.SecretKey));
            request.Signature = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(rawSignature))).Replace("-", "").ToLower();

            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync(_momoConfig.ApiUrl, request);
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            if (json.TryGetProperty("payUrl", out var payUrlProperty) && payUrlProperty.ValueKind == JsonValueKind.String)
            {
                return payUrlProperty.GetString()!;
            }

            throw new InvalidOperationException("The response does not contain a valid 'payUrl'.");
        }

        public bool VerifySignatureFromCallback(IQueryCollection queryParams)
        {
            var rawSignature = $"accessKey={_momoConfig.AccessKey}&amount={queryParams["amount"]}&extraData={queryParams["extraData"]}" +
                               $"&message={queryParams["message"]}&orderId={queryParams["orderId"]}&orderInfo={queryParams["orderInfo"]}" +
                               $"&orderType={queryParams["orderType"]}&partnerCode={queryParams["partnerCode"]}&payType={queryParams["payType"]}" +
                               $"&requestId={queryParams["requestId"]}&responseTime={queryParams["responseTime"]}&resultCode={queryParams["resultCode"]}" +
                               $"&transId={queryParams["transId"]}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_momoConfig.SecretKey));
            var computedSignature = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(rawSignature)))
                .Replace("-", "").ToLower();

            return computedSignature == queryParams["signature"];
        }
    }
}
