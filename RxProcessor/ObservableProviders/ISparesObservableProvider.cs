using RxProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxProcessor.ObservableProviders
{
    public interface ISparesObservableProvider : IProcessorInitializable
    {
        public void AddSpares(SparesRegistration request);
        public IObservable<SparesRegistration> GetStream();
    }
}
