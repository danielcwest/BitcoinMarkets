using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HitbtcSharp.RPC
{
    public static class RpcProcessorExtensions
    {
        public static void On<T, TResult>(this RpcProcessor rpc, string method, Func<T, Task<TResult>> handler)
        {
            rpc.Register(method, async (sender, payload) =>
            {
                var result = await handler(
                    FromParameter<T>(method, payload)
                    );
                return ToReturn(result);
            });
        }

        public static void On<T>(this RpcProcessor rpc, string method, Func<T> handler)
        {
            rpc.On(method, async () => await Task.Run(handler));
        }


        public static async Task<TResult> Call<TResult>(this RpcProcessor rpc, string method)
        {
            var result = await rpc.Invoke(method, new JToken[0]);
            return FromReturn<TResult>(method, result);
        }

        public static async Task<TResult> Call<T, TResult>(this RpcProcessor rpc, string method, T arg1)
        {
            var parameters = new[]
            {
                ToParameter(arg1)
            };

            var result = await rpc.Invoke(method, parameters);
            return FromReturn<TResult>(method, result);
        }

        public static async Task<TResult> Call<T1, T2, TResult>(this RpcProcessor rpc, string method, T1 arg1, T2 arg2)
        {
            var parameters = new[]
            {
                ToParameter(arg1),
                ToParameter(arg2)
            };

            var result = await rpc.Invoke(method, parameters);
            return FromReturn<TResult>(method, result);
        }

        public static async Task<TResult> Call<T1, T2, T3, TResult>(this RpcProcessor rpc, string method, T1 arg1, T2 arg2, T3 arg3)
        {
            var parameters = new[]
            {
                ToParameter(arg1),
                ToParameter(arg2),
                ToParameter(arg3)
            };

            var result = await rpc.Invoke(method, parameters);
            return FromReturn<TResult>(method, result);
        }

        public static async Task<TResult> Call<T1, T2, T3, T4, TResult>(this RpcProcessor rpc, string method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var parameters = new[]
            {
                ToParameter(arg1),
                ToParameter(arg2),
                ToParameter(arg3),
                ToParameter(arg4)
            };

            var result = await rpc.Invoke(method, parameters);
            return FromReturn<TResult>(method, result);
        }

        private static void CheckParameters(string method, JToken[] parameters, int expectedLength)
        {
            if (parameters.Length != expectedLength)
                throw new InvalidOperationException(string.Format("Method '{0}' requires {1} arguments", method, expectedLength));
        }

        private static void CheckPayload(string method, JToken payload)
        {
            Console.WriteLine(payload);
        }

        private static T FromParameter<T>(string method, JToken payload)
        {
            try
            {
                return payload.ToObject<T>();
            }
            catch
            {
                throw new InvalidOperationException(string.Format("Payload for method '{0}' must be of type '{1}'", method, typeof(T).FullName));
            }
        }

        // for On
        private static T FromParameters<T>(string method, JToken[] parameters, int index)
        {
            try
            {
                return parameters[index].ToObject<T>();
            }
            catch
            {
                throw new InvalidOperationException(string.Format("Argument {0} for method '{1}' must be of type '{2}'", index + 1, method, typeof(T).FullName));
            }
        }
        
        // for On
        private static JToken ToReturn<T>(T value)
        {
            if (ReferenceEquals(value, null)) return new JValue((object)null);
            return JToken.FromObject(value); // TODO: should this be error checked?
        }

        // for Call
        private static JToken ToParameter<T>(T value)
        {
            if (ReferenceEquals(value, null)) return new JValue((object)null);
            return JToken.FromObject(value); // TODO: should this be error checked?
        }

        // for Call
        private static T FromReturn<T>(string method, JToken value)
        {
            try
            {
                return value.ToObject<T>();
            }
            catch
            {
                throw new Exception(string.Format("Return value for method '{0}' must be of type '{1}'", method, typeof(T).FullName));
            }
        }
    }
}
