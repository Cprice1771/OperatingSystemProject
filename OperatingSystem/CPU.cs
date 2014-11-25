using OperatingSystem.Exceptions;
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

        public PCB PCB { get; set; }
        public bool HasJob {
            get
            {
                lock (_threadExecution)
                {
                    if (PCB != null)
                        return true;
                    else
                        return false;
                }
            }
              }
        public int ExecutionCycles;
        public bool IsWaiting
        {
            get
            {
                lock (_threadExecution)
                {
                    if ((PCB == null || PCB.State != ProcessState.Running))
                        return true;
                    else
                        return false;
                }
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
            _threadExecution = new object();
            ExecutionCycles = 0;
        }

        public void UnloadPCB()
        {
            lock (_threadExecution)
            {
                PCB = null;
            }
        }

        public void LoadPCB(PCB input, ref RAM ram)
        {
            lock (_threadExecution)
            {
                _registerA = input.RegisterA;
                _registerB = input.RegisterB;
                _registerC = input.RegisterC;
                _registerD = input.RegisterD;
                _accumulator = input.Accumulator;
                PCB = input;
                _ram = ram;
            }
        }

        public void Execute(Object threadContext)
        {
            lock (_threadExecution)
            {
                //Only do anything if the process is running
                if (PCB == null || PCB.State != ProcessState.Running)
                    return;

                ExecutionCycles++;
                bool RemovePCB = false;

                Instruction currentInstruction;

                lock (_ram.Instructions)
                {
                    currentInstruction = _ram.Instructions[PCB.Index + PCB.PC];
                }

                lock (PCB)
                {

                    //Only do anything if the process is running
                    if (PCB == null || PCB.State != ProcessState.Running)
                        return;

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
                            PCB.State = ProcessState.IO;
                            PCB.IOQueueCycles = arg3;
                            break;
                        case CommandType.sto:
                            _accumulator = arg3;
                            break;
                        case CommandType.wt:
                            PCB.State = ProcessState.Waiting;
                            PCB.WaitQueueCycles = arg3;
                            SavePCB();
                            break;
                        case CommandType.wr:
                            PCB.State = ProcessState.IO;
                            PCB.IOQueueCycles = arg3;
                            SavePCB();
                            break;
                        case CommandType.nul:
                            ResetRegisters();
                            break;
                        case CommandType.stp:
                            SavePCB();
                            //This should flag the STS to send it back to the end of the RQ
                            PCB.State = ProcessState.Stopped;
                            break;
                        case CommandType.err:
                            SavePCB();
                            PCB.State = ProcessState.Terminated;
                            PCB.Location = JobLocation.TERMINATED;
                            RemovePCB = true;

                            break;
                        default:
                            throw new UnknownCommandException();
                    }

                    if(PCB.State != ProcessState.Terminated)
                    {
                        PCB.PC++;

                        //If we did all the instuctions, then terminate
                        if (PCB.Length <= PCB.PC)
                        {
                            SavePCB();
                            PCB.State = ProcessState.Terminated;
                            PCB.Location = JobLocation.TERMINATED;
                            RemovePCB = true;
                        }
                    }
                }

                if (RemovePCB)
                    _ram.RemoveJob(PCB);
            }
            
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
