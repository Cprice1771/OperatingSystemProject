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
        /// Moves PCB's from the HDD to the ram on a first come first serve basis
        /// </summary>
        /// <param name="hdd">HDD to get the instructions from</param>
        /// <param name="ram">RAM to move the instructions to</param>
        /// <param name="sysMem">PCB's for the instructions in the HDD</param>
        /// <param name="readyQueue">ref to the ready queue to move the jobs into</param>
        public static void FCFS(HDD hdd, RAM ram, SystemMemory sysMem, ref List<PCB> readyQueue)
        {
            foreach (PCB pcb in sysMem)
            {
                if (pcb.Location == JobLocation.HDD && ram.MaxSize > (ram.size + pcb.Length))
                {
                    pcb.Index = ram.AddJob(hdd.Instructions.GetRange(pcb.Index, pcb.Length));
                    pcb.Location = JobLocation.RAM;
                    readyQueue.Add(pcb);
                }  
            }
        }

        /// <summary>
        /// Moves PCB's from the HDD to the ram based on priority
        /// </summary>
        /// <param name="hdd">HDD to get the instructions from</param>
        /// <param name="ram">RAM to move the instructions to</param>
        /// <param name="sysMem">PCB's for the instructions in the HDD</param>
        /// <param name="readyQueue">ref to the ready queue to move the jobs into</param>
        public static void Priority(HDD hdd, RAM ram, SystemMemory sysMem, ref List<PCB> readyQueue)
        {
            sysMem.Sort(CompareByPriority);

            foreach (PCB pcb in sysMem)
            {
                if (pcb.Location == JobLocation.HDD && ram.MaxSize > (ram.size + pcb.Length))
                {
                    pcb.Index = ram.AddJob(hdd.Instructions.GetRange(pcb.Index, pcb.Length));
                    pcb.Location = JobLocation.RAM;
                    readyQueue.Add(pcb);
                }      
            }
        }

        /// <summary>
        /// Moves PCB's from the HDD to the ram starting with the shortest job first
        /// </summary>
        /// <param name="hdd">HDD to get the instructions from</param>
        /// <param name="ram">RAM to move the instructions to</param>
        /// <param name="sysMem">PCB's for the instructions in the HDD</param>
        /// <param name="readyQueue">ref to the ready queue to move the jobs into</param>
        public static void ShortestFirst(HDD hdd, RAM ram, SystemMemory sysMem, ref List<PCB> readyQueue)
        {
            sysMem.Sort(CompareByLength);

            foreach (PCB pcb in sysMem)
            {
                if (pcb.Location == JobLocation.HDD && ram.MaxSize > (ram.size + pcb.Length))
                {
                    pcb.Index = ram.AddJob(hdd.Instructions.GetRange(pcb.Index, pcb.Length));
                    pcb.Location = JobLocation.RAM;
                    readyQueue.Add(pcb);
                }     

            }
        }

        /// <summary>
        /// Compare method for comparing two PCB's by priority
        /// </summary>
        /// <param name="A">first PCB to compare</param>
        /// <param name="B">second PCB to compare</param>
        /// <returns>0 if equal, 1 if A less than B, -1 if A is greater than B</returns>
        private static int CompareByPriority(PCB A, PCB B)
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
        private static int CompareByLength(PCB A, PCB B)
        {
            if (A == null && B == null) return 0;
            else if (A.Length > B.Length) return 1;
            else if (A.Length == B.Length) return 0;
            else return -1;
        }
    }
}
