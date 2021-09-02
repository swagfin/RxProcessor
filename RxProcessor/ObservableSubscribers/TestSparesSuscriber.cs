using Microsoft.Extensions.Logging;
using RxProcessor.Models;
using RxProcessor.ObservableProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace RxProcessor.ObservableSubscribers
{
    public class TestSparesSubscriber : IProcessorInitializable
    {
        private ILogger<TestSparesSubscriber> logger;
        private readonly ISparesObservableProvider _provider;

        private IObservable<SparesRegistration> sparesRegStream;

        public TestSparesSubscriber(ILogger<TestSparesSubscriber> logger, ISparesObservableProvider provider)
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
            sparesRegStream = _provider.GetStream();
        }

        private void logAll()
        {
            sparesRegStream.Do(spares => HandleOnSparesRegistration(spares))
                        .Subscribe();
        }

        private void HandleOnSparesRegistration(SparesRegistration spares)
        {
            logger.LogInformation($"Test spares Reg Subscribber received spares: {spares.ToString()}");
        }
    }
}
