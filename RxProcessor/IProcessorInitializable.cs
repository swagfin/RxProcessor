namespace RxProcessor
{
    public interface IProcessorInitializable
    {
        /// <summary>
        /// Initialized Processor Functions. Ensure you are handling errors as well
        /// </summary>
        void Initialize();
    }
}
