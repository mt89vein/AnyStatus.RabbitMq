using AnyStatus.API;
using AnyStatus.Plugins.RabbitMq.Abstractions;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.RabbitMq.Policies
{
    [DisplayName("Policies")]
    [DisplayColumn("RabbitMq")]
    [Description("Ensures that policies is set.")]
    public class PoliciesWidget : Build, IRabbitMqWidget, ISchedulable, IHealthCheck
    {
        private const string CATEGORY = "RabbitMq.Policies";

        [Url]
        [Required]
        [PropertyOrder(10)]
        [Category(CATEGORY)]
        [Description("RabbitMQ Management api Url")]
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
        public string VirtualHost { get; set; } = "/";

        [Required]
        [PropertyOrder(50)]
        [Category(CATEGORY)]
        [Description("Policy names to check existance.")]
        public List<string> RequiredPolicies { get; set; } = new List<string>();

        [Required]
        [PropertyOrder(60)]
        [Category(CATEGORY)]
        [Description("Url segment to retrive policies.")]
        public string PoliciesUrlPath { get; set; } = "/api/policies";

        public PoliciesWidget()
        {
            Name = "Policies checker";
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