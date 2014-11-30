using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class JobResult
    {
        public int JobNum { get; set; }
        public int RegA { get; set; }
        public int RegB { get; set; }
        public int RegC { get; set; }
        public int RegD { get; set; }
        public int Acc { get; set; }
        public double TurnAroundTime { get; set; }
        public double ResponseTime { get; set; }
        public double WaitTime { get; set; }
    }
}
