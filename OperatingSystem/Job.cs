using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class Job
    {
        public List<Instruction> Instructions {get; set;}
        public PCB JobPCB;
        public int JobNumber;

        public Job(byte p, int jn)
        {
            JobPCB = new PCB(p, 0, 0, JobLocation.HDD, jn);
            Instructions = new List<Instruction>();
            JobNumber = jn;
        }

        /// <summary>
        /// Adds an instruction to the job
        /// </summary>
        /// <param name="instruction">Instruction to add</param>
        public void AddInstruction(Instruction instruction)
        {
            Instructions.Add(instruction);
            JobPCB.Length = Instructions.Count;
        }

        

        
    }
}
