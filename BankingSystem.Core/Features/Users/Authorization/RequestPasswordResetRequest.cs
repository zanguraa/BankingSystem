﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Users.Authorization
{
    public class RequestPasswordResetRequest
    {
        public string Email { get; set; }
    }
}