using RxProcessor.Models;
using System;

namespace RxProcessor.ObservableProviders
{
    public interface ICarsObservableProvider : IProcessorInitializable
    {
        public void HandleRequest(CarRegistration request);
        public IObservable<CarRegistration> GetStream();

    }
}
