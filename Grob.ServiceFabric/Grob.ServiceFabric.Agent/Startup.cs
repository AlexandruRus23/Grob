namespace Grob.ServiceFabric.Agent
{
    using Grob.ServiceFabric.Agent.App_Start;
    using Owin;
    using System.Web.Http;

    public class Startup : IOwinAppBuilder
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            FormatterConfig.ConfigureFormatters(config.Formatters);

            appBuilder.UseWebApi(config);
        }
    }
}