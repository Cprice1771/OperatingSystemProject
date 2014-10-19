using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class PCB
    {
        public byte Priority { set; get; }
        public byte Length { set; get; }
        public ProcessState State { get; set; }
        public JobLocation Location { get; set; }
        public int Index { get; set; }
        public int Accumulator { get; set; }
        public int RegisterA { get; set; }
        public int RegisterB { get; set; }
        public int RegisterC { get; set; }
        public int RegisterD { get; set; }
        public int PC {get; set;}
        public int WaitQueueCycles { get; set; }
        public int IOQueueCycles { get; set; }


        public PCB(byte p, byte len, int instNum, JobLocation loc)
        {
            Priority = p;
            Length = len;
            PC = instNum;
            State = ProcessState.New;
            Location = loc;

            RegisterA = 1;
            RegisterB = 3;
            RegisterC = 5;
            RegisterD = 7;
            Accumulator = 9;

            WaitQueueCycles = 0;
            IOQueueCycles = 0;
        }
    }
}
