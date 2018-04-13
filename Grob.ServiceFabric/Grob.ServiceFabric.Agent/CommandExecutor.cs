using Grob.Docker;
using Grob.Agent.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Agent
{
    public class CommandExecutor
    {
        private GrobAgentCommand _command;
        private DockerManager _dockerManager;

        public CommandExecutor(GrobAgentCommand command)
        {
            _command = command;
            _dockerManager = new DockerManager();
        }

        public void Run()
        {
            switch (_command.CommandType)
            {
                case GrobAgentCommandTypeEnum.BuildImage:
                    break;
                case GrobAgentCommandTypeEnum.RunImage:
                    _dockerManager.StartContainer(_command.Command);
                    break;
            }
        }
    }
}
