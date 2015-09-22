using System;

namespace Lychee.Core
{
    /// <summary>
    /// The Precondition class provides convenience methods relating to the fast-fail pattern (see: http://martinfowler.com/ieeeSoftware/failFast.pdf),
    /// and is a port of the Precondition utility class from Google Guava, with some additional support for .NET programming constructs.
    /// 
    /// This, in many ways, performs the same sort of tasks as .NET's code contracts (Contract.Requires<...>(...)), but without the assosicated
    /// overhead
    /// </summary>
    public static class Precondition
    {
        /// <summary>
        /// Ensures the truth of an expression involving one or more parameters to the calling method.
        /// </summary>
        /// <param name="expression">a boolean expression</param>
        /// <param name="parameterName">the parameter name associated with the expression</param>
        /// <param name="message">error message or template</param>
        /// <param name="values">message template values</param>
        /// <exception cref="System.ArgumentException">if the expression is false</exception>
        public static void CheckArgument(bool expression, string parameterName, string message = null, params object[] values)
        {
            if (!expression)
            {
                throw message == null ? 
                    new ArgumentException(string.Format("Precondition check for '{0}' failed.", parameterName), parameterName) :
                    new ArgumentException(string.Format(message, values), parameterName);
            }
        }

        /// <summary>
        /// Ensures the truth of an expression involving one or more parameters to the calling method.
        /// </summary>
        /// <param name="expression">an expression producing a boolean output</param>
        /// <param name="parameterName">the parameter name associated with the expression</param>
        /// <param name="message">error message or template</param>
        /// <param name="values">message template values</param>
        /// <exception cref="System.ArgumentException">if the expression is false</exception>
        public static void CheckArgument(Func<bool> expression, string parameterName, string message = null, params object[] values)
        {
            CheckArgument(expression.Invoke(), parameterName, message, values);
        }

        /// <summary>
        /// Ensures the truth of an expression involving the state of the calling instance, but not involving any parameters to the calling method.
        /// </summary>
        /// <param name="expression">a boolean expression</param>
        /// <param name="message">the error message (or template) relating to the current state assuming that the value of expression is false</param>
        /// <param name="values">message template values</param>
        /// <exception cref="System.InvalidOperationException">if the expression is false</exception>
        public static void CheckState(bool expression, string message = null, params object[] values)
        {
            if (!expression)
            {
                throw message == null ? new InvalidOperationException() : new InvalidOperationException(string.Format(message, values));
            }
        }

        /// <summary>
        ///  Ensures the truth of an expression involving the state of the calling instance, but not involving any parameters to the calling method.
        /// </summary>
        /// <param name="expression">an expression producing a boolean output</param>
        /// <param name="message">the error message (or template) relating to the current state assuming that the value of expression is false</param>
        /// <param name="values">message template values</param>
        /// <exception cref="System.InvalidOperationException">if the expression is false</exception>
        public static void CheckState(Func<bool> expression, string message = null, params object[] values)
        {
            CheckState(expression.Invoke(), message, values);
        }

        /// <summary>
        /// Ensures that an object reference passed as a parameter to the calling method is not null.
        /// </summary>
        /// <typeparam name="T">object reference type</typeparam>
        /// <param name="obj">an object reference</param>
        /// <param name="parameterName">the parameter name associated with the supplied object</param>
        /// <param name="message">error message or template</param>
        /// <param name="values">message template values</param>
        /// <exception cref="System.ArgumentNullException">if the object is null</exception>
        public static void CheckNotNull<T>(T obj, string parameterName, string message = null, params object[] values)
            where T: class
        {
            if (obj == null)
            {
                throw message == null ? new ArgumentNullException(parameterName) : new ArgumentNullException(parameterName, string.Format(message, values));
            }
        }
    }
}
