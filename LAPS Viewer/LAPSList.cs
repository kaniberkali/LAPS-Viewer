using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAPS_Viewer
{
    internal class LAPSList
    {
        public string creationDate { get; set; }
        public string wifiName { get; set; }
        public List<string> ips { get; set; }
    }
}
