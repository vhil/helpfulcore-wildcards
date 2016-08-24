using System;
using System.Runtime.Serialization;

namespace Helpfulcore.Wildcards
{
    [Serializable]
    public class WildcardException : Exception
    {
        public WildcardException(string message)
            : base (message)
        {
        }

        public WildcardException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public WildcardException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
