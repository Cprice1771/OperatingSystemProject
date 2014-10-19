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
        PCB _currentPCB;
        RAM _ram;


        public CPU(RAM ram)
        {
            _registerA = 1;
            _registerB = 3;
            _registerC = 5;
            _registerD = 7;
            _accumulator = 9;
            _ram = ram;
        }

        public void LoadPCB(PCB input)
        {
            _registerA = input.RegisterA;
            _registerB = input.RegisterB;
            _registerC = input.RegisterC;
            _registerD = input.RegisterD;
            _accumulator = input.Accumulator;
            _currentPCB = input;
        }

        public void Execute()
        {
            //Only do anything if the process is ready
            if (_currentPCB.State != ProcessState.Ready)
                return;

            //Get all the arguments
            Instruction currentInstruction = _ram.Instructions[_currentPCB.PC];
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
                    _currentPCB.State = ProcessState.Waiting;
                    _currentPCB.IOQueueCycles = arg3;
                    break ;
                case  CommandType.sto:
                    _accumulator = arg3;
                    break ;
                case CommandType.wt:
                    _currentPCB.State = ProcessState.Waiting;
                    _currentPCB.WaitQueueCycles = arg3;
                    break ;
                case CommandType.wr:
                    _currentPCB.State = ProcessState.Waiting;
                    _currentPCB.IOQueueCycles = arg3;
                    break ;
                case CommandType.nul:
                    ResetRegisters();
                    break ;
                case CommandType.stp:
                    SavePCB();
                    //This should flag the STS to send it back to the end of the RQ
                    _currentPCB.State = ProcessState.Stopped;
                    break ;
                case CommandType.err:
                    SavePCB();
                    _currentPCB.State = ProcessState.Terminated;
                    break ;
                default:
                    throw new UnknownCommandException();
            }
        }

        private void SavePCB()
        {
            _currentPCB.RegisterA = _registerA;
            _currentPCB.RegisterB = _registerB;
            _currentPCB.RegisterC = _registerC;
            _currentPCB.RegisterD = _registerD;
            _currentPCB.Accumulator = _accumulator;
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
