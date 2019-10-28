using AnyStatus.API;

namespace AnyStatus.Plugins.RabbitMq.Nodes.DiskSpaceUsage
{
    public class OpenRabbitMqWebPage : OpenWebPage<NodeDiskSpaceUsageWidget>
    {
        public OpenRabbitMqWebPage(IProcessStarter ps) : base(ps)
        {
        }
    }
}