using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Shared.Exceptions
{
    public class CreateUserFailedExpception : BaseException
    {

        public override int StatusCode => 500;
        public CreateUserFailedExpception(string message) : base(message)
        {
        }
    }
}
