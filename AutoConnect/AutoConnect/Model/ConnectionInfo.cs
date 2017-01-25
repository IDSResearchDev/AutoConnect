using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConnect.Model
{
    public class ConnectionInfo
    {
        //public string PProfile;
        public Line Primary;

        public int PrimaryId;
        public string PrimaryType;
        public string Orientation;

        public string SecondaryProfile;
        public Line Secondary;

        public double XIntersection;
        public double YIntersection;

        public double Level;

        public string ConnectionType { get; set; }
    }
}
