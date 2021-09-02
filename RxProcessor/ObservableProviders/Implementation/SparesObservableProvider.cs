using Microsoft.Extensions.Logging;
using RxProcessor.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RxProcessor.ObservableProviders.Implementation
{
    public class SparesObservableProvider : ISparesObservableProvider
    {
        private ILogger<SparesObservableProvider> _logger;
        private ConcurrentQueue<SparesRegistration> _queue;
        private IObservable<SparesRegistration> stream;


        public SparesObservableProvider(ILogger<SparesObservableProvider> logger)
        {
            _logger = logger;

        }
        public void Initialize()
        {
            this._queue = new ConcurrentQueue<SparesRegistration>();
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

        private IObservable<SparesRegistration> InitStream()
        {
            _logger.LogInformation("initializing stream of device heartbeats");
            var o = Observable.Create((IObserver<SparesRegistration> observer, CancellationToken token) =>
            {
                _logger.LogInformation("creating observable in initStream");

                return Task.Run(() =>
                {
                    SparesRegistration nextItem;
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

        public void AddSpares(SparesRegistration request)
        {
            _queue.Enqueue(request);
        }

        public IObservable<SparesRegistration> GetStream()
        {
            return stream;
        }

    }
}
