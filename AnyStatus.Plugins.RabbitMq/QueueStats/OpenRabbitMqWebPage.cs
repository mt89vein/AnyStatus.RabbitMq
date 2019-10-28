using AnyStatus.API;

namespace AnyStatus.Plugins.RabbitMq.QueueStats
{
    public class OpenRabbitMqWebPage : OpenWebPage<QueueStatsWidget>
    {
        public OpenRabbitMqWebPage(IProcessStarter ps) : base(ps)
        {
        }
    }
}