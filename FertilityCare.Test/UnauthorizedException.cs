using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Shared.Exceptions;

public class UnauthorizedException : BaseException
{
    public override int StatusCode => 401;

    public UnauthorizedException(string message) : base(message)
    {
    }

}
