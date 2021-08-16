using RxProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace RxProcessor.Queries
{
    public static class SparesQueryExtensions
    {
        public static IObservable<int> GetNumberOfSparesByPeriod(this IObservable<SparesRegistration> allspares, int numberOfSeconds)
        {
            var sparesCount = allspares.Window(TimeSpan.FromSeconds(numberOfSeconds)).Select(x => x.Distinct(x => x.Id).Count()).Merge();
            return sparesCount;
        }
    }
}
