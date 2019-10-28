using AnyStatus.API;
using AnyStatus.Plugins.RabbitMq.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.RabbitMq.Policies
{
    public class PoliciesHealthCheck : ICheckHealth<PoliciesWidget>
    {
        public async Task Handle(HealthCheckRequest<PoliciesWidget> request, CancellationToken cancellationToken)
        {
            var ctx = request.DataContext;
            var client = new RabbitMqClient(ctx.URL, ctx.Username, ctx.Password);

            try
            {
                var policies = await client.GetPoliciesAsync(ctx.PoliciesUrlPath, ctx.VirtualHost).ConfigureAwait(false);

                var missedPolicies = new List<string>();
                foreach (var requiredPolicy in ctx.RequiredPolicies)
                {
                    if (!policies.Any(x => x.Name == requiredPolicy))
                    {
                        missedPolicies.Add(requiredPolicy);
                    }
                }

                if (missedPolicies.Any())
                {
                    ctx.Message = "Missed policies: " + string.Join(", ", missedPolicies);
                    ctx.State = State.Failed;
                }
                else
                {
                    ctx.State = State.Ok;
                }
            }
            catch (Exception e)
            {
                ctx.Message = e.Message;
                ctx.State = State.Failed;
            }
        }
    }
}