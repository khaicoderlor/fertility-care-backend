using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Configurations
{
    public class MomoPaymentConfiguration
    {
        public required string ApiUrl { get; set; }
        public required string SecretKey { get; set; }
        public required string AccessKey { get; set; }
        public required string ReturnUrl { get; set; }
        public required string NotifyUrl { get; set; }
        public required string PartnerCode { get; set; }
        public string RequestType { get; set; } = "captureWallet";
    }

}
