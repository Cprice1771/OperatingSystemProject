using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class SystemMemory
    {
        List<PCB> _readyQueue; //Ready queue of PCB's ready to be run
        List<PCB> _waitQueue; //Wait queue of PCB's in wait steps
        List<PCB> _IOQueue; //IO queue of PCB's doing IO
        List<PCB> _terminateQueue; //List of all terminated PCB's


        public Dictionary<QueueType, List<PCB>> Queues { get; private set; }

        //Singleton
        public static SystemMemory Instance
        {
            get
            {
                if(instance == null)
                    instance = new SystemMemory();

                return instance;
            }
        }
        public List<PCB> Jobs;

        private static SystemMemory instance;
        private SystemMemory()
        {
            Jobs = new List<PCB>();
            _readyQueue = new List<PCB>();
            _waitQueue = new List<PCB>();
            _IOQueue = new List<PCB>();
            _terminateQueue = new List<PCB>();
            Queues = new Dictionary<QueueType, List<PCB>>();
            Queues.Add(QueueType.Ready, _readyQueue);
            Queues.Add(QueueType.IO, _IOQueue);
            Queues.Add(QueueType.Waiting, _waitQueue);
            Queues.Add(QueueType.Terminated, _terminateQueue);
        }

        /// <summary>
        /// Returns false if all PCB's are in the terminate queue or if there are no jobs
        /// </summary>
        public bool HasJobs { 
            get
            {
                foreach (PCB pcb in Jobs)
                {
                    if (pcb.State == ProcessState.Terminated)
                        continue;

                    return true;
                }

                return false;
            } 
        }

        internal static void Flush()
        {
            instance = new SystemMemory();
        }
    }
}
