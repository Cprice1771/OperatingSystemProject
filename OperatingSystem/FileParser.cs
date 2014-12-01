using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public static class FileParser
    {
        /// <summary>
        /// Parses a text file for jobs
        /// </summary>
        /// <param name="file">The path to the file to parse</param>
        /// <returns>A list of all jobs</returns>
        internal static List<Job> Parse(string file)
        {
            List<Job> jobs = new List<Job>();
            //Open up the given file
            using (StreamReader reader = File.OpenText(file))
            {
                string line;
                Job currentJob;
                Instruction currentInstruction;
                //for even line in the file
                while ((line = reader.ReadLine()) != null)
                {
                    //split up the contents by commas
                    string[] arguments = line.Split(',');
                    //First line is the job header
                    //Get the job number
                    int jobNum = Int32.Parse(arguments[0].Substring(4, arguments[0].Length - 4));
                    //Job length
                    int length = Int32.Parse(arguments[1]);
                    //Priortity of the job
                    byte priority = byte.Parse(arguments[2]);
                    //Create a new job object
                    currentJob = new Job(priority, jobNum);
                    //Now parse the instructions for the job
                    for (int i = 0; i < length; i++)
                    {
                        line = reader.ReadLine();
                        //Again comma seperated
                        string[] instructionArgs = line.Split(',');
                        //get the instruction number
                        int instructionNumber = Int32.Parse(instructionArgs[0]);
                        //Then the command, and parse it to an enum
                        CommandType command = Enums.ParseInstruction(instructionArgs[1].Trim());
                        //First arguemnt and parse it to a register enum
                        Register arg1 = Enums.ParseRegister(instructionArgs[2].Trim());
                        //second arguemnt and parse it to a register enum
                        Register arg2 = Enums.ParseRegister(instructionArgs[3].Trim());
                        //Third argument is a byte
                        byte arg3 = byte.Parse(instructionArgs[4]);
                        //Create a new instruction object
                        currentInstruction = new Instruction(instructionNumber, command, arg1, arg2, arg3);
                        //Add the instruction to the job
                        currentJob.AddInstruction(currentInstruction);
                    }

                    //Add the job with all the instructions attatched
                    jobs.Add(currentJob);
                }
            }

            //Return all the jobs in the file
            return jobs;
        }
    }
}
