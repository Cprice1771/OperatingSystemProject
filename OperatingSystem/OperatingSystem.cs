using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class OperatingSystem
    {
        HDD _hdd; //My harddrive
        RAM _ram; //My RAM
        Boot _bootloader; //Our bootloader that parses the file of insturctions
        LTSAlgorithm _algorithm; //Enum of the algorith we should use
        int _ramSize; //Size of ram in number of instructions
        int _cpuCount; //How many cpus we should use
        List<CPU> _cpus; //all of our CPU's
        int _totalCycles; //Total cycles of the big loop
        Stopwatch _throughput; //timer for throughput of 1 loop
        Result _result; //Results object for the total results of all iterations
        int _iterations; //Number of times to run the simluation

        /// <summary>
        /// Constructor for our OS
        /// </summary>
        /// <param name="algorithm">Which algorithm to use</param>
        /// <param name="ramSize">How big should ram be</param>
        /// <param name="cpuCount">How many CPU's should we use</param>
        /// <param name="iterations">How many times should we run the simulation</param>
        public OperatingSystem(LTSAlgorithm algorithm, int ramSize, int cpuCount, int iterations)
        {
            _hdd = new HDD();
            _ram = new RAM();
            SystemMemory.Flush();
            _bootloader = new Boot();
            _algorithm = algorithm;
            _ramSize = ramSize;
            _cpuCount = cpuCount;
            _totalCycles = 0;
            _throughput = new Stopwatch();
            _cpus = new List<CPU>(cpuCount);
            
            for (int i = 0; i < cpuCount; i++)
            {
                _cpus.Add(new CPU(_ram));
            }
            _result = new Result();
            _result.CPUUtilizations = new List<double>(cpuCount);
            _result.JobResults = new List<JobResult>();

            _iterations = iterations;
        }

        /// <summary>
        /// Our main method, this kicks off the os, and should be equivelent to pressing the power button on your computer
        /// </summary>
        /// <param name="inputFile">File name of the input file to get jobs from</param>
        /// <returns>Currently ruturns the contents of RAM</returns>
        public string Start(string inputFile)
        {
            for (int run = 0; run < _iterations; run++)
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


                //Add all the instructions in the file to the HDD, and create PCB's for each job
                foreach (Job j in jobs)
                {
                    //Point the PCB to its first instruction location in the HDD
                    j.JobPCB.Index = _hdd.AddInstructions(j.Instructions);
                    SystemMemory.Instance.Jobs.Add(j.JobPCB);
                }

                //Sort the pcb's
                switch (_algorithm)
                {
                    case LTSAlgorithm.FCFS:
                        //Do nothing
                        break;
                    case LTSAlgorithm.Priority:
                        SystemMemory.Instance.Jobs.Sort(LTS.CompareByPriority);
                        break;
                    case LTSAlgorithm.Shortest:
                        SystemMemory.Instance.Jobs.Sort(LTS.CompareByLength);
                        break;
                }

                _throughput.Start();
                //While we still have jobs that are not terminated
                while (SystemMemory.Instance.HasJobs)
                {
                    _totalCycles++;
                    LTS.Run(_hdd, _ram);

                    //Run the STS algorithm
                    foreach (CPU cpu in _cpus)
                    {
                        STS.SupplyCPU(cpu, ref _ram);
                        //Execute 1 instruction in the CPU
                        ThreadPool.QueueUserWorkItem(cpu.Execute);
                    }

                    //For all the jobs in the IO queue
                    for (int i = SystemMemory.Instance.Queues[QueueType.IO].Count - 1; i >= 0; i--)
                    {
                        //Subtract 1 from the cycles remaining
                        SystemMemory.Instance.Queues[QueueType.IO][i].IOQueueCycles--;
                        //if there's 0 cycles remaining 
                        if (SystemMemory.Instance.Queues[QueueType.IO][i].IOQueueCycles <= 0)
                        {
                            //move it back to the ready queue
                            SystemMemory.Instance.Queues[QueueType.IO][i].WaitingTimer.Start();
                            SystemMemory.Instance.Queues[QueueType.Ready].Add(SystemMemory.Instance.Queues[QueueType.IO][i]);
                            SystemMemory.Instance.Queues[QueueType.IO].RemoveAt(i);
                        }
                    }

                    //For all the jobs in the Wait queue
                    for (int i = SystemMemory.Instance.Queues[QueueType.Waiting].Count - 1; i >= 0; i--)
                    {
                        //Subtract 1 from the cycles remaining
                        SystemMemory.Instance.Queues[QueueType.Waiting][i].WaitQueueCycles--;
                        //if there's 0 cycles remaining 
                        if (SystemMemory.Instance.Queues[QueueType.Waiting][i].WaitQueueCycles <= 0)
                        {
                            //move it back to the ready queue
                            SystemMemory.Instance.Queues[QueueType.Waiting][i].WaitingTimer.Start();
                            SystemMemory.Instance.Queues[QueueType.Ready].Add(SystemMemory.Instance.Queues[QueueType.Waiting][i]);
                            SystemMemory.Instance.Queues[QueueType.Waiting].RemoveAt(i);
                        }
                    }

                }

                _throughput.Stop();

                //Sort the list of jobs by job number
                SystemMemory.Instance.Jobs.Sort(CompareByJobNum);

                _result.Throughput += Math.Round((double)SystemMemory.Instance.Jobs.Count / ((double)_throughput.ElapsedTicks / (double)Stopwatch.Frequency) / 1000, 5);

                for (int i = 0; i < _cpuCount; i++)
                {
                    if (_result.CPUUtilizations.Count < i + 1)
                        _result.CPUUtilizations.Add(Math.Round((double)_cpus[i].ExecutionCycles / (double)_totalCycles, 2));
                    else
                        _result.CPUUtilizations[i] += Math.Round((double)_cpus[i].ExecutionCycles / (double)_totalCycles, 2);
                }

                for (int i = 0; i < SystemMemory.Instance.Jobs.Count; i++)
                {
                    if (_result.JobResults.Count < i + 1)
                        _result.JobResults.Add(new JobResult());

                    _result.JobResults[i].JobNum = SystemMemory.Instance.Jobs[i].JobNumber;
                    _result.JobResults[i].Acc += SystemMemory.Instance.Jobs[i].Accumulator;
                    _result.JobResults[i].RegA += SystemMemory.Instance.Jobs[i].RegisterA;
                    _result.JobResults[i].RegB += SystemMemory.Instance.Jobs[i].RegisterB;
                    _result.JobResults[i].RegC += SystemMemory.Instance.Jobs[i].RegisterC;
                    _result.JobResults[i].RegD += SystemMemory.Instance.Jobs[i].RegisterD;
                    _result.JobResults[i].TurnAroundTime += Math.Round((double)SystemMemory.Instance.Jobs[i].TurnaroundTimer.Elapsed.TotalMilliseconds, 4);
                    _result.JobResults[i].WaitTime += Math.Round((double)SystemMemory.Instance.Jobs[i].WaitingTimer.Elapsed.TotalMilliseconds, 4);
                    _result.JobResults[i].ResponseTime += Math.Round((double)SystemMemory.Instance.Jobs[i].ResponseTimer.Elapsed.TotalMilliseconds, 4);
                }

                _throughput.Reset();

                SystemMemory.Flush();
                _cpus = new List<CPU>(_cpuCount);
                for (int i = 0; i < _cpuCount; i++)
                {
                    _cpus.Add(new CPU(_ram));
                }
                _totalCycles = 0;
                _ram.Flush();
                _hdd = new HDD();
            }

            return GetResults();
        }

        /// <summary>
        /// Compares 2 PCB's by job number
        /// </summary>
        /// <param name="A">First pcb to compare</param>
        /// <param name="B">second pcb to compare</param>
        /// <returns></returns>
        private static int CompareByJobNum(PCB A, PCB B)
        {
            if (A == null && B == null) return 0;
            else if (A.JobNumber > B.JobNumber) return 1;
            else if (A.JobNumber == B.JobNumber) return 0;
            else return -1;
        }

        /// <summary>
        /// Returns the results of the run as a block of text
        /// </summary>
        /// <returns>The results in a formated string</returns>
        private string GetResults()
        {
            string output = "Throughput: " + Math.Round((_result.Throughput / _iterations), 4) + " Job/ms \n";

            double sum = 0.0;

            for(int i = 0; i < _result.CPUUtilizations.Count; i++)
            {
                double util = Math.Round(_result.CPUUtilizations[i] / _iterations, 3);
                sum += util;
                output += "CPU: " + i + " Utilization: " + util + "\n";
            }

            output += "Total CPU Utilization: " + sum / _result.CPUUtilizations.Count + "\n";

            double TurnAroundSum = 0.0;
            double WaitSum = 0.0;
            double ResponseSum = 0.0;
            //return out the header
            output += "Job\tAcc\t A\t B\t C\t D \tTA \t Wait \t Resp\n"; 
            //return out all the jobs
            for (int i = 0; i < _result.JobResults.Count; i++)
            {
                output += _result.JobResults[i].JobNum + "\t";
                output += _result.JobResults[i].Acc / _iterations + "\t ";
                output += _result.JobResults[i].RegA / _iterations + "\t ";
                output += _result.JobResults[i].RegB / _iterations + "\t ";
                output += _result.JobResults[i].RegC / _iterations + "\t ";
                output += _result.JobResults[i].RegD / _iterations + "\t ";
                TurnAroundSum += Math.Round(_result.JobResults[i].TurnAroundTime / _iterations, 4);
                output += Math.Round(_result.JobResults[i].TurnAroundTime / _iterations, 4) + "\t ";
                WaitSum += Math.Round(_result.JobResults[i].WaitTime / _iterations, 4);
                output += Math.Round(_result.JobResults[i].WaitTime / _iterations, 4) + "\t ";
                ResponseSum += Math.Round(_result.JobResults[i].ResponseTime / _iterations, 4);
                output += Math.Round(_result.JobResults[i].ResponseTime / _iterations, 4) + "\n";
            }

            output += "Job Totals: \nTA \t Wait \t Resp\n";

            output += Math.Round(TurnAroundSum / _result.JobResults.Count, 4) + " \t";
            output += Math.Round(WaitSum / _result.JobResults.Count, 4) + " \t";
            output += Math.Round(ResponseSum / _result.JobResults.Count, 4)  + " \n";
            



            return output;
        }
    }
}
