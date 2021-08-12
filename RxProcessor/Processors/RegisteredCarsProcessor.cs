using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RxProcessor.Models;
using RxProcessor.ObservableProviders;
using RxProcessor.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace RxProcessor.Processors
{
    public class RegisteredCarsProcessor :IProcessorInitializable
        //: IProcessorInitializable
    {
        private readonly ICarsObservableProvider _carsObservableProvider;
        private readonly ILogger<RegisteredCarsProcessor> _logger;
        private readonly ConfigOptions _options;

        public RegisteredCarsProcessor(ICarsObservableProvider carsObservableProvider, ILogger<RegisteredCarsProcessor> logger, IOptions<ConfigOptions> options)
        {
            _carsObservableProvider = carsObservableProvider;
            _logger = logger;
            _options = options.Value;
        }

        public void GetCarsByTimeSpan() {
            var carsObservableStream = _carsObservableProvider.GetStream();
            carsObservableStream.GetNumberOfCarsByPeriod(20).Subscribe(x => printRegisteredCarsTotal(x, _options.TimeSpan));

        }

        private void printRegisteredCarsTotal(int numberOfCars, int timeSpan)
        {
            _logger.LogInformation($"Total Registered cars in the  last {timeSpan} seconds,    Vehicles : {numberOfCars} cars");
            
        }

        public void Initialize()
        {
            GetCarsByTimeSpan();
        }

    }
}
