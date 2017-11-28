using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

    internal class RpcNotification : IRpcMessage
    {
        [JsonIgnore]
        public RpcMessageType Type { get { return RpcMessageType.Notification; } }

        [JsonProperty("method")]
        public string Method;

        [JsonProperty("params")]
        public JToken Payload;
    }

    internal class RpcRequest2 : IRpcMessage
    {
        [JsonIgnore]
        public RpcMessageType Type { get { return RpcMessageType.Request; } }

        [JsonProperty("method")]
        public string Method;

        [JsonProperty("params")]
        public JToken Parameters;

        [JsonProperty("id")]
        public string Id;
    }

    internal class RpcRequest : IRpcMessage
    {
        [JsonIgnore]
        public RpcMessageType Type { get { return RpcMessageType.Request; } }

        [JsonProperty("id")]
        public string Id;

        [JsonProperty("params")]
        public JToken[] Parameters;

        [JsonProperty("method")]
        public string Method;
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

    internal static class RpcMessage
    {
        public static IRpcMessage Read(string json)
        {
            // var type = data.Substring(0, 3);
            // var json = data.Substring(3);
            IRpcMessage message;
            dynamic data = JsonConvert.DeserializeObject<dynamic>(json);

            
            if(data.id == null)//Notification
            {
                message = JsonConvert.DeserializeObject<RpcNotification>(json);
            }
            else if(data.result != null)//Response
            {
                message = JsonConvert.DeserializeObject<RpcResponse>(json);
            }
            else
            {
                throw new Exception("Request???");
            }

            //switch (type)
            //{
            //    case "req":
            //        message = JsonConvert.DeserializeObject<RpcRequest>(json);
            //        break;
            //    case "res":
            //        message = JsonConvert.DeserializeObject<RpcResponse>(json);
            //        break;
            //    default:
            //        message = JsonConvert.DeserializeObject<RpcResponse>(data);
            //        break;
            //}

            return message;
        }

        public static string Write(IRpcMessage message)
        {
            //string type;
            //switch (message.Type)
            //{
            //    case RpcMessageType.Request:
            //        type = "req";
            //        break;
            //    case RpcMessageType.Response:
            //        type = "res";
            //        break;
            //    default:
            //        throw new NotSupportedException(string.Format("Unsupported message type '{0}'", message.Type));
            //}

            //var data = type + JsonConvert.SerializeObject(message);
            return JsonConvert.SerializeObject(message);
        }
    }
}
