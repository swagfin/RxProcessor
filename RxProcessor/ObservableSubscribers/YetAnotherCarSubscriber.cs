using Microsoft.Extensions.Logging;
using RxProcessor.Models;
using RxProcessor.ObservableProviders;
using System;
using System.Reactive.Linq;

namespace RxProcessor.ObservableSubscribers
{
    public class YetAnotherCarSubscriber : IProcessorInitializable
    {
        private ILogger<YetAnotherCarSubscriber> logger;
        private readonly ICarsObservableProvider _provider;

        private IObservable<CarRegistration> CarRegStream;

        public YetAnotherCarSubscriber(ILogger<YetAnotherCarSubscriber> logger, ICarsObservableProvider provider)
        {
            this.logger = logger;
            this._provider = provider;
        }

        public void Initialize()
        {
            setupRx();

            logAll();
            //logFailedHeartbeats();
            //logStatusChanges();
        }

        private void setupRx()
        {
            logger.LogInformation("setting up rx in test processor");
            CarRegStream = _provider.GetStream();
        }

        private void logAll()
        {
            CarRegStream.Do(car => HandleOnCarRegistration(car))
                        .Subscribe();
        }

        private void HandleOnCarRegistration(CarRegistration car)
        {
            logger.LogInformation($"Another Car Subscriber has began Queuing for Box IDs {car.ToString()}");
        }
    }
}
