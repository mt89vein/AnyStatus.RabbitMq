using AnyStatus.API;

namespace AnyStatus.Plugins.RabbitMq.Policies
{
    public class OpenRabbitMqWebPage : OpenWebPage<PoliciesWidget>
    {
        public OpenRabbitMqWebPage(IProcessStarter ps) : base(ps)
        {
        }
    }
}