﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Shared.Exceptions
{
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(string message, params object?[]? parameters) : base(message)
        {

        }
    }
}
