using AnyStatus.API;

namespace AnyStatus.Plugins.RabbitMq.Nodes.ClusterHealth
{
    public class OpenRabbitMqWebPage : OpenWebPage<ClusterHealthCheckWidget>
    {
        public OpenRabbitMqWebPage(IProcessStarter ps) : base(ps)
        {
        }
    }
}