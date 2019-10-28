namespace AnyStatus.Plugins.RabbitMq.Policies.Contracts
{
    public class Policy
    {
        public string Vhost { get; set; }
        public string Name { get; set; }
        public string Pattern { get; set; }
        public string Applyto { get; set; }
        public Definition Definition { get; set; }
        public int Priority { get; set; }
    }
}