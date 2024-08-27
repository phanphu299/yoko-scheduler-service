using System;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.Application.Helper
{
    public static class RetryHelper
    {
        internal static int[] DelayPerAttemptInSeconds =
        {
            (int) TimeSpan.FromMinutes(1).TotalSeconds,
            (int) TimeSpan.FromMinutes(1).TotalSeconds,
            (int) TimeSpan.FromMinutes(2).TotalSeconds
        };

        public static async Task Execute(Func<Task> action, int numberOfRetries)
        {
            var tries = 0;
            while (tries <= numberOfRetries)
            {
                try
                {
                    await action();
                    return;
                }
                catch
                {
                    var delay = IncreasingDelayInSeconds(tries);
                    await Task.Delay((delay * 1000));
                    tries++;
                }
            }
        }

        private static int IncreasingDelayInSeconds(int failedAttempts)
        {
            return failedAttempts >= DelayPerAttemptInSeconds.Length ? DelayPerAttemptInSeconds.Last() : DelayPerAttemptInSeconds[failedAttempts];
        }
    }
}