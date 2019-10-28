using AnyStatus.API;
using AnyStatus.Plugins.RabbitMq.Abstractions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.RabbitMq.Nodes.MemoryUsage
{
    [DisplayName("Node memory usage")]
    [DisplayColumn("RabbitMq")]
    [Description("Node memory usage.")]
    public class SingleNodeMemoryWidget : Sparkline, IRabbitMqWidget, ISchedulable
    {
        private const string CATEGORY = "RabbitMq.NodeMemoryUsage";

        [Url]
        [Required]
        [PropertyOrder(10)]
        [Category(CATEGORY)]
        [Description("RabbitMq management api url")]
        public string URL { get; set; } = "http://localhost:15672";

        [Required]
        [PropertyOrder(20)]
        [Category(CATEGORY)]
        [XmlIgnore]
        public string Username { get; set; } = "guest";

        [Required]
        [PropertyOrder(30)]
        [Category(CATEGORY)]
        [XmlIgnore]
        [Editor(typeof(PasswordEditor), typeof(PasswordEditor))]
        public string Password { get; set; } = "guest";

        [Required]
        [PropertyOrder(40)]
        [Category(CATEGORY)]
        [Description("Url segment to retrive nodes info.")]
        public string NodesUrlPath { get; set; } = "/api/nodes";

        [Required]
        [Category(CATEGORY)]
        [PropertyOrder(50)]
        [Description("Node name.")]
        public string NodeName { get; set; }

        [Required]
        [PropertyOrder(60)]
        [Category(CATEGORY)]
        [Description("Max node memory usage in percent.")]
        public int MaxMemoryUsagePercent { get; set; } = 85;

        public SingleNodeMemoryWidget()
        {
            Name = "Node memory usage";
            MaxValue = 100;
            Symbol = "%";
            Interval = 1;
            Units = IntervalUnits.Minutes;
        }

        public override Notification CreateNotification()
        {
            return State == State.Failed
                ? new Notification(Message, NotificationIcon.Error)
                : Notification.Empty;
        }
    }
}
