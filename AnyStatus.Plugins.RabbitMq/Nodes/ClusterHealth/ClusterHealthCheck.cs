using AnyStatus.API;
using AnyStatus.Plugins.RabbitMq.Clients;
using AnyStatus.Plugins.RabbitMq.Nodes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.RabbitMq.Nodes.ClusterHealth
{
    public class ClusterHealthCheck : ICheckHealth<ClusterHealthCheckWidget>
    {
        public async Task Handle(HealthCheckRequest<ClusterHealthCheckWidget> request,
            CancellationToken cancellationToken)
        {
            var ctx = request.DataContext;
            var client = new RabbitMqClient(ctx.URL, ctx.Username, ctx.Password);

            try
            {
                var nodesInfo = await client.GetNodeInfosAsync(ctx.NodesUrlPath).ConfigureAwait(false);

                var errors = new List<string>();
                foreach (var nodeInfo in nodesInfo)
                {
                    var error = string.Empty;

                    if (!nodeInfo.IsMemoryHealthy(ctx.MaxMemoryUsagePercent, out var memoryErrorMessage, out _))
                    {
                        error += memoryErrorMessage;
                    }

                    if (!nodeInfo.IsHasEnoughDiskSpace(ctx.MinFreeDiskSpacePercent, out var usedDiskSpaceError, out _))
                    {
                        error += usedDiskSpaceError;
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        error = "For node " + nodeInfo.NodeName + " " + error;
                        errors.Add(error);
                    }
                }

                if (errors.Any())
                {
                    ctx.Message = string.Join(", ", errors);
                    ctx.State = State.Failed;
                }
                else
                {
                    ctx.State = State.Ok;
                }
            }
            catch (Exception e)
            {
                ctx.State = State.Failed;
                ctx.Message = e.Message;
            }
        }
    }
}