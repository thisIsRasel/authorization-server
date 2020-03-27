using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidRequestException: Exception
    {
        public InvalidRequestException()
        {

        }

        public InvalidRequestException(string message) : base(message)
        {

        }

        public InvalidRequestException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
