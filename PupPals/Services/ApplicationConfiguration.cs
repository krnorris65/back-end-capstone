﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PupPals.Services
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public string GoogleAPIKey { get; set; }

    }
}
