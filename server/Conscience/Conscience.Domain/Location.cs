using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Location
    {
        public int Id
        {
            get;
            set;
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Accuracy { get; set; }
        public double? Altitude { get; set; }
        public double? AltitudeAccuracy { get; set; }
        public double? Heading { get; set; }
        public double? HeadingAccuracy { get; set; }
        public double? Speed { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
