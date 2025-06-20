using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Shared.Exceptions;

public class ValidationException : BaseException
{
    public override int StatusCode => 400;

    public ValidationException(string message) : base(message)
    {
    }

}
