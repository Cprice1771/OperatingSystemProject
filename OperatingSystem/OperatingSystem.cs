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
        Queue<PCB> _readyQueue; //Ready queue of PCB's ready to be run
        Queue<PCB> _waitQueue; //Wait queue of PCB's in wait steps

        public OperatingSystem(LTSAlgorithm algorithm, int ramSize, int cpuCount)
        {
            _hdd = new HDD();
            _ram = new RAM();
            _sysMem = new SystemMemory();
            _bootloader = new Boot();
            _algorithm = algorithm;
            _ramSize = ramSize;
            _cpuCount = cpuCount;
            _readyQueue = new Queue<PCB>();
            _waitQueue = new Queue<PCB>();
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

            return output;
        }
    }
}
