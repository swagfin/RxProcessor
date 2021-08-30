using RxProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace RxProcessor.Queries
{
    public static class CarQueryExtensions
    {
        public static IObservable<int> GetNumberOfCarsByPeriod(this IObservable<CarRegistration> allCars, int  numberOfSeconds)
        {
            var carsCount = allCars.Window(TimeSpan.FromSeconds(numberOfSeconds)).Select(x => x.Distinct(x => x.Id).Count()).Merge();
            return carsCount;
        }
        public static IObservable<IObservable<CarRegistration>> GetCarsByPeriod(this IObservable<CarRegistration> allCars, int numberOfSeconds)
        {
            var carsCount = allCars.Window(TimeSpan.FromSeconds(numberOfSeconds));
            return carsCount;
        }
        public static IObservable<IList<CarRegistration>> GetCarsList(this IObservable<CarRegistration> allCars, int numberOfSeconds)
        {
            var cars = allCars.Buffer(TimeSpan.FromSeconds(numberOfSeconds));
            return cars;
        }

    }
}
