using FertilityCare.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Share.Exceptions
{
    public class AppointmentNotCompleteException : BaseException
    {

        public override int StatusCode => 1002;

        public AppointmentNotCompleteException(string message) : base(message)
        {
        }
    }
}
