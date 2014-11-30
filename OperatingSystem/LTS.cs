using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    /// <summary>
    /// This is a static class becasue it holds no data. It only moves instructions from the HDD to RAM, and PCB's to the ready queue
    /// </summary>
    public static class LTS
    {
        /// <summary>
        /// Compare method for comparing two PCB's by priority
        /// </summary>
        /// <param name="A">first PCB to compare</param>
        /// <param name="B">second PCB to compare</param>
        /// <returns>0 if equal, 1 if A less than B, -1 if A is greater than B</returns>
        internal static int CompareByPriority(PCB A, PCB B)
        {
            if (A == null && B == null) return 0;
            else if (A.Priority < B.Priority) return 1;
            else if (A.Priority == B.Priority) return 0;
            else return -1;
        }

        /// <summary>
        /// Compare method for comparing two PCB's by length
        /// </summary>
        /// <param name="A">first PCB to compare</param>
        /// <param name="B">second PCB to compare</param>
        /// <returns>0 if equal, 1 if A less than B, -1 if A is greater than B</returns>
        internal static int CompareByLength(PCB A, PCB B)
        {
            if (A == null && B == null) return 0;
            else if (A.Length > B.Length) return 1;
            else if (A.Length == B.Length) return 0;
            else return -1;
        }


        /// <summary>
        /// Moves PCB's from the HDD to the ram
        /// </summary>
        /// <param name="hdd">HDD to get the instructions from</param>
        /// <param name="ram">RAM to move the instructions to</param>
        internal static void Run(HDD hdd, RAM ram)
        {
            foreach (PCB pcb in SystemMemory.Instance.Jobs)
            {
                if (pcb.Location == JobLocation.HDD && ram.MaxSize > (ram.size + pcb.Length))
                {
                    pcb.Index = ram.AddJob(hdd.Instructions.GetRange(pcb.Index, pcb.Length));
                    pcb.Location = JobLocation.RAM;
                    pcb.State = ProcessState.Ready;
                    SystemMemory.Instance.Queues[QueueType.Ready].Add(pcb);
                    pcb.WaitingTimer.Start();
                    pcb.ResponseTimer.Start();
                }
            }
        }
    }
}
