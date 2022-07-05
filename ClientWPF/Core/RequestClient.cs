﻿using JsonSubTypes;
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
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ClientWPF
{
    public class RequestClient
    {
        private TcpClient client;
        private readonly IPAddress ip;
        private readonly ushort port;
        private readonly Dispatcher dispatcher;

        public bool IsConnected { get { return client != null && client.Connected; } }

        private readonly Thread receivingThread;
        private readonly Dictionary<long,Response> responses = new Dictionary<long, Response>();
        private long sequence = 0;

        private readonly Dictionary<Type, Dictionary<object, List<Action<object>>>> callbacks = new Dictionary<Type, Dictionary<object, List<Action<object>>>>();

        public event EventHandler<RequestFailure> OnFailReceived;

        public RequestClient(IPAddress ip, ushort port, Dispatcher dispatcher)
        {
            this.ip = ip;
            this.port = port;
            this.dispatcher = dispatcher;

            receivingThread = new Thread(ReceivePackets)
            {
                IsBackground = true
            };
            receivingThread.Start();
        }

        public void SubscribeTo<T>(object sender, Action<T> callback) where T : Alert
        {
            Action<object> wrappedCallback = (o) => callback((T)o);

            lock (callbacks)
            {
                if (!callbacks.ContainsKey(typeof(T)))
                    callbacks.Add(typeof(T), new Dictionary<object, List<Action<object>>>());

                if (!callbacks[typeof(T)].ContainsKey(sender))
                    callbacks[typeof(T)].Add(sender, new List<Action<object>>());

                callbacks[typeof(T)][sender].Add(wrappedCallback);
            }
        }

        public void UnsubscribeAll(object sender)
        {
            lock (callbacks)
            {
                foreach(var type in callbacks.Values)
                {
                    if(type.ContainsKey(sender))
                        type.Remove(sender);
                }
            }
        }

        private void ReceivePackets(object obj)
        {
            int receivingSeq = 0;
            while (true)
            {
                if (IsConnected && client.Available > 0)
                {
                    using BinaryReader reader = new BinaryReader(client.GetStream(), Encoding.ASCII, leaveOpen: true);
                    string[] response = reader.ReadString().Split("\n");
                    Type type = Type.GetType(response[0]);
                    object data = JsonConvert.DeserializeObject(response[1], type, SerializationSettings.current);

                    if (type.IsSubclassOf(typeof(Alert)))
                    {
                        if (!callbacks.ContainsKey(type))
                            continue;


                        List<Action<object>> calls = new();
                        lock (callbacks)
                        {
                            foreach (var x in callbacks[type].Values)
                            {
                                calls.AddRange(x);
                            }
                        }
                        foreach (var callback in calls)
                            dispatcher.Invoke(() => callback(data));
                    }
                    else
                    {
                        if (type == typeof(RequestFailure))
                        {
                            var exception = (RequestFailure)data;

                            dispatcher.Invoke(() => OnFailReceived?.Invoke(this, exception));
                            lock (responses)
                            {
                                responses.Add(receivingSeq++, new Response(exception));
                            }
                        }
                        else
                        {
                            lock (responses)
                            {
                                responses.Add(receivingSeq++, new Response(data, type));
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

        private long Send(object request)
        {
            EnforceConnection();

            using BinaryWriter writer = new BinaryWriter(client.GetStream(), Encoding.ASCII, leaveOpen: true);
            writer.Write($"{request.GetType().AssemblyQualifiedName}\n{JsonConvert.SerializeObject(request, SerializationSettings.current)}");

            return sequence++;
        }

        public Task<Response> SendRequest<TRequest>(TRequest request, Player credentials) where TRequest : AuthenticatedRequest
        {
            request.Name = credentials.Name;
            request.SessionId = credentials.SessionToken;

            long seq = Send(request);

            return WaitForResponse(seq);
        }

        private Task<Response> WaitForResponse(long seq)
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    lock (responses)
                    {
                        if (!responses.ContainsKey(seq))
                            continue;

                        var response = responses[seq];
                        responses.Remove(seq);
                        return response;
                    }
                }

            });
        }

        public Task<Response> SendRequest(object request)
        {
            long seq = Send(request);

            return WaitForResponse(seq);
        }

        public void SendAction<TRequest>(TRequest request, Player credentials) where TRequest : AuthenticatedRequest
        {
            request.Name = credentials.Name;
            request.SessionId = credentials.SessionToken;

            Send(request);
            sequence--;
        }
    }
}
