using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Shared.Exceptions
{
    public class BookingAppointmentFailedExpception : BaseException
    {
        public BookingAppointmentFailedExpception(string message) : base(message)
        {
        }

        public override int StatusCode => 400;


    }
}
