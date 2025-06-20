using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Shared.Exceptions
{
    public class UserNotExistException : BaseException
    {
        public override int StatusCode => 404;
        public UserNotExistException(string message) : base(message)
        {
        }
    }
}
