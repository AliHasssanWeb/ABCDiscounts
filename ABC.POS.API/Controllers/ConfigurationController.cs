using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using ABC.Shared.Interface;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusCodes = ABC.Shared.DataConfig.StatusCodes;

namespace ABC.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {

        protected readonly ABCDiscountsContext db;
        private readonly IMailService mailService;
        public ConfigurationController(ABCDiscountsContext _db, IMailService mailService)
        {
            db = _db;
            this.mailService = mailService;
            GenericPrograms emailsend = new GenericPrograms(mailService);

        }

        // Start Warehouse
        [HttpGet("WareHouseGet")]
        public IActionResult WareHouseGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<WareHouse>>();
                var record = db.WareHouses.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<WareHouse>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("WareHouseCreate")]
        public async Task<IActionResult> WareHouseCreate(WareHouse obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<WareHouse>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.WareHouses.ToList().Exists(p => p.WareHouseName.Equals(obj.WareHouseName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }

                db.WareHouses.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<WareHouse>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("WareHouseUpdate/{id}")]
        public async Task<IActionResult> WareHouseUpdate(int id, WareHouse data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<WareHouse>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    return BadRequest("Id Required");
                }
                bool isValid = db.WareHouses.ToList().Exists(x => x.WareHouseName.Equals(data.WareHouseName, StringComparison.CurrentCultureIgnoreCase) && x.WareHouseId != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.WareHouseId)
                {
                    return BadRequest();
                }
                var record = await db.WareHouses.FindAsync(id);
                if (data.WareHouseName != null && data.WareHouseName != "undefined")
                {
                    record.WareHouseName = data.WareHouseName;
                }
                data = record;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<WareHouse>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("WareHouseByID/{id}")]
        public IActionResult WareHouseByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<WareHouse>();
                var record = db.WareHouses.Find(id);
                if (record != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<WareHouse>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteWareHouse/{id}")]
        public async Task<IActionResult> DeleteWareHouse(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<WareHouse>();
                WareHouse data = await db.WareHouses.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.WareHouses.Remove(data);
                await db.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<WareHouse>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("BrandCreate")]
        public async Task<IActionResult> BrandCreate(Brand obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Brand>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Brands.ToList().Exists(p => p.Name.Equals(obj.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }

                db.Brands.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<WareHouse>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BrandGet")]
        public IActionResult BrandGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Brand>>();
                var record = db.Brands.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Brand>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteBrand/{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Brand>();
                Brand data = await db.Brands.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.Brands.Remove(data);
                await db.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Brand>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ArticleCreate")]
        public async Task<IActionResult> ArticleCreate(ArticleType articleType)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ArticleType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var getaal = db.ArticleTypes.ToList();
                if(getaal.Count() > 0)
                {
                    for (int i = 0; i < getaal.Count(); i++)
                    {
                        var name = getaal[i].ArticleTypeName;
                        if(name != null)
                        {
                            bool checkname = name.Equals(articleType.ArticleTypeName, StringComparison.CurrentCultureIgnoreCase);
                            if (checkname)
                            {
                                ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                            }
                        }
                    }
                }
                db.ArticleTypes.Add(articleType);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ArticleType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteArticle/{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ArticleType>();
                ArticleType data = await db.ArticleTypes.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.ArticleTypes.Remove(data);
                await db.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ArticleType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ArticleTypeGet")]
        public IActionResult ArticleTypeGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<ArticleType>>();
                var record = db.ArticleTypes.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ArticleType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
    }
}
