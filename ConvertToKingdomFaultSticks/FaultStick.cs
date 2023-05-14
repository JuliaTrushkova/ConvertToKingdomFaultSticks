using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertToKingdomFaultSticks
{    
    internal class FaultStick
    {
        public string? NameOfProfile { get; set; }

        public List<FaultStickPoint> FaultStickPoints;

        public int NumberOfStick { get; set; }

        public FaultStick()
        {
            FaultStickPoints = new List<FaultStickPoint>();
            NumberOfStick = 1;
        }

        public FaultStick(int number)
        {
            FaultStickPoints = new List<FaultStickPoint>();
            NumberOfStick = number;
        }

        public static FaultStick CreateNewFaultStick(string[] line, int numberOfStick)
        {
            FaultStick faultStick = new FaultStick(numberOfStick);

            faultStick.NameOfProfile = line[3];
            faultStick.FaultStickPoints.Add(FaultStickPoint.CreateNewFaultStickPoint(line));  
            
            return faultStick;
        }

        public void AddFaultStickPoint(string[] line)
        {
            FaultStickPoint faultStickPoint = FaultStickPoint.CreateNewFaultStickPoint(line);
            this.FaultStickPoints.Add(faultStickPoint);
        }

        public string ToString(string nameOfFault)
        {
            string[] result = new string[FaultStickPoints.Count];
            int counter = 0;
            foreach (FaultStickPoint faultStickPoint in FaultStickPoints) 
            {
                string s = NameOfProfile + "   " + faultStickPoint.ToString() + " " + nameOfFault + "    " + NumberOfStick.ToString() + " " + NameOfProfile;
                result[counter] = s;
                counter++;
            }
            return string.Join('\n', result);
        }
    }
}
