using System;

namespace RxProcessor.Models.Response
{
    public class CarByModelResponse
    {
        public string Model { get; set; }
        public int Count { get; set; } = 0;
        public DateTime LastSyncUTC { get; set; } = DateTime.Now;
    }
}
