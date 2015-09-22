using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Lychee.Core;
using NUnit.Framework;

namespace Lychee.Tests
{
    public class AttemptTests
    {
        [Test]
        public void FirstAttempt_ForAction_Passes()
        {
            var retry = Attempt.NoMoreThan(5);

            var result = retry.ExecuteAsync(() => { /* no-op */});

            Task.WaitAny(result);

            Assert.IsNull(result.Exception);
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
        }

        [Test]
        public void SecondAttempt_ForAction_Passes()
        {
            var retry = Attempt.NoMoreThan(5);
            var result =
                retry
                    .UsingRetryPolicy(
                        (attempt) => attempt < 1 ? TimeSpan.FromSeconds(0.5) : TimeSpan.FromSeconds(5))
                    .ExecuteAsync(() => Thread.Sleep(TimeSpan.FromMilliseconds(2000)));

            Task.WaitAny(result);

            Assert.IsNull(result.Exception);
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
        }

        [Test]
        public void AcceptedTransientException_ForAction_Passes()
        {
            var retry = Attempt.NoMoreThan(5);
            var simulatedCount = 0;

            var result =
                retry
                    .AllowRetryWhen((e) => e is XmlException)
                    .ExecuteAsync(() =>
                    {
                        /**
                         * fault the first attempt with something accepted as a
                         * transient exception.
                         */
                        if (simulatedCount == 0)
                        {
                            simulatedCount++;
                            throw new XmlException();
                        }
                        /* no-op */
                    });

            Task.WaitAny(result);

            Assert.IsNull(result.Exception);
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
        }

        [Test]
        public void Attempt_Failure_ForAction_FaultsTask()
        {
            var retry = Attempt
                .NoMoreThan(1)
                .UsingRetryPolicy((i) => TimeSpan.FromMilliseconds(500));

            var result = retry.ExecuteAsync(() => Thread.Sleep(TimeSpan.FromMilliseconds(10000)));

            Task.WaitAny(result);
            Assert.AreEqual(TaskStatus.Faulted, result.Status);
            Assert.IsTrue(result.Exception != null && result.Exception.InnerException is TimeoutException);
        }

        [Test]
        public void FirstAttempt_ForFunction_Passes()
        {
            var retry = Attempt.NoMoreThan(5);

            var result = retry.ExecuteAsync(() => "test");

            Task.WaitAny(result);

            Assert.IsNull(result.Exception);
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            Assert.AreEqual("test", result.Result);
        }

        [Test]
        public void SecondAttempt_ForFunction_Passes()
        {
            var retry = Attempt.NoMoreThan(5);
            var result =
                retry
                    .UsingRetryPolicy(
                        (attempt) => attempt < 1 ? TimeSpan.FromSeconds(0.5) : TimeSpan.FromSeconds(5))
                    .ExecuteAsync(() =>
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(2000));
                        return "test";
                    });

            Task.WaitAny(result);

            Assert.IsNull(result.Exception);
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            Assert.AreEqual("test", result.Result);
        }

        [Test]
        public void Attempt_Failure_ForFunction_FaultsTask()
        {
            var retry = Attempt
                .NoMoreThan(1)
                .UsingRetryPolicy((i) => TimeSpan.FromMilliseconds(500));

            var result = retry.ExecuteAsync(() =>
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(10000));
                return "test";
            });

            Task.WaitAny(result);
            Assert.AreEqual(TaskStatus.Faulted, result.Status);
            Assert.IsTrue(result.Exception != null && result.Exception.InnerException is TimeoutException);
        }

        [Test]
        public void AcceptedTransientException_ForFunction_Passes()
        {
            var retry = Attempt.NoMoreThan(5);
            var simulatedCount = 0;

            var result =
                retry
                    .AllowRetryWhen((e) => e is XmlException)
                    .ExecuteAsync(() =>
                    {
                        /**
                         * fault the first attempt with something accepted as a
                         * transient exception.
                         */
                        if (simulatedCount == 0)
                        {
                            simulatedCount++;
                            throw new XmlException();
                        }
                        return "test";
                    });

            Task.WaitAny(result);

            Assert.IsNull(result.Exception);
            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            Assert.AreEqual("test", result.Result);
        }
    }
}
