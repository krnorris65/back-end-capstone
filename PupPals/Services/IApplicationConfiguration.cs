using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PupPals.Services
{
    public interface IApplicationConfiguration
    {
        string GoogleAPIKey { get; set; }
    }
}
