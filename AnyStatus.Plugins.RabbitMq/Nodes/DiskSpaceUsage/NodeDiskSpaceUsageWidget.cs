using AnyStatus.API;
using AnyStatus.Plugins.RabbitMq.Abstractions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.RabbitMq.Nodes.DiskSpaceUsage
{
    [DisplayName("Node disk space usage")]
    [DisplayColumn("RabbitMq")]
    [Description("Node disk space usage.")]
    public class NodeDiskSpaceUsageWidget : Sparkline, IRabbitMqWidget, ISchedulable
    {
        private const string CATEGORY = "RabbitMq.NodeDiskSpaceUsage";

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
        [PropertyOrder(50)]
        [Category(CATEGORY)]
        [Description("Node name.")]
        public string NodeName { get; set; }

        [Required]
        [PropertyOrder(60)]
        [Category(CATEGORY)]
        [Description("Min node free disk space.")]
        public int MinFreeDiskSpacePercent { get; set; } = 25;

        public NodeDiskSpaceUsageWidget()
        {
            Name = "Node disk space usage";
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
