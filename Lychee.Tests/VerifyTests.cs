using Lychee.Core;
using NUnit.Framework;

namespace Lychee.Tests
{
    public class VerifyTests
    {
        [Test]
        [ExpectedException(typeof(VerifyException))]
        public void False_Condition_Check_Throws_VerifyException()
        {
            Verify.Ensure(false);
        }

        [Test]
        [ExpectedException(typeof(VerifyException))]
        public void False_Expression_Check_Throws_VerifyException()
        {
            Verify.Ensure(() => false);
        }

        [Test]
        public void True_Condition_Check_Completes()
        {
            Verify.Ensure(true);
        }

        [Test]
        public void True_Expression_Check_Completes()
        {
            Verify.Ensure(() => true);
        }

        [Test]
        [ExpectedException(typeof(VerifyException))]
        public void Null_Check_Throws_ArgumentNullException()
        {
            Verify.EnsureNotNull<string>(null);
        }

        [Test]
        public void NotNull_Check_Completes()
        {
            Verify.EnsureNotNull("");
        }
    }
}
