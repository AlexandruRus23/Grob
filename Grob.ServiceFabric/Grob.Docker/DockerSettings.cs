using Grob.Docker.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Docker
{
	public static class DockerSettings
	{
		public static string DockerUri => Settings.Default.DockerUri;
		public static string DockerEmail => Settings.Default.DockerEmail;
		public static string DockerUserName => Settings.Default.DockerUsername;
		public static string DockerPassword => Settings.Default.DockerPassword;
		
	}
}
