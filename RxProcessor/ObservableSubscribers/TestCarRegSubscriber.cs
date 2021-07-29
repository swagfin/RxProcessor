using Microsoft.Extensions.Logging;
using RxProcessor.Models;
using RxProcessor.ObservableProviders;
using System;
using System.Reactive.Linq;

namespace RxProcessor.ObservableSubscribers
{
    public class TestCarRegSubscriber : IProcessorInitializable
    {
        private ILogger<TestCarRegSubscriber> logger;
        private readonly ICarsObservableProvider _provider;

        private IObservable<CarRegistration> CarRegStream;

        public TestCarRegSubscriber(ILogger<TestCarRegSubscriber> logger, ICarsObservableProvider provider)
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
            logger.LogInformation($"Test Car Reg Subscribber received Car: {car.ToString()}");
        }
    }
}
