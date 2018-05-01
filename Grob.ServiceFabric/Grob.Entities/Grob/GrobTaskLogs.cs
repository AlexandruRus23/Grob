using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Entities.Grob
{
    public class GrobTaskLogs
    {
        public string Agent { get; set; }
        public string Timestamp { get; set; }
        public string Logs { get; set; }
        public bool WasSuccessful { get; set; }
        public string PrivateUri { get; set; }

        public GrobTaskLogs()
        {

        }

        public GrobTaskLogs(string agent, string timestamp, string logs, bool success, string privateUri)
        {
            Agent = agent;
            Timestamp = timestamp;
            Logs = logs;
            WasSuccessful = success;
            PrivateUri = privateUri;
        }
    }
}
