using System;
using System.Runtime.Serialization;

namespace Reports.DAL.Tools
{
    public class ReportsDalExceptions : Exception
    {
        public ReportsDalExceptions()
        {
        }

        public ReportsDalExceptions(string message)
            : base(message)
        {
        }

        public ReportsDalExceptions(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ReportsDalExceptions(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}