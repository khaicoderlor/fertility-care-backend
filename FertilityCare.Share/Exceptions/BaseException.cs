using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Shared.Exceptions;

public abstract class BaseException : Exception
{
    public virtual int StatusCode { get; } = 500;

    protected BaseException(string message) : base(message)
    {

    }

}
