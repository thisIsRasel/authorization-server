﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Exceptions
{
    public class ExpiredRefreshTokenException: Exception
    {
        public ExpiredRefreshTokenException(): base()
        {

        }
    }
}
