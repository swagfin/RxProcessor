using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RxProcessor.Models;
using RxProcessor.ObservableProviders;
using RxProcessor.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxProcessor.Processors
{
    public class RegisteredSparesProcessor : IProcessorInitializable
    {

        private readonly ISparesObservableProvider _sparesObservableProvider;
        private readonly ILogger<RegisteredSparesProcessor> _logger;
        private readonly ConfigOptions _options;

        public RegisteredSparesProcessor(ISparesObservableProvider sparesObservableProvider, ILogger<RegisteredSparesProcessor> logger, IOptions<ConfigOptions> options)
        {
            _sparesObservableProvider = sparesObservableProvider;
            _logger = logger;
            _options = options.Value;
        }

        public void GetSparesByTimeSpan()
        {
            var sparesObservableStream = _sparesObservableProvider.GetStream();
            sparesObservableStream.GetNumberOfSparesByPeriod(20).Subscribe(x => printRegisteredSparesTotal(x, _options.TimeSpan));

        }

        private void printRegisteredSparesTotal(int numberOfSpares, int timeSpan)
        {
            _logger.LogInformation($"Total Registered spares in the  last {timeSpan} seconds,    Spares : {numberOfSpares} spares");

        }

        public void Initialize()
        {
            GetSparesByTimeSpan();
        }
    }
}
