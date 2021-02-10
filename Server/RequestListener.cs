using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Server.Pipelining;
using Server.RequestHandlers;
using Shared;
using Shared.Alerts;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class RequestListener : IAlerter
    {
        private readonly Dictionary<Type, Type> registeredRequests = new Dictionary<Type, Type>();
        private readonly TcpListener tcpListener;
        private IServiceProvider serviceProvider;
        private readonly Pipeline pipeline;
        private Type interactionHandler;

        private readonly Dictionary<Thread, string> users = new Dictionary<Thread, string>();
        private readonly List<AlertBroadcast> alerts = new List<AlertBroadcast>();

        public RequestListener(ushort port)
        {
            pipeline = new Pipeline();
            tcpListener = new TcpListener(IPAddress.Any, port);
        }

        public ListenerBuilder Configure()
        {
            return new ListenerBuilder(this);
        }

        public void RegisterUser(string username)
        {
            lock (users)
            {
                if (users.ContainsKey(Thread.CurrentThread))
                    users.Remove(Thread.CurrentThread);

                users.Add(Thread.CurrentThread, username);
            }
        }

        public void SendAlerts<T>(T alert, ICollection<string> receivers) where T : Alert
        {
            List<string> activeUsers = users.Values.Intersect(receivers).ToList();
            lock (alerts)
            {
                alerts.Add(new AlertBroadcast(activeUsers, alert));
            }
        }

        private void RegisterRequest<TR, TH>()
        {
            registeredRequests.Add(typeof(TR), typeof(TH));
        }

        private void StartListening()
        {

            tcpListener.Start();

            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();

                ThreadPool.QueueUserWorkItem(HandleConnection, client);
            }
        }

        private void HandleConnection(object obj)
        {
            TcpClient client = obj as TcpClient; 
            Stopwatch watch = Stopwatch.StartNew();

            using (NetworkStream stream = client.GetStream())
            {
                while (client.Connected)
                {
                    if (stream.DataAvailable)
                    {
                        HandleRequest(stream);

                        watch.Restart();
                    }

                    HandleAlerts(stream);
                }

                stream.Flush();
            }

            client.Close();

            lock (users)
            {
                if(users.ContainsKey(Thread.CurrentThread))
                    users.Remove(Thread.CurrentThread);
            }
        }

        private void HandleAlerts(NetworkStream stream)
        {
            string user = null;
            lock (users)
            {
                if(users.ContainsKey(Thread.CurrentThread))
                    user = users[Thread.CurrentThread];
            }

            if (user != null)
            {
                
                bool sentAlerts = false;

                for (int i = 0; i < alerts.Count; i++)
                {
                    AlertBroadcast alertBroadcast = alerts[i];
                    if (alertBroadcast == null)
                        continue;

                    bool shouldSend = false;
                    lock (alertBroadcast)
                    {
                        shouldSend = alertBroadcast.Receivers.Contains(user);
                    }

                    if (shouldSend)
                    {
                        SendResponse(Response.From(alertBroadcast.Content), stream);
                        sentAlerts = true;
                    }
                }

                //Cleanup
                if (sentAlerts)
                {
                    lock (alerts)
                    {
                        for(int i = alerts.Count -1; i >= 0; i--)
                        {
                            alerts[i].Receivers.Remove(user);
                            if (alerts[i].Receivers.Count == 0)
                                alerts.RemoveAt(i);
                        }
                    }
                }
            }
        }

        private void HandleRequest(NetworkStream stream)
        {
            using BinaryReader reader = new BinaryReader(stream, Encoding.ASCII, leaveOpen: true);
            string[] request = reader.ReadString().Split("\n");
            string type = request[0];
            string json = request[1];

            Response response = CallRequestHandler(type, json);

            if (response.HasData)
                SendResponse(response, stream);
        }

        private void SendResponse(Response response, Stream stream)
        {
            using BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII, leaveOpen: true);
            writer.Write(response.DataString);
        }

        private Response CallRequestHandler(string strType, string json)
        {
            Type type = Type.GetType(strType);
            object data = JsonConvert.DeserializeObject(json, type);

            if(data is InteractionRequest)
            {
                var handler = (IHandler)serviceProvider.GetRequiredService(interactionHandler);

                return pipeline.SendRequest(data, handler);
            }
            else
            {
                var handlerType = registeredRequests[type];
                var handler = (IHandler)serviceProvider.GetRequiredService(handlerType);

                return pipeline.SendRequest(data, handler);
            }

            
        }

        public class ListenerBuilder
        {
            private readonly RequestListener subject;
            private readonly IServiceCollection services;
            private bool isBuilt;

            public ListenerBuilder(RequestListener subject)
            {
                this.subject = subject;
                isBuilt = false;

                services = new ServiceCollection();
            }

            public ListenerBuilder BuildDependencies(Action<IServiceCollection> addDependencies)
            {
                IsBuiltCheck();
                addDependencies(services);
                return this;
            }

            private void IsBuiltCheck()
            {
                if (isBuilt)
                    throw new Exception("Listener has already started and therefor cannot be configured further");
            }

            public ListenerBuilder AddHandler<TR, TH>() where TH : RequestHandler<TR>
            {
                IsBuiltCheck();

                services.AddTransient<TH>();
                subject.RegisterRequest<TR, TH>();

                return this;
            }

            public void StartListening()
            {
                IsBuiltCheck();
                isBuilt = true;

                var serviceProvider = services.BuildServiceProvider();
                subject.AddServiceProvider(serviceProvider);

                subject.StartListening();
            }

            public ListenerBuilder AddMiddleware(IMiddleware middleware)
            {
                IsBuiltCheck();

                subject.pipeline.AddMiddleware(middleware);
                return this;
            }

            public ListenerBuilder AddInteractionHandler<T>() where T : class
            {
                IsBuiltCheck();

                services.AddTransient<T>();
                subject.interactionHandler = typeof(T);

                return this;
            }
        }

        private void AddServiceProvider(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        
    }
}
