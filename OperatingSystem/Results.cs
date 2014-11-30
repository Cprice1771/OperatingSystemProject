using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem
{
    public class Results
    {
        public double Throughput { get; set; }
        public List<double> CPUUtilizations { get; set; }
        public List<JobResult> JobResults { get; set; }
    }
}
