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

        public void RemoveJob(int start, int length)
        {
            lock (Instructions)
            {
                Instructions.RemoveRange(start, length);
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
    }
}
