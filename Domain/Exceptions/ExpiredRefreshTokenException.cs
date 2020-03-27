using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ExpiredRefreshTokenException: Exception
    {

        public ExpiredRefreshTokenException()
        {

        }

        public ExpiredRefreshTokenException(string message): base(message)
        {

        }

        public ExpiredRefreshTokenException(string message, Exception inner): base(message, inner)
        {

        }
    }
}
