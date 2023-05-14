using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertToKingdomFaultSticks
{
    internal class FaultStickPoint
    {
        public string X { get; set; }
        public string Y { get; set; }
        public string Cdp { get; set; }
        public string Time { get; set; }

        public static FaultStickPoint CreateNewFaultStickPoint(string[] line)
        {
            FaultStickPoint faultStickPoint = new FaultStickPoint()
            { 
                X = line[4],
                Y = line[5],
                Cdp = line[2],
                Time = line[8],
            };
            return faultStickPoint;
        }

        public override string ToString()
        {
            return Cdp + " " + X + " " + Y + "  " + Time;
        }
    }
}
