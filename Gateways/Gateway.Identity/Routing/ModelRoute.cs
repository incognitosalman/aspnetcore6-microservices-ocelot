using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Identity.Routing
{
    public class ModelRoute
    {
        public string Endpoint { get; set; }
        public Destination Destination { get; set; }
    }
}
