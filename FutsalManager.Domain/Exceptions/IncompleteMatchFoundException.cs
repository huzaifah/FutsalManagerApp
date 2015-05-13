using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Runtime.Serialization;

namespace FutsalManager.Domain.Exceptions
{
    public class IncompleteMatchFoundException : Exception, ISerializable
    {
        public IncompleteMatchFoundException() : base() {}

        public IncompleteMatchFoundException(string message) : base(message) { }

        public IncompleteMatchFoundException(string message, Exception inner) : base(message, inner) { }

        public IncompleteMatchFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}