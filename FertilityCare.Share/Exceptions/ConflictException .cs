using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Shared.Exceptions;

public class ConflictException : BaseException
{
    public override int StatusCode => 409;

    public ConflictException(string message) : base(message)
    {
    }
}
