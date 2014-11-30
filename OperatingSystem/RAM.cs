using OperatingSystem.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class RAM
    {
        public int MaxSize;
        public List<int> removedJobs = new List<int>();
        public List<Instruction> Instructions { get; set; }


        public int size
        {
            get
            {
                int c;
                lock (Instructions)
                {
                    c = Instructions.Count;
                }

                return c;
            }
        }

        //Size in loc
        
        public RAM(int s = 100)
        {
            MaxSize = s;
            Instructions = new List<Instruction>();
        }

        public int AddJob(List<Instruction> instructions)
        {
            int index;
            lock (Instructions)
            {
                index = Instructions.Count;

                if ((size + instructions.Count) < MaxSize)
                    Instructions.AddRange(instructions);
                else
                    throw new InsufficientRAMException();

                
            }
            return index;
        }

        public void RemoveJob(PCB pcb)
        {
            lock (Instructions)
            {
                if(!removedJobs.Contains(pcb.JobNumber))
                    removedJobs.Add(pcb.JobNumber);
                else
                    throw new InvalidRamOperationException();

                Instructions.RemoveRange(pcb.Index, pcb.Length);
                CompactRam(pcb.Index, pcb.Length);
                pcb.Location = JobLocation.TERMINATED;
            }
            
        }


        public override string ToString()
        {
            string contents = "";

            foreach (Instruction i in Instructions)
                contents += i.ToString(); 

            return contents;
        }

        internal void Flush()
        {
            Instructions = new List<Instruction>();
        }

        private void CompactRam(int start, int length)
        {
            foreach (PCB pcb in SystemMemory.Instance.Jobs)
            {
                lock (pcb)
                {
                    if (pcb.Location == JobLocation.RAM)
                    {
                        if (pcb.Index > start)
                        {
                            if (pcb.Index < length)
                                throw new InvalidRamOperationException();
                            else
                                pcb.Index -= length;
                        }
                    }
                }
            }   
  
        }
    }
}
