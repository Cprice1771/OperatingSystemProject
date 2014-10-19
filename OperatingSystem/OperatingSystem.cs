using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class OperatingSystem
    {
        HDD _hdd; //My harddrive
        RAM _ram; //My RAM
        SystemMemory _sysMem; //My System memory containing the PCB's
        Boot _bootloader; //Our bootloader that parses the file of insturctions
        LTSAlgorithm _algorithm; //Enum of the algorith we should use
        int _ramSize; //Size of ram in number of instructions
        int _cpuCount; //How many cpus we should use
        List<CPU> _cpus;
        List<PCB> _readyQueue; //Ready queue of PCB's ready to be run
        List<PCB> _waitQueue; //Wait queue of PCB's in wait steps
        List<PCB> _IOQueue; //IO queue of PCB's doing IO
        List<PCB> _terminateQueue; //List of all terminated PCB's
        Dictionary<QueueType, List<PCB>> _queues;

        public OperatingSystem(LTSAlgorithm algorithm, int ramSize, int cpuCount)
        {
            _hdd = new HDD();
            _ram = new RAM();
            _sysMem = new SystemMemory();
            _bootloader = new Boot();
            _algorithm = algorithm;
            _ramSize = ramSize;
            _cpuCount = cpuCount;
            _readyQueue = new List<PCB>();
            _waitQueue = new List<PCB>();
            _IOQueue = new List<PCB>();
            _terminateQueue = new List<PCB>();
            _queues = new Dictionary<QueueType, List<PCB>>();
            _cpus = new List<CPU>(cpuCount);
        }

        /// <summary>
        /// Our main method, this kicks off the os, and should be equivelent to pressing the power button on your computer
        /// </summary>
        /// <param name="inputFile">File name of the input file to get jobs from</param>
        /// <returns>Currently ruturns the contents of RAM</returns>
        public string Start(string inputFile)
        {
            //Our list of jobs
            List<Job> jobs;
            try
            {
                //Run boot, which parses the file and returns with a list of jobs for us
                jobs = _bootloader.Run(inputFile);
            }
            //Exception handling
            catch (FormatException)
            {
                return "Invalid file selected";
            }
            catch (Win32Exception)
            {
                return "File not found";
            }
            catch (FileNotFoundException)
            {
                return "File not found";
            }

            
            //Initialize our ram
            _ram = new RAM(_ramSize);
            string output = "";

            
            //Add all the instructions in the file to the HDD, and create PCB's for each job
            foreach (Job j in jobs)
            {
                //Point the PCB to its first instruction location in the HDD
                j.JobPCB.Index = _hdd.AddInstructions(j.Instructions);
                _sysMem.Add(j.JobPCB);
            }

            //While we still have jobs that are not terminated
            while (_sysMem.HasJobs)
            {
                //Run the LTS which grabs jobs from the HDD and tries to put them in RAM based on the specified algorithm
                switch (_algorithm)
                {
                    case LTSAlgorithm.FCFS:
                        LTS.FCFS(_hdd, _ram, _sysMem, ref _readyQueue);
                        output += _ram.ToString();
                        output += "Size: " + _ram.size;
                        _ram.Flush();
                        break;
                    case LTSAlgorithm.Priority:
                        LTS.Priority(_hdd, _ram, _sysMem, ref _readyQueue);
                        output += _ram.ToString();
                        output += "Size: " + _ram.size;
                        _ram.Flush();
                        break;
                    case LTSAlgorithm.Shortest:
                        LTS.ShortestFirst(_hdd, _ram, _sysMem, ref _readyQueue);
                        output += _ram.ToString();
                        output += "Size: " + _ram.size;
                        _ram.Flush();
                        break;
                }

                //Run the STS algorithm
                foreach (CPU cpu in _cpus)
                {
                    STS.SupplyCPU(cpu, _queues);
                    cpu.Execute();
                }

                for (int i = _queues[QueueType.IO].Count; i > 0; i--)
                {
                    _queues[QueueType.IO][i].WaitQueueCycles--;
                    if (_queues[QueueType.IO][i].WaitQueueCycles <= 0)
                    {
                        _queues[QueueType.Ready].Add(_queues[QueueType.IO][i]);
                        _queues[QueueType.IO].RemoveAt(i);
                    }
                }

                for (int i = _queues[QueueType.Waiting].Count; i > 0; i--)
                {
                    _queues[QueueType.Waiting][i].WaitQueueCycles--;
                    if (_queues[QueueType.Waiting][i].WaitQueueCycles <= 0)
                    {
                        _queues[QueueType.Ready].Add(_queues[QueueType.Waiting][i]);
                        _queues[QueueType.Waiting].RemoveAt(i);
                    }
                }

            }

            return output;
        }
    }
}
