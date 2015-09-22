using System;
using Lychee.Core;
using NUnit.Framework;

namespace Lychee.Tests
{
    public class PreconditionTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Precondition check for 'argument' failed.\r\nParameter name: argument")]
        public void False_Condition_Check_Throws_ArgumentException()
        {
            Precondition.CheckArgument(false, "argument");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Precondition check for 'argument' failed.\r\nParameter name: argument")]
        public void False_Expression_Check_Throws_ArgumentException()
        {
            Precondition.CheckArgument(() => false, "argument");
        }

        [Test]
        public void True_Condition_Check_Completes()
        {
            Precondition.CheckArgument(true, "argument");
        }

        [Test]
        public void True_Expression_Check_Completes()
        {
            Precondition.CheckArgument(() => true, "argument");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: argument")]
        public void Null_Check_Throws_ArgumentNullException()
        {
            Precondition.CheckNotNull<string>(null, "argument");
        }

        [Test]
        public void NonNull_Check_Completes()
        {
            Precondition.CheckNotNull("", "argument");
        }

        [Test]
        public void True_State_Check_Completes()
        {
            Precondition.CheckState(true, "Shouldn't see this");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Invalid state message")]
        public void False_State_Check_Throws_InvalidOperationException()
        {
            Precondition.CheckState(false, "Invalid state message");
        }
    }
}
