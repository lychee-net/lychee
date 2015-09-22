using System;
using System.Runtime.Serialization;

namespace Lychee.Core
{
    [Serializable]
    public class VerifyException : Exception
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public VerifyException()
        {
        }

        /// <summary>
        /// Constructs an exception with a supplied message
        /// </summary>
        /// <param name="message">exception message</param>
        public VerifyException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructs an exception with a supplied message, with information as to what caused it
        /// </summary>
        /// <param name="message">exception message</param>
        /// <param name="inner">cause of this exception</param>
        public VerifyException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Internal serialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected VerifyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
