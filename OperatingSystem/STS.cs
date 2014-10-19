using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    /// <summary>
    /// This is a static class becasue it holds no data. It only moves PCB's from the ready queue to the CPU, and out of the wait/IO queue when they are ready
    /// </summary>
    public static class STS
    {
        public static void SupplyCPU(CPU cpu, Dictionary<QueueType, List<PCB>> queues)
        {
            if (cpu.IsWaiting)
            {
                if (cpu.HasJob)
                {
                    if (cpu.PCB.State == ProcessState.Ready)
                        queues[QueueType.Ready].Add(cpu.PCB);
                    else if(cpu.PCB.State == ProcessState.IO)
                        queues[QueueType.IO].Add(cpu.PCB);
                    else if(cpu.PCB.State == ProcessState.Waiting)
                        queues[QueueType.Waiting].Add(cpu.PCB);
                    else if (cpu.PCB.State == ProcessState.Terminated)
                        queues[QueueType.Terminated].Add(cpu.PCB);
                    else if(cpu.PCB.State == ProcessState.Stopped)
                    {
                        cpu.PCB.State = ProcessState.Ready;
                        queues[QueueType.Ready].Add(cpu.PCB);
                    }
                }

                if (queues[QueueType.Ready].Count > 0)
                {
                    cpu.LoadPCB(queues[QueueType.Ready][0]);
                    queues[QueueType.Ready][0].State = ProcessState.Running;
                    queues[QueueType.Ready].RemoveAt(0);
                }
            }
        }


    }
}
