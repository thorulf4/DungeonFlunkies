using JsonSubTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Response
    {
        public readonly RequestFailure exception;
        public readonly object data;
        public readonly Type type;

        public bool HasData => data != null;
        public string DataString => $"{type.AssemblyQualifiedName}\n{JsonConvert.SerializeObject(data, SerializationSettings.current)}";
        public bool Success => exception == null;

        public Response(object data, Type type)
        {
            this.data = data;
            this.type = type;
        }

        public Response(RequestFailure failure)
        {
            exception = failure;
            data = exception;
            type = typeof(RequestFailure);
        }

        public static Response Ok()
        {
            return new Response(null, null);
        }

        //public static Response From<T>(T data)
        //{
        //    return new Response(data, typeof(T));
        //}

        public static Response From(object data)
        {
            return new Response(data, data.GetType());
        }

        public static Response Fail(string message)
        {
            return new Response(new RequestFailure(message));
        }

    }
}
