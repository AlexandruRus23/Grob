using System.Fabric;
using System.Fabric.Description;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;

namespace Grob.ServiceFabric.Agent
{
    internal class HttpCommunicationListener : ICommunicationListener
    {
        private HttpListener httpListener;
        private StatelessServiceContext context;

        public HttpCommunicationListener(string uriPrefix, string uriPublished, StatelessServiceContext context) 
        {
            this.context = context;
        }

        public void Abort()
        {
            throw new System.NotImplementedException();
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            EndpointResourceDescription endpoint = context.CodePackageActivationContext.GetEndpoint("WebEndpoint");

            string uriPrefix = $"{endpoint.Protocol}://+:{endpoint.Port}/agent/";

            this.httpListener = new HttpListener();
            this.httpListener.Prefixes.Add(uriPrefix);
            this.httpListener.Start();

            string publishUri = uriPrefix.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);
            return Task.FromResult(publishUri);
        }

        private async Task ProcessInputRequest(HttpListenerContext context, CancellationToken cancelRequest)
        {
            var message = "Hello World!";

            await context.Response.OutputStream.WriteAsync(Encoding.ASCII.GetBytes(message), 0, 0);
        }
    }
}