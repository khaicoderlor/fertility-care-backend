using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Shared.Exceptions
{
    public class BadRequestException : BaseException
    {
        public override int StatusCode => 400;
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
