using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class SystemMemory : List<PCB>
    {
        public List<PCB> PCBs;

        public SystemMemory()
        {
            PCBs = new List<PCB>();
        }
    }
}
