using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Core.Model;
using Taxually.TechnicalTest.Core.Processors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Taxually.TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatRegistrationController : ControllerBase
    {
        private readonly IVatRegistrationProcessorFactory _vatRegistrationProcessorFactory;

        public VatRegistrationController(IVatRegistrationProcessorFactory vatRegistrationProcessorFactory)
        {
            _vatRegistrationProcessorFactory = vatRegistrationProcessorFactory;
        }

        /// <summary>
        /// Registers a company for a VAT number in a given country
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VatRegistrationRequest request)
        {
            await _vatRegistrationProcessorFactory
                .Create(request.Country)
                .Process(request);
            
            return Ok();
        }
    }
}
