using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class Boot
    {
        public Boot()
        {
        }

        public List<Job> Run(string inputFile)
        {
            //parse the file
            return FileParser.Parse(inputFile);
        }
    }
}
