using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOlib
{
    internal class Jobdto
    {
        public string CratedAt { get; set; }
        public string LastExecution { get; set; }

        public string NextExecution { get; set; }

        public string JobID { get; set; }

        public string Cron {  get; set; }



    }
}
