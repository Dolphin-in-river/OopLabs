using System;
using System.Runtime.Serialization;

namespace Reports.Server.Tools
{
    public class ReportServerException : Exception
    {
        public ReportServerException()
        {
        }

        public ReportServerException(string message)
            : base(message)
        {
        }

        public ReportServerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ReportServerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
