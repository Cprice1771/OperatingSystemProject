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
        public static void SupplyCPU(CPU cpu, RAM ram)
        {
            if (cpu.IsWaiting)
            {
                if (cpu.HasJob)
                {
                    if (cpu.PCB.State == ProcessState.Ready)
                    {
                        SystemMemory.Instance.Queues[QueueType.Ready].Add(cpu.PCB);
                        cpu.PCB.WaitingTimer.Start();
                    }
                    else if (cpu.PCB.State == ProcessState.IO)
                        SystemMemory.Instance.Queues[QueueType.IO].Add(cpu.PCB);
                    else if (cpu.PCB.State == ProcessState.Waiting)
                        SystemMemory.Instance.Queues[QueueType.Waiting].Add(cpu.PCB);
                    else if (cpu.PCB.State == ProcessState.Terminated)
                        SystemMemory.Instance.Queues[QueueType.Terminated].Add(cpu.PCB);
                    else if (cpu.PCB.State == ProcessState.Stopped)
                    {
                        cpu.PCB.State = ProcessState.Ready;
                        SystemMemory.Instance.Queues[QueueType.Ready].Add(cpu.PCB);
                    }

                    cpu.PCB.TurnaroundTimer.Stop();
                    cpu.UnloadPCB();
                    


                }

                if (SystemMemory.Instance.Queues[QueueType.Ready].Count > 0)
                {
                    cpu.LoadPCB(SystemMemory.Instance.Queues[QueueType.Ready][0], ram);
                    SystemMemory.Instance.Queues[QueueType.Ready][0].State = ProcessState.Running;
                    SystemMemory.Instance.Queues[QueueType.Ready][0].WaitingTimer.Stop();
                    SystemMemory.Instance.Queues[QueueType.Ready][0].TurnaroundTimer.Start();
                    SystemMemory.Instance.Queues[QueueType.Ready].RemoveAt(0);
                }
            }
        }


    }
}
