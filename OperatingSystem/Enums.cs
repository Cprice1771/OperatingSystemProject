using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public enum CommandType
    {
        add, //Take first register and add it to second register, then add that value to the acc
        sub, // Subtract second register from first and add the result to ACC.
        mul, // Mutliple the two registers and add the result to ACC.
        div, // Divide the second register by the first and add the result to the ACC.
        rd, // Send the job to the IO queue for the specified number of cycles
        wr, // Send the job to the IO queue for the specified number of cycles
        wt, // Send the job to the wait queue for the specifiec number of cycles
        sto, // Store the value in the ACC (Replace the current value)
        rcl, // Copy the ACC to the first specified register/Argument
        nul, // Reset all register and the ACC to default values
        stp, // Halt execution, save state, return the job directly to the ready queue to be dispatched again
        err, // Error condition; save the state to the PCB and terminate the program.
        Unknown //Instruction was not recognized
    }

    public enum LTSAlgorithm
    {
        FCFS,
        Priority,
        Shortest
    }

    public enum Register
    {
        A,
        B,
        C,
        D,
        Unknown
    }

    public enum JobLocation
    {
        HDD,
        RAM
    }

    public enum ProcessState
    {
        New,
        Waiting,
        Ready,
        Running,
        Stopped,
        Terminated
    }

    public class Enums
    {
        public static CommandType ParseInstruction(string command)
        {
            switch(command)
            {
                case "mul":
                    return CommandType.mul;
                case "div":
                    return CommandType.div;
                case "sub":
                    return CommandType.sub;
                case "add":
                    return CommandType.add;
                case "rcl":
                    return CommandType.rcl;
                case "_rd":
                    return CommandType.rd;
                case  "sto":
                    return CommandType.sto;
                case "_wt":
                    return CommandType.wt;
                case "_wr":
                    return CommandType.wr;
                case "nul":
                    return CommandType.nul;
                case "stp":
                    return CommandType.stp;
                case "err":
                    return CommandType.err;
                default:
                    return CommandType.Unknown;
            }
        }

        public static Register ParseRegister(string command)
        {
            switch(command)
            {
                case "A":
                    return Register.A;
                case "B":
                    return Register.B;
                case "C":
                    return Register.C;
                case "D":
                    return Register.D;
                default:
                    return Register.Unknown;
            }
        }
    }


}
