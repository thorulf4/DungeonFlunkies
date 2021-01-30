using Newtonsoft.Json;
using Shared;
using Shared.Alerts;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace ClientWPF
{
    public class RequestClient
    {
        private TcpClient client;
        private IPAddress ip;
        private ushort port;
        private Dispatcher dispatcher;

        public bool IsConnected { get { return client != null && client.Connected; } }

        private Thread receivingThread;
        private Queue<Response> responses = new Queue<Response>();
        private Dictionary<Type, Dictionary<object, List<Action<object>>>> callbacks = new Dictionary<Type, Dictionary<object, List<Action<object>>>>();


        public RequestClient(IPAddress ip, ushort port, Dispatcher dispatcher)
        {
            this.ip = ip;
            this.port = port;
            this.dispatcher = dispatcher;

            receivingThread = new Thread(ReceivePackets);
            receivingThread.IsBackground = true;
            receivingThread.Start();
        }

        public void SubscribeTo<T>(object sender, Action<T> callback) where T : Alert
        {
            Action<object> wrappedCallback = (o) => callback((T)o);

            if (!callbacks.ContainsKey(typeof(T)))
                callbacks.Add(typeof(T), new Dictionary<object, List<Action<object>>>());

            if (!callbacks[typeof(T)].ContainsKey(sender))
                callbacks[typeof(T)].Add(sender, new List<Action<object>>());

            callbacks[typeof(T)][sender].Add(wrappedCallback);
        }

        public void Unsubscribe(object sender)
        {
            foreach(var type in callbacks.Values)
            {
                if(type.ContainsKey(sender))
                    type.Remove(sender);
            }
        }

        private void ReceivePackets(object obj)
        {
            while (true)
            {
                if (IsConnected && client.Available > 0)
                {
                    using (BinaryReader reader = new BinaryReader(client.GetStream(), Encoding.ASCII, leaveOpen: true))
                    {
                        string[] response = reader.ReadString().Split("\n");
                        Type type = Type.GetType(response[0]);
                        object data = JsonConvert.DeserializeObject(response[1], type);

                        if (type.IsSubclassOf(typeof(Alert)))
                        {
                            foreach(var x in callbacks[type].Values)
                            {
                                foreach (var callback in x)
                                    dispatcher.Invoke(() => callback(data));
                            }
                        }
                        else
                        {
                            lock (responses)
                            {
                                if (type == typeof(RequestFailure))
                                    responses.Enqueue(new Response((RequestFailure)data));
                                else
                                    responses.Enqueue(new Response(data, type));
                            }
                        }
                    }
                }
                else
                {
                    EnforceConnection();
                }
            }
        }

        public void EnforceConnection()
        {
            if (!IsConnected)
            {
                if (client != null)
                    client.Dispose();

                client = new TcpClient();
                client.Connect(new IPEndPoint(ip, port));

                //TODO send a reconnection packet to resubscribe to alerts
            }
        }

        private void Send(object request)
        {
            EnforceConnection();

            using (BinaryWriter writer = new BinaryWriter(client.GetStream(), Encoding.ASCII, leaveOpen: true))
            {
                writer.Write($"{request.GetType().AssemblyQualifiedName}\n{JsonConvert.SerializeObject(request)}");
            }
        }

        public Response SendRequest<TRequest>(TRequest request, Player credentials) where TRequest : AuthenticatedRequest
        {
            request.Name = credentials.name;
            request.SessionId = credentials.sessionToken;

            Send(request);
            return WaitForResponse();
        }

        public Response SendRequest(object request)
        {
            Send(request);
            return WaitForResponse();
        }

        public void SendAction<TRequest>(TRequest request, Player credentials) where TRequest : AuthenticatedRequest
        {
            request.Name = credentials.name;
            request.SessionId = credentials.sessionToken;

            Send(request);
        }

        private Response WaitForResponse()
        {
            while (responses.Count == 0) ;
            return responses.Dequeue();
        }
    }
}
