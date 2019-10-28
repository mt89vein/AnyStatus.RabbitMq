using AnyStatus.API;

namespace AnyStatus.Plugins.RabbitMq.Nodes.MemoryUsage
{
    public class OpenRabbitMqWebPage : OpenWebPage<SingleNodeMemoryWidget>
    {
        public OpenRabbitMqWebPage(IProcessStarter ps) : base(ps)
        {
        }
    }
}