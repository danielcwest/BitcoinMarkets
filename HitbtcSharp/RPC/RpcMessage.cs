using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace HitbtcSharp.RPC
{
    internal enum RpcMessageType
    {
        Request, Response, Notification
    }

    internal interface IRpcMessage
    {
        RpcMessageType Type { get; }
    }

    internal class RpcRequest : IRpcMessage
    {
        [JsonIgnore]
        public RpcMessageType Type { get { return RpcMessageType.Request; } }

        [JsonProperty("id")]
        public string Id;

        [JsonProperty("method")]
        public string Method;

        [JsonProperty("params")]
        public JToken Parameters;
    }

    internal class RpcResponse : IRpcMessage
    {
        [JsonIgnore]
        public RpcMessageType Type { get { return RpcMessageType.Response; } }

        [JsonProperty("id")]
        public string Id;

        [JsonProperty("result")]
        public JToken Result;

        [JsonProperty("error")]
        public string Error;
    }

    internal class RpcNotification : IRpcMessage
    {
        [JsonIgnore]
        public RpcMessageType Type { get { return RpcMessageType.Notification; } }

        [JsonProperty("params")]
        public JToken Payload;

        [JsonProperty("method")]
        public string Method;
    }

    internal static class RpcMessage
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static IRpcMessage Read(string data)
        {
            IRpcMessage message;
            dynamic msg = JsonConvert.DeserializeObject<dynamic>(data);

            if(msg.id == null)//Notification
            {
                message = JsonConvert.DeserializeObject<RpcNotification>(data);
            }
            else if(msg.result != null)
            {
                message = JsonConvert.DeserializeObject<RpcResponse>(data);
            }
            else
            {
                logger.Error(msg);
                throw new Exception("Request???");

            }

            return message;
        }

        public static string Write(IRpcMessage message)
        {
            var data = JsonConvert.SerializeObject(message);
            return data;
        }
    }
}
