using Newtonsoft.Json;
using Shared;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientWPF
{
    public class RequestClient
    {
        private TcpClient client;
        private IPAddress ip;
        private ushort port;

        public bool IsConnected { get { return client != null && client.Connected; } }

        public RequestClient(IPAddress ip, ushort port)
        {
            this.ip = ip;
            this.port = port;
        }

        public void EnforceConnection()
        {
            if (!IsConnected)
            {
                if (client != null)
                    client.Dispose();

                client = new TcpClient();
                client.Connect(new IPEndPoint(ip, port));
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

        private Response WaitForResponse()
        {
            using (BinaryReader reader = new BinaryReader(client.GetStream(), Encoding.ASCII, leaveOpen: true))
            {
                string[] response = reader.ReadString().Split("\n");
                Type type = Type.GetType(response[0]);
                object data = JsonConvert.DeserializeObject(response[1], type);

                if (type == typeof(RequestFailure))
                    return new Response((RequestFailure)data);
                else
                    return new Response(data, type);
            }
        }
    }
}
