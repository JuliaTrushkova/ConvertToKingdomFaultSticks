using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertToKingdomFaultSticks
{
    internal class FaultsForOneFile
    {
        public string FileName { get; set; }

        public List<Fault> Faults = new List<Fault>();
    }
}
