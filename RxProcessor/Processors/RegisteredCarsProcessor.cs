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
            carsObservableStream.GetNumberOfCarsByPeriod(_options.TimeSpan).Subscribe(x => printRegisteredCarsTotal(x, _options.TimeSpan));

        }

        public void GetMinimumPricedCarsListByTimeSpan()
        {
            var carsObservableStream = _carsObservableProvider.GetStream();
            carsObservableStream.GetCarsByPeriod(_options.TimeSpan*2).Subscribe(x => printRegisteredCarsList(x, _options.TimeSpan*2));

        }

        public void GetCarsBufferListByTimeSpan()
        {
            var carsObservableStream = _carsObservableProvider.GetStream();
            carsObservableStream.GetCarsList(_options.TimeSpan).Subscribe(x => printBufferedCarsList(x, _options.TimeSpan));

        }

        private void printBufferedCarsList(IList<CarRegistration> cars, int timeSpan)
        {
            var counter = 1;
            _logger.LogInformation($" Registered cars in Buffer in the  last {timeSpan} seconds,    Vehicles Count : {cars.Count} cars");
            foreach (var car in cars) {
                _logger.LogInformation($" [{counter}] , {car.Model}, {car.Color}, {car.Price}");
                counter+=1;
            }

        }

        private void printRegisteredCarsTotal(int numberOfCars, int timeSpan)
        {
            _logger.LogInformation($"Total Registered cars in the  last {timeSpan} seconds,    Vehicles : {numberOfCars} cars");
            
        }

        private void printRegisteredCarsList(IObservable<CarRegistration> cars, int timeSpan)
        {
            cars.MinBy(x => x.Price).Subscribe(x => handleListOfCars(x, timeSpan));
            //_logger.LogInformation($" Registered cars in the  last {timeSpan} seconds,    Vehicles : {)} cars");

        }

        private void handleListOfCars(IList<CarRegistration> cars, int timeSpan)
        {
            var counter = 1;
            _logger.LogInformation($" Least Priced cars in the  last {timeSpan} seconds,    Vehicles Count : {cars.Count} cars");
            foreach (var car in cars) {
                _logger.LogInformation($" [{counter}] , {car.Model}, {car.Color}, {car.Price}");
            }
        }

        public void Initialize()
        {
            GetCarsByTimeSpan();
            GetMinimumPricedCarsListByTimeSpan();
            GetCarsBufferListByTimeSpan();
        }

    }
}
