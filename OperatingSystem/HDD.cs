using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    /// <summary>
    /// Basically a giant list of instructions meant to emulated the HDD
    /// </summary>
    public class HDD
    {
        //Our list of instructions
        public List<Instruction> Instructions { get; set; }

        public HDD()
        {
            Instructions = new List<Instruction>();
        }

        /// <summary>
        /// Appends a list of instructions to the current list of instructions in the HDD
        /// </summary>
        /// <param name="instructions">List of instructions to append</param>
        /// <returns>returns the index of the first entry of the list of instructions</returns>
        public int AddInstructions(List<Instruction> instructions)
        {
            //Get the index of where we're putting it
            int index = Instructions.Count;
            Instructions.AddRange(instructions);
            return index;
        }

        /// <summary>
        /// Add a single instruction to the list of instructions in the HDD
        /// </summary>
        /// <param name="instruction">The instruction to add</param>
        /// <returns>returns the index of the instruction</returns>
        public int AddInstruction(Instruction instruction)
        {
            //Get the index of where we're putting it
            int index = Instructions.Count;
            Instructions.Add(instruction);
            return index;
        }

        /// <summary>
        /// Get the contents of the HDD
        /// </summary>
        /// <returns>Returns the contents of the HDD as a string</returns>
        public override string ToString()
        {
            string contents = "";
            foreach (Instruction i in Instructions)
            {
                contents += i.ToString();
            }

            return contents;
        }
    }
}
