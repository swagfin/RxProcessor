using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RxProcessor.Models.Request;
using RxProcessor.Models;
using RxProcessor.ObservableProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SparesController : ControllerBase
    {
        private readonly ILogger<SparesController> logger;
        private readonly ISparesObservableProvider contentProvider;

        public SparesController(ILogger<SparesController> logger, ISparesObservableProvider contentProvider)
        {
            this.logger = logger;
            this.contentProvider = contentProvider;
        }

        [HttpPost]
        public IActionResult Post([FromBody] SparesRegistrationRequest request)
        {
            return handleSpareRegistrationRequest(request);
        }
        private IActionResult handleSpareRegistrationRequest(SparesRegistrationRequest request)
        {
            //Mapping here
            SparesRegistration sparesModel = new SparesRegistration { Name = request.Name, Price = request.Price, Color = request.Color };
            //Pass to Content Delivery
            contentProvider.AddSpares(sparesModel);
            return new OkObjectResult(sparesModel);
        }
    }
}
