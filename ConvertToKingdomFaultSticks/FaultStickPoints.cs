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

        public static FaultStickPoint CreateNewFaultStickPoint(string[] line, FilePattern filePattern)
        {
            FaultStickPoint faultStickPoint = new FaultStickPoint()
            { 
                X = line[filePattern.ColumnX],
                Y = line[filePattern.ColumnY],
                Cdp = line[filePattern.ColumnCdp],
                Time = line[filePattern.ColumnTime]
            };
            return faultStickPoint;
        }

        public override string ToString()
        {
            return Cdp + " " + X + " " + Y + "  " + Time;
        }
    }
}
