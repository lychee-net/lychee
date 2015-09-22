using System;

namespace Lychee.Core
{
    /// <summary>
    /// Utility class to support/encapsulate in-code verifications, to be used as an alternate to .NET code contracts & Contract.Ensure.
    /// </summary>
    public static class Verify
    {
        /// <summary>
        /// Ensures the truth of an expression involving the state of the calling instance, but not involving any parameters to the calling method.
        /// </summary>
        /// <param name="expression">a boolean expression</param>
        /// <param name="message">the error message (or template) relating to the current state assuming that the value of expression is false</param>
        /// <param name="values">message template values</param>
        /// <exception cref="VerifyException">if the expression is false</exception>
        public static void Ensure(bool expression, string message = null, params object[] values)
        {
            if (!expression)
            {
                throw message == null ? new VerifyException() : new VerifyException(string.Format(message, values));
            }
        }

        /// <summary>
        ///  Ensures the truth of an expression involving the state of the calling instance, but not involving any parameters to the calling method.
        /// </summary>
        /// <param name="expression">an expression producing a boolean output</param>
        /// <param name="message">the error message (or template) relating to the current state assuming that the value of expression is false</param>
        /// <param name="values">message template values</param>
        /// <exception cref="VerifyException">if the expression is false</exception>
        public static void Ensure(Func<bool> expression, string message = null, params object[] values)
        {
            Ensure(expression.Invoke(), message, values);
        }

        /// <summary>
        /// Verifies that an object reference passed as a parameter to the calling method is not null.
        /// </summary>
        /// <typeparam name="T">object reference type</typeparam>
        /// <param name="obj">an object reference</param>
        /// <param name="message">error message or template</param>
        /// <param name="values">message template values</param>
        /// <exception cref="VerifyException">if the object is null</exception>
        public static void EnsureNotNull<T>(T obj, string message = null, params object[] values)
            where T : class
        {
            Ensure(obj != null, message, values);
        }
    }
}
