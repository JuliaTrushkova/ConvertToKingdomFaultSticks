using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

namespace ConvertToKingdomFaultSticks
{
    internal class Fault : IComparable, IComparer
    {
        public string Name { get; set; }

        public List<FaultStick> FaultSticks;
        public Fault()
        { 
            FaultSticks = new List<FaultStick>();
        }
        public Fault(string name)
        {
            FaultSticks = new List<FaultStick>();
            Name = name;
        }

        public static Fault CreateNewFault(string[]? line, FilePattern filePattern)
        {
            if (line is null) throw new ArgumentNullException(nameof(line));

            int numberOfFirstStick = 1;
            FaultStick faultStick = FaultStick.CreateNewFaultStick(line, numberOfFirstStick, filePattern);
            Fault fault = new Fault();
            fault.Name = line[filePattern.ColumnNameOfFault];  
            fault.FaultSticks.Add(faultStick);
            return fault;
        }

        public void AddNewFaultStick(string[]? line, int numberOfStick, FilePattern filePattern)
        {
            if (line is null) throw new ArgumentNullException(nameof(line));

            FaultStick faultStick = FaultStick.CreateNewFaultStick(line, numberOfStick, filePattern);
            this.FaultSticks.Add(faultStick);
        }

        public int CompareTo(object? other)
        {
            if (other == null) throw new ArgumentNullException();

            Fault otherFault = other as Fault;
            return Convert.ToInt32(this.Name == otherFault?.Name);
        }

        public int Compare(object? firstFault, object? secondFault)
        {
            if (firstFault == null || secondFault == null) throw new ArgumentNullException();

            return Convert.ToInt32((firstFault as Fault)?.Name == (secondFault as Fault)?.Name); ;
        }

        public override string ToString()
        {
            string[] sb = new string[this.FaultSticks.Count];
            int counter = 0;
            foreach (FaultStick  faultStick in this.FaultSticks)
            {
                sb[counter] = faultStick.ToString(this.Name);
                counter++;
            }          
            
            return string.Join('\n', sb);            
        }
    }
}
