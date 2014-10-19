using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class RAM
    {
        //public const int MAX_SIZE = 100;
        public int MaxSize;

        public List<Instruction> Instructions { get; set; }

        public int size
        {
            get
            {
                return Instructions.Count;
            }
        }

        //Size in loc
        
        public RAM(int s = 100)
        {
            MaxSize = s;
            Instructions = new List<Instruction>();
        }

        public void AddJob(List<Instruction> instructions)
        {
            if ((size + instructions.Count) < MaxSize)
                Instructions.AddRange(instructions);
            else
                throw new InsufficientRAMException();
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
    }
}
