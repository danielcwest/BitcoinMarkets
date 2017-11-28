using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WebSocket4Net;

namespace HitbtcSharp.RPC
{
    public class RpcClient : RpcProcessor
    {
        private WebSocket _client;

        public RpcClient(string uri)
        {
            _client = new WebSocket(uri);
            _client.NoDelay = true;

            _client.Opened += (sender, args) =>
                           Task.Run(() => DispatchConnected());

            _client.Closed += (sender, args) =>
                Task.Run(() => SafeCall(DispatchDisconnected));

            Send = message => SafeCall(() =>
            {
                var data = RpcMessage.Write(message);
                _client.Send(data);
            });

            _client.MessageReceived += (sender, args) => SafeCall(() =>
            {
                var data = args.Message;
                var message = RpcMessage.Read(data);
                Task.Run(() => ProcessMessage(message));
            });

            Task.Run(() => SafeCall(_client.Open));
        }

        public override void Dispose()
        {
            if (_client != null)
                ((IDisposable)_client).Dispose();

            base.Dispose();
        }

        private void SafeCall(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                DispatchError(e);
            }
        }
    }
}
