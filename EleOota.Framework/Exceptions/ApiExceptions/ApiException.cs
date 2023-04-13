using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EleOota.FrameworkExceptions.ApiExceptions
{
    [Serializable]
    public abstract class ApiException<T> : ApiException
    {
        private T details;
        protected ApiException (string message) : base(message) { }
        protected ApiException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext) { }
        public new T Details
        {
            get => details; protected set { details = value; base.Details = value; }
        }
    }
    [Serializable]
    public class ApiException : Exception
    {
        protected ApiException(string message) : base(message) { }

        protected ApiException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext) { }

        public int Status { get; protected set; }

        public Object Details { get; protected set; }
    }
}
