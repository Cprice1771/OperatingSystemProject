using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    

    public class Instruction
    {
        public int InstructionNum { get; set; }
        public CommandType Command { get; set; } 
        public Register Arg1 { get; set; }
        public Register  Arg2 { get; set; }
        public byte Arg3;

        public Instruction(int i, CommandType c, Register a1, Register a2, byte a3)
        {
            InstructionNum = i;
            Command = c;
            Arg1 = a1;
            Arg2 = a2;
            Arg3 = a3;
        }

        /// <summary>
        /// Get the instruction as a string
        /// </summary>
        /// <returns>the instruction, comma seperated like in the input file</returns>
        public override string ToString()
        {
            //contents += "Instruction Num: " + InstructionNum + " Command: " + Command.ToString() + " Arg1: " + Arg1.ToString() + " Arg2: " + Arg2.ToString() + " Arg3: " + Arg3 + "\n";
            return InstructionNum + ", " + Command.ToString() + ", " + Arg1.ToString() + ", " + Arg2.ToString() + ", " + Arg3 + "\n";
        }

    }
}
