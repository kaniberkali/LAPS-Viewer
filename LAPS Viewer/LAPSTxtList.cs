using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAPS_Viewer
{
    internal class LAPSTxtList
    {
        public bool connection { get; set; }
        public bool index { get; set; }
        public int portsCount { get; set; }
        public double timeToFindAddress { get; set; }
        public double totalTime { get; set; }
        public List<int> ports { get; set; }
    }
}
