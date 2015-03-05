using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Exceptions
{
    public class ExceedTotalTeamsException : Exception, ISerializable
    {
        public ExceedTotalTeamsException() : base() {}

        public ExceedTotalTeamsException(string message) : base(message) { }

        public ExceedTotalTeamsException(string message, Exception inner) : base(message, inner) { }

        public ExceedTotalTeamsException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
