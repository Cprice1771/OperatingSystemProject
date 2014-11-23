using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class PCB
    {
        public int Priority { set; get; }
        public int Length { set; get; }
        public int Start { get; set; }
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
        public int JobNumber { get; set; }
        public Stopwatch TurnaroundTimer { get; set; }
        public Stopwatch WaitingTimer { get; set; }
        public Stopwatch ResponseTimer { get; set; }


        public PCB(byte p, byte len, int instNum, JobLocation loc, int jn)
        {
            Priority = p;
            Length = len;
            PC = instNum;
            Start = PC;
            State = ProcessState.New;
            Location = loc;
            JobNumber = jn;

            RegisterA = 1;
            RegisterB = 3;
            RegisterC = 5;
            RegisterD = 7;
            Accumulator = 9;

            WaitQueueCycles = 0;
            IOQueueCycles = 0;

            TurnaroundTimer = new Stopwatch();
            WaitingTimer = new Stopwatch();
            ResponseTimer = new Stopwatch();

        }
    }
}
