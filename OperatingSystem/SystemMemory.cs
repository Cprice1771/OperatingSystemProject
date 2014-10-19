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

        /// <summary>
        /// Returns false if all PCB's are in the terminate queue
        /// </summary>
        public bool HasJobs { 
            get
            {
                foreach (PCB pcb in PCBs)
                {
                    if (pcb.State == ProcessState.Terminated)
                        continue;

                    return true;
                }

                return false;
            } 
        }
    }
}
