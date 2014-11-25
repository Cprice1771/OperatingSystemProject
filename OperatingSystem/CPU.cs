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

        public int ExecutionCycles { get; set; }
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
                if (PCB == null || PCB.State != ProcessState.Running)
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
            ExecutionCycles = 0;
        }

        public void UnloadPCB()
        {
            PCB = null;
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

        public void Execute()
        {
            //Only do anything if the process is running
            if (PCB == null || PCB.State != ProcessState.Running)
                return;

            if (PCB.ResponseTimer.IsRunning)
            {
                PCB.ResponseTimer.Stop();
            }

            ExecutionCycles++;

            //Get all the arguments
            Instruction currentInstruction = _ram.Instructions[PCB.Index + PCB.PC];
            int arg1 = GetArg(currentInstruction.Arg1);
            int arg2 = GetArg(currentInstruction.Arg2);
            int arg3 = currentInstruction.Arg3;

            //Execute the command
            switch (currentInstruction.Command)
            {
                case CommandType.mul:
                    _accumulator += (arg1 * arg2);
                    break ;
                case CommandType.div:
                    _accumulator += (arg2 / arg1);
                    break ;
                case CommandType.sub:
                    _accumulator += (arg1 - arg2);
                    break ;
                case CommandType.add:
                    _accumulator += (arg1 + arg2);
                    break ;
                case CommandType.rcl:
                    CopyAccTo(currentInstruction.Arg1);
                    break ;
                case CommandType.rd:
                    PCB.State = ProcessState.IO;
                    PCB.IOQueueCycles = arg3;
                    break ;
                case  CommandType.sto:
                    _accumulator = arg3;
                    break ;
                case CommandType.wt:
                    PCB.State = ProcessState.Waiting;
                    PCB.WaitQueueCycles = arg3;
                    SavePCB();
                    break ;
                case CommandType.wr:
                    PCB.State = ProcessState.IO;
                    PCB.IOQueueCycles = arg3;
                    SavePCB();
                    break ;
                case CommandType.nul:
                    ResetRegisters();
                    break ;
                case CommandType.stp:
                    SavePCB();
                    //This should flag the STS to send it back to the end of the RQ
                    PCB.State = ProcessState.Stopped;
                    PCB.TurnaroundTimer.Stop();
                    break ;
                case CommandType.err:
                    SavePCB();
                    PCB.State = ProcessState.Terminated;
                    PCB.TurnaroundTimer.Stop();
                    _ram.RemoveJob(PCB.Start, PCB.Length);
                    //Update all the values of the other PCB's after we remove the main one
                    
                    break ;
                default:
                    throw new UnknownCommandException();
            }

            PCB.PC++;

            

            //If we did all the instuctions, then terminate
            if (PCB.Length <= PCB.PC && PCB.State != ProcessState.Terminated)
            {
                SavePCB();
                PCB.State = ProcessState.Terminated;
                PCB.TurnaroundTimer.Stop();
                _ram.RemoveJob(PCB.Start, PCB.Length);
                //Update all the values of the other PCB's after we remove the main one
                return;
            }
        }

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
