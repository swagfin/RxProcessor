using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RxProcessor.Models;
using RxProcessor.Models.Request;
using RxProcessor.ObservableProviders;

namespace RxProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ILogger<CarsController> logger;
        private readonly ICarsObservableProvider contentProvider;

        public CarsController(ILogger<CarsController> logger, ICarsObservableProvider contentProvider)
        {
            this.logger = logger;
            this.contentProvider = contentProvider;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CarRegistrationRequest request)
        {
            return handleCarRegistrationRequest(request);
        }
        private IActionResult handleCarRegistrationRequest(CarRegistrationRequest request)
        {
            //Mapping here
            CarRegistration model = new CarRegistration { Model = request.Model, Price = request.Price, Color = request.Color };
            //Pass to Content Delivery
            contentProvider.HandleRequest(model);
            return new OkObjectResult(model);
        }

    }
}
