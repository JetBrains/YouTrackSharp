using System;
using System.Threading.Tasks;

namespace YouTrackSharp.Internal
{
    /// <summary>
    /// A class that can execute a specific code block multiple times, using linear retries.
    /// </summary>
    internal class LinearRetryPolicy<TResult>
    {
        private readonly Func<Task<TResult>> _action;
        private readonly Func<TResult, Task<bool>> _shouldRetry;
        private readonly TimeSpan _delay;
        private readonly int _maximumNumberOfAttempts;

        /// <summary>
        /// Creates an instance of the <see cref="LinearRetryPolicy&lt;TResult&gt;"/> class.
        /// </summary>
        /// <param name="action">Action to perform.</param>
        /// <param name="shouldRetry">Should the action be retried based on the result?</param>
        /// <param name="delay">Delay between runs.</param>
        /// <param name="maximumNumberOfAttempts">Maximum number of attempts.</param>
        public LinearRetryPolicy(Func<Task<TResult>> action, Func<TResult, Task<bool>> shouldRetry, TimeSpan delay, int maximumNumberOfAttempts)
        {
            _action = action;
            _shouldRetry = shouldRetry;
            _delay = delay;
            _maximumNumberOfAttempts = maximumNumberOfAttempts;
        }

        /// <summary>
        /// Executes the <see cref="LinearRetryPolicy&lt;TResult&gt;"/>.
        /// </summary>
        /// <returns>A <see cref="TResult"/> if a result could be retrieved, the default value for <see cref="TResult"/> otherwise.</returns>
        public async Task<TResult> Execute()
        {
            for (var i = 0; i < _maximumNumberOfAttempts; i++)
            {
                var result = await _action();
                if (await _shouldRetry(result))
                {
                    await Task.Delay(_delay);
                }
                else
                {
                    return result;
                }
            }

            return default(TResult);
        }
    }
}