using Microsoft.Extensions.Logging;
using RxProcessor.Models;
using System;
using System.Collections.Concurrent;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RxProcessor.ObservableProviders.Implementation
{
    public class CarsObservableProvider : ICarsObservableProvider
    {
        private readonly ILogger<CarsObservableProvider> _logger;
        private ConcurrentQueue<CarRegistration> _queue;
        private IObservable<CarRegistration> stream;


        public CarsObservableProvider(ILogger<CarsObservableProvider> logger)
        {
            this._logger = logger;
        }
        public void Initialize()
        {
            this._queue = new ConcurrentQueue<CarRegistration>();
            _logger.LogInformation("constructing.....");
            // Initialize inner Observable - at this point there are no Observers yet so, we don't have an instance of Observer to push data towards
            var innerObservable = InitStream();
            _logger.LogInformation("initialized inner-observable - no subscriptions yet");
            // Subscribe to the inner Observable ourselves and make sure that subscription is shared by anyone else
            var connectedObservable = innerObservable.Publish();          // Ensure subscription is shared amongst all Observers
            _logger.LogInformation("published inner-observable");
            _ = connectedObservable.Connect();// Connect right away so all subscribers as of now get the same data pushed
            stream = connectedObservable;
            _logger.LogInformation("connected inner-observable");
            _logger.LogInformation("constructed......");
        }

        public void HandleRequest(CarRegistration request)
        {
            _queue.Enqueue(request);
        }
        private IObservable<CarRegistration> InitStream()
        {
            _logger.LogInformation("initializing stream of device heartbeats");
            var o = Observable.Create((IObserver<CarRegistration> observer, CancellationToken token) =>
            {
                _logger.LogInformation("creating observable in initStream");
                return Task.Run(() =>
                {
                    CarRegistration nextItem;
                    while (!token.IsCancellationRequested)
                    {
                        if (_queue.TryDequeue(out nextItem))
                        {
                            observer.OnNext(nextItem);
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                    }
                }, token);
            });

            return o.ObserveOn(NewThreadScheduler.Default);
        }

        public IObservable<CarRegistration> GetStream()
        {
            return stream;
        }


    }
}
