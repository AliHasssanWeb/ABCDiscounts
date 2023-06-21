using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusCodes = ABC.Shared.DataConfig.StatusCodes;

namespace ABC.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataTransferController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        public DataTransferController(ABCDiscountsContext _db)
        {
            db = _db;
        }

        [HttpGet("PurchaseGet")]
        public IActionResult PurchaseGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Purchase>>();
                
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Purchase>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
    }
}
