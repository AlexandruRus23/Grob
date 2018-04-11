using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Entities
{
    public class GrobAgentCommand
    {
        public GrobAgentCommandTypeEnum CommandType { get; set; }
        public string Command { get; set; }

        public GrobAgentCommand(GrobAgentCommandTypeEnum commandType, string command)
        {
            CommandType = commandType;
            Command = command;
        }
    }
}
