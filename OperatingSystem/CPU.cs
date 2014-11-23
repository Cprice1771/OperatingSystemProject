using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class CPU
    {
        int _accumulator;
        int _registerA;
        int _registerB;
        int _registerC;
        int _registerD;
        
        RAM _ram;

        object _threadExecution;

        public bool _executing;
        public PCB PCB { get; set; }
        public bool HasJob {
            get
            {
                if (PCB != null)
                    return true;
                else
                    return false;
            }
              }
        public bool IsWaiting
        {
            get
            {
                if ((PCB == null || PCB.State != ProcessState.Running) && _executing == false)
                    return true;
                else
                    return false;
            }
        }

        public CPU(RAM ram)
        {
            _registerA = 1;
            _registerB = 3;
            _registerC = 5;
            _registerD = 7;
            _accumulator = 9;
            _ram = ram;
            _executing = false;
            _threadExecution = new object();
        }

        public void UnloadPCB()
        {
            while (_executing) ;

            lock (PCB)
            {
                PCB = null;
            }
        }

        public void LoadPCB(PCB input, RAM ram)
        {
            _registerA = input.RegisterA;
            _registerB = input.RegisterB;
            _registerC = input.RegisterC;
            _registerD = input.RegisterD;
            _accumulator = input.Accumulator;
            PCB = input;
            _ram = ram;
        }

        public void Execute(Object threadContext)
        {
            if (_executing)
                return;

            //Only do anything if the process is running
            if (PCB == null || PCB.State != ProcessState.Running)
                return;

            

            _executing = true;

            Instruction currentInstruction;
            //Get all the arguments
            currentInstruction = _ram.Instructions[PCB.Index + PCB.PC];

            int arg1 = GetArg(currentInstruction.Arg1);
            int arg2 = GetArg(currentInstruction.Arg2);
            int arg3 = currentInstruction.Arg3;

            //Execute the command
            switch (currentInstruction.Command)
            {
                case CommandType.mul:
                    _accumulator += (arg1 * arg2);
                    break;
                case CommandType.div:
                    _accumulator += (arg2 / arg1);
                    break;
                case CommandType.sub:
                    _accumulator += (arg1 - arg2);
                    break;
                case CommandType.add:
                    _accumulator += (arg1 + arg2);
                    break;
                case CommandType.rcl:
                    CopyAccTo(currentInstruction.Arg1);
                    break;
                case CommandType.rd:
                    lock (PCB)
                    {
                        PCB.State = ProcessState.IO;
                        PCB.IOQueueCycles = arg3;
                    }
                    break;
                case CommandType.sto:
                    _accumulator = arg3;
                    break;
                case CommandType.wt:
                    lock (PCB)
                    {
                        PCB.State = ProcessState.Waiting;
                        PCB.WaitQueueCycles = arg3;
                        SavePCB();
                    }
                    break;
                case CommandType.wr:
                    lock (PCB)
                    {
                        PCB.State = ProcessState.IO;
                        PCB.IOQueueCycles = arg3;
                        SavePCB();
                    }
                    break;
                case CommandType.nul:
                    ResetRegisters();
                    break;
                case CommandType.stp:
                    lock (PCB)
                    {
                        SavePCB();
                        //This should flag the STS to send it back to the end of the RQ
                        PCB.State = ProcessState.Stopped;
                    }
                    break;
                case CommandType.err:
                    lock (PCB)
                    {
                        SavePCB();
                        PCB.State = ProcessState.Terminated;
                    }

                    _ram.RemoveJob(PCB.Start, PCB.Length);

                    lock (PCB)
                    {
                        //Update all the values of the other PCB's after we remove the main one
                        foreach (PCB pcb in SystemMemory.Instance.Jobs)
                        {
                            if (pcb.Location == JobLocation.RAM)
                            {
                                if (pcb.Index > PCB.Start)
                                    pcb.Index -= PCB.Length;
                            }
                        }
                    }
                    break;
                default:
                    throw new UnknownCommandException();
            }

            lock (PCB)
            {
                PCB.PC++;

                //If we did all the instuctions, then terminate
                if (PCB.Length >= PCB.PC && PCB.State != ProcessState.Terminated)
                {
                    SavePCB();
                    PCB.State = ProcessState.Terminated;
                    _ram.RemoveJob(PCB.Start, PCB.Length);
                    //Update all the values of the other PCB's after we remove the main one
                    foreach (PCB pcb in SystemMemory.Instance.Jobs)
                    {
                        if (pcb.Location == JobLocation.RAM)
                        {
                            if (pcb.Index > PCB.Start)
                                pcb.Index -= PCB.Length;
                        }
                    }
                }
            }

                

            _executing = false;
        }

        /// <summary>
        /// Saves the PCB to the current register values in the CPU, is thread not safe.
        /// </summary>
        private void SavePCB()
        {
            PCB.RegisterA = _registerA;
            PCB.RegisterB = _registerB;
            PCB.RegisterC = _registerC;
            PCB.RegisterD = _registerD;
            PCB.Accumulator = _accumulator;
        }

        private void ResetRegisters()
        {
            _registerA = 1;
            _registerB = 3;
            _registerC = 5;
            _registerD = 7;
            _accumulator = 9;
        }

        private void CopyAccTo(Register register)
        {
            switch (register)
            {
                case Register.A:
                    _registerA = _accumulator;
                    break;
                case Register.B:
                    _registerB = _accumulator;
                    break;
                case Register.C:
                    _registerC = _accumulator;
                    break;
                case Register.D:
                    _registerD = _accumulator;
                    break;
                default:
                    throw new UnknownRegisterException();
            }
        }

        private int GetArg(Register register)
        {
            switch (register)
            {
                case Register.A:
                    return _registerA;
                case Register.B:
                    return _registerB;
                case Register.C:
                    return _registerC;
                case Register.D:
                    return _registerD;
                default:
                    throw new UnknownRegisterException();
            }
        }


        
    }
}
