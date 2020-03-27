using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidRefreshTokenException: Exception
    {
        public InvalidRefreshTokenException()
        {

        }

        public InvalidRefreshTokenException(string message) : base(message)
        {

        }

        public InvalidRefreshTokenException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
