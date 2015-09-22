using System;
using System.Threading.Tasks;

namespace Lychee.Core
{
    /// <summary>
    /// Basic construct for a retry policy. Uses a fluent syntax to initialise the maximum number of times
    /// something should be attempted, when something should be attempted, and a calculator to determine
    /// the time to wait between retries.
    /// </summary>
    public class Attempt
    {
        private readonly int _maxAttempts;

        private Predicate<Exception> _tryAgainWhen;
        private Func<int, TimeSpan> _calculateRetry;

        private bool _started = false;

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="maxAttempts">Maximum number of attempts. Must be greater than zero.</param>
        private Attempt(int maxAttempts)
        {
            Precondition.CheckArgument(maxAttempts > 0, "maxAttempts", "maxAttempts must be > 0");
            _maxAttempts = maxAttempts;
            _tryAgainWhen = (e) => true;
            _calculateRetry = (count) => TimeSpan.FromSeconds(1.0);

        }

        /// <summary>
        /// Initialises the retry policy, specifying the maximum number of times a task can be called
        /// before giving up. Retry will be allowed on all exceptions, and will fire after a one
        /// second delay.
        /// </summary>
        /// <param name="maxAttempts">maximum number of attempts to try to do something</param>
        /// <returns></returns>
        public static Attempt NoMoreThan(int maxAttempts)
        {
            return new Attempt(maxAttempts);
        }

        /// <summary>
        /// Set the function that decides whether or not a task should be attempted again.
        /// </summary>
        /// <param name="exceptionCheck">true/false function that determines if the task should be tried again</param>
        /// <returns>this object</returns>
        public Attempt AllowRetryWhen(Predicate<Exception> exceptionCheck)
        {
            Precondition.CheckNotNull(exceptionCheck, "exceptionCheck", "Exception predicate cannot be null");
            Precondition.CheckState(!_started, "Cannot change exception predicate while running");

            _tryAgainWhen = exceptionCheck;
            return this;
        }

        public Attempt UsingRetryPolicy(Func<int, TimeSpan> calculateRetry)
        {
            Precondition.CheckNotNull(calculateRetry, "calculateRetry", "Retry calculator cannot be null");
            Precondition.CheckState(!_started, "Cannot change retry policy while running");

            _calculateRetry = calculateRetry;
            return this;
        }

        public async Task ExecuteAsync(Action action)
        {
            Precondition.CheckNotNull(action, "action");
            Precondition.CheckState(!_started, "Task is already running");

            var attempts = 0;

            try
            {
                for (;;)
                {
                    _started = true;
                    try
                    {
                        var timeout = _calculateRetry(attempts);
                        var task = Task.Factory.StartNew(action);
                        if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                        {
                            await task;
                            break;
                        }

                        throw new TimeoutException("Call to task timed out");

                    }
                    catch (TimeoutException)
                    {
                        attempts++;
                        if (attempts >= _maxAttempts)
                        {
                            throw;
                        }
                    }
                    catch (Exception e)
                    {
                        attempts++;
                        if (attempts > _maxAttempts || !_tryAgainWhen(e))
                        {
                            throw;
                        }
                    }

                }
            }
            finally
            {
                _started = false;
            }
            

            
        }

        public async Task<T> ExecuteAsync<T>(Func<T> function)
        {
            Precondition.CheckNotNull(function, "function");

            var attempts = 0;

            try
            {
                for (;;)
                {
                    try
                    {
                        var timeout = _calculateRetry(attempts);
                        var task = Task.Factory.StartNew(function);

                        if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                        {
                            return await task;
                        }
                        throw new TimeoutException("Call to task timed out");
                    }
                    catch (TimeoutException)
                    {
                        attempts++;
                        if (attempts >= _maxAttempts)
                        {
                            throw;
                        }
                    }
                    catch (Exception e)
                    {
                        attempts++;
                        if (attempts >= _maxAttempts || !_tryAgainWhen(e))
                        {
                            throw;
                        }
                    }
                }
            }
            finally
            {
                _started = false;
            }
        }
    }
}
