using FertilityCare.Infrastructure.Configurations;
using FertilityCare.UseCase.DTOs.Payments;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FertilityCare.Infrastructure.Services
{
    public interface IMomoService
    {
        Task<string> CreatePaymentAsync(CreateMomoRequest request);

        bool VerifySignatureFromCallback(IQueryCollection queryParams);

    }

    public class MomoService : IMomoService
    {
        private readonly MomoPaymentConfiguration _cfg;

        private readonly HttpClient _client;

        public MomoService(IOptions<MomoPaymentConfiguration> cfg, HttpClient httpClient)
        {
            _cfg = cfg.Value;
            _client = httpClient;
        }

        public async Task<string> CreatePaymentAsync(CreateMomoRequest src)
        {
            string originalOrderInfo = src.OrderInfo;
            string encodedOrderInfo = Uri.EscapeDataString(originalOrderInfo); // chỉ dùng sau

            var req = new MomoPaymentRequest
            {
                PartnerCode = _cfg.PartnerCode,
                AccessKey = _cfg.AccessKey,
                RequestId = Guid.NewGuid().ToString(),
                Amount = src.Amount.ToString(),
                OrderId = src.OrderId,
                RedirectUrl = _cfg.ReturnUrl,
                IpnUrl = _cfg.NotifyUrl,
                RequestType = _cfg.RequestType,
                ExtraData = ""
            };

            // raw string để ký → dùng bản GỐC chưa encode
            var raw = $"accessKey={req.AccessKey}&amount={req.Amount}&extraData={req.ExtraData}" +
                      $"&ipnUrl={req.IpnUrl}&orderId={req.OrderId}&orderInfo={originalOrderInfo}" +
                      $"&partnerCode={req.PartnerCode}&redirectUrl={req.RedirectUrl}" +
                      $"&requestId={req.RequestId}&requestType={req.RequestType}";

            Console.WriteLine("Raw for signature: " + raw);

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_cfg.SecretKey));
            req.Signature = Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(raw))).ToLowerInvariant();

            // Gán bản ENCODED vào JSON để gửi đi
            req.OrderInfo = encodedOrderInfo;

            var json = JsonConvert.SerializeObject(req);
            Console.WriteLine("Request JSON: " + json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await _client.PostAsync(_cfg.ApiUrl, content);
            var resContent = await res.Content.ReadAsStringAsync();

            Console.WriteLine("MoMo response: " + resContent);
            res.EnsureSuccessStatusCode();

            dynamic response = JsonConvert.DeserializeObject(resContent)!;
            if (response.payUrl != null)
            {
                return response.payUrl;
            }

            throw new InvalidOperationException("Response lacks payUrl");
        }


        public bool VerifySignatureFromCallback(IQueryCollection q)
        {
            q.TryGetValue("message", out var message);

            var raw = $"accessKey={_cfg.AccessKey}&amount={q["amount"]}&extraData={q["extraData"]}" +
                      $"&message={message}&orderId={q["orderId"]}&orderInfo={q["orderInfo"]}&orderType={q["orderType"]}" +
                      $"&partnerCode={q["partnerCode"]}&payType={q["payType"]}&requestId={q["requestId"]}" +
                      $"&responseTime={q["responseTime"]}&resultCode={q["resultCode"]}&transId={q["transId"]}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_cfg.SecretKey));
            var sign = Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(raw))).ToLowerInvariant();

            return string.Equals(sign, q["signature"], StringComparison.OrdinalIgnoreCase);
        }

    }
}
