using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace finale
{
    public class Balloon
    {
        public int Altitude {get; set; }
        public Localisation Location { get; set; }

        public Balloon(int altitude, Localisation location)
        {
            Altitude = altitude;
            Location = location;
        }
    }
}
