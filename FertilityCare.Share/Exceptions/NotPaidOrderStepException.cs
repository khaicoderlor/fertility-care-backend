using FertilityCare.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Share.Exceptions
{
    public class NotPaidOrderStepException : BaseException
    {
        public override int StatusCode => 1001;


        public NotPaidOrderStepException(string message) : base(message)
        {
        }
 
    }
}
