using Newtonsoft.Json;

namespace AnyStatus.Plugins.RabbitMq.Nodes.Contracts
{
    public class NodeInfo
    {
        [JsonProperty("mem_used")]
        public long UsedMemory { get; set; }

        [JsonProperty("mem_limit")]
        public long MemoryLimit { get; set; }

        [JsonProperty("disk_free_limit")]
        public long DiskLimit { get; set; }

        [JsonProperty("disk_free")]
        public long DiskFree { get; set; }

        [JsonProperty("name")]
        public string NodeName { get; set; }
    }
}