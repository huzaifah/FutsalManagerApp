using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Exceptions
{
    public class PlayerNotFoundException : Exception, ISerializable
    {
        public PlayerNotFoundException() : base() { }

        public PlayerNotFoundException(string message) : base(message) { }

        public PlayerNotFoundException(string message, Exception inner) : base(message, inner) { }

        public PlayerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
