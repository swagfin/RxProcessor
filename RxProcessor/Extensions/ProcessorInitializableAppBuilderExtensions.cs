using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;

namespace RxProcessor.Extensions
{
    public static class ProcessorInitializableAppBuilderExtensions
    {
        public static void UseProcessInitializables(this IApplicationBuilder builder)
        {
            var processorService = (IEnumerable<IProcessorInitializable>)builder.ApplicationServices.GetService(typeof(IEnumerable<IProcessorInitializable>));
            if (processorService != null)
                foreach (IProcessorInitializable processor in processorService)
                    processor.Initialize();
        }
    }
}
