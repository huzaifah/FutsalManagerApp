using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Exceptions
{
    public class TeamNotFoundException : Exception, ISerializable
    {
        public TeamNotFoundException() : base() {}

        public TeamNotFoundException(string message) : base(message) { }

        public TeamNotFoundException(string message, Exception inner) : base(message, inner) { }

        public TeamNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
