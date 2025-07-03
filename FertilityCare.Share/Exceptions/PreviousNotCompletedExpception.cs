using FertilityCare.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Share.Exceptions
{
    public class PreviousNotCompletedExpception : BaseException
    {
        public override int StatusCode => 1000;

        public PreviousNotCompletedExpception(string message) : base(message)
        {

        }
    }
}
