using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace FaceDemo.Misc
{
    public static class TaskExtensions
    {
        public static async Task<T> TryExecute<T>(Func<Task<T>> asyncFunc, int? retries = null, TimeSpan? timeout = null)
        {
            var obs = Observable
                .Defer(() => Observable.FromAsync(asyncFunc))
                .Timeout(timeout ?? TimeSpan.FromDays(2))
                .Retry(retries ?? int.MaxValue);

            return await obs.ToTask();
        }

        /// <summary>
        /// An exponential back off strategy
        /// </summary>
        public static readonly Func<int, TimeSpan> ExponentialBackoff = n => TimeSpan.FromSeconds(Math.Pow(2, n));

        public static IObservable<T> RetryWithBackoffStrategy<T>(
            this IObservable<T> source,
            int retryCount = 3,
            Func<int, TimeSpan> strategy = null,
            Func<Exception, bool> retryOnError = null,
            IScheduler scheduler = null)
        {
            strategy = strategy ?? ExponentialBackoff;
            scheduler = scheduler ?? Scheduler.Default;

            if (retryOnError == null)
            {
                retryOnError = e => true;
            }

            var attempt = 0;

            return Observable.Defer(
                    () =>
                    {
                        return ((++attempt == 1) ? source : source.DelaySubscription(strategy(attempt - 1), scheduler))
                            .Select(Notification.CreateOnNext)
                            .Catch(
                                (Exception e) => retryOnError(e)
                                    ? Observable.Throw<Notification<T>>(e)
                                    : Observable.Return(Notification.CreateOnError<T>(e)));
                    })
                .Retry(retryCount)
                .Dematerialize();
        }
    }
}