﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCMarketPlace.Model.TccOAuth
{
    /// <summary>
    /// Login status.
    /// </summary>
    public class LoginResult
    {
        public int UserId { get; set; }
        public string Result { get; set; }
    }
}
