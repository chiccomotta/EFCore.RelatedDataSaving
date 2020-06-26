using System.Collections.Generic;
using EFRelatedData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EF.WebJsonApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EfcoreController : ControllerBase
    {
        private readonly ILogger<EfcoreController> _logger;

        public EfcoreController(ILogger<EfcoreController> logger)
        {
            _logger = logger;
        }

        /*
         * Esempio di request: invio solo i campi da aggiornare:
         *
                {
                  "Url": "http://www.cicciopino.it",
                  "Ranking": 77,
                  "InsertDate": "2020-04-23T18:25:43.511Z"
                }
         *
         * 
         */

        [HttpPost]
        public ActionResult Process([FromBody] Dictionary<string, dynamic> request)
        {
            Sample.Example1(request);

            var blog = Sample.GetBlogById(1);
            return Ok(blog);
        }
    }
}
