using ABC.DTOs.Library.Adaptors;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using StatusCodes = ABC.Shared.DataConfig.StatusCodes;

namespace ABC.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CustomerController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomerController(ABCDiscountsContext _db, IWebHostEnvironment webHostEnvironment)
        {
            db = _db;

            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("CustomersGet")]
        public IActionResult CustomersGet()
        {
            try
            {
                var record = db.CustomerInformations.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerInformation>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ProviderCreate")]
        public async Task<IActionResult> ProviderCreate(Provider obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Provider>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Providers.ToList().Exists(p => p.Name.Equals(obj.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.Providers.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Provider>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ProviderGet")]
        public IActionResult ProviderGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Provider>>();
                var record = db.Providers.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Provider>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("ProviderUpdate/{id}")]
        public async Task<IActionResult> ProviderUpdate(int id, Provider data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Provider>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    return BadRequest("Id Required");
                }
                bool isValid = db.Providers.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.Providers.FindAsync(id);
                if (data.Name != null && data.Name != "undefined")
                {
                    record.Name = data.Name;
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
                    var Response = ResponseBuilder.BuildWSResponse<Provider>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ProviderByID/{id}")]
        public IActionResult ProviderByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Provider>();
                var record = db.Providers.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Provider>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteProvider/{id}")]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Provider>();
                Provider data = await db.Providers.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.Providers.Remove(data);
                await db.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Provider>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //CustomerType
        [HttpPost("CustomerTypeCreate")]
        public async Task<IActionResult> CustomerTypeCreate(CustomerType obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.CustomerTypes.ToList().Exists(p => p.TypeName.Equals(obj.TypeName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.CustomerTypes.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CustomerTypeGet")]
        public IActionResult CustomerTypeGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerType>>();
                var record = db.CustomerTypes.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("CustomerTypeUpdate/{id}")]
        public async Task<IActionResult> CustomerTypeUpdate(int id, CustomerType data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    return BadRequest("Id Required");
                }
                bool isValid = db.CustomerTypes.ToList().Exists(x => x.TypeName.Equals(data.TypeName, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.CustomerTypes.FindAsync(id);
                if (data.TypeName != null && data.TypeName != "undefined")
                {
                    record.TypeName = data.TypeName;
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CustomerTypeByID/{id}")]
        public IActionResult CustomerTypeByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerType>();
                var record = db.CustomerTypes.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteCustomerType/{id}")]
        public async Task<IActionResult> DeleteCustomerType(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerType>();
                CustomerType data = await db.CustomerTypes.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.CustomerTypes.Remove(data);
                await db.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //SubGroup

        [HttpPost("SubGroupCreate")]
        public async Task<IActionResult> SubGroupCreate(SubGroup obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SubGroup>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.SubGroups.ToList().Exists(p => p.SubGroupName.Equals(obj.SubGroupName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                if (obj.GroupId != null)
                {
                    var foungroup = db.Groups.ToList().Where(x => x.Id == obj.GroupId).FirstOrDefault();
                    if (foungroup != null)
                    {
                        obj.ParentGroupName = foungroup.Name;
                    }
                }
                else
                {
                    obj.ParentGroupName = null;
                }
                db.SubGroups.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SubGroup>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("SubGroupGet")]
        public IActionResult SubGroupGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<SubGroup>>();
                var record = db.SubGroups.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SubGroup>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("SubGroupUpdate/{id}")]
        public async Task<IActionResult> SubGroupUpdate(int id, SubGroup data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SubGroup>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    return BadRequest("Id Required");
                }
                bool isValid = db.SubGroups.ToList().Exists(x => x.SubGroupName.Equals(data.SubGroupName, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.SubGroups.FindAsync(id);
                if (data.SubGroupName != null && data.SubGroupName != "undefined")
                {
                    record.SubGroupName = data.SubGroupName;
                }
                if (data.GroupId != null)
                {
                    record.GroupId = data.GroupId;
                    var foungroup = db.Groups.ToList().Where(x => x.Id == data.GroupId).FirstOrDefault();
                    if (foungroup != null)
                    {
                        data.ParentGroupName = foungroup.Name;
                    }
                    //data.ParentGroupName = db.Groups.Where(x => x.Id == data.GroupId).FirstOrDefault().Name;
                    record.ParentGroupName = data.ParentGroupName;
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
                    var Response = ResponseBuilder.BuildWSResponse<SubGroup>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("SubGroupByID/{id}")]
        public IActionResult SubGroupByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SubGroup>();
                var record = db.SubGroups.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<SubGroup>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteSubGroup/{id}")]
        public async Task<IActionResult> DeleteSubGroup(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SubGroup>();
                SubGroup data = await db.SubGroups.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.SubGroups.Remove(data);
                await db.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SubGroup>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        //BusinessType

        [HttpPost("BusinessTypeCreate")]
        public async Task<IActionResult> BusinessTypeCreate(BusinessType obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<BusinessType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.BusinessTypes.ToList().Exists(p => p.TypeName.Equals(obj.TypeName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.BusinessTypes.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<BusinessType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("BusinessTypeGet")]
        public IActionResult BusinessTypeGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<BusinessType>>();
                var record = db.BusinessTypes.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<BusinessType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("BusinessTypeUpdate/{id}")]
        public async Task<IActionResult> BusinessTypeUpdate(int id, BusinessType data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<BusinessType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    return BadRequest("Id Required");
                }
                bool isValid = db.BusinessTypes.ToList().Exists(x => x.TypeName.Equals(data.TypeName, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.BusinessTypes.FindAsync(id);
                if (data.TypeName != null && data.TypeName != "undefined")
                {
                    record.TypeName = data.TypeName;
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
                    var Response = ResponseBuilder.BuildWSResponse<BusinessType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("BusinessTypeByID/{id}")]
        public IActionResult BusinessTypeByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<BusinessType>();
                var record = db.BusinessTypes.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<BusinessType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteBusinessType/{id}")]
        public async Task<IActionResult> DeleteBusinessType(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<BusinessType>();
                BusinessType data = await db.BusinessTypes.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.BusinessTypes.Remove(data);
                await db.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<BusinessType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        //Salesman

        [HttpPost("SalesmanCreate")]
        public async Task<IActionResult> BusinessTypeCreate(Salesman obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Salesman>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Salesmen.ToList().Exists(p => p.Name.Equals(obj.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.Salesmen.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Salesman>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("SalesmanGet")]
        public IActionResult SalesmanGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Salesman>>();
                var record = db.Salesmen.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Salesman>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("SalesmanUpdate/{id}")]
        public async Task<IActionResult> SalesmanUpdate(int id, Salesman data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Salesman>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    return BadRequest("Id Required");
                }
                bool isValid = db.Salesmen.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.Salesmen.FindAsync(id);
                if (data.Name != null && data.Name != "undefined")
                {
                    record.Name = data.Name;
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
                    var Response = ResponseBuilder.BuildWSResponse<Salesman>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("SalesmanByID/{id}")]
        public IActionResult SalesmanByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Salesman>();
                var record = db.Salesmen.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Salesman>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteSalesman/{id}")]
        public async Task<IActionResult> DeleteSalesman(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Salesman>();
                Salesman data = await db.Salesmen.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.Salesmen.Remove(data);
                await db.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Salesman>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        // Group Start
        [HttpGet("GroupGet")]
        public IActionResult GroupGet()
        {
            try
            {
                var record = db.Groups.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Group>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Group>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GroupCreate")]
        public async Task<IActionResult> GroupCreate(Group users)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Group>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Groups.ToList().Exists(p => p.Name.Equals(users.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.Groups.Add(users);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Group>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("GroupUpdate/{id}")]
        public async Task<IActionResult> GroupUpdate(int id, Group data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Group>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.Groups.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.Groups.FindAsync(id);
                if (data.Name != null && data.Name != "undefined")
                {
                    record.Name = data.Name;
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
                    var Response = ResponseBuilder.BuildWSResponse<Group>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GroupByID/{id}")]
        public IActionResult GroupByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Group>();

                var record = db.Groups.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Group>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteGroup/{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<Group>();
                Group data = await db.Groups.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Groups.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Group>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        // Zone Start
        [HttpGet("ZoneGet")]
        public IActionResult ZoneGet()
        {
            try
            {
                var record = db.Zones.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Zone>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Zone>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ZoneCreate")]
        public async Task<IActionResult> ZoneCreate(Zone obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Zone>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Zones.ToList().Exists(p => p.Name.Equals(obj.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.Zones.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Zone>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ZoneUpdate/{id}")]
        public async Task<IActionResult> ZoneUpdate(int id, Zone data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Zone>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.Zones.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.Zones.FindAsync(id);
                if (data.Name != null && data.Name != "undefined")
                {
                    record.Name = data.Name;
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
                    var Response = ResponseBuilder.BuildWSResponse<Zone>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ZoneByID/{id}")]
        public IActionResult ZoneByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Zone>();

                var record = db.Zones.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Zone>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteZone/{id}")]
        public async Task<IActionResult> DeleteZone(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<Zone>();
                Zone data = await db.Zones.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Zones.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Zone>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // Driver Start
        [HttpGet("DriverGet")]
        public IActionResult DriverGet()
        {
            try
            {
                var record = db.Drivers.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Driver>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Driver>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DriverCreate")]
        public async Task<IActionResult> DriverCreate(Driver obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Driver>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Drivers.ToList().Exists(p => p.Name.Equals(obj.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.Drivers.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Driver>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("DriverUpdate/{id}")]
        public async Task<IActionResult> DriverUpdate(int id, Driver data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Driver>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.Drivers.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.Drivers.FindAsync(id);
                if (data.Name != null && data.Name != "undefined")
                {
                    record.Name = data.Name;
                }
                if (data.Address != null && data.Address != "undefined")
                {
                    record.Address = data.Address;
                }
                if (data.Address1 != null && data.Address1 != "undefined")
                {
                    record.Address1 = data.Address1;
                }
                if (data.City != null && data.City != "undefined")
                {
                    record.City = data.City;
                }
                if (data.Country != null && data.Country != "undefined")
                {
                    record.Country = data.Country;
                }
                if (data.DrivingLicenseNumber != null && data.DrivingLicenseNumber != "undefined")
                {
                    record.DrivingLicenseNumber = data.DrivingLicenseNumber;
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
                    var Response = ResponseBuilder.BuildWSResponse<Driver>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("DriverByID/{id}")]
        public IActionResult DriverByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Driver>();

                var record = db.Drivers.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Driver>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteDriver/{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<Driver>();
                Driver data = await db.Drivers.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Drivers.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Driver>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //Route
        [HttpGet("RouteGet")]
        public IActionResult RouteGet()
        {
            try
            {
                var record = db.Routes.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Route>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Route>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RouteCreate")]
        public async Task<IActionResult> RouteCreate(Route obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Route>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Routes.ToList().Exists(p => p.RouteName.Equals(obj.RouteName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.Routes.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Route>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("RouteUpdate/{id}")]
        public async Task<IActionResult> RouteUpdate(int id, Route data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Route>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.Routes.ToList().Exists(x => x.RouteName.Equals(data.RouteName, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.Routes.FindAsync(id);
                if (data.RouteName != null && data.RouteName != "undefined")
                {
                    record.RouteName = data.RouteName;
                }
                if (data.InitialLocation != null && data.InitialLocation != "undefined")
                {
                    record.InitialLocation = data.InitialLocation;
                }
                if (data.DesignationLocation != null && data.DesignationLocation != "undefined")
                {
                    record.DesignationLocation = data.DesignationLocation;
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
                    var Response = ResponseBuilder.BuildWSResponse<Route>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("RouteByID/{id}")]
        public IActionResult RouteByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Route>();

                var record = db.Routes.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Route>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteRoute/{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<Route>();
                Route data = await db.Routes.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Routes.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Route>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        //CustomerState
        [HttpGet("CustomerStateGet")]
        public IActionResult CustomerStateGet()
        {
            try
            {
                var record = db.CustomerStates.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerState>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerState>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CustomerStateCreate")]
        public async Task<IActionResult> CustomerStateCreate(CustomerState obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerState>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.CustomerStates.ToList().Exists(p => p.StateName.Equals(obj.StateName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.CustomerStates.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerState>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CustomerStateUpdate/{id}")]
        public async Task<IActionResult> CustomerStateUpdate(int id, CustomerState data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerState>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.CustomerStates.ToList().Exists(x => x.StateName.Equals(data.StateName, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.CustomerStates.FindAsync(id);
                if (data.StateName != null && data.StateName != "undefined")
                {
                    record.StateName = data.StateName;
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerState>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CustomerStateByID/{id}")]
        public IActionResult CustomerStateByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerState>();

                var record = db.CustomerStates.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerState>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteCustomerState/{id}")]
        public async Task<IActionResult> DeleteCustomerState(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<CustomerState>();
                CustomerState data = await db.CustomerStates.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.CustomerStates.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerState>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        //Shipment
        [HttpGet("ShipmentPurchaseGet")]
        public IActionResult ShipmentPurchaseGet()
        {
            try
            {
                var record = db.ShipmentPurchases.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<ShipmentPurchase>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ShipmentPurchase>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ShipmentPurchaseCreate")]
        public async Task<IActionResult> ShipmentPurchaseCreate(ShipmentPurchase obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ShipmentPurchase>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.ShipmentPurchases.ToList().Exists(p => p.Type.Equals(obj.Type, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.ShipmentPurchases.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ShipmentPurchase>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ShipmentPurchaseUpdate/{id}")]
        public async Task<IActionResult> ShipmentPurchaseUpdate(int id, ShipmentPurchase data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ShipmentPurchase>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.ShipmentPurchases.ToList().Exists(x => x.Type.Equals(data.Type, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.ShipmentPurchases.FindAsync(id);
                if (data.Type != null && data.Type != "undefined")
                {
                    record.Type = data.Type;
                }
                if (data.ShipNumber != null && data.ShipNumber != "undefined")
                {
                    record.ShipNumber = data.ShipNumber;
                }
                if (data.Reference != null && data.Reference != "undefined")
                {
                    record.Reference = data.Reference;
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
                    var Response = ResponseBuilder.BuildWSResponse<ShipmentPurchase>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ShipmentPurchaseByID/{id}")]
        public IActionResult ShipmentPurchaseByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ShipmentPurchase>();

                var record = db.ShipmentPurchases.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<ShipmentPurchase>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteShipmentPurchase/{id}")]
        public async Task<IActionResult> DeleteShipmentPurchase(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<ShipmentPurchase>();
                ShipmentPurchase data = await db.ShipmentPurchases.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.ShipmentPurchases.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ShipmentPurchase>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        //DrivingLicenseState
        [HttpGet("DrivingLicenseStateGet")]
        public IActionResult DrivingLicenseStateGet()
        {
            try
            {
                var record = db.DrivingLicenseStates.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<DrivingLicenseState>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<DrivingLicenseState>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DrivingLicenseStateCreate")]
        public async Task<IActionResult> DrivingLicenseStateCreate(DrivingLicenseState obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<DrivingLicenseState>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.DrivingLicenseStates.ToList().Exists(p => p.Name.Equals(obj.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.DrivingLicenseStates.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<DrivingLicenseState>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("DrivingLicenseStateUpdate/{id}")]
        public async Task<IActionResult> DrivingLicenseStateUpdate(int id, DrivingLicenseState data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<DrivingLicenseState>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.DrivingLicenseStates.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.DrivingLicenseStates.FindAsync(id);
                if (data.Name != null && data.Name != "undefined")
                {
                    record.Name = data.Name;
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
                    var Response = ResponseBuilder.BuildWSResponse<DrivingLicenseState>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("DrivingLicenseStateByID/{id}")]
        public IActionResult DrivingLicenseStateByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<DrivingLicenseState>();

                var record = db.DrivingLicenseStates.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<DrivingLicenseState>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteDrivingLicenseState/{id}")]
        public async Task<IActionResult> DeleteDrivingLicenseState(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<DrivingLicenseState>();
                DrivingLicenseState data = await db.DrivingLicenseStates.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.DrivingLicenseStates.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<DrivingLicenseState>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        //CustomerInformation
        [HttpGet("CustomerInformationGet")]
        public IActionResult CustomerInformationGet()
        {
            try
            {
                var inforecord = db.CustomerInformations.ToList();
                List<CustomerInformation> record = new List<CustomerInformation>();
                CustomerInformation customerinformation = null;
                if (inforecord != null)
                {
                    foreach (var item in inforecord)
                    {
                        customerinformation = new CustomerInformation();
                        customerinformation.Id = item.Id;
                        customerinformation.Company = item.Company;
                        customerinformation.Gender = item.Gender;
                        customerinformation.FirstName = item.FirstName;
                        customerinformation.LastName = item.LastName;
                        customerinformation.Street = item.Street;
                        customerinformation.City = item.City;
                        customerinformation.StateId = item.StateId;
                        customerinformation.State = item.State;
                        customerinformation.Zip = item.Zip;
                        customerinformation.Country = item.Country;
                        customerinformation.CheckAddress = item.CheckAddress;
                        customerinformation.Phone = item.Phone;
                        customerinformation.Fax = item.Fax;
                        customerinformation.Cell = item.Cell;
                        customerinformation.ProviderId = item.ProviderId;
                        customerinformation.Provider = item.Provider;
                        customerinformation.Email = item.Email;
                        customerinformation.Website = item.Website;
                        customerinformation.TaxIdfein = item.TaxIdfein;
                        customerinformation.StateIdnumber = item.StateIdnumber;
                        customerinformation.TobaccoLicenseNumber = item.TobaccoLicenseNumber;
                        customerinformation.Vendor = item.Vendor;
                        customerinformation.CigaretteLicenseNumber = item.CigaretteLicenseNumber;
                        customerinformation.Dea = item.Dea;
                        customerinformation.Memo = item.Memo;
                        customerinformation.Authorized = item.Authorized;
                        customerinformation.OwnerAddress = item.OwnerAddress;
                        customerinformation.BusinessAddress = item.BusinessAddress;
                        customerinformation.VehicleNumber = item.VehicleNumber;
                        customerinformation.CustomerTypeId = item.CustomerTypeId;
                        customerinformation.CustomerType = item.CustomerType;
                        customerinformation.CustomerCode = item.CustomerCode;
                        customerinformation.Balance = item.Balance;
                        customerinformation.Dob = item.Dob;
                        customerinformation.Ssn = item.Ssn;
                        customerinformation.DrivingLicenseNumber = item.DrivingLicenseNumber;
                        customerinformation.DrivingLicenseStateId = item.DrivingLicenseStateId;
                        customerinformation.DrivingLicenseState = item.DrivingLicenseState;
                        var classificationrecord = db.CustomerClassifications.Where(x => x.CustomerInfoId == customerinformation.Id).FirstOrDefault();
                        customerinformation.CustomerClassification = classificationrecord;
                        var billinginforecord = db.CustomerBillingInfos.Where(x => x.CustomerInformationId == customerinformation.Id).FirstOrDefault();
                        customerinformation.CustomerBillingInfo = billinginforecord;
                        record.Add(customerinformation);
                    }
                    var Response = ResponseBuilder.BuildWSResponse<List<CustomerInformation>>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                else
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CustomerInformationCreate")]
        public async Task<IActionResult> CustomerInformationCreate(CustomerInformation obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                CustomerInformation customerinformation = new CustomerInformation();

                var respcustomer = db.CustomerInformations.ToList();
                if (respcustomer.Count() > 0)
                {
                    var customeroderecord = respcustomer;
                    if (customeroderecord != null && customeroderecord.Count() > 0)
                    {
                        CustomerInformation newcustomer = new CustomerInformation();
                        var fullcode = "";
                        if (customeroderecord[0].CustomerCode != null && customeroderecord[0].CustomerCode != "string" && customeroderecord[0].CustomerCode != "")
                        {
                            int large, small;
                            int CustomerInfoID = 0;
                            large = Convert.ToInt32(customeroderecord[0].CustomerCode);
                            small = Convert.ToInt32(customeroderecord[0].CustomerCode);
                            for (int i = 0; i < customeroderecord.Count(); i++)
                            {
                                if (customeroderecord[i].CustomerCode != null)
                                {
                                    var t = Convert.ToInt32(customeroderecord[i].CustomerCode);
                                    if (Convert.ToInt32(customeroderecord[i].CustomerCode) > large)
                                    {
                                        CustomerInfoID = Convert.ToInt32(customeroderecord[i].Id);
                                        large = Convert.ToInt32(customeroderecord[i].CustomerCode);

                                    }
                                    else if (Convert.ToInt32(customeroderecord[i].CustomerCode) < small)
                                    {
                                        small = Convert.ToInt32(customeroderecord[i].CustomerCode);
                                    }
                                    else
                                    {
                                        if (large < 2)
                                        {
                                            CustomerInfoID = Convert.ToInt32(customeroderecord[i].Id);
                                        }
                                    }
                                }
                            }
                            newcustomer = customeroderecord.ToList().Where(x => x.Id == CustomerInfoID).FirstOrDefault();
                            if (newcustomer != null)
                            {
                                if (newcustomer.CustomerCode != null)
                                {
                                    var VcodeSplit = newcustomer.CustomerCode;
                                    int code = Convert.ToInt32(VcodeSplit) + 1;

                                    if (Convert.ToDouble(VcodeSplit) > 9)
                                    {
                                        fullcode = "00" + Convert.ToString(code);
                                    }
                                    else if (Convert.ToDouble(VcodeSplit) > 99)
                                    {
                                        fullcode = "0" + Convert.ToString(code);
                                    }
                                    else if (Convert.ToDouble(VcodeSplit) > 999)
                                    {
                                        fullcode = Convert.ToString(code);
                                    }
                                    else
                                    {
                                        fullcode = "000" + Convert.ToString(code);
                                    }
                                }
                                else
                                {
                                    fullcode = "000" + "1";
                                }
                            }
                            else
                            {
                                fullcode = "000" + "1";
                            }
                        }
                        else
                        {
                            fullcode = "000" + "1";
                        }

                        customerinformation.CustomerCode = fullcode;
                        obj.CustomerCode = customerinformation.CustomerCode;
                    }
                    else
                    {
                        customerinformation.CustomerCode = "000" + "1";
                        obj.CustomerCode = customerinformation.CustomerCode;
                    }
                }
                else
                {
                    customerinformation.CustomerCode = "000" + "1";
                    obj.CustomerCode = customerinformation.CustomerCode;
                }
                bool checkcustomercode = db.CustomerInformations.ToList().Exists(x => x.CustomerCode.Equals(obj.CustomerCode, StringComparison.CurrentCultureIgnoreCase) && x.Company == obj.Company);
                if (checkcustomercode)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                if (obj != null)
                {
                    if (obj.CheckAddress == null)
                    {
                        customerinformation.CheckAddress = false;
                    }
                    if (obj.Authorized == null)
                    {
                        customerinformation.Authorized = false;
                    }
                    customerinformation.Balance = obj.Balance;
                    customerinformation.Company = obj.Company;
                    customerinformation.Gender = obj.Gender;
                    customerinformation.FirstName = obj.FirstName;
                    customerinformation.LastName = obj.LastName;
                    customerinformation.Street = obj.Street;
                    customerinformation.City = obj.City;
                    customerinformation.StateId = obj.StateId;
                    customerinformation.Country = obj.Country;
                    customerinformation.Phone = obj.Phone;
                    customerinformation.Fax = obj.Fax;
                    customerinformation.CheckAddress = obj.CheckAddress;
                    customerinformation.Email = obj.Email;
                    customerinformation.Cell = obj.Cell;
                    customerinformation.ProviderId = obj.ProviderId;
                    customerinformation.Other = obj.Other;
                    customerinformation.Website = obj.Website;
                    customerinformation.TobaccoLicenseNumber = obj.TobaccoLicenseNumber;
                    customerinformation.CigaretteLicenseNumber = obj.CigaretteLicenseNumber;
                    customerinformation.OwnerAddress = obj.OwnerAddress;
                    customerinformation.BusinessAddress = obj.BusinessAddress;
                    customerinformation.TaxIdfein = obj.TaxIdfein;
                    customerinformation.StateIdnumber = obj.StateIdnumber;
                    customerinformation.Vendor = obj.Vendor;
                    customerinformation.Dea = obj.Dea;
                    customerinformation.Memo = obj.Memo;
                    customerinformation.CustomerTypeId = obj.CustomerTypeId;
                    customerinformation.Dob = obj.Dob;
                    customerinformation.Ssn = obj.Ssn;
                    customerinformation.DrivingLicenseNumber = obj.DrivingLicenseNumber;
                    customerinformation.DrivingLicenseStateId = obj.DrivingLicenseStateId;
                    customerinformation.VehicleNumber = obj.VehicleNumber;
                    customerinformation.Authorized = obj.Authorized;
                    customerinformation.Pending = false;
                    customerinformation.Rejected = false;
                    customerinformation.Approved = true;

                    if (obj.ProviderId != null && obj.ProviderId != 0)
                    {
                        var getprovider = db.Providers.Find(obj.ProviderId);
                        if (getprovider != null)
                        {
                            customerinformation.Provider = getprovider.Name;
                        }

                    }

                    if (obj.CustomerTypeId != null && obj.CustomerTypeId != 0)
                    {
                        var getcustomer = db.CustomerTypes.Find(obj.CustomerTypeId);
                        if (getcustomer != null)
                        {
                            customerinformation.CustomerType = getcustomer.TypeName;
                        }

                    }

                    if (obj.StateId != null && obj.StateId != 0)
                    {
                        var getcustomerstate = db.CustomerStates.Find(obj.StateId);
                        if (getcustomerstate != null)
                        {
                            customerinformation.State = getcustomerstate.StateName;
                        }

                    }

                    if (obj.DrivingLicenseStateId != null && obj.DrivingLicenseStateId != 0)
                    {
                        var getdrivinglicense = db.DrivingLicenseStates.Find(obj.DrivingLicenseStateId);
                        if (getdrivinglicense != null)
                        {
                            customerinformation.DrivingLicenseState = getdrivinglicense.Name;
                        }

                    }

                    if (obj.CustomerClassification.GroupId != null && obj.CustomerClassification.GroupId != 0)
                    {
                        var getgroup = db.Groups.Find(obj.CustomerClassification.GroupId);
                        if (getgroup != null)
                        {
                            obj.CustomerClassification.GroupName = getgroup.Name;
                        }

                    }

                    if (obj.CustomerClassification.SubGroupId != null && obj.CustomerClassification.SubGroupId != 0)
                    {
                        var getsubgroup = db.SubGroups.Find(obj.CustomerClassification.SubGroupId);
                        if (getsubgroup != null)
                        {
                            obj.CustomerClassification.SubGroupName = getsubgroup.SubGroupName;
                        }

                    }


                    if (obj.CustomerClassification.BusinessTypeId != null && obj.CustomerClassification.BusinessTypeId != 0)
                    {
                        var getbusiness = db.BusinessTypes.Find(obj.CustomerClassification.BusinessTypeId);
                        if (getbusiness != null)
                        {
                            obj.CustomerClassification.BusinessType = getbusiness.TypeName;
                        }

                    }
                    if (obj.CustomerClassification.ZoneId != null && obj.CustomerClassification.ZoneId != 0)
                    {
                        var getzone = db.Zones.Find(obj.CustomerClassification.ZoneId);
                        if (getzone != null)
                        {
                            obj.CustomerClassification.Zone = getzone.Name;
                        }

                    }
                    if (obj.CustomerClassification.SalesmanId != null && obj.CustomerClassification.SalesmanId != 0)
                    {
                        var getsaleman = db.Salesmen.Find(obj.CustomerClassification.SalesmanId);
                        if (getsaleman != null)
                        {
                            obj.CustomerClassification.Salesman = getsaleman.Name;
                        }

                    }
                    if (obj.CustomerClassification.ShippedViaId != null && obj.CustomerClassification.ShippedViaId != 0)
                    {
                        var getshipment = db.ShipmentPurchases.Find(obj.CustomerClassification.ShippedViaId);
                        if (getshipment != null)
                        {
                            obj.CustomerClassification.ShippedVia = getshipment.Type;
                        }

                    }
                    if (obj.CustomerClassification.RouteId != null && obj.CustomerClassification.RouteId != 0)
                    {
                        var getroute = db.Routes.Find(obj.CustomerClassification.RouteId);
                        if (getroute != null)
                        {
                            obj.CustomerClassification.RouteName = getroute.RouteName;
                        }

                    }
                    if (obj.CustomerClassification.DriverId != null && obj.CustomerClassification.DriverId != 0)
                    {
                        var getdriver = db.Drivers.Find(obj.CustomerClassification.DriverId);
                        if (getdriver != null)
                        {
                            obj.CustomerClassification.DriverName = getdriver.Name;
                        }

                    }
                    if (obj.CustomerClassification.ShiptoReferenceId != null && obj.CustomerClassification.ShiptoReferenceId != 0)
                    {
                        var getshipreference = db.ShiptoReferences.Find(obj.CustomerClassification.ShiptoReferenceId);
                        if (getshipreference != null)
                        {
                            obj.CustomerClassification.ShiptoReference = getshipreference.Name;
                        }
                    }
                    if (obj.CustomerBillingInfo.PaymentTermsId != null && obj.CustomerBillingInfo.PaymentTermsId != 0)
                    {
                        var getpaymentterm = db.PaymentTerms.Find(obj.CustomerBillingInfo.PaymentTermsId);
                        if (getpaymentterm != null)
                        {
                            obj.CustomerBillingInfo.PaymentTerms = getpaymentterm.Name;
                        }
                    }
                    if (obj.CustomerBillingInfo.PricingId != null && obj.CustomerBillingInfo.PricingId != 0)
                    {
                        var getpricing = db.Pricings.Find(obj.CustomerBillingInfo.PricingId);
                        if (getpricing != null)
                        {
                            obj.CustomerBillingInfo.Pricing = getpricing.Name;
                        }
                    }
                }


                //account
                Account objacount = null;
                objacount = new Account();
                var subaccrecord = db.AccountSubGroups.ToList().Where(x => x.Title == "Customers").FirstOrDefault();
                if (subaccrecord != null)
                {
                    var getAccount = db.Accounts.ToList().Where(x => x.AccountSubGroupId == subaccrecord.AccountSubGroupId).LastOrDefault();
                    if (getAccount != null)
                    {
                        var code = getAccount.AccountId.Split("-")[3];
                        int getcode = 0;
                        if (code != null)
                        {

                            getcode = Convert.ToInt32(code) + 1;
                        }
                        if (getcode > 9)
                        {
                            objacount.AccountId = subaccrecord.AccountSubGroupId + "-00" + Convert.ToString(getcode);

                        }
                        else if (getcode > 99)
                        {
                            objacount.AccountId = subaccrecord.AccountSubGroupId + "-0" + Convert.ToString(getcode);
                        }
                        else
                        {
                            objacount.AccountId = subaccrecord.AccountSubGroupId + "-000" + Convert.ToString(getcode);
                        }
                        objacount.Title = obj.Company;
                        objacount.Status = 1;
                        objacount.AccountSubGroupId = subaccrecord.AccountSubGroupId;
                        var customeracc = db.Accounts.Add(objacount);
                        db.SaveChanges();

                        customerinformation.AccountId = customeracc.Entity.AccountId;
                        customerinformation.AccountNumber = customeracc.Entity.AccountId;
                        customerinformation.AccountTitle = customeracc.Entity.Title;
                    }
                    else
                    {
                        objacount.AccountId = subaccrecord.AccountSubGroupId + "-0001";
                        objacount.Title = obj.Company;
                        objacount.Status = 1;
                        objacount.AccountSubGroupId = subaccrecord.AccountSubGroupId;
                        var customeracc = db.Accounts.Add(objacount);
                        db.SaveChanges();
                        customerinformation.AccountId = customeracc.Entity.AccountId;
                        customerinformation.AccountNumber = customeracc.Entity.AccountId;
                        customerinformation.AccountTitle = customeracc.Entity.Title;
                    }
                }

                var record = db.CustomerInformations.Add(customerinformation);
                await db.SaveChangesAsync();

                CustomerClassification customerclassification = new CustomerClassification();

                if (record.Entity.CustomerClassification != null)
                {
                    if (obj.CustomerClassification.OutOfStateCustomer == null)
                    {
                        customerclassification.OutOfStateCustomer = false;
                    }
                    if (obj.CustomerClassification.AddtoemailTextList == null)
                    {
                        customerclassification.AddtoemailTextList = false;
                    }
                    if (obj.CustomerClassification.AddtoMaillingList == null)
                    {
                        customerclassification.AddtoMaillingList = false;
                    }
                    if (obj.CustomerClassification.RejectPromotion == null)
                    {
                        customerclassification.RejectPromotion = false;
                    }
                    if (obj.CustomerClassification.ViewInvoicePrevBalance == null)
                    {
                        customerclassification.ViewInvoicePrevBalance = false;
                    }
                    if (obj.CustomerClassification.ViewRetailandDiscount == null)
                    {
                        customerclassification.ViewRetailandDiscount = false;
                    }
                    if (obj.CustomerClassification.SpecialInvoiceCustom == null)
                    {
                        customerclassification.SpecialInvoiceCustom = false;
                    }
                    if (obj.CustomerClassification.UseDefaultInvMemo == null)
                    {
                        customerclassification.UseDefaultInvMemo = false;
                    }

                    customerclassification.CustomerInfoId = record.Entity.Id;
                    customerclassification.CustomerCode = record.Entity.CustomerCode;
                    customerclassification.GroupId = obj.CustomerClassification.GroupId;
                    customerclassification.GroupName = obj.CustomerClassification.GroupName;
                    customerclassification.SubGroupId = obj.CustomerClassification.SubGroupId;
                    customerclassification.SubGroupName = obj.CustomerClassification.SubGroupName;
                    customerclassification.ZoneId = obj.CustomerClassification.ZoneId;
                    customerclassification.Zone = obj.CustomerClassification.Zone;
                    customerclassification.SalesmanId = obj.CustomerClassification.SalesmanId;
                    customerclassification.Salesman = obj.CustomerClassification.Salesman;
                    customerclassification.ShippedViaId = obj.CustomerClassification.ShippedViaId;
                    customerclassification.ShippedVia = obj.CustomerClassification.ShippedVia;
                    customerclassification.RouteId = obj.CustomerClassification.RouteId;
                    customerclassification.RouteName = obj.CustomerClassification.RouteName;
                    customerclassification.RouteDeliveryDay = obj.CustomerClassification.RouteDeliveryDay;
                    customerclassification.DriverId = obj.CustomerClassification.DriverId;
                    customerclassification.DriverName = obj.CustomerClassification.DriverName;
                    customerclassification.ShiptoReferenceId = obj.CustomerClassification.ShiptoReferenceId;
                    customerclassification.ShiptoReference = obj.CustomerClassification.ShiptoReference;
                    customerclassification.OutOfStateCustomer = obj.CustomerClassification.OutOfStateCustomer;
                    customerclassification.AddtoMaillingList = obj.CustomerClassification.AddtoMaillingList;
                    customerclassification.AddtoemailTextList = obj.CustomerClassification.AddtoemailTextList;
                    customerclassification.RejectPromotion = obj.CustomerClassification.RejectPromotion;
                    customerclassification.ViewInvoicePrevBalance = obj.CustomerClassification.ViewInvoicePrevBalance;
                    customerclassification.ViewRetailandDiscount = obj.CustomerClassification.ViewRetailandDiscount;
                    customerclassification.BarCodeId = obj.CustomerClassification.BarCodeId;
                    customerclassification.BarCode = obj.CustomerClassification.BarCode;
                    customerclassification.SpecialInvoiceCustom = obj.CustomerClassification.SpecialInvoiceCustom;
                    customerclassification.OtherCustomerReference = obj.CustomerClassification.OtherCustomerReference;
                    customerclassification.UseDefaultInvMemo = obj.CustomerClassification.UseDefaultInvMemo;
                    customerclassification.InvoiceMemo = obj.CustomerClassification.InvoiceMemo;
                }
                else
                {
                    if (obj.CustomerClassification.OutOfStateCustomer == null)
                    {
                        customerclassification.OutOfStateCustomer = false;
                    }
                    if (obj.CustomerClassification.AddtoemailTextList == null)
                    {
                        customerclassification.AddtoemailTextList = false;
                    }
                    if (obj.CustomerClassification.AddtoMaillingList == null)
                    {
                        customerclassification.AddtoMaillingList = false;
                    }
                    if (obj.CustomerClassification.RejectPromotion == null)
                    {
                        customerclassification.RejectPromotion = false;
                    }
                    if (obj.CustomerClassification.ViewInvoicePrevBalance == null)
                    {
                        customerclassification.ViewInvoicePrevBalance = false;
                    }
                    if (obj.CustomerClassification.ViewRetailandDiscount == null)
                    {
                        customerclassification.ViewRetailandDiscount = false;
                    }
                    if (obj.CustomerClassification.SpecialInvoiceCustom == null)
                    {
                        customerclassification.SpecialInvoiceCustom = false;
                    }
                    if (obj.CustomerClassification.UseDefaultInvMemo == null)
                    {
                        customerclassification.UseDefaultInvMemo = false;
                    }
                    customerclassification.CustomerInfoId = record.Entity.Id;
                    customerclassification.CustomerCode = record.Entity.CustomerCode;
                    customerclassification.GroupId = obj.CustomerClassification.GroupId;
                    customerclassification.GroupName = obj.CustomerClassification.GroupName;
                    customerclassification.SubGroupId = obj.CustomerClassification.SubGroupId;
                    customerclassification.SubGroupName = obj.CustomerClassification.SubGroupName;
                    customerclassification.ZoneId = obj.CustomerClassification.ZoneId;
                    customerclassification.Zone = obj.CustomerClassification.Zone;
                    customerclassification.SalesmanId = obj.CustomerClassification.SalesmanId;
                    customerclassification.Salesman = obj.CustomerClassification.Salesman;
                    customerclassification.ShippedViaId = obj.CustomerClassification.ShippedViaId;
                    customerclassification.ShippedVia = obj.CustomerClassification.ShippedVia;
                    customerclassification.RouteId = obj.CustomerClassification.RouteId;
                    customerclassification.RouteName = obj.CustomerClassification.RouteName;
                    customerclassification.RouteDeliveryDay = obj.CustomerClassification.RouteDeliveryDay;
                    customerclassification.DriverId = obj.CustomerClassification.DriverId;
                    customerclassification.DriverName = obj.CustomerClassification.DriverName;
                    customerclassification.ShiptoReferenceId = obj.CustomerClassification.ShiptoReferenceId;
                    customerclassification.ShiptoReference = obj.CustomerClassification.ShiptoReference;
                    customerclassification.OutOfStateCustomer = obj.CustomerClassification.OutOfStateCustomer;
                    customerclassification.AddtoMaillingList = obj.CustomerClassification.AddtoMaillingList;
                    customerclassification.AddtoemailTextList = obj.CustomerClassification.AddtoemailTextList;
                    customerclassification.RejectPromotion = obj.CustomerClassification.RejectPromotion;
                    customerclassification.ViewInvoicePrevBalance = obj.CustomerClassification.ViewInvoicePrevBalance;
                    customerclassification.ViewRetailandDiscount = obj.CustomerClassification.ViewRetailandDiscount;
                    customerclassification.BarCodeId = obj.CustomerClassification.BarCodeId;
                    customerclassification.BarCode = obj.CustomerClassification.BarCode;
                    customerclassification.SpecialInvoiceCustom = obj.CustomerClassification.SpecialInvoiceCustom;
                    customerclassification.OtherCustomerReference = obj.CustomerClassification.OtherCustomerReference;
                    customerclassification.UseDefaultInvMemo = obj.CustomerClassification.UseDefaultInvMemo;
                    customerclassification.InvoiceMemo = obj.CustomerClassification.InvoiceMemo;
                }
                var classificationrecord = db.CustomerClassifications.Add(customerclassification);
                await db.SaveChangesAsync();
                CustomerBillingInfo customerbillinginfo = new CustomerBillingInfo();

                if (record.Entity.CustomerBillingInfo != null)
                {
                    if (obj.CustomerBillingInfo.IsGetSalesDiscounts == null)
                    {
                        customerbillinginfo.IsGetSalesDiscounts = false;
                    }
                    if (obj.CustomerBillingInfo.IsOutOfStateCustomer == null)
                    {
                        customerbillinginfo.IsOutOfStateCustomer = false;
                    }
                    if (obj.CustomerBillingInfo.IsCreditHold == null)
                    {
                        customerbillinginfo.IsCreditHold = false;
                    }
                    if (obj.CustomerBillingInfo.IsBillToBill == null)
                    {
                        customerbillinginfo.IsBillToBill = false;
                    }
                    if (obj.CustomerBillingInfo.IsNoCheckAccepted == null)
                    {
                        customerbillinginfo.IsNoCheckAccepted = false;
                    }
                    if (obj.CustomerBillingInfo.IsExclude == null)
                    {
                        customerbillinginfo.IsExclude = false;
                    }
                    if (obj.CustomerBillingInfo.IsCashBackBalance == null)
                    {
                        customerbillinginfo.IsCashBackBalance = false;
                    }
                    if (obj.CustomerBillingInfo.IsPopupMessage == null)
                    {
                        customerbillinginfo.IsPopupMessage = false;
                    }

                    customerbillinginfo.CustomerInformationId = record.Entity.Id;
                    customerbillinginfo.CustomerCode = record.Entity.CustomerCode;
                    customerbillinginfo.CustomerClassificationId = classificationrecord.Entity.Id;
                    customerbillinginfo.IsTaxExempt = obj.CustomerBillingInfo.IsTaxExempt;
                    customerbillinginfo.PricingId = obj.CustomerBillingInfo.PricingId;
                    customerbillinginfo.Pricing = obj.CustomerBillingInfo.Pricing;
                    customerbillinginfo.RetailPlus = obj.CustomerBillingInfo.RetailPlus;
                    customerbillinginfo.RetailPlusPercentage = obj.CustomerBillingInfo.RetailPlusPercentage;
                    customerbillinginfo.IsGetSalesDiscounts = obj.CustomerBillingInfo.IsGetSalesDiscounts;
                    customerbillinginfo.IsOutOfStateCustomer = obj.CustomerBillingInfo.IsOutOfStateCustomer;
                    customerbillinginfo.AdditionalInvoiceCharge = obj.CustomerBillingInfo.AdditionalInvoiceCharge;
                    customerbillinginfo.AdditionalInvoiceDiscount = obj.CustomerBillingInfo.AdditionalInvoiceDiscount;
                    customerbillinginfo.ScheduleMessage = obj.CustomerBillingInfo.ScheduleMessage;
                    customerbillinginfo.ScheduleMessageFromDate = obj.CustomerBillingInfo.ScheduleMessageFromDate;
                    customerbillinginfo.ScheduleMessageToDate = obj.CustomerBillingInfo.ScheduleMessageToDate;
                    customerbillinginfo.PaymentTermsId = obj.CustomerBillingInfo.PaymentTermsId;
                    customerbillinginfo.PaymentTerms = obj.CustomerBillingInfo.PaymentTerms;
                    customerbillinginfo.CreditLimit = obj.CustomerBillingInfo.CreditLimit;
                    customerbillinginfo.IsCreditHold = obj.CustomerBillingInfo.IsCreditHold;
                    customerbillinginfo.IsBillToBill = obj.CustomerBillingInfo.IsBillToBill;
                    customerbillinginfo.IsNoCheckAccepted = obj.CustomerBillingInfo.IsNoCheckAccepted;
                    customerbillinginfo.IsExclude = obj.CustomerBillingInfo.IsExclude;
                    customerbillinginfo.ThirdPartyCheckCharge = obj.CustomerBillingInfo.ThirdPartyCheckCharge;
                    customerbillinginfo.IsCashBackBalance = obj.CustomerBillingInfo.IsCashBackBalance;
                    customerbillinginfo.CashBackBalance = obj.CustomerBillingInfo.CashBackBalance;
                    customerbillinginfo.IsPopupMessage = obj.CustomerBillingInfo.IsPopupMessage;
                    customerbillinginfo.PopupMessage = obj.CustomerBillingInfo.PopupMessage;
                }
                else
                {
                    if (obj.CustomerBillingInfo.IsGetSalesDiscounts == null)
                    {
                        customerbillinginfo.IsGetSalesDiscounts = false;
                    }
                    if (obj.CustomerBillingInfo.IsOutOfStateCustomer == null)
                    {
                        customerbillinginfo.IsOutOfStateCustomer = false;
                    }
                    if (obj.CustomerBillingInfo.IsCreditHold == null)
                    {
                        customerbillinginfo.IsCreditHold = false;
                    }
                    if (obj.CustomerBillingInfo.IsBillToBill == null)
                    {
                        customerbillinginfo.IsBillToBill = false;
                    }
                    if (obj.CustomerBillingInfo.IsNoCheckAccepted == null)
                    {
                        customerbillinginfo.IsNoCheckAccepted = false;
                    }
                    if (obj.CustomerBillingInfo.IsExclude == null)
                    {
                        customerbillinginfo.IsExclude = false;
                    }
                    if (obj.CustomerBillingInfo.IsCashBackBalance == null)
                    {
                        customerbillinginfo.IsCashBackBalance = false;
                    }
                    if (obj.CustomerBillingInfo.IsPopupMessage == null)
                    {
                        customerbillinginfo.IsPopupMessage = false;
                    }
                    customerbillinginfo.CustomerInformationId = record.Entity.Id;
                    customerbillinginfo.CustomerCode = record.Entity.CustomerCode;
                    customerbillinginfo.CustomerClassificationId = classificationrecord.Entity.Id;
                    customerbillinginfo.IsTaxExempt = obj.CustomerBillingInfo.IsTaxExempt;
                    customerbillinginfo.PricingId = obj.CustomerBillingInfo.PricingId;
                    customerbillinginfo.Pricing = obj.CustomerBillingInfo.Pricing;
                    customerbillinginfo.RetailPlus = obj.CustomerBillingInfo.RetailPlus;
                    customerbillinginfo.RetailPlusPercentage = obj.CustomerBillingInfo.RetailPlusPercentage;
                    customerbillinginfo.IsGetSalesDiscounts = obj.CustomerBillingInfo.IsGetSalesDiscounts;
                    customerbillinginfo.IsOutOfStateCustomer = obj.CustomerBillingInfo.IsOutOfStateCustomer;
                    customerbillinginfo.AdditionalInvoiceCharge = obj.CustomerBillingInfo.AdditionalInvoiceCharge;
                    customerbillinginfo.AdditionalInvoiceDiscount = obj.CustomerBillingInfo.AdditionalInvoiceDiscount;
                    customerbillinginfo.ScheduleMessage = obj.CustomerBillingInfo.ScheduleMessage;
                    customerbillinginfo.ScheduleMessageFromDate = obj.CustomerBillingInfo.ScheduleMessageFromDate;
                    customerbillinginfo.ScheduleMessageToDate = obj.CustomerBillingInfo.ScheduleMessageToDate;
                    customerbillinginfo.PaymentTermsId = obj.CustomerBillingInfo.PaymentTermsId;
                    customerbillinginfo.PaymentTerms = obj.CustomerBillingInfo.PaymentTerms;
                    customerbillinginfo.CreditLimit = obj.CustomerBillingInfo.CreditLimit;
                    customerbillinginfo.IsCreditHold = obj.CustomerBillingInfo.IsCreditHold;
                    customerbillinginfo.IsBillToBill = obj.CustomerBillingInfo.IsBillToBill;
                    customerbillinginfo.IsNoCheckAccepted = obj.CustomerBillingInfo.IsNoCheckAccepted;
                    customerbillinginfo.IsExclude = obj.CustomerBillingInfo.IsExclude;
                    customerbillinginfo.ThirdPartyCheckCharge = obj.CustomerBillingInfo.ThirdPartyCheckCharge;
                    customerbillinginfo.IsCashBackBalance = obj.CustomerBillingInfo.IsCashBackBalance;
                    customerbillinginfo.CashBackBalance = obj.CustomerBillingInfo.CashBackBalance;
                    customerbillinginfo.IsPopupMessage = obj.CustomerBillingInfo.IsPopupMessage;
                    customerbillinginfo.PopupMessage = obj.CustomerBillingInfo.PopupMessage;
                }
                db.CustomerBillingInfos.Add(customerbillinginfo);
                await db.SaveChangesAsync();



                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CustomerInformationUpdate/{id}")]
        public async Task<IActionResult> CustomerInformationUpdate(int id, CustomerInformation data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != data.Id)
                {
                    return BadRequest();
                }
                if (data.ProviderId != null && data.ProviderId != 0)
                {
                    var getprovider = db.Providers.Find(data.ProviderId);
                    if (getprovider != null)
                    {
                        data.Provider = getprovider.Name;
                    }

                }
                //db.CustomerInformations.Where(x=>x.cus)
                if (data.CustomerTypeId != null && data.CustomerTypeId != 0)
                {
                    var getcustomer = db.CustomerTypes.Find(data.CustomerTypeId);
                    if (getcustomer != null)
                    {
                        data.CustomerType = getcustomer.TypeName;
                    }

                }

                if (data.StateId != null && data.StateId != 0)
                {
                    var getcustomerstate = db.CustomerStates.Find(data.StateId);
                    if (getcustomerstate != null)
                    {
                        data.State = getcustomerstate.StateName;
                    }

                }

                if (data.DrivingLicenseStateId != null && data.DrivingLicenseStateId != 0)
                {
                    var getdrivinglicense = db.DrivingLicenseStates.Find(data.DrivingLicenseStateId);
                    if (getdrivinglicense != null)
                    {
                        data.CustomerType = getdrivinglicense.Name;
                    }

                }

                if (data.CustomerClassification.GroupId != null && data.CustomerClassification.GroupId != 0)
                {
                    var getgroup = db.Groups.Find(data.CustomerClassification.GroupId);
                    if (getgroup != null)
                    {
                        data.CustomerClassification.GroupName = getgroup.Name;
                    }

                }

                if (data.CustomerClassification.SubGroupId != null && data.CustomerClassification.SubGroupId != 0)
                {
                    var getsubgroup = db.SubGroups.Find(data.CustomerClassification.SubGroupId);
                    if (getsubgroup != null)
                    {
                        data.CustomerClassification.SubGroupName = getsubgroup.SubGroupName;
                    }

                }


                if (data.CustomerClassification.BusinessTypeId != null && data.CustomerClassification.BusinessTypeId != 0)
                {
                    var getbusiness = db.BusinessTypes.Find(data.CustomerClassification.BusinessTypeId);
                    if (getbusiness != null)
                    {
                        data.CustomerClassification.BusinessType = getbusiness.TypeName;
                    }

                }
                if (data.CustomerClassification.ZoneId != null && data.CustomerClassification.ZoneId != 0)
                {
                    var getzone = db.Zones.Find(data.CustomerClassification.ZoneId);
                    if (getzone != null)
                    {
                        data.CustomerClassification.Zone = getzone.Name;
                    }

                }
                if (data.CustomerClassification.SalesmanId != null && data.CustomerClassification.SalesmanId != 0)
                {
                    var getsaleman = db.Salesmen.Find(data.CustomerClassification.SalesmanId);
                    if (getsaleman != null)
                    {
                        data.CustomerClassification.Salesman = getsaleman.Name;
                    }

                }
                if (data.CustomerClassification.ShippedViaId != null && data.CustomerClassification.ShippedViaId != 0)
                {
                    var getshipment = db.ShipmentPurchases.Find(data.CustomerClassification.ShippedViaId);
                    if (getshipment != null)
                    {
                        data.CustomerClassification.ShippedVia = getshipment.Type;
                    }

                }
                if (data.CustomerClassification.RouteId != null && data.CustomerClassification.RouteId != 0)
                {
                    var getroute = db.Routes.Find(data.CustomerClassification.RouteId);
                    if (getroute != null)
                    {
                        data.CustomerClassification.RouteName = getroute.RouteName;
                    }

                }
                if (data.CustomerClassification.DriverId != null && data.CustomerClassification.DriverId != 0)
                {
                    var getdriver = db.Drivers.Find(data.CustomerClassification.DriverId);
                    if (getdriver != null)
                    {
                        data.CustomerClassification.DriverName = getdriver.Name;
                    }

                }
                if (data.CustomerClassification.ShiptoReferenceId != null && data.CustomerClassification.ShiptoReferenceId != 0)
                {
                    var getshipreference = db.ShiptoReferences.Find(data.CustomerClassification.ShiptoReferenceId);
                    if (getshipreference != null)
                    {
                        data.CustomerClassification.ShiptoReference = getshipreference.Name;
                    }
                }
                if (data.CustomerBillingInfo.PaymentTermsId != null && data.CustomerBillingInfo.PaymentTermsId != 0)
                {
                    var getpaymentterm = db.PaymentTerms.Find(data.CustomerBillingInfo.PaymentTermsId);
                    if (getpaymentterm != null)
                    {
                        data.CustomerBillingInfo.PaymentTerms = getpaymentterm.Name;
                    }
                }
                if (data.CustomerBillingInfo.PricingId != null && data.CustomerBillingInfo.PricingId != 0)
                {
                    var getpricing = db.Pricings.Find(data.CustomerBillingInfo.PricingId);
                    if (getpricing != null)
                    {
                        data.CustomerBillingInfo.Pricing = getpricing.Name;
                    }
                }
               
                var inforecord = await db.CustomerInformations.FindAsync(id);
                if (inforecord != null)
                {
                    if (data.Company != null && data.Company != "undefined")
                    {
                        inforecord.Company = data.Company;
                    }
                    if (data.Gender != null && data.Gender != "undefined")
                    {
                        inforecord.Gender = data.Gender;
                    }
                    if (data.FirstName != null && data.FirstName != "undefined")
                    {
                        inforecord.FirstName = data.FirstName;
                    }
                    if (data.LastName != null && data.LastName != "undefined")
                    {
                        inforecord.LastName = data.LastName;
                    }
                    if (data.Street != null && data.Street != "undefined")
                    {
                        inforecord.Street = data.Street;
                    }
                    if (data.City != null && data.City != "undefined")
                    {
                        inforecord.City = data.City;
                    }
                    if (data.StateId != null)
                    {
                        inforecord.StateId = data.StateId;
                    }
                    if (data.State != null && data.State != "undefined")
                    {
                        inforecord.State = data.State;
                    }
                    if (data.Zip != null && data.Zip != "undefined")
                    {
                        inforecord.Zip = data.Zip;
                    }
                    if (data.Country != null && data.Country != "undefined")
                    {
                        inforecord.Country = data.Country;
                    }
                    if (data.CheckAddress != null)
                    {
                        inforecord.CheckAddress = data.CheckAddress;
                    }
                    if (data.Phone != null && data.Phone != "undefined")
                    {
                        inforecord.Phone = data.Phone;
                    }
                    if (data.Fax != null && data.Fax != "undefined")
                    {
                        inforecord.Fax = data.Fax;
                    }
                    if (data.Cell != null && data.Cell != "undefined")
                    {
                        inforecord.Cell = data.Cell;
                    }
                    if (data.Other != null && data.Other != "undefined")
                    {
                        inforecord.Other = data.Other;
                    }
                    if (data.ProviderId != null)
                    {
                        inforecord.ProviderId = data.ProviderId;
                    }
                    if (data.Provider != null && data.Provider != "undefined")
                    {
                        inforecord.Provider = data.Provider;
                    }
                    if (data.Email != null && data.Email != "undefined")
                    {
                        inforecord.Email = data.Email;
                    }
                    if (data.Website != null && data.Website != "undefined")
                    {
                        inforecord.Website = data.Website;
                    }
                    if (data.TaxIdfein != null && data.TaxIdfein != "undefined")
                    {
                        inforecord.TaxIdfein = data.TaxIdfein;
                    }
                    if (data.StateIdnumber != null && data.StateIdnumber != "undefined")
                    {
                        inforecord.StateIdnumber = data.StateIdnumber;
                    }
                    if (data.TobaccoLicenseNumber != null && data.TobaccoLicenseNumber != "undefined")
                    {
                        inforecord.TobaccoLicenseNumber = data.TobaccoLicenseNumber;
                    }
                    if (data.CigaretteLicenseNumber != null && data.CigaretteLicenseNumber != "undefined")
                    {
                        inforecord.CigaretteLicenseNumber = data.CigaretteLicenseNumber;
                    }
                    if (data.Vendor != null && data.Vendor != "undefined")
                    {
                        inforecord.Vendor = data.Vendor;
                    }
                    if (data.Dea != null && data.Dea != "undefined")
                    {
                        inforecord.Dea = data.Dea;
                    }
                    if (data.Memo != null && data.Memo != "undefined")
                    {
                        inforecord.Memo = data.Memo;
                    }
                    if (data.Authorized != null)
                    {
                        inforecord.Authorized = data.Authorized;
                    }
                    if (data.OwnerAddress != null && data.OwnerAddress != "undefined")
                    {
                        inforecord.OwnerAddress = data.OwnerAddress;
                    }
                    if (data.BusinessAddress != null && data.BusinessAddress != "undefined")
                    {
                        inforecord.BusinessAddress = data.BusinessAddress;
                    }
                    if (data.VehicleNumber != null && data.VehicleNumber != "undefined")
                    {
                        inforecord.VehicleNumber = data.VehicleNumber;
                    }
                    if (data.CustomerTypeId != null)
                    {
                        inforecord.CustomerTypeId = data.CustomerTypeId;
                    }
                    if (data.CustomerType != null && data.CustomerType != "undefined")
                    {
                        inforecord.CustomerType = data.CustomerType;
                    }
                    if (data.Dob != null)
                    {
                        inforecord.Dob = data.Dob;
                    }
                    if (data.Ssn != null && data.Ssn != "undefined")
                    {
                        inforecord.Ssn = data.Ssn;
                    }
                    if (data.DrivingLicenseStateId != null)
                    {
                        inforecord.DrivingLicenseStateId = data.DrivingLicenseStateId;
                    }
                    if (data.DrivingLicenseState != null && data.DrivingLicenseState != "undefined")
                    {
                        inforecord.DrivingLicenseState = data.DrivingLicenseState;
                    }

                    db.Entry(inforecord).State = EntityState.Modified;
                }
                bool isValid = db.CustomerClassifications.ToList().Exists(x => x.CustomerCode.Equals(inforecord.CustomerCode, StringComparison.CurrentCultureIgnoreCase) && x.CustomerInfoId == inforecord.Id);
                if (isValid)
                {
                    var classificationrecord = db.CustomerClassifications.Where(x => x.CustomerInfoId == inforecord.Id && x.CustomerCode == inforecord.CustomerCode).FirstOrDefault();
                    if (classificationrecord != null)
                    {
                        if (data.CustomerClassification.GroupId != null)
                        {
                            classificationrecord.GroupId = data.CustomerClassification.GroupId;
                        }
                        if (data.CustomerClassification.GroupName != null && data.CustomerClassification.GroupName != "undefined")
                        {
                            classificationrecord.GroupName = data.CustomerClassification.GroupName;
                        }
                        if (data.CustomerClassification.SubGroupId != null)
                        {
                            classificationrecord.SubGroupId = data.CustomerClassification.SubGroupId;
                        }
                        if (data.CustomerClassification.SubGroupName != null && data.CustomerClassification.SubGroupName != "undefined")
                        {
                            classificationrecord.SubGroupName = data.CustomerClassification.SubGroupName;
                        }
                        if (data.CustomerClassification.ZoneId != null)
                        {
                            classificationrecord.ZoneId = data.CustomerClassification.ZoneId;
                        }
                        if (data.CustomerClassification.Zone != null && data.CustomerClassification.Zone != "undefined")
                        {
                            classificationrecord.Zone = data.CustomerClassification.Zone;
                        }
                        if (data.CustomerClassification.BusinessTypeId != null)
                        {
                            classificationrecord.BusinessTypeId = data.CustomerClassification.BusinessTypeId;
                        }
                        if (data.CustomerClassification.BusinessType != null && data.CustomerClassification.BusinessType != "undefined")
                        {
                            classificationrecord.BusinessType = data.CustomerClassification.BusinessType;
                        }
                        if (data.CustomerClassification.SalesmanId != null)
                        {
                            classificationrecord.SalesmanId = data.CustomerClassification.SalesmanId;
                        }
                        if (data.CustomerClassification.Salesman != null && data.CustomerClassification.Salesman != "undefined")
                        {
                            classificationrecord.Salesman = data.CustomerClassification.Salesman;
                        }
                        if (data.CustomerClassification.ShippedViaId != null)
                        {
                            classificationrecord.ShippedViaId = data.CustomerClassification.ShippedViaId;
                        }
                        if (data.CustomerClassification.ShippedVia != null && data.CustomerClassification.ShippedVia != "undefined")
                        {
                            classificationrecord.ShippedVia = data.CustomerClassification.ShippedVia;
                        }
                        if (data.CustomerClassification.RouteId != null)
                        {
                            classificationrecord.RouteId = data.CustomerClassification.RouteId;
                        }
                        if (data.CustomerClassification.RouteName != null && data.CustomerClassification.RouteName != "undefined")
                        {
                            classificationrecord.RouteName = data.CustomerClassification.RouteName;
                        }
                        if (data.CustomerClassification.RouteDeliveryDay != null && data.CustomerClassification.RouteDeliveryDay != "undefined")
                        {
                            classificationrecord.RouteDeliveryDay = data.CustomerClassification.RouteDeliveryDay;
                        }
                        if (data.CustomerClassification.DriverId != null)
                        {
                            classificationrecord.DriverId = data.CustomerClassification.DriverId;
                        }
                        if (data.CustomerClassification.DriverName != null && data.CustomerClassification.DriverName != "undefined")
                        {
                            classificationrecord.DriverName = data.CustomerClassification.DriverName;
                        }
                        if (data.CustomerClassification.ShiptoReferenceId != null)
                        {
                            classificationrecord.ShiptoReferenceId = data.CustomerClassification.ShiptoReferenceId;
                        }
                        if (data.CustomerClassification.ShiptoReference != null && data.CustomerClassification.ShiptoReference != "undefined")
                        {
                            classificationrecord.ShiptoReference = data.CustomerClassification.ShiptoReference;
                        }
                        if (data.CustomerClassification.OutOfStateCustomer != null)
                        {
                            classificationrecord.OutOfStateCustomer = data.CustomerClassification.OutOfStateCustomer;
                        }
                        if (data.CustomerClassification.AddtoMaillingList != null)
                        {
                            classificationrecord.AddtoMaillingList = data.CustomerClassification.AddtoMaillingList;
                        }
                        if (data.CustomerClassification.AddtoemailTextList != null)
                        {
                            classificationrecord.AddtoemailTextList = data.CustomerClassification.AddtoemailTextList;
                        }
                        if (data.CustomerClassification.RejectPromotion != null)
                        {
                            classificationrecord.RejectPromotion = data.CustomerClassification.RejectPromotion;
                        }
                        if (data.CustomerClassification.ViewInvoicePrevBalance != null)
                        {
                            classificationrecord.ViewInvoicePrevBalance = data.CustomerClassification.ViewInvoicePrevBalance;
                        }
                        if (data.CustomerClassification.ViewRetailandDiscount != null)
                        {
                            classificationrecord.ViewRetailandDiscount = data.CustomerClassification.ViewRetailandDiscount;
                        }
                        if (data.CustomerClassification.BarCodeId != null)
                        {
                            classificationrecord.BarCodeId = data.CustomerClassification.BarCodeId;
                        }
                        if (data.CustomerClassification.BarCode != null && data.CustomerClassification.BarCode != "undefined")
                        {
                            classificationrecord.BarCode = data.CustomerClassification.BarCode;
                        }
                        if (data.CustomerClassification.DefaultInvoiceCopies != null && data.CustomerClassification.DefaultInvoiceCopies != "undefined")
                        {
                            classificationrecord.DefaultInvoiceCopies = data.CustomerClassification.DefaultInvoiceCopies;
                        }
                        if (data.CustomerClassification.SpecialInvoiceCustom != null)
                        {
                            classificationrecord.SpecialInvoiceCustom = data.CustomerClassification.SpecialInvoiceCustom;
                        }
                        if (data.CustomerClassification.OtherCustomerReference != null && data.CustomerClassification.OtherCustomerReference != "undefined")
                        {
                            classificationrecord.OtherCustomerReference = data.CustomerClassification.OtherCustomerReference;
                        }
                        if (data.CustomerClassification.UseDefaultInvMemo != null)
                        {
                            classificationrecord.UseDefaultInvMemo = data.CustomerClassification.UseDefaultInvMemo;
                        }
                        if (data.CustomerClassification.InvoiceMemo != null && data.CustomerClassification.InvoiceMemo != "undefined")
                        {
                            classificationrecord.InvoiceMemo = data.CustomerClassification.InvoiceMemo;
                        }
                        db.Entry(classificationrecord).State = EntityState.Modified;
                    }
                }
                bool isbillinginfocheck = db.CustomerBillingInfos.ToList().Exists(x => x.CustomerCode.Equals(inforecord.CustomerCode, StringComparison.CurrentCultureIgnoreCase) && x.CustomerInformationId == inforecord.Id);
                if (isbillinginfocheck)
                {
                    var billinginforecord = db.CustomerBillingInfos.Where(x => x.CustomerInformationId == inforecord.Id && x.CustomerCode == inforecord.CustomerCode).FirstOrDefault();
                    if (data.CustomerBillingInfo.IsTaxExempt != null)
                    {
                        billinginforecord.IsTaxExempt = data.CustomerBillingInfo.IsTaxExempt;
                    }
                    if (data.CustomerBillingInfo.PricingId != null)
                    {
                        billinginforecord.PricingId = data.CustomerBillingInfo.PricingId;
                    }
                    if (data.CustomerBillingInfo.RetailPlus != null && data.CustomerBillingInfo.RetailPlus != "undefined")
                    {
                        billinginforecord.RetailPlus = data.CustomerBillingInfo.RetailPlus;
                    }
                    if (data.CustomerBillingInfo.RetailPlusPercentage != null && data.CustomerBillingInfo.RetailPlusPercentage != "undefined")
                    {
                        billinginforecord.RetailPlusPercentage = data.CustomerBillingInfo.RetailPlusPercentage;
                    }
                    if (data.CustomerBillingInfo.IsGetSalesDiscounts != null)
                    {
                        billinginforecord.IsGetSalesDiscounts = data.CustomerBillingInfo.IsGetSalesDiscounts;
                    }
                    if (data.CustomerBillingInfo.IsOutOfStateCustomer != null)
                    {
                        billinginforecord.IsOutOfStateCustomer = data.CustomerBillingInfo.IsOutOfStateCustomer;
                    }
                    if (data.CustomerBillingInfo.AdditionalInvoiceCharge != null && data.CustomerBillingInfo.AdditionalInvoiceCharge != "undefined")
                    {
                        billinginforecord.AdditionalInvoiceCharge = data.CustomerBillingInfo.AdditionalInvoiceCharge;
                    }
                    if (data.CustomerBillingInfo.AdditionalInvoiceDiscount != null && data.CustomerBillingInfo.AdditionalInvoiceDiscount != "undefined")
                    {
                        billinginforecord.AdditionalInvoiceDiscount = data.CustomerBillingInfo.AdditionalInvoiceDiscount;
                    }
                    if (data.CustomerBillingInfo.ScheduleMessage != null && data.CustomerBillingInfo.ScheduleMessage != "undefined")
                    {
                        billinginforecord.ScheduleMessage = data.CustomerBillingInfo.ScheduleMessage;
                    }
                    if (data.CustomerBillingInfo.ScheduleMessageFromDate != null)
                    {
                        billinginforecord.ScheduleMessageFromDate = data.CustomerBillingInfo.ScheduleMessageFromDate;
                    }
                    if (data.CustomerBillingInfo.ScheduleMessageToDate != null)
                    {
                        billinginforecord.ScheduleMessageToDate = data.CustomerBillingInfo.ScheduleMessageToDate;
                    }
                    if (data.CustomerBillingInfo.PaymentTermsId != null)
                    {
                        billinginforecord.PaymentTermsId = data.CustomerBillingInfo.PaymentTermsId;
                    }
                    if (data.CustomerBillingInfo.CreditLimit != null && data.CustomerBillingInfo.CreditLimit != "undefined")
                    {
                        billinginforecord.CreditLimit = data.CustomerBillingInfo.CreditLimit;
                    }
                    if (data.CustomerBillingInfo.IsCreditHold != null)
                    {
                        billinginforecord.IsCreditHold = data.CustomerBillingInfo.IsCreditHold;
                    }
                    if (data.CustomerBillingInfo.IsBillToBill != null)
                    {
                        billinginforecord.IsBillToBill = data.CustomerBillingInfo.IsBillToBill;
                    }
                    if (data.CustomerBillingInfo.IsNoCheckAccepted != null)
                    {
                        billinginforecord.IsNoCheckAccepted = data.CustomerBillingInfo.IsNoCheckAccepted;
                    }
                    if (data.CustomerBillingInfo.IsExclude != null)
                    {
                        billinginforecord.IsExclude = data.CustomerBillingInfo.IsExclude;
                    }
                    if (data.CustomerBillingInfo.IsCashBackBalance != null)
                    {
                        billinginforecord.IsCashBackBalance = data.CustomerBillingInfo.IsCashBackBalance;
                    }
                    if (data.CustomerBillingInfo.ThirdPartyCheckCharge != null && data.CustomerBillingInfo.ThirdPartyCheckCharge != "undefined")
                    {
                        billinginforecord.ThirdPartyCheckCharge = data.CustomerBillingInfo.ThirdPartyCheckCharge;
                    }
                    if (data.CustomerBillingInfo.CashBackBalance != null && data.CustomerBillingInfo.CashBackBalance != "undefined")
                    {
                        billinginforecord.CashBackBalance = data.CustomerBillingInfo.CashBackBalance;
                    }
                    if (data.CustomerBillingInfo.PopupMessage != null && data.CustomerBillingInfo.PopupMessage != "undefined")
                    {
                        billinginforecord.PopupMessage = data.CustomerBillingInfo.PopupMessage;
                    }
                    if (data.CustomerBillingInfo.IsPopupMessage != null)
                    {
                        billinginforecord.IsPopupMessage = data.CustomerBillingInfo.IsPopupMessage;
                    }
                    db.Entry(billinginforecord).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CustomerInformationByID/{id}")]
        public IActionResult CustomerInformationByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                var record = db.CustomerInformations.ToList().Where(x => x.Id == id).FirstOrDefault();
                if (record != null)
                {

                    if(record.State != null)
                    {
                        var getState = db.States.ToList().Where(x => x.StateName == record.State).FirstOrDefault();
                        if(getState != null)
                        {
                            record.State = getState.Id.ToString();
                        }

                        var getStateDL = db.States.ToList().Where(x => x.StateName == record.DrivingLicenseState).FirstOrDefault();
                        if (getState != null)
                        {
                            record.DrivingLicenseStateId = getState.Id;
                        }

                    }

                    if (record.CustomerType != null)
                    {
                        var getState = db.CustomerTypes.ToList().Where(x => x.TypeName == record.CustomerType).FirstOrDefault();
                        if (getState != null)
                        {
                            record.CustomerType = getState.Id.ToString();
                        }

                    }
                    var getnote = db.CustomerNotes.ToList().Where(x => x.CustomerId == record.Id).FirstOrDefault();
                    if(getnote != null)
                    {
                        record.CustomerNote = getnote.CustomerNote1;
                        record.NoteDate = Convert.ToString(getnote.NoteDate);
                    }

                    var getpay = db.Receivables.ToList().Where(x => x.AccountId == record.AccountId).FirstOrDefault();
                    if (getpay != null)
                    {
                        if(getpay.Amount == null)
                        {
                            getpay.Amount = "0.00";
                        }
                        else
                        {
                            var amount = Convert.ToDouble(getpay.Amount);
                            record.Balance = Math.Round(amount, 2).ToString();
                        }

                    }
                    else
                    {
                        record.Balance = "0.00";
                    }
                    var getCertificate = db.CertificateExemptionInstructions.ToList().Where(x => x.CustomerId == record.Id).FirstOrDefault();
                    if (getCertificate != null)
                    {
                        record.CertificateExemptionInstructions = getCertificate;
                        var getBusiness = db.CertificateBusinessTypes.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getBusiness != null)
                        {
                            record.CertificateBusinessTypes = getBusiness;
                        }
                        var getIdentifications = db.CertificateIdentifications.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getIdentifications != null)
                        {
                            record.CertificateIdentifications = getIdentifications;
                        }
                        var getReasons = db.CertificateReasonExemptions.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getReasons != null)
                        {
                            record.CertificateReasonExemptions = getReasons;
                        }
                        var getuser = db.AspNetUsers.ToList().Where(x => x.Email == record.Email).FirstOrDefault();
                        if (getuser != null)
                        {
                            record.AspNetUser = getuser;
                        }
                    }
                    var classificationdata = db.CustomerClassifications.Where(x => x.CustomerInfoId == record.Id && x.CustomerCode == record.CustomerCode).FirstOrDefault();
                    if (classificationdata != null)
                    {
                        record.CustomerClassification = classificationdata;
                    }
                    else
                    {
                        record.CustomerClassification = null;
                    }
                    var billinginfodata = db.CustomerBillingInfos.Where(x => x.CustomerInformationId == record.Id && x.CustomerCode == record.CustomerCode).FirstOrDefault();
                    if (billinginfodata != null)
                    {
                        if (billinginfodata.Pricing != null)
                        {
                            var GetPricing = db.Pricings.ToList().Where(x => x.Name == billinginfodata.Pricing).FirstOrDefault();
                            if (GetPricing != null)
                            {
                                billinginfodata.PricingId = GetPricing.Id;
                            }
                        }
                        record.CustomerBillingInfo = billinginfodata;
                    }
                    else
                    {
                        record.CustomerBillingInfo = null;
                    }

                    var paperworkdata = db.CustomerPaperWorks.Where(x => x.CustomerId == record.Id).FirstOrDefault();
                    if (paperworkdata != null)
                    {
                        record.paperWork = new CustomerPaperWork();
                        record.paperWork = paperworkdata;
                    }
                    else
                    {
                        record.paperWork = new CustomerPaperWork();
                    }

                    if (record.AccountId == null)
                    {
                        Account objacount = null;
                        objacount = new Account();

                        Account customeracc = null;
                        customeracc = new Account();
                        var subaccrecord = db.AccountSubGroups.ToList().Where(x => x.Title == "Customers").FirstOrDefault();
                        if (subaccrecord != null)
                        {
                            var getAccount = db.Accounts.ToList().Where(x => x.AccountSubGroupId == subaccrecord.AccountSubGroupId).LastOrDefault();
                            if (getAccount != null)
                            {
                                var code = getAccount.AccountId.Split("-")[3];
                                int getcode = 0;
                                if (code != null)
                                {

                                    getcode = Convert.ToInt32(code) + 1;
                                }
                                if (getcode > 9)
                                {
                                    objacount.AccountId = subaccrecord.AccountSubGroupId + "-00" + Convert.ToString(getcode);

                                }
                                else if (getcode > 99)
                                {
                                    objacount.AccountId = subaccrecord.AccountSubGroupId + "-0" + Convert.ToString(getcode);
                                }
                                else
                                {
                                    objacount.AccountId = subaccrecord.AccountSubGroupId + "-000" + Convert.ToString(getcode);
                                }
                                objacount.Title = record.FullName;
                                objacount.Status = 1;
                                objacount.AccountSubGroupId = subaccrecord.AccountSubGroupId;
                                customeracc = db.Accounts.Add(objacount).Entity;
                                db.SaveChanges();
                            }
                            else
                            {
                                objacount.AccountId = subaccrecord.AccountSubGroupId + "-0001";
                                objacount.Title = record.FullName;
                                objacount.Status = 1;
                                objacount.AccountSubGroupId = subaccrecord.AccountSubGroupId;
                                customeracc = db.Accounts.Add(objacount).Entity;
                                db.SaveChanges();
                            }
                            if (customeracc != null)
                            {

                                // foundPOrder.StockItemNumber = fullcode;
                                record.AccountId = customeracc.AccountId;
                                record.AccountNumber = customeracc.AccountId;
                                record.AccountTitle = customeracc.Title;
                                db.Entry(record).State = EntityState.Modified;
                                db.SaveChanges();
                            }

                        }
                        else
                        {
                            //ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                        }
                    }
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteCustomerInformation/{id}")]
        public async Task<IActionResult> DeleteCustomerInformation(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                CustomerInformation data = await db.CustomerInformations.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                var isclassificationdata = db.CustomerClassifications.ToList().Exists(x => x.CustomerCode == data.CustomerCode && x.CustomerInfoId == data.Id);
                if (isclassificationdata)
                {
                    var classificationdataid = db.CustomerClassifications.Where(x => x.CustomerInfoId == data.Id && x.CustomerCode == data.CustomerCode).FirstOrDefault().Id;
                    var classificationdata = await db.CustomerClassifications.FindAsync(classificationdataid);
                    if (classificationdata != null)
                    {
                        db.CustomerClassifications.Remove(classificationdata);
                    }
                }
                var isbillinginfodata = db.CustomerBillingInfos.ToList().Exists(x => x.CustomerCode == data.CustomerCode && x.CustomerInformationId == data.Id);
                if (isbillinginfodata)
                {
                    var billinginfodataid = db.CustomerBillingInfos.Where(x => x.CustomerInformationId == data.Id && x.CustomerCode == data.CustomerCode).FirstOrDefault().Id;
                    var billinginfodata = await db.CustomerBillingInfos.FindAsync(billinginfodataid);
                    if (billinginfodata != null)
                    {
                        db.CustomerBillingInfos.Remove(billinginfodata);
                    }
                }
                db.CustomerInformations.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CustomerInformationByCompany/{companyname}")]
        public IActionResult CustomerInformationByCompany(string companyname)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();

                var record = db.CustomerInformations.ToList().Exists(x => x.Company.Equals(companyname, StringComparison.CurrentCultureIgnoreCase));
                if (record)
                {
                    var infodata = db.CustomerInformations.Where(x => x.Company == companyname).FirstOrDefault();
                    bool isValid = db.CustomerClassifications.ToList().Exists(x => x.CustomerCode.Equals(infodata.CustomerCode, StringComparison.CurrentCultureIgnoreCase) && x.CustomerInfoId == infodata.Id);
                    if (isValid)
                    {
                        var classificationdata = db.CustomerClassifications.Where(x => x.CustomerInfoId == infodata.Id).FirstOrDefault();
                        if (classificationdata != null)
                        {
                            infodata.CustomerClassification = classificationdata;
                        }
                        else
                        {
                            infodata.CustomerClassification = null;
                        }
                    }
                    bool isbillinginfodata = db.CustomerBillingInfos.ToList().Exists(x => x.CustomerCode.Equals(infodata.CustomerCode, StringComparison.CurrentCultureIgnoreCase) && x.CustomerInformationId == infodata.Id);
                    if (isbillinginfodata)
                    {
                        var billinginfodata = db.CustomerBillingInfos.Where(x => x.CustomerInformationId == infodata.Id).FirstOrDefault();
                        if (billinginfodata != null)
                        {
                            infodata.CustomerBillingInfo = billinginfodata;
                        }
                        else
                        {
                            infodata.CustomerClassification = null;
                        }
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, infodata);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //CustomerClassification
        [HttpGet("CustomerClassificationGet")]
        public IActionResult CustomerClassificationGet()
        {
            try
            {
                var record = db.CustomerClassifications.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerClassification>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerClassification>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CustomerClassificationCreate")]
        public async Task<IActionResult> CustomerClassificationCreate(CustomerClassification obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerClassification>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                //bool checkgroup = db.CustomerClassifications.ToList().Exists(p => p.GroupName.Equals(obj.GroupName, StringComparison.CurrentCultureIgnoreCase));
                //if (checkgroup)

                //{
                //    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                //    return Ok(Response);
                //}
                var record = db.CustomerClassifications.Add(obj);
                await db.SaveChangesAsync();
                if (record.Entity != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record.Entity);

                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerClassification>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CustomerClassificationUpdate/{id}")]
        public async Task<IActionResult> CustomerClassificationUpdate(int id, CustomerClassification data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerClassification>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isvalid = db.CustomerClassifications.ToList().Exists(x => x.GroupName.Equals(data.GroupName, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isvalid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.CustomerClassifications.FindAsync(id);
                if (data.GroupName != null && data.GroupName != "undefined")
                {
                    record.GroupName = data.GroupName;
                }
                if (data.SubGroupName != null && data.SubGroupName != "undefined")
                {
                    record.SubGroupName = data.SubGroupName;
                }
                if (data.Zone != null && data.Zone != "undefined")
                {
                    record.Zone = data.Zone;
                }
                if (data.Salesman != null && data.Salesman != "undefined")
                {
                    record.Salesman = data.Salesman;
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerClassification>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CustomerClassificationByID/{id}")]
        public IActionResult CustomerClassificationByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerClassification>();

                var record = db.CustomerClassifications.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerClassification>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteCustomerClassification/{id}")]
        public async Task<IActionResult> DeleteCustomerClassification(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<CustomerClassification>();
                CustomerClassification data = await db.CustomerClassifications.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.CustomerClassifications.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerClassification>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //CustomerClassification
        [HttpGet("CustomerBillingInfoGet")]
        public IActionResult CustomerBillingInfoGet()
        {
            try
            {
                var record = db.CustomerBillingInfos.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerBillingInfo>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerBillingInfo>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CustomerBillingInfoCreate")]
        public async Task<IActionResult> CustomerBillingInfoCreate(CustomerBillingInfo obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerBillingInfo>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkbillinginfo = db.CustomerBillingInfos.ToList().Exists(p => p.CustomerCode.Equals(obj.CustomerCode, StringComparison.CurrentCultureIgnoreCase));
                if (checkbillinginfo)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                var record = db.CustomerBillingInfos.Add(obj);
                await db.SaveChangesAsync();
                if (record.Entity != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record.Entity);

                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerBillingInfo>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CustomerBillingInfoUpdate/{id}")]
        public async Task<IActionResult> CustomerBillingInfoUpdate(int id, CustomerBillingInfo data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerBillingInfo>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isvalid = db.CustomerBillingInfos.ToList().Exists(x => x.CustomerCode.Equals(data.CustomerCode, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isvalid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.CustomerBillingInfos.FindAsync(id);
                //if (data.GroupName != null && data.GroupName != "undefined")
                //{
                //    record.GroupName = data.GroupName;
                //}
                //if (data.SubGroupName != null && data.SubGroupName != "undefined")
                //{
                //    record.SubGroupName = data.SubGroupName;
                //}
                //if (data.Zone != null && data.Zone != "undefined")
                //{
                //    record.Zone = data.Zone;
                //}
                //if (data.Salesman != null && data.Salesman != "undefined")
                //{
                //    record.Salesman = data.Salesman;
                //}

                data = record;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerBillingInfo>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CustomerBillingInfoByID/{id}")]
        public IActionResult CustomerBillingInfoByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerBillingInfo>();

                var record = db.CustomerBillingInfos.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerBillingInfo>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteCustomerBillingInfo/{id}")]
        public async Task<IActionResult> DeleteCustomerBillingInfo(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<CustomerBillingInfo>();
                CustomerBillingInfo data = await db.CustomerBillingInfos.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.CustomerBillingInfos.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerBillingInfo>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        //ShiptoReference
        [HttpGet("ShiptoReferenceGet")]
        public IActionResult ShiptoReferenceGet()
        {
            try
            {
                var record = db.ShiptoReferences.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<ShiptoReference>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ShiptoReference>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ShiptoReferenceCreate")]
        public async Task<IActionResult> ShiptoReferenceCreate(ShiptoReference obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ShiptoReference>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.ShiptoReferences.ToList().Exists(p => p.Name.Equals(obj.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.ShiptoReferences.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ShiptoReference>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ShiptoReferenceUpdate/{id}")]
        public async Task<IActionResult> ShiptoReferenceUpdate(int id, ShiptoReference data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ShiptoReference>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.ShiptoReferences.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.ShiptoReferences.FindAsync(id);
                if (data.Name != null && data.Name != "undefined")
                {
                    record.Name = data.Name;
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
                    var Response = ResponseBuilder.BuildWSResponse<ShiptoReference>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ShiptoReferenceByID/{id}")]
        public IActionResult ShiptoReferenceByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ShiptoReference>();

                var record = db.ShiptoReferences.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<ShiptoReference>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteShiptoReference/{id}")]
        public async Task<IActionResult> DeleteShiptoReference(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<ShiptoReference>();
                ShiptoReference data = await db.ShiptoReferences.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.ShiptoReferences.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ShiptoReference>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        ////PaymentTerm
        [HttpGet("PaymentTermGet")]
        public IActionResult PaymentTermGet()
        {
            try
            {
                var record = db.PaymentTerms.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<PaymentTerm>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PaymentTerm>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PaymentTermCreate")]
        public async Task<IActionResult> PaymentTermCreate(PaymentTerm obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<PaymentTerm>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.PaymentTerms.ToList().Exists(p => p.Name.Equals(obj.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.PaymentTerms.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PaymentTerm>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PaymentTermUpdate/{id}")]
        public async Task<IActionResult> PaymentTermUpdate(int id, PaymentTerm data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<PaymentTerm>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.PaymentTerms.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.PaymentTerms.FindAsync(id);
                if (data.Name != null && data.Name != "undefined")
                {
                    record.Name = data.Name;
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
                    var Response = ResponseBuilder.BuildWSResponse<PaymentTerm>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("PaymentTermByID/{id}")]
        public IActionResult PaymentTermByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<PaymentTerm>();

                var record = db.PaymentTerms.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<PaymentTerm>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeletePaymentTerm/{id}")]
        public async Task<IActionResult> DeletePaymentTerm(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<PaymentTerm>();
                PaymentTerm data = await db.PaymentTerms.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.PaymentTerms.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PaymentTerm>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        ////Pricing
        [HttpGet("PricingGet")]
        public IActionResult PricingGet()
        {
            try
            {
                var record = db.Pricings.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Pricing>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PricingCreate")]
        public async Task<IActionResult> PricingCreate(Pricing obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Pricings.ToList().Exists(p => p.Name.Equals(obj.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.Pricings.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PricingUpdate/{id}")]
        public async Task<IActionResult> PricingUpdate(int id, Pricing data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.Pricings.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.Pricings.FindAsync(id);
                if (data.Name != null && data.Name != "undefined")
                {
                    record.Name = data.Name;
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
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("PricingByID/{id}")]
        public IActionResult PricingByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Pricing>();

                var record = db.Pricings.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeletePricing/{id}")]
        public async Task<IActionResult> DeletePricing(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                Pricing data = await db.Pricings.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Pricings.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SaleGetByCustomerID/{id}")]
        public IActionResult GetSaleByCustomerID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                var record = db.PosSales.ToList().Where(x => x.CustomerId == id).ToList();
                if (record != null)
                {
                    if (record.Count > 0)
                    {
                        var query = record.GroupBy(x => x.InvoiceNumber).Select(g => g.FirstOrDefault()).ToList();
                        for (int i = 0; i < record.Count(); i++)
                        {
                            record[i].OrderDate = record[i].InvoiceDate.Value.ToString("dd/MM/yyyy");

                            if (record[i].CashierName == null)
                            {
                                record[i].CashierName = "N/A";
                            }
                            if (record[i].PaidAmount == null)
                            {
                                record[i].PaidAmount = "0";
                            }
                            if (record[i].AmountDue == null)
                            {
                                record[i].AmountDue = "0";
                            }
                        }
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, query);
                        return Ok(Response);
                    }
                    var Responsee = ResponseBuilder.BuildWSResponse<PosSale>();
                    ResponseBuilder.SetWSResponse(Responsee, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Responsee);

                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PosSale>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPaymentsByCustomerID/{id}")]
        public IActionResult GetPaymentsByCustomerID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                var record = db.PosSales.ToList().Where(x => x.CustomerId == id).ToList();
                if (record != null)
                {
                    if (record.Count > 0)
                    {
                        var query = record.GroupBy(x => x.InvoiceNumber).Select(g => g.FirstOrDefault()).ToList();
                        for (int i = 0; i < record.Count(); i++)
                        {
                            record[i].OrderDate = record[i].InvoiceDate.Value.ToString("dd/MM/yyyy");
                            record[i].OrderTime = record[i].InvoiceDate.Value.ToString("HH:mm");
                            //record[i].paym
                            if (record[i].CashierName == null)
                            {
                                record[i].CashierName = "N/A";
                            }
                            if (record[i].PaidAmount == null)
                            {
                                record[i].PaidAmount = "0";
                            }
                            if (record[i].AmountDue == null)
                            {
                                record[i].AmountDue = "0";
                            }
                            if (record[i].SaleManName == null)
                            {
                                record[i].SaleManName = "N/A";
                            }
                            if (record[i].PaymentTerms == null)
                            {
                                record[i].PaymentTerms = "N/A";
                            }
                        }
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, query);
                        return Ok(Response);
                    }
                    var Responsee = ResponseBuilder.BuildWSResponse<PosSale>();
                    ResponseBuilder.SetWSResponse(Responsee, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Responsee);

                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PosSale>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        //ZipCode
        [HttpGet("ZipCodeGet")]
        public IActionResult ZipCodeGet()
        {
            try
            {
                var record = db.ZipCodes.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<ZipCode>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ZipCode>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ZipCodeCreate")]
        public async Task<IActionResult> ZipCodeCreate(ZipCode obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ZipCode>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.ZipCodes.ToList().Exists(p => p.Code.Equals(obj.Code, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.ZipCodes.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ZipCode>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ZipCodeUpdate/{id}")]
        public async Task<IActionResult> ZipCodeUpdate(int id, ZipCode data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ZipCode>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.ZipCodes.ToList().Exists(x => x.Code.Equals(data.Code, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.ZipCodes.FindAsync(id);
                if (data.Code != null && data.Code != "undefined")
                {
                    record.Code = data.Code;
                }
                if (data.City != null && data.City != "undefined")
                {
                    record.City = data.City;
                }
                if (data.State != null && data.State != "undefined")
                {
                    record.State = data.State;
                }
                if (data.StateShortcut != null && data.StateShortcut != "undefined")
                {
                    record.StateShortcut = data.StateShortcut;
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
                    var Response = ResponseBuilder.BuildWSResponse<ZipCode>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ZipCodeByID/{id}")]
        public IActionResult ZipCodeByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ZipCode>();

                var record = db.ZipCodes.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<ZipCode>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteZipCode/{id}")]
        public async Task<IActionResult> DeleteZipCode(int id)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<ZipCode>();
                ZipCode data = await db.ZipCodes.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.ZipCodes.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ZipCode>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("POSSaleGetByID/{customerid}/{currentInvoice}")]
        public IActionResult POSSaleGetByID(int customerid, string currentInvoice)
        {
            try

            {
                //if (invoicenumber.Contains("-"))
                //{
                //    invoicenumber = "00" + invoicenumber;
                //}
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();

                var record = db.PosSales.ToList().Where(x => x.CustomerId == customerid && x.InvoiceNumber == currentInvoice).ToList();
                if (record != null && record.Count > 0)
                {

                    for (int i = 0; i < record.Count(); i++)
                    {
                        record[i].OrderDate = record[i].InvoiceDate.Value.ToString("dd/MM/yyyy");
                    }
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
                    var Response = ResponseBuilder.BuildWSResponse<PosSale>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }




        [HttpGet("BillToBillCustomersGet")]
        public IActionResult BillToBillCustomersGet()
        {
            try
            {
                List<CustomerInformation> newlist = new List<CustomerInformation>();
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerInformation>>();
                var record = db.CustomerInformations.ToList();
                var recordBillingInfo = db.CustomerBillingInfos.ToList();
                for (int i = 0; i < record.Count(); i++)
                {
                    foreach (var item in recordBillingInfo.ToList().Where(x => x.CustomerInformationId == record[i].Id).ToList())
                    {
                        record[i].CustomerBillingInfo = item;
                    }
                    if (record[i].CustomerBillingInfo != null)
                    {
                        if (record[i].CustomerBillingInfo.IsBillToBill == true)
                        {
                            newlist.Add(record[i]);
                        }
                    }
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, newlist);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("DeliverOrderById")]
        public IActionResult DeliverOrderById(int userid, string ordernum)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CartDetail>>();
                var record = db.CustomerOrders.Where(x => x.TicketId == ordernum && x.AdminStatus == true && x.IsPulled == true && x.IsInvoiced == true && x.Delivered == false && x.IsRejected == false).ToList();
                if (record.Count() > 0)
                {
                    var CurrentUser = db.AspNetUsers.Find(userid);

                    for (int i = 0; i < record.Count(); i++)
                    {
                        if (CurrentUser != null)
                        {
                            if (CurrentUser.Firstname != null && CurrentUser.Lastname != null)
                            {
                                record[i].DeliveredBy = CurrentUser.Firstname + " " + CurrentUser.Lastname;
                            }
                            else
                            {
                                if (CurrentUser.UserName != null)
                                {
                                    record[i].DeliveredBy = CurrentUser.UserName;
                                }
                            }
                        }
                        record[i].Delivered = true;
                        record[i].DeliveredDate = DateTime.Now;
                        record[i].DeliveredTime = DateTime.Now.TimeOfDay.ToString();
                        db.Entry(record[i]).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    return Ok(Response);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PullOrderById")]
        public IActionResult PullOrderById(int userid, string ordernum)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CartDetail>>();
                var record = db.CustomerOrders.Where(x => x.TicketId == ordernum && x.AdminStatus == true && x.IsPulled == true && x.IsInvoiced == false && x.Delivered == false && x.IsRejected == false).ToList();
                //List<CartDetail> cartDetail = new List<CartDetail>();
                //var isNotDeliver = false;
                //if (record != null)
                //{
                //    //var cart = db.CartDetails.Where(x => x.UserId == record.UserId && x.PendingForApproval == true && x.IsDelivered == false).ToList();
                //    var cart = db.CartDetails.Where(x => x.TicketId == record.TicketId && x.PendingForApproval == true && x.IsDelivered == false).ToList();
                //    //foreach (var item in cart)
                //    //{
                //    //    var stock = db.InventoryStocks.Where(x => x.ProductId == item.Id)?.FirstOrDefault();
                //    //    if (stock != null)
                //    //    {
                //    //        if (item.Quantity <= int.Parse(stock.Quantity))
                //    //        {
                //    //            //stock.Quantity = (int.Parse(stock.Quantity) - item.Quantity).ToString();
                //    //            //record.Delivered = true;
                //    //            //item.IsDelivered = true;
                //    //            //db.SaveChanges();
                //    //            isNotDeliver = false;
                //    //        }
                //    //        else
                //    //        {
                //    //            isNotDeliver = true;
                //    //            //cartDetail.Add(item);
                //    //        }
                //    //    }

                //    //}
                //    if (isNotDeliver)
                //    {
                //        record.Delivered = false;
                //        db.SaveChanges();
                //        ResponseBuilder.SetWSResponse(Response, StatusCodes.Exceed_Credit_Limit, null, cartDetail);
                //        return Ok(Response);
                //    }
                //    else
                //    {
                //        foreach (var item in cart)
                //        {
                //            var stock = db.InventoryStocks.Where(x => x.ProductId == item.Id)?.FirstOrDefault();
                //            stock.Quantity = (int.Parse(stock.Quantity) - item.Quantity).ToString();
                //            record.Delivered = true;
                //            item.IsDelivered = true;
                //            db.SaveChanges();
                //        }
                //    }

                if (record.Count() > 0)
                {
                    var CurrentUser = db.AspNetUsers.Find(userid);

                    for (int i = 0; i < record.Count(); i++)
                    {
                        if (CurrentUser != null)
                        {
                            if (CurrentUser.Firstname != null && CurrentUser.Lastname != null)
                            {
                                record[i].PulledBy = CurrentUser.Firstname + " " + CurrentUser.Lastname;
                            }
                            else
                            {
                                if (CurrentUser.UserName != null)
                                {
                                    record[i].PulledBy = CurrentUser.UserName;
                                }
                            }
                        }
                        record[i].IsPulled = true;
                        record[i].PulledDate = DateTime.Now;
                        record[i].PulledTime = DateTime.Now.TimeOfDay.ToString();
                        db.Entry(record[i]).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    return Ok(Response);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AllOrderGet")]
        public IActionResult AllOrderGet()
        {
            try
            {
                var list = db.CustomerOrders.ToList();
                var Record = db.CartDetails.ToList();
                for (int i = 0; i < list.Count(); i++)
                {
                    var Counttickets = Record.ToList().Where(x => x.TicketId == list[i].TicketId).Count();
                    list[i].LineCounts = Counttickets;
                    if (CheckDays(Convert.ToDateTime(list[i].AdminActionDate)) == false)
                    {
                        list[i].OrderDaysAlert = true;
                    }
                }
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerOrder>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, list);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("AllCashierOrderGet/{userid}")]
        public IActionResult AllCashierOrderGet(string userid)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerOrder>>();
                var GetEmployee = db.AspNetUsers.Find(Convert.ToInt32(userid));
                if (GetEmployee != null)
                {
                    var list = db.CustomerOrders.ToList().Where(x => x.InvoiceEmployeeId == Convert.ToInt32(GetEmployee.EmployeeId)).ToList();
                    var Record = db.CartDetails.ToList();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        var Counttickets = Record.ToList().Where(x => x.TicketId == list[i].TicketId).Count();
                        list[i].LineCounts = Counttickets;
                        if (CheckDays(Convert.ToDateTime(list[i].AdminActionDate)) == false)
                        {
                            list[i].OrderDaysAlert = true;
                        }
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, list);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AllCashierOrderGet")]
        public IActionResult AllCashierOrderGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerOrder>>();
                //var GetEmployee = db.AspNetUsers.Find(Convert.ToInt32(userid));

                var list = db.CustomerOrders.ToList();
                var Record = db.CartDetails.ToList();
                for (int i = 0; i < list.Count(); i++)
                {
                    var Counttickets = Record.ToList().Where(x => x.TicketId == list[i].TicketId).Count();
                    list[i].LineCounts = Counttickets;
                    if (CheckDays(Convert.ToDateTime(list[i].AdminActionDate)) == false)
                    {
                        list[i].OrderDaysAlert = true;
                    }
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, list);

                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserCartByAdminApproval/{ticketId}")]
        public IActionResult GetUserCartByAdminApproval(string ticketId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CartDetail>>();
                // var record = db.CartDetails.Where(x => x.UserId == id && x.PendingForApproval == true && x.IsDelivered == false).ToList();
                var record = db.CartDetails.Where(x => x.TicketId == ticketId).ToList();
                var customerorder = db.CustomerOrders.Where(x => x.TicketId == ticketId).ToList();
                foreach (var item in record)
                {
                    foreach (var item2 in customerorder)
                    {
                        if (item.Name == item2.ProductName)
                        {
                            var productinformation = db.Products.Where(x => x.ProductCode == item.ProductCode).FirstOrDefault();
                            item.ShipmentLimit = productinformation.ShipmentLimit;
                            item.ChangeQuantity = item2.ChangeQuantity;
                        }
                    }
                }
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
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        public bool CheckDays(DateTime actionDate)
        {

            try
            {
                var CurrentDate = DateTime.Now;
                var FoundedDays = (CurrentDate - actionDate.Date).Days;
                if (FoundedDays >= 2)
                {
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpGet("GetcustomerOrders/{ticketId}")]
        public IActionResult GetcustomerOrders(string ticketId)
        {
            try
            {


                var Response = ResponseBuilder.BuildWSResponse<List<CustomerOrder>>();
                var orderlist = db.CustomerOrders.Where(x => x.TicketId == ticketId).ToList();
                if (orderlist.Count != 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, orderlist);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }




        [HttpGet("ApproveExceedRequest/{ticketid}")]
        public async Task<IActionResult> ApproveExceedRequest(string ticketid)
        {
            try
            {


                var Response = ResponseBuilder.BuildWSResponse<List<CustomerOrder>>();
                var orderlist = db.CustomerOrders.Where(x => x.TicketId == ticketid && x.NeedApproval == true && x.AcceptApproval == false).ToList();
                if (orderlist.Count != 0)
                {
                    foreach (var item in orderlist)
                    {
                        item.AcceptApproval = true;
                        db.Entry(item).State = EntityState.Modified;
                    }

                    await db.SaveChangesAsync();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        //[HttpGet("CustomerTransactionByID/{id}")]
        //public async Task<IActionResult> CustomerTransactionByID(int id)
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<List<Transaction>>();
        //        var record = db.CustomerInformations.ToList().Where(x => x.Id == id).FirstOrDefault();
        //        if (record != null)
        //        {

        //            var CustomerAccount = db.Accounts.ToList().Where(x => x.AccountId == record.AccountId).FirstOrDefault();
        //            if (CustomerAccount != null)
        //            {
        //                var CustomerTrans = db.Transactions.ToList().Where(x => x.AccountNumber == CustomerAccount.AccountId).ToList();
        //                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, CustomerTrans);
        //                return Ok(Response);
        //            }


        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
        //            return Ok(Response);

        //        }
        //        return Ok(Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpGet("CustomerStatisticsByID/{id}")]
        //public async Task<IActionResult> CustomerStatisticsByID(int id)
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
        //        var record = db.AspNetUsers.ToList().Where(x => x.Id == id).FirstOrDefault();
        //        if (record != null)
        //        {
        //            //record.lastsale = db.CustomerOrders.ToList().Where(x => x.UserId == record.Id).LastOrDefault().DeliveredDate.ToString();

        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
        //            return Ok(Response);
        //        }
        //        else
        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
        //        }

        //        return Ok(Response);

        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpGet("GetCustomerAttachmentsByID/{id}")]
        //public async Task<IActionResult> GetCustomerAttachmentsByID(int id)
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<CustomerPaperWork>();
        //        var record = db.CustomerPaperWorks.ToList().Where(x => x.CustomerId == id).FirstOrDefault();
        //        if (record != null)
        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
        //            return Ok(Response);
        //        }
        //        else
        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
        //        }

        //        return Ok(Response);

        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet("CustomerTransactionByID/{id}")]
        public async Task<IActionResult> CustomerTransactionByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Transaction>>();
                var record = db.CustomerInformations.ToList().Where(x => x.Id == id).FirstOrDefault();
                if (record != null)
                {

                    var CustomerAccount = db.Accounts.ToList().Where(x => x.AccountId == record.AccountId).FirstOrDefault();
                    if (CustomerAccount != null)
                    {
                        var CustomerTrans = db.Transactions.ToList().Where(x => x.AccountNumber == CustomerAccount.AccountId).ToList();
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, CustomerTrans);
                        return Ok(Response);
                    }


                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);

                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CustomerStatisticsByID/{id}")]
        public async Task<IActionResult> CustomerStatisticsByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
                var record = db.AspNetUsers.ToList().Where(x => x.Id == id).FirstOrDefault();
                if (record != null)
                {
                    var CheckLastSale = db.CustomerOrders.ToList().Where(x => x.UserId == record.Id).LastOrDefault();
                    if (CheckLastSale != null)
                    {
                        record.lastsale = CheckLastSale.DeliveredDate.ToString();

                    }

                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCustomerAttachmentsByID/{id}")]
        public async Task<IActionResult> GetCustomerAttachmentsByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerPaperWork>();
                var record = db.CustomerPaperWorks.ToList().Where(x => x.CustomerId == id).FirstOrDefault();
                if (record != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("SupplierDocumentTypeCreate")]
        public async Task<IActionResult> SupplierDocumentTypeCreate(SupplierDocumentType document)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SupplierDocumentType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.SupplierDocumentTypes.ToList().Exists(p => p.DocumentType.Equals(document.DocumentType, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (document.DocumentType != null)
                {
                    db.SupplierDocumentTypes.Add(document);
                    await db.SaveChangesAsync();
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SupplierDocumentType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CustomerDocumentCreate")]
        public IActionResult CustomerDocumentCreate(CustomerDocument document)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerDocument>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.CustomerDocuments.ToList().Exists(p => p.DocumentName.Equals(document.DocumentName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }


                if (document.Image != null)
                {
                    var imgPath = SaveDocument(document.Image, Guid.NewGuid().ToString());
                    document.ImageByPath = imgPath;
                    document.Image = null;

                }

                if (document.DocumentTypeId != null)
                {
                    var foundtype = db.SupplierDocumentTypes.Find(Convert.ToInt32(document.DocumentTypeId));
                    if (foundtype != null)
                    {
                        document.DocumentType = foundtype.DocumentType;
                    }
                }
                db.CustomerDocuments.Add(document);
                db.SaveChanges();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CustomerDocumentGetByCustomerID/{id}")]
        public IActionResult CustomerDocumentGetByCustomerID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerDocument>>();

                var record = db.CustomerDocuments.ToList().Where(x => x.UserId == id && x.DocumentType != "Customer Notes").ToList();
                if (record.Count() > 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record.ToList());

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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CustomerDocumentUpdate/{id}")]
        public async Task<IActionResult> CustomerDocumentUpdate(int id, CustomerDocument data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerDocument>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != data.DocumentId)
                {
                    return BadRequest();
                }
                bool isValid = db.CustomerDocuments.ToList().Exists(x => x.DocumentName.Equals(data.DocumentName, StringComparison.CurrentCultureIgnoreCase) && x.DocumentId != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);

                }

                else
                {
                    var record = await db.CustomerDocuments.FindAsync(id);
                    if (data.DocumentType != null && data.DocumentType != "undefined")
                    {
                        record.DocumentType = data.DocumentType;
                    }
                    if (data.DocumentName != null && data.DocumentName != "undefined")
                    {
                        record.DocumentName = data.DocumentName;
                    }
                    if (data.ImageByPath != null && data.ImageByPath != "undefined")
                    {
                        record.ImageByPath = data.ImageByPath;
                    }

                    data = record;
                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Ok(Response);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SupplierDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("CustomerDocumentDelete/{id}")]
        public async Task<IActionResult> CustomerDocumentDelete(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerDocument>();
                CustomerDocument data = await db.CustomerDocuments.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.CustomerDocuments.Remove(data);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SaveDocument/{str,ImgName}")]
        public string SaveDocument(byte[] str, string ImgName)
        {
            string hostRootPath = _webHostEnvironment.WebRootPath;
            string webRootPath = _webHostEnvironment.ContentRootPath;
            string imgPath = string.Empty;

            if (!string.IsNullOrEmpty(webRootPath))
            {
                string path = webRootPath + "\\images/documents\\";
                string imageName = ImgName + ".pdf";
                imgPath = Path.Combine(path, imageName);
                //imgPath = path + imageName;
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                //imgPath = "https://localhost:44371/images/documents/" + imageName;
                imgPath = "http://38.17.51.207:8048/images/documents/" + imageName;
               // imgPath = "https://localhost:5001/images/documents/" + imageName;
                return imgPath;
            }
            else if (!string.IsNullOrEmpty(hostRootPath))
            {
                string path = hostRootPath + "\\images/documents\\";
                string imageName = ImgName + ".pdf";
                imgPath = Path.Combine(path, imageName);
                //imgPath = path + imageName;
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
               //imgPath = "https://localhost:44371/images/documents/" + imageName;
               imgPath = "http://38.17.51.207:8048/images/documents/" + imageName;
               // imgPath = "https://localhost:5001/images/documents/" + imageName;
                return imgPath;
            }
            imgPath = imgPath.Replace(" ", "");
            return imgPath;
        }

        [HttpGet("SupplierDocumentTypeGet")]
        public IActionResult SupplierDocumentTypeGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<SupplierDocumentType>>();
                var record = db.SupplierDocumentTypes.ToList();
                //   return Ok(record);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities, See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SupplierDocumentType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CustomerNoteById/{CustomerId}")]
        public IActionResult CustomerNoteById(string CustomerId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerNotesAdp>>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var response = db.CustomerNotes.Where(x => x.CustomerId == Convert.ToInt32(CustomerId)).Select(x => new CustomerNotesAdp {
                    NoteId = x.NoteId,
                    NoteDate = x.NoteDate,
                    CustomerNote = x.CustomerNote1
                }).ToList();

                ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, response);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<List<CustomerNotesAdp>>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNotesByID/{noteId}")]
        public IActionResult GetNotesByID(int noteId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerNote>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var note = db.CustomerNotes.FirstOrDefault(x => x.NoteId == noteId);

                if (note != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, note);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerNote>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CustomerNoteCreate")]
        public IActionResult CustomerNoteCreate(CustomerNote data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerNote>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                data.NoteDate = DateTime.Now;
                db.CustomerNotes.Add(data);
                db.SaveChanges();

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, data);
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerNote>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCustomerNotesByID/{id}")]
        public IActionResult GetCustomerNotesByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                var note = db.CustomerInformations.ToList().Where(x => x.Id == id).FirstOrDefault();
                if (note != null)
                {
                    var checknote = db.CustomerNotes.ToList().Where(x => x.CustomerId == Convert.ToInt32(note.CustomerCode)).FirstOrDefault();
                    if(checknote != null)
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, note);
                    }

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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerNote>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CustomerNotesUpdate/{id}")]
        public async Task<IActionResult> CustomerNotesUpdate(int id, CustomerNote data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerNote>();
                var note = db.CustomerNotes.ToList().Where(x => x.CustomerId == id).FirstOrDefault();
                if (note != null)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if(data.CustomerNote1 != null)
                    {
                        note.CustomerNote1 = data.CustomerNote1;
                        db.Entry(note).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, note);

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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerNote>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CustomerNotesMultipleDocumentCreate")]
        public IActionResult CustomerNotesMultipleDocumentCreate(CustomerDocument document)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerDocument>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.CustomerDocuments.ToList().Exists(p => p.DocumentName != null && p.DocumentName.Equals(document?.DocumentName, StringComparison.CurrentCultureIgnoreCase));

                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }


                if (document.Image != null)
                {
                    var imgPath = SaveDocument(document.Image, Guid.NewGuid().ToString());
                    document.ImageByPath = imgPath;
                    document.Image = null;

                }

                if (document.DocumentType == null)
                {

                    document.DocumentType = "Customer Notes";

                }
                document.UploadDate = DateTime.Now;
                db.CustomerDocuments.Add(document);
                db.SaveChanges();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("CustomerNotesDocumentCreate")]
        public IActionResult CustomerNotesDocumentCreate(CustomerDocument document)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerDocument>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.CustomerDocuments.ToList().Exists(p => p.DocumentName.Equals(document.DocumentName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }


                if (document.Image != null)
                {
                    var imgPath = SaveDocument(document.Image, Guid.NewGuid().ToString());
                    document.ImageByPath = imgPath;
                    document.Image = null;

                }

                if (document.DocumentType == null)
                {

                    document.DocumentType = "Customer Notes"; 
                    
                }
                document.UploadDate = DateTime.Now;
                db.CustomerDocuments.Add(document);
                db.SaveChanges();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetNotesAttachmentByCustomerId/{id}")]
        public IActionResult GetNotesAttachmentByCustomerId(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerDocument>>();

                var record = db.CustomerDocuments.ToList().Where(x => x.UserId == id && x.DocumentType == "Customer Notes").ToList();
                if (record.Count() > 0)
                {
                   
                        record[0].UserName = db.CustomerInformations.ToList().Where(x => x.Id == id).FirstOrDefault().FirstName;
                    
                    
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record.ToList());

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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("CustomerNotestDelete/{id}")]
        public async Task<IActionResult> CustomerNotestDelete(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerDocument>();
                var data = db.CustomerDocuments.ToList().Where(x => x.DocumentId == id).FirstOrDefault();
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.CustomerDocuments.Remove(data);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetEmployeeInfoo/{id}")]
        public IActionResult GetCustomerinfoo(int id)
        {
            try
            {
                var record = db.Employees.ToList().Where(x => x.EmployeeId == id).FirstOrDefault();
                var Response = ResponseBuilder.BuildWSResponse<Employee>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employee>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAllCustomersCounter")]
        public IActionResult GetAllCustomersCounter()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<int>();

                int counter = db.CustomerInformations.ToList().Count();
                if (counter != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, counter);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, counter);
                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<int>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, 0);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllVendorsCounter")]
        public IActionResult GetAllVendorsCounter()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<int>();

                int counter = db.Vendors.ToList().Count();
                if (counter != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, counter);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, counter);
                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<int>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, 0);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLastActivityLog/{id}")]
        public IActionResult GetLastActivityLog(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ActivityLog>();
                var record = db.ActivityLogs.ToList().Where(x => x.UserId == id).LastOrDefault();
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CustomerInformationByUserID/{UserId}")]
        public IActionResult CustomerInformationByUserID(int UserId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                var record = db.CustomerInformations.ToList().Where(x => x.UserId == UserId).FirstOrDefault();
                if (record != null)
                {
                    var getnote = db.CustomerNotes.ToList().Where(x => x.CustomerId == record.Id).FirstOrDefault();
                    if (getnote != null)
                    {
                        record.CustomerNote = getnote.CustomerNote1;
                        record.NoteDate = Convert.ToString(getnote.NoteDate);
                    }

                    var getpay = db.Receivables.ToList().Where(x => x.AccountId == record.AccountId).FirstOrDefault();
                    if (getpay != null)
                    {
                        if (getpay.Amount == null)
                        {
                            getpay.Amount = "0.00";
                        }
                        else
                        {
                            var amount = Convert.ToDouble(getpay.Amount);
                            record.Balance = Math.Round(amount, 2).ToString();
                        }

                    }
                    else
                    {
                        record.Balance = "0.00";
                    }
                    var getCertificate = db.CertificateExemptionInstructions.ToList().Where(x => x.CustomerId == record.Id).FirstOrDefault();
                    if (getCertificate != null)
                    {
                        record.CertificateExemptionInstructions = getCertificate;
                        var getBusiness = db.CertificateBusinessTypes.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getBusiness != null)
                        {
                            record.CertificateBusinessTypes = getBusiness;
                        }
                        var getIdentifications = db.CertificateIdentifications.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getIdentifications != null)
                        {
                            record.CertificateIdentifications = getIdentifications;
                        }
                        var getReasons = db.CertificateReasonExemptions.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getReasons != null)
                        {
                            record.CertificateReasonExemptions = getReasons;
                        }
                        var getuser = db.AspNetUsers.ToList().Where(x => x.Email == record.Email).FirstOrDefault();
                        if (getuser != null)
                        {
                            record.AspNetUser = getuser;
                        }
                    }
                    var classificationdata = db.CustomerClassifications.Where(x => x.CustomerInfoId == record.Id && x.CustomerCode == record.CustomerCode).FirstOrDefault();
                    if (classificationdata != null)
                    {
                        record.CustomerClassification = classificationdata;
                    }
                    else
                    {
                        record.CustomerClassification = null;
                    }
                    var billinginfodata = db.CustomerBillingInfos.Where(x => x.CustomerInformationId == record.Id && x.CustomerCode == record.CustomerCode).FirstOrDefault();
                    if (billinginfodata != null)
                    {
                        record.CustomerBillingInfo = billinginfodata;
                    }
                    else
                    {
                        record.CustomerBillingInfo = null;
                    }

                    var paperworkdata = db.CustomerPaperWorks.Where(x => x.CustomerId == record.Id).FirstOrDefault();
                    if (paperworkdata != null)
                    {
                        record.paperWork = new CustomerPaperWork();
                        record.paperWork = paperworkdata;
                    }
                    else
                    {
                        record.paperWork = new CustomerPaperWork();
                    }
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
    }
}
