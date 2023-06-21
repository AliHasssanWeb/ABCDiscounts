using ABC.Customer.API.ViewModel;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared;
using ABC.Shared.DataConfig;
using ABC.Shared.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using StatusCodes = ABC.Shared.DataConfig.StatusCodes;

namespace ABC.Customer.API.Controllers
{

    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        private readonly IMailService mailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private EmailService emailService = new EmailService();
        public CustomerController(ABCDiscountsContext _db, IMailService mailService, IWebHostEnvironment webHostEnvironment)
        {
            db = _db;
            this.mailService = mailService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("CustomersinfoGet")]
        public IActionResult CustomersinfoGet()
        {
            try
            {
                var record = db.Customers.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<ABC.EFCore.Repository.Edmx.Customer>>();
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
        public async Task<IActionResult> ProviderCreate(Provider obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Provider>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Providers.ToList().Exists(p => p.Name.Equals(obj1.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.Providers.Add(obj1);
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
        public async Task<IActionResult> CustomerTypeCreate(CustomerType obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.CustomerTypes.ToList().Exists(p => p.TypeName.Equals(obj1.TypeName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.CustomerTypes.Add(obj1);
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
        public async Task<IActionResult> SubGroupCreate(SubGroup obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SubGroup>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.SubGroups.ToList().Exists(p => p.SubGroupName.Equals(obj1.SubGroupName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                if (obj1.GroupId != null)
                {
                    var foungroup = db.Groups.ToList().Where(x => x.Id == obj1.GroupId).FirstOrDefault();
                    if (foungroup != null)
                    {
                        obj1.ParentGroupName = foungroup.Name;
                    }
                }
                else
                {
                    obj1.ParentGroupName = null;
                }
                db.SubGroups.Add(obj1);
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
        public async Task<IActionResult> BusinessTypeCreate(BusinessType obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<BusinessType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.BusinessTypes.ToList().Exists(p => p.TypeName.Equals(obj1.TypeName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.BusinessTypes.Add(obj1);
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
        public async Task<IActionResult> BusinessTypeCreate(Salesman obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Salesman>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Salesmen.ToList().Exists(p => p.Name.Equals(obj1.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.Salesmen.Add(obj1);
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
        public async Task<IActionResult> ZoneCreate(Zone obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Zone>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Zones.ToList().Exists(p => p.Name.Equals(obj1.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.Zones.Add(obj1);
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
        public async Task<IActionResult> DriverCreate(Driver obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Driver>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Drivers.ToList().Exists(p => p.Name.Equals(obj1.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.Drivers.Add(obj1);
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
        public async Task<IActionResult> RouteCreate(Route obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Route>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Routes.ToList().Exists(p => p.RouteName.Equals(obj1.RouteName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.Routes.Add(obj1);
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
        public async Task<IActionResult> CustomerStateCreate(CustomerState obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerState>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.CustomerStates.ToList().Exists(p => p.StateName.Equals(obj1.StateName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.CustomerStates.Add(obj1);
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
        public async Task<IActionResult> ShipmentPurchaseCreate(ShipmentPurchase obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ShipmentPurchase>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.ShipmentPurchases.ToList().Exists(p => p.Type.Equals(obj1.Type, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.ShipmentPurchases.Add(obj1);
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
        public async Task<IActionResult> DrivingLicenseStateCreate(DrivingLicenseState obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<DrivingLicenseState>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.DrivingLicenseStates.ToList().Exists(p => p.Name.Equals(obj1.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.DrivingLicenseStates.Add(obj1);
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
                        var billinginforecord = db.CustomerBillingInfos.ToList().Where(x => x.CustomerInformationId == customerinformation.Id).FirstOrDefault();
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


        //public async Task<IActionResult> CustomerInformationCreate(RegisterCustomer obj1)
        //{
        //    try
        //    {
        //        RegisterCustomer obj = new RegisterCustomer();
        //        var Response = ResponseBuilder.BuildWSResponse<RegisterCustomer>();
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest();
        //        }
        //        RegisterCustomer customerInformation = new RegisterCustomer();

        //        var respcustomer = db.RegisterCustomer.ToList();
        //        if (respcustomer.Count() > 0)
        //        {
        //            var customeroderecord = respcustomer;
        //            if (customeroderecord != null && customeroderecord.Count() > 0)
        //            {
        //                CustomerInformation newcustomer = new CustomerInformation();
        //                var fullcode = "";
        //                if (customeroderecord[0].customerInformation.CustomerCode != null && customeroderecord[0].customerInformation.CustomerCode != "string" && customeroderecord[0].customerInformation.CustomerCode != "")
        //                {
        //                    int large, small;
        //                    int CustomerInfoID = 0;
        //                    large = Convert.ToInt32(customeroderecord[0].customerInformation.CustomerCode.Split('-')[1]);
        //                    small = Convert.ToInt32(customeroderecord[0].customerInformation.CustomerCode.Split('-')[1]);
        //                    for (int i = 0; i < customeroderecord.Count(); i++)
        //                    {
        //                        if (customeroderecord[i].customerInformation.CustomerCode != null)
        //                        {
        //                            var t = Convert.ToInt32(customeroderecord[i].customerInformation.CustomerCode.Split('-')[1]);
        //                            if (Convert.ToInt32(customeroderecord[i].customerInformation.CustomerCode.Split('-')[1]) > large)
        //                            {
        //                                CustomerInfoID = Convert.ToInt32(customeroderecord[i].customerInformation.Id);
        //                                large = Convert.ToInt32(customeroderecord[i].customerInformation.CustomerCode.Split('-')[1]);

        //                            }
        //                            else if (Convert.ToInt32(customeroderecord[i].customerInformation.CustomerCode.Split('-')[1]) < small)
        //                            {
        //                                small = Convert.ToInt32(customeroderecord[i].customerInformation.CustomerCode.Split('-')[1]);
        //                            }
        //                            else
        //                            {
        //                                if (large < 2)
        //                                {
        //                                    CustomerInfoID = Convert.ToInt32(customeroderecord[i].customerInformation.Id);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    var newcustomerr = customeroderecord.ToList().Where(x => x.customerInformation.Id == CustomerInfoID).FirstOrDefault();
        //                    if (newcustomerr != null)
        //                    {
        //                        if (newcustomerr.customerInformation.CustomerCode != null)
        //                        {
        //                            var VcodeSplit = newcustomerr.customerInformation.CustomerCode.Split('-');
        //                            int code = Convert.ToInt32(VcodeSplit[1]) + 1;
        //                            fullcode = "00-" + Convert.ToString(code);
        //                        }
        //                        else
        //                        {
        //                            fullcode = "00-" + "1";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        fullcode = "00-" + "1";
        //                    }
        //                }
        //                else
        //                {
        //                    fullcode = "00-" + "1";
        //                }

        //                customerInformation.customerInformation.CustomerCode = fullcode;
        //                obj1.customerInformation.CustomerCode = customerInformation.customerInformation.CustomerCode;
        //            }
        //            else
        //            {
        //                customerInformation.customerInformation.CustomerCode = "00-" + "1";

        //                obj1.customerInformation.CustomerCode = customerInformation.customerInformation.CustomerCode;
        //            }
        //        }
        //        else
        //        {
        //            customerInformation.customerInformation.CustomerCode = "00-" + "1";
        //            obj1.customerInformation.CustomerCode = customerInformation.customerInformation.CustomerCode;
        //        }
        //        bool checkcustomercode = db.CustomerInformations.ToList().Exists(x => x.CustomerCode.Equals(obj1.customerInformation.CustomerCode, StringComparison.CurrentCultureIgnoreCase) /*&& x.Company == obj1.Company*/);
        //        if (checkcustomercode)
        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
        //            return Ok(Response);
        //        }
        //        if (obj1 != null)
        //        {
        //            customerInformation.customerInformation.Balance = obj1.customerInformation.Balance;
        //            customerInformation.customerInformation.Company = obj1.customerInformation.Company;
        //            customerInformation.customerInformation.Gender = obj1.customerInformation.Gender;
        //            customerInformation.customerInformation.FirstName = obj1.customerInformation.FirstName;
        //            customerInformation.customerInformation.LastName = obj1.customerInformation.LastName;
        //            customerInformation.customerInformation.Street = obj1.customerInformation.Street;
        //            customerInformation.customerInformation.City = obj1.customerInformation.City;
        //            customerInformation.customerInformation.StateId = obj1.customerInformation.StateId;
        //            customerInformation.customerInformation.Country = obj1.customerInformation.Country;
        //            customerInformation.customerInformation.Phone = obj1.customerInformation.Phone;
        //            customerInformation.customerInformation.Fax = obj1.customerInformation.Fax;
        //            customerInformation.customerInformation.CheckAddress = obj1.customerInformation.CheckAddress;
        //            customerInformation.customerInformation.Email = obj1.customerInformation.Email;
        //            customerInformation.customerInformation.Cell = obj1.customerInformation.Cell;
        //            customerInformation.customerInformation.ProviderId = obj1.customerInformation.ProviderId;
        //            customerInformation.customerInformation.Other = obj1.customerInformation.Other;
        //            customerInformation.customerInformation.Website = obj1.customerInformation.Website;
        //            customerInformation.customerInformation.TobaccoLicenseNumber = obj1.customerInformation.TobaccoLicenseNumber;
        //            customerInformation.customerInformation.CigaretteLicenseNumber = obj1.customerInformation.CigaretteLicenseNumber;
        //            customerInformation.customerInformation.OwnerAddress = obj1.customerInformation.OwnerAddress;
        //            customerInformation.customerInformation.BusinessAddress = obj1.customerInformation.BusinessAddress;
        //            customerInformation.customerInformation.TaxIdfein = obj1.customerInformation.TaxIdfein;
        //            customerInformation.customerInformation.StateIdnumber = obj1.customerInformation.StateIdnumber;
        //            customerInformation.customerInformation.Vendor = obj1.customerInformation.Vendor;
        //            customerInformation.customerInformation.Dea = obj1.customerInformation.Dea;
        //            customerInformation.customerInformation.Memo = obj1.customerInformation.Memo;
        //            customerInformation.customerInformation.CustomerTypeId = obj1.customerInformation.CustomerTypeId;
        //            customerInformation.customerInformation.Dob = obj1.customerInformation.Dob;
        //            customerInformation.customerInformation.Ssn = obj1.customerInformation.Ssn;
        //            customerInformation.customerInformation.DrivingLicenseNumber = obj1.customerInformation.DrivingLicenseNumber;
        //            customerInformation.customerInformation.DrivingLicenseStateId = obj1.customerInformation.DrivingLicenseStateId;
        //            customerInformation.customerInformation.VehicleNumber = obj1.customerInformation.VehicleNumber;
        //            customerInformation.customerInformation.Authorized = obj1.customerInformation.Authorized;

        //            if (obj1.customerInformation.ProviderId != null && obj1.customerInformation.ProviderId != 0)
        //            {
        //                var getprovider = db.Providers.Find(obj1.customerInformation.ProviderId);
        //                if (getprovider != null)
        //                {
        //                    customerInformation.customerInformation.Provider = getprovider.Name;
        //                }

        //            }

        //            if (obj1.customerInformation.CustomerTypeId != null && obj1.customerInformation.CustomerTypeId != 0)

        //            {
        //                var getcustomer = db.CustomerTypes.Find(obj1.customerInformation.CustomerTypeId);
        //                if (getcustomer != null)
        //                {
        //                    customerInformation.customerInformation.CustomerType = getcustomer.TypeName;
        //                }

        //            }

        //            if (obj1.customerInformation.StateId != null && obj1.customerInformation.StateId != 0)
        //            {
        //                var getcustomerstate = db.CustomerStates.Find(obj1.customerInformation.StateId);
        //                if (getcustomerstate != null)
        //                {
        //                    customerInformation.customerInformation.State = getcustomerstate.StateName;
        //                }

        //            }

        //            if (obj1.customerInformation.DrivingLicenseStateId != null && obj1.customerInformation.DrivingLicenseStateId != 0)
        //            {
        //                var getdrivinglicense = db.DrivingLicenseStates.Find(obj1.customerInformation.DrivingLicenseStateId);
        //                if (getdrivinglicense != null)
        //                {
        //                    customerInformation.customerInformation.CustomerType = getdrivinglicense.Name;
        //                }

        //            }

        //            if (obj1.CustomerClassification.GroupId != null)
        //            {
        //                var getgroup = db.Groups.Find(obj1.CustomerClassification.GroupId);
        //                if (getgroup != null)
        //                {
        //                    obj1.CustomerClassification.GroupName = getgroup.Name;
        //                }

        //            }

        //            if (obj1.CustomerClassification.SubGroupId != null && obj1.CustomerClassification.SubGroupId != 0)
        //            {
        //                var getsubgroup = db.SubGroups.Find(obj1.CustomerClassification.SubGroupId);
        //                if (getsubgroup != null)
        //                {
        //                    obj1.CustomerClassification.SubGroupName = getsubgroup.SubGroupName;
        //                }

        //            }


        //            if (obj1.CustomerClassification.BusinessTypeId != null && obj1.CustomerClassification.BusinessTypeId != 0)
        //            {
        //                var getbusiness = db.BusinessTypes.Find(obj1.CustomerClassification.BusinessTypeId);
        //                if (getbusiness != null)
        //                {
        //                    obj1.CustomerClassification.BusinessType = getbusiness.TypeName;
        //                }

        //            }
        //            if (obj1.CustomerClassification.ZoneId != null && obj1.CustomerClassification.ZoneId != 0)
        //            {
        //                var getzone = db.Zones.Find(obj1.CustomerClassification.ZoneId);
        //                if (getzone != null)
        //                {
        //                    obj1.CustomerClassification.Zone = getzone.Name;
        //                }

        //            }
        //            if (obj1.CustomerClassification.SalesmanId != null && obj1.CustomerClassification.SalesmanId != 0)
        //            {
        //                var getsaleman = db.Salesmen.Find(obj1.CustomerClassification.SalesmanId);
        //                if (getsaleman != null)
        //                {
        //                    obj1.CustomerClassification.Salesman = getsaleman.Name;
        //                }

        //            }
        //            if (obj1.CustomerClassification.ShippedViaId != null && obj1.CustomerClassification.ShippedViaId != 0)
        //            {
        //                var getshipment = db.ShipmentPurchases.Find(obj1.CustomerClassification.ShippedViaId);
        //                if (getshipment != null)
        //                {
        //                    customerInformation.customerInformation.CustomerClassification.ShippedVia = getshipment.Type;
        //                }

        //            }
        //            if (obj1.CustomerClassification.RouteId != null && obj1.CustomerClassification.RouteId != 0)
        //            {
        //                var getroute = db.Routes.Find(obj1.CustomerClassification.RouteId);
        //                if (getroute != null)
        //                {
        //                    obj1.CustomerClassification.RouteName = getroute.RouteName;
        //                }

        //            }
        //            if (obj1.CustomerClassification.DriverId != null && obj1.CustomerClassification.DriverId != 0)
        //            {
        //                var getdriver = db.Drivers.Find(obj1.CustomerClassification.DriverId);
        //                if (getdriver != null)
        //                {
        //                    obj1.CustomerClassification.DriverName = getdriver.Name;
        //                }

        //            }
        //            if (obj1.CustomerClassification.ShiptoReferenceId != null && obj1.CustomerClassification.ShiptoReferenceId != 0)
        //            {
        //                var getshipreference = db.ShiptoReferences.Find(obj1.CustomerClassification.ShiptoReferenceId);
        //                if (getshipreference != null)
        //                {
        //                    obj1.CustomerClassification.ShiptoReference = getshipreference.Name;
        //                }
        //            }
        //            if (obj1.customerInformation.CustomerBillingInfo.PaymentTermsId != null && obj1.customerInformation.CustomerBillingInfo.PaymentTermsId != 0)
        //            {
        //                var getpaymentterm = db.PaymentTerms.Find(obj1.customerInformation.CustomerBillingInfo.PaymentTermsId);
        //                if (getpaymentterm != null)
        //                {
        //                    obj1.customerInformation.CustomerBillingInfo.PaymentTerms = getpaymentterm.Name;
        //                }
        //            }
        //            if (obj1.customerInformation.CustomerBillingInfo.PricingId != null && obj1.customerInformation.CustomerBillingInfo.PricingId != 0)
        //            {
        //                var getpricing = db.Pricings.Find(obj1.customerInformation.CustomerBillingInfo.PricingId);
        //                if (getpricing != null)
        //                {
        //                    obj1.customerInformation.CustomerBillingInfo.Pricing = getpricing.Name;
        //                }
        //            }
        //        }


        //        //account
        //        Account obj1acount = null;
        //        obj1acount = new Account();
        //        var subaccrecord = db.AccountSubGroups.ToList().Where(x => x.Title == "Customers").FirstOrDefault();
        //        if (subaccrecord != null)
        //        {
        //            var getAccount = db.Accounts.ToList().Where(x => x.AccountSubGroupId == subaccrecord.AccountSubGroupId).LastOrDefault();
        //            if (getAccount != null)
        //            {
        //                var code = getAccount.AccountId.Split("-")[3];
        //                int getcode = 0;
        //                if (code != null)
        //                {

        //                    getcode = Convert.ToInt32(code) + 1;
        //                }
        //                obj1acount.AccountId = subaccrecord.AccountSubGroupId + "000" + Convert.ToString(getcode);
        //                obj1acount.Title = obj1.customerInformation.Company;
        //                obj1acount.Status = 1;
        //                obj1acount.AccountSubGroupId = subaccrecord.AccountSubGroupId;
        //                var customeracc = db.Accounts.Add(obj1acount);
        //                db.SaveChanges();

        //                customerInformation.customerInformation.AccountId = customeracc.Entity.AccountId;
        //                customerInformation.customerInformation.AccountNumber = customeracc.Entity.AccountId;
        //                customerInformation.customerInformation.AccountTitle = customeracc.Entity.Title;
        //            }

        //        }

        //        var record = db.CustomerInformations.Add(customerInformation.customerInformation);
        //        await db.SaveChangesAsync();

        //        CustomerClassification customerclassification = new CustomerClassification();

        //        if (record.Entity.CustomerClassification != null)
        //        {
        //            customerclassification.CustomerInfoId = record.Entity.Id;
        //            customerclassification.CustomerCode = record.Entity.CustomerCode;
        //            customerclassification.GroupId = obj1.CustomerClassification.GroupId;
        //            customerclassification.GroupName = obj1.CustomerClassification.GroupName;
        //            customerclassification.SubGroupId = obj1.CustomerClassification.SubGroupId;
        //            customerclassification.SubGroupName = obj1.CustomerClassification.SubGroupName;
        //            customerclassification.ZoneId = obj1.CustomerClassification.ZoneId;
        //            customerclassification.Zone = obj1.CustomerClassification.Zone;
        //            customerclassification.SalesmanId = obj1.CustomerClassification.SalesmanId;
        //            customerclassification.Salesman = obj1.CustomerClassification.Salesman;
        //            customerclassification.ShippedViaId = obj1.CustomerClassification.ShippedViaId;
        //            customerclassification.ShippedVia = obj1.CustomerClassification.ShippedVia;
        //            customerclassification.RouteId = obj1.CustomerClassification.RouteId;
        //            customerclassification.RouteName = obj1.CustomerClassification.RouteName;
        //            customerclassification.RouteDeliveryDay = obj1.CustomerClassification.RouteDeliveryDay;
        //            customerclassification.DriverId = obj1.CustomerClassification.DriverId;
        //            customerclassification.DriverName = obj1.CustomerClassification.DriverName;
        //            customerclassification.ShiptoReferenceId = obj1.CustomerClassification.ShiptoReferenceId;
        //            customerclassification.ShiptoReference = obj1.CustomerClassification.ShiptoReference;
        //            customerclassification.OutOfStateCustomer = obj1.CustomerClassification.OutOfStateCustomer;
        //            customerclassification.AddtoMaillingList = obj1.CustomerClassification.AddtoMaillingList;
        //            customerclassification.AddtoemailTextList = obj1.CustomerClassification.AddtoemailTextList;
        //            customerclassification.RejectPromotion = obj1.CustomerClassification.RejectPromotion;
        //            customerclassification.ViewInvoicePrevBalance = obj1.CustomerClassification.ViewInvoicePrevBalance;
        //            customerclassification.ViewRetailandDiscount = obj1.CustomerClassification.ViewRetailandDiscount;
        //            customerclassification.BarCodeId = obj1.CustomerClassification.BarCodeId;
        //            customerclassification.BarCode = obj1.CustomerClassification.BarCode;
        //            customerclassification.SpecialInvoiceCustom = obj1.CustomerClassification.SpecialInvoiceCustom;
        //            customerclassification.OtherCustomerReference = obj1.CustomerClassification.OtherCustomerReference;
        //            customerclassification.UseDefaultInvMemo = obj1.CustomerClassification.UseDefaultInvMemo;
        //            customerclassification.InvoiceMemo = obj1.CustomerClassification.InvoiceMemo;
        //        }
        //        else
        //        {
        //            customerclassification.CustomerInfoId = record.Entity.Id;
        //            customerclassification.CustomerCode = record.Entity.CustomerCode;
        //            customerclassification.GroupId = obj1.CustomerClassification.GroupId;
        //            customerclassification.GroupName = obj1.CustomerClassification.GroupName;
        //            customerclassification.SubGroupId = obj1.CustomerClassification.SubGroupId;
        //            customerclassification.SubGroupName = obj1.CustomerClassification.SubGroupName;
        //            customerclassification.ZoneId = obj1.CustomerClassification.ZoneId;
        //            customerclassification.Zone = obj1.CustomerClassification.Zone;
        //            customerclassification.SalesmanId = obj1.CustomerClassification.SalesmanId;
        //            customerclassification.Salesman = obj1.CustomerClassification.Salesman;
        //            customerclassification.ShippedViaId = obj1.CustomerClassification.ShippedViaId;
        //            customerclassification.ShippedVia = obj1.CustomerClassification.ShippedVia;
        //            customerclassification.RouteId = obj1.CustomerClassification.RouteId;
        //            customerclassification.RouteName = obj1.CustomerClassification.RouteName;
        //            customerclassification.RouteDeliveryDay = obj1.CustomerClassification.RouteDeliveryDay;
        //            customerclassification.DriverId = obj1.CustomerClassification.DriverId;
        //            customerclassification.DriverName = obj1.CustomerClassification.DriverName;
        //            customerclassification.ShiptoReferenceId = obj1.CustomerClassification.ShiptoReferenceId;
        //            customerclassification.ShiptoReference = obj1.CustomerClassification.ShiptoReference;
        //            customerclassification.OutOfStateCustomer = obj1.CustomerClassification.OutOfStateCustomer;
        //            customerclassification.AddtoMaillingList = obj1.CustomerClassification.AddtoMaillingList;
        //            customerclassification.AddtoemailTextList = obj1.CustomerClassification.AddtoemailTextList;
        //            customerclassification.RejectPromotion = obj1.CustomerClassification.RejectPromotion;
        //            customerclassification.ViewInvoicePrevBalance = obj1.CustomerClassification.ViewInvoicePrevBalance;
        //            customerclassification.ViewRetailandDiscount = obj1.CustomerClassification.ViewRetailandDiscount;
        //            customerclassification.BarCodeId = obj1.CustomerClassification.BarCodeId;
        //            customerclassification.BarCode = obj1.CustomerClassification.BarCode;
        //            customerclassification.SpecialInvoiceCustom = obj1.CustomerClassification.SpecialInvoiceCustom;
        //            customerclassification.OtherCustomerReference = obj1.CustomerClassification.OtherCustomerReference;
        //            customerclassification.UseDefaultInvMemo = obj1.CustomerClassification.UseDefaultInvMemo;
        //            customerclassification.InvoiceMemo = obj1.CustomerClassification.InvoiceMemo;
        //        }
        //        var classificationrecord = db.CustomerClassifications.Add(customerclassification);
        //        await db.SaveChangesAsync();
        //        CustomerBillingInfo customerbillinginfo = new CustomerBillingInfo();

        //        if (record.Entity.CustomerBillingInfo != null)
        //        {
        //            customerbillinginfo.CustomerInformationId = record.Entity.Id;
        //            customerbillinginfo.CustomerCode = record.Entity.CustomerCode;
        //            customerbillinginfo.CustomerClassificationId = classificationrecord.Entity.Id;

        //            customerbillinginfo.IsTaxExempt = obj1.customerInformation.CustomerBillingInfo.IsTaxExempt;
        //            customerbillinginfo.PricingId = obj1.customerInformation.CustomerBillingInfo.PricingId;
        //            customerbillinginfo.Pricing = obj1.customerInformation.CustomerBillingInfo.Pricing;
        //            customerbillinginfo.RetailPlus = obj1.customerInformation.CustomerBillingInfo.RetailPlus;
        //            customerbillinginfo.RetailPlusPercentage = obj1.customerInformation.CustomerBillingInfo.RetailPlusPercentage;
        //            customerbillinginfo.IsGetSalesDiscounts = obj1.customerInformation.CustomerBillingInfo.IsGetSalesDiscounts;
        //            customerbillinginfo.IsOutOfStateCustomer = obj1.customerInformation.CustomerBillingInfo.IsOutOfStateCustomer;
        //            customerbillinginfo.AdditionalInvoiceCharge = obj1.customerInformation.CustomerBillingInfo.AdditionalInvoiceCharge;
        //            customerbillinginfo.AdditionalInvoiceDiscount = obj1.customerInformation.CustomerBillingInfo.AdditionalInvoiceDiscount;
        //            customerbillinginfo.ScheduleMessage = obj1.customerInformation.CustomerBillingInfo.ScheduleMessage;
        //            customerbillinginfo.ScheduleMessageFromDate = obj1.customerInformation.CustomerBillingInfo.ScheduleMessageFromDate;
        //            customerbillinginfo.ScheduleMessageToDate = obj1.customerInformation.CustomerBillingInfo.ScheduleMessageToDate;
        //            customerbillinginfo.PaymentTermsId = obj1.customerInformation.CustomerBillingInfo.PaymentTermsId;
        //            customerbillinginfo.PaymentTerms = obj1.customerInformation.CustomerBillingInfo.PaymentTerms;
        //            customerbillinginfo.CreditLimit = obj1.customerInformation.CustomerBillingInfo.CreditLimit;
        //            customerbillinginfo.IsCreditHold = obj1.customerInformation.CustomerBillingInfo.IsCreditHold;
        //            customerbillinginfo.IsBillToBill = obj1.customerInformation.CustomerBillingInfo.IsBillToBill;
        //            customerbillinginfo.IsNoCheckAccepted = obj1.customerInformation.CustomerBillingInfo.IsNoCheckAccepted;
        //            customerbillinginfo.IsExclude = obj1.customerInformation.CustomerBillingInfo.IsExclude;
        //            customerbillinginfo.ThirdPartyCheckCharge = obj1.customerInformation.CustomerBillingInfo.ThirdPartyCheckCharge;
        //            customerbillinginfo.IsCashBackBalance = obj1.customerInformation.CustomerBillingInfo.IsCashBackBalance;
        //            customerbillinginfo.CashBackBalance = obj1.customerInformation.CustomerBillingInfo.CashBackBalance;
        //            customerbillinginfo.IsPopupMessage = obj1.customerInformation.CustomerBillingInfo.IsPopupMessage;
        //            customerbillinginfo.PopupMessage = obj1.customerInformation.CustomerBillingInfo.PopupMessage;
        //        }
        //        else
        //        {
        //            customerbillinginfo.CustomerInformationId = record.Entity.Id;
        //            customerbillinginfo.CustomerCode = record.Entity.CustomerCode;
        //            customerbillinginfo.CustomerClassificationId = classificationrecord.Entity.Id;
        //            customerbillinginfo.IsTaxExempt = obj1.customerInformation.CustomerBillingInfo.IsTaxExempt;
        //            customerbillinginfo.PricingId = obj1.customerInformation.CustomerBillingInfo.PricingId;
        //            customerbillinginfo.Pricing = obj1.customerInformation.CustomerBillingInfo.Pricing;
        //            customerbillinginfo.RetailPlus = obj1.customerInformation.CustomerBillingInfo.RetailPlus;
        //            customerbillinginfo.RetailPlusPercentage = obj1.customerInformation.CustomerBillingInfo.RetailPlusPercentage;
        //            customerbillinginfo.IsGetSalesDiscounts = obj1.customerInformation.CustomerBillingInfo.IsGetSalesDiscounts;
        //            customerbillinginfo.IsOutOfStateCustomer = obj1.customerInformation.CustomerBillingInfo.IsOutOfStateCustomer;
        //            customerbillinginfo.AdditionalInvoiceCharge = obj1.customerInformation.CustomerBillingInfo.AdditionalInvoiceCharge;
        //            customerbillinginfo.AdditionalInvoiceDiscount = obj1.customerInformation.CustomerBillingInfo.AdditionalInvoiceDiscount;
        //            customerbillinginfo.ScheduleMessage = obj1.customerInformation.CustomerBillingInfo.ScheduleMessage;
        //            customerbillinginfo.ScheduleMessageFromDate = obj1.customerInformation.CustomerBillingInfo.ScheduleMessageFromDate;
        //            customerbillinginfo.ScheduleMessageToDate = obj1.customerInformation.CustomerBillingInfo.ScheduleMessageToDate;
        //            customerbillinginfo.PaymentTermsId = obj1.customerInformation.CustomerBillingInfo.PaymentTermsId;
        //            customerbillinginfo.PaymentTerms = obj1.customerInformation.CustomerBillingInfo.PaymentTerms;
        //            customerbillinginfo.CreditLimit = obj1.customerInformation.CustomerBillingInfo.CreditLimit;
        //            customerbillinginfo.IsCreditHold = obj1.customerInformation.CustomerBillingInfo.IsCreditHold;
        //            customerbillinginfo.IsBillToBill = obj1.customerInformation.CustomerBillingInfo.IsBillToBill;
        //            customerbillinginfo.IsNoCheckAccepted = obj1.customerInformation.CustomerBillingInfo.IsNoCheckAccepted;
        //            customerbillinginfo.IsExclude = obj1.customerInformation.CustomerBillingInfo.IsExclude;
        //            customerbillinginfo.ThirdPartyCheckCharge = obj1.customerInformation.CustomerBillingInfo.ThirdPartyCheckCharge;
        //            customerbillinginfo.IsCashBackBalance = obj1.customerInformation.CustomerBillingInfo.IsCashBackBalance;
        //            customerbillinginfo.CashBackBalance = obj1.customerInformation.CustomerBillingInfo.CashBackBalance;
        //            customerbillinginfo.IsPopupMessage = obj1.customerInformation.CustomerBillingInfo.IsPopupMessage;
        //            customerbillinginfo.PopupMessage = obj1.customerInformation.CustomerBillingInfo.PopupMessage;
        //        }
        //        db.CustomerBillingInfos.Add(customerbillinginfo);
        //        await db.SaveChangesAsync();



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

                bool isValid = db.CustomerClassifications.ToList().Exists(x => x.CustomerCode.Equals(inforecord.CustomerCode, StringComparison.CurrentCultureIgnoreCase) && x.CustomerInfoId == inforecord.Id);
                if (isValid)
                {
                    var classificationrecord = db.CustomerClassifications.Where(x => x.CustomerInfoId == inforecord.Id && x.CustomerCode == inforecord.CustomerCode).FirstOrDefault();
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

                var record = db.CustomerInformations.Find(id);
                if (record != null)
                {
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
                var classificationdataid = db.CustomerClassifications.Where(x => x.CustomerInfoId == data.Id && x.CustomerCode == data.CustomerCode).FirstOrDefault().Id;
                var classificationdata = await db.CustomerClassifications.FindAsync(classificationdataid);
                if (classificationdata != null)
                {
                    db.CustomerClassifications.Remove(classificationdata);
                }
                var billinginfodataid = db.CustomerBillingInfos.Where(x => x.CustomerInformationId == data.Id && x.CustomerCode == data.CustomerCode).FirstOrDefault().Id;
                var billinginfodata = await db.CustomerBillingInfos.FindAsync(billinginfodataid);
                if (classificationdata != null)
                {
                    db.CustomerBillingInfos.Remove(billinginfodata);
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
        public async Task<IActionResult> CustomerClassificationCreate(CustomerClassification obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerClassification>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                //bool checkgroup = db.CustomerClassifications.ToList().Exists(p => p.GroupName.Equals(obj1.GroupName, StringComparison.CurrentCultureIgnoreCase));
                //if (checkgroup)

                //{
                //    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                //    return Ok(Response);
                //}
                var record = db.CustomerClassifications.Add(obj1);
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
        public async Task<IActionResult> CustomerBillingInfoCreate(CustomerBillingInfo obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerBillingInfo>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkbillinginfo = db.CustomerBillingInfos.ToList().Exists(p => p.CustomerCode.Equals(obj1.CustomerCode, StringComparison.CurrentCultureIgnoreCase));
                if (checkbillinginfo)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                var record = db.CustomerBillingInfos.Add(obj1);
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
        public async Task<IActionResult> ShiptoReferenceCreate(ShiptoReference obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ShiptoReference>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.ShiptoReferences.ToList().Exists(p => p.Name.Equals(obj1.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.ShiptoReferences.Add(obj1);
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
        public async Task<IActionResult> PaymentTermCreate(PaymentTerm obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<PaymentTerm>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }


                bool checkname = db.PaymentTerms.ToList().Exists(p => p.Name.Equals(obj1.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.PaymentTerms.Add(obj1);
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
        public async Task<IActionResult> PricingCreate(Pricing obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Pricings.ToList().Exists(p => p.Name.Equals(obj1.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.Pricings.Add(obj1);
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

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(CartDetail obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CartDetail>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (obj1.Quantity == null)
                {
                    obj1.Quantity = 1;
                }
                var fullcode = "";
                var Cartfullcode = "";

                var CurrentgetCart = db.CartDetails.Where(x => x.PendingForApproval == false).ToList();

                var recordemp = db.CustomerOrders.ToList();
                if (CurrentgetCart != null && CurrentgetCart.Count() > 0)
                {
                    if (CurrentgetCart != null && CurrentgetCart.Count() > 0)
                    {
                        if (CurrentgetCart[0].TicketId != null && CurrentgetCart[0].TicketId != "string" && CurrentgetCart[0].TicketId != "")
                        {
                            int large, small;
                            int salesID = 0;
                            large = Convert.ToInt32(CurrentgetCart[0].TicketId.Split('-')[1]);
                            small = Convert.ToInt32(CurrentgetCart[0].TicketId.Split('-')[1]);
                            for (int i = 0; i < CurrentgetCart.Count(); i++)
                            {
                                if (CurrentgetCart[i].TicketId != null)
                                {
                                    var t = Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]);
                                    if (Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]) > large)
                                    {
                                        salesID = Convert.ToInt32(CurrentgetCart[i].CartId);
                                        large = Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]);
                                    }
                                    else if (Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]) < small)
                                    {
                                        small = Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]);
                                    }
                                    else
                                    {
                                        //if (large < 2)
                                        //{
                                            salesID = Convert.ToInt32(CurrentgetCart[i].CartId);
                                        //}
                                    }
                                }
                            }
                            var Cartnewitems = CurrentgetCart.ToList().Where(x => x.CartId == salesID).FirstOrDefault();
                            if (Cartnewitems != null)
                            {
                                if (Cartnewitems.TicketId != null)
                                {

                                    int code = 0;
                                    var VcodeSplit = Cartnewitems.TicketId.Split('-');
                                    if(Cartnewitems.UserId == obj1.UserId)
                                    {
                                        code = Convert.ToInt32(VcodeSplit[1]);
                                    }
                                    else
                                    {
                                        code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                    }
                                    long checknumber = Convert.ToInt64(VcodeSplit[1]);
                                    if (checknumber > 9)
                                    {
                                        Cartfullcode = "0000000-00" + Convert.ToString(code);
                                    }
                                    else if (checknumber > 99)
                                    {
                                        Cartfullcode = "0000000-0" + Convert.ToString(code);
                                    }
                                    else if (checknumber > 999)
                                    {
                                        Cartfullcode = "0000000-" + Convert.ToString(code);
                                    }
                                    else if (checknumber > 9999)
                                    {
                                        //10000
                                        long ndcode = Convert.ToInt64(VcodeSplit[0]) + 1;

                                        Cartfullcode = "000000" + Convert.ToString(ndcode) + "-" + "9999";
                                    }
                                    else
                                    {
                                        Cartfullcode = "0000000-000" + Convert.ToString(code);
                                    }
                                    //fullcode = "0000000" + "-" + Convert.ToString(code);
                                }
                                else
                                {
                                    Cartfullcode = "0000000" + "-" + "0001";
                                }
                            }
                            else
                            {
                                Cartfullcode = "0000000" + "-" + "0001";
                            }
                        }
                        else
                        {
                            Cartfullcode = "0000000" + "-" + "0001";
                        }
                    }
                    else
                    {
                        Cartfullcode = "0000000" + "-" + "0001";
                    }
                    obj1.TicketId = Cartfullcode;
                }
                else if (recordemp != null && recordemp.Count() > 0)
                {
                    if (recordemp[0].TicketId != null && recordemp[0].TicketId != "string" && recordemp[0].TicketId != "")
                    {
                        int large, small;
                        int salesID = 0;
                        large = Convert.ToInt32(recordemp[0].TicketId.Split('-')[1]);
                        small = Convert.ToInt32(recordemp[0].TicketId.Split('-')[1]);
                        for (int i = 0; i < recordemp.Count(); i++)
                        {
                            if (recordemp[i].TicketId != null)
                            {
                                var t = Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]);
                                if (Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]) > large)
                                {
                                    salesID = Convert.ToInt32(recordemp[i].OrderId);
                                    large = Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]);
                                }
                                else if (Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]) < small)
                                {
                                    small = Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]);
                                }
                                else
                                {
                                   // if (large <= 2)
                                    //{
                                        salesID = Convert.ToInt32(recordemp[i].OrderId);
                                   // }
                                }
                            }
                        }
                        var newitems = recordemp.ToList().Where(x => x.OrderId == salesID).FirstOrDefault();
                        if (newitems != null)
                        {
                            if (newitems.TicketId != null)
                            {


                                var VcodeSplit = newitems.TicketId.Split('-');
                                int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                long checknumber = Convert.ToInt64(VcodeSplit[1]);
                                if (checknumber > 9)
                                {
                                    fullcode = "0000000-00" + Convert.ToString(code);
                                }
                                else if (checknumber > 99)
                                {
                                    fullcode = "0000000-0" + Convert.ToString(code);
                                }
                                else if (checknumber > 999)
                                {
                                    fullcode = "0000000-" + Convert.ToString(code);
                                }
                                else if (checknumber > 9999)
                                {
                                    //10000
                                    long ndcode = Convert.ToInt64(VcodeSplit[0]) + 1;

                                    fullcode = "000000" + Convert.ToString(ndcode) + "-" + "9999";
                                }
                                else
                                {
                                    fullcode = "0000000-000" + Convert.ToString(code);
                                }
                                //fullcode = "0000000" + "-" + Convert.ToString(code);
                            }
                            else
                            {
                                fullcode = "0000000" + "-" + "0001";
                            }
                        }
                        else
                        {
                            fullcode = "0000000" + "-" + "0001";
                        }
                    }
                    else
                    {
                        fullcode = "0000000" + "-" + "0001";
                    }
                    obj1.TicketId = fullcode;

                }
                else
                {
                    fullcode = "0000000" + "-" + "0001";
                    obj1.TicketId = fullcode;
                }
                var getStock = db.InventoryStocks.ToList().Where(x => x.ProductId == obj1.Id)?.FirstOrDefault();
                if (getStock != null)
                {
                    var checkproduct = db.CartDetails.Where(x => x.Id == obj1.Id && x.PendingForApproval == false && x.TicketId == obj1.TicketId).FirstOrDefault();
                    if (checkproduct != null)
                    {
                        checkproduct.Quantity = obj1.Quantity + checkproduct.Quantity;
                        db.Entry(checkproduct).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                        return Ok(Response);
                    }
                   // if (Convert.ToDouble(getStock.Quantity) >= Convert.ToDouble(obj1.Quantity))
                    //{
                        //if (checkproduct != null)
                        //{
                        //    checkproduct.Quantity = obj1.Quantity;
                        //    //await db.SaveChangesAsync();
                        //    //ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                        //    //return Ok(Response);
                        //}
                        var tax = 0.0;
                        var productSt = db.Sttaxes.Where(x => x.ProductId == obj1.Id)?.FirstOrDefault();
                        if (productSt != null)
                        {
                            tax = ((double)(tax + (double.Parse(productSt.Tax) * obj1.Quantity)));
                            obj1.Tax = productSt.Tax;
                        }
                        var retailProduct = db.Products.Where(x => x.Id == obj1.Id)?.FirstOrDefault();
                        if (retailProduct != null)
                        {
                            obj1.Name = retailProduct.Name;
                            obj1.ProductCode = retailProduct.ProductCode;
                            obj1.Retail = retailProduct.Retail.ToString();
                            double amountTotal = Convert.ToDouble(obj1.Quantity) * Convert.ToDouble(obj1.Retail);
                            obj1.Total = Math.Round(amountTotal, 2).ToString();

                        }
                        obj1.TotalTaxes = tax.ToString();
                        obj1.PendingForApproval = false;
                        obj1.IsDelivered = false;
                        if (obj1.OrderDate == null)
                        {
                            obj1.OrderDate = DateTime.Now; ;
                        }
                        else
                        {
                            obj1.OrderDate = obj1.OrderDate;
                        }
                        if (retailProduct.ItemImageByPath != null)
                        {
                            obj1.ImagePath = retailProduct.ItemImageByPath;
                        }
                        db.CartDetails.Add(obj1);
                        await db.SaveChangesAsync();
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                        return Ok(Response);
                    //}
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.Exceed_Credit_Limit, null, null);
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

        [HttpGet("DeleteCartItemById")]
        public async Task<IActionResult> DeleteCartItemById(int id, string ticketId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CartDetail>();
                var record = db.CartDetails.Where(x => x.Id == id && x.TicketId == ticketId).FirstOrDefault();
                if (record != null)
                {
                    db.CartDetails.Remove(record);
                    await db.SaveChangesAsync();
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
        [HttpGet("GetUserCartById/{id}")]
        public IActionResult GetUserCartById(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CartDetail>>();
                var record = db.CartDetails.ToList().Where(x => x.UserId == id && x.PendingForApproval == false).ToList();
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


        [HttpGet("GetUserCartByGroup/{id}")]
        public IActionResult GetUserCartByGroup(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CartDetail>>();
                var record = db.CartDetails.ToList().GroupBy(x => x.TicketId).Select(i => i.LastOrDefault()).Where(x => x.UserId == id && x.PendingForApproval == false).ToList();
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
        [HttpGet("GetTotalCart")]
        public IActionResult GetTotalCart()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CartDetail>>();
                var record = db.CartDetails.Where(x => x.PendingForApproval == false).ToList();
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
                    var Response = ResponseBuilder.BuildWSResponse<List<CartDetail>>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetStockById/{id}")]
        public IActionResult GetStockById(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<InventoryStock>();
                var record = db.InventoryStocks.ToList().Where(x => x.ProductId == id)?.FirstOrDefault();
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
                    var Response = ResponseBuilder.BuildWSResponse<InventoryStock>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("SaveCustomerOrder")]
        public async Task<IActionResult> SaveCustomerOrder(CustomerOrder obj1)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var fullcode = "";
                //var Cartfullcode = "";
                //var newva = "";

                //var recordemp = db.CustomerOrders.ToList();
                //if (recordemp != null && recordemp.Count() > 0)
                //{
                //    if (recordemp[0].TicketId != null && recordemp[0].TicketId != "string" && recordemp[0].TicketId != "")
                //    {
                //        int large, small;
                //        int salesID = 0;
                //        large = Convert.ToInt32(recordemp[0].TicketId.Split('-')[1]);
                //        small = Convert.ToInt32(recordemp[0].TicketId.Split('-')[1]);
                //        for (int i = 0; i < recordemp.Count(); i++)
                //        {
                //            if (recordemp[i].TicketId != null)
                //            {
                //                var t = Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]);
                //                if (Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]) > large)
                //                {
                //                    salesID = Convert.ToInt32(recordemp[i].OrderId);
                //                    large = Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]);
                //                }
                //                else if (Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]) < small)
                //                {
                //                    small = Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]);
                //                }
                //                else
                //                {
                //                    //if (large < 2)
                //                   // {
                //                        salesID = Convert.ToInt32(recordemp[i].OrderId);
                //                    //}
                //                }
                //            }
                //        }
                //        var newitems = recordemp.ToList().Where(x => x.OrderId == salesID).FirstOrDefault();
                //        if (newitems != null)
                //        {
                //            if (newitems.TicketId != null)
                //            {


                //                var VcodeSplit = newitems.TicketId.Split('-');
                //                int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                //                long checknumber = Convert.ToInt64(VcodeSplit[1]);
                //                if (checknumber > 9)
                //                {
                //                    fullcode = "0000000-00" + Convert.ToString(code);
                //                }
                //                else if (checknumber > 99)
                //                {
                //                    fullcode = "0000000-0" + Convert.ToString(code);
                //                }
                //                else if (checknumber > 999)
                //                {
                //                    fullcode = "0000000-" + Convert.ToString(code);
                //                }
                //                else if (checknumber > 9999)
                //                {
                //                    //10000
                //                    long ndcode = Convert.ToInt64(VcodeSplit[0]) + 1;

                //                    fullcode = "000000" + Convert.ToString(ndcode) + "-" + "9999";
                //                }
                //                else
                //                {
                //                    fullcode = "0000000-000" + Convert.ToString(code);
                //                }
                //                //fullcode = "0000000" + "-" + Convert.ToString(code);
                //            }
                //            else
                //            {
                //                fullcode = "0000000" + "-" + "0001";
                //            }
                //        }
                //        else
                //        {
                //            fullcode = "0000000" + "-" + "0001";
                //        }
                //    }
                //    else
                //    {
                //        fullcode = "0000000" + "-" + "0001";
                //    }
                //    newva = fullcode;

                //}
                //else
                //{
                //    var CurrentgetCart = db.CartDetails.Where(x => x.PendingForApproval == false).ToList();
                //    if (CurrentgetCart != null && CurrentgetCart.Count() > 0)
                //    {
                //        if (CurrentgetCart[0].TicketId != null && CurrentgetCart[0].TicketId != "string" && CurrentgetCart[0].TicketId != "")
                //        {
                //            int large, small;
                //            int salesID = 0;
                //            large = Convert.ToInt32(CurrentgetCart[0].TicketId.Split('-')[1]);
                //            small = Convert.ToInt32(CurrentgetCart[0].TicketId.Split('-')[1]);
                //            for (int i = 0; i < CurrentgetCart.Count(); i++)
                //            {
                //                if (CurrentgetCart[i].TicketId != null)
                //                {
                //                    var t = Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]);
                //                    if (Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]) > large)
                //                    {
                //                        salesID = Convert.ToInt32(CurrentgetCart[i].CartId);
                //                        large = Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]);
                //                    }
                //                    else if (Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]) < small)
                //                    {
                //                        small = Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]);
                //                    }
                //                    else
                //                    {
                //                       // if (large < 2)
                //                        //{
                //                            salesID = Convert.ToInt32(CurrentgetCart[i].CartId);
                //                       // }
                //                    }
                //                }
                //            }
                //            var Cartnewitems = CurrentgetCart.ToList().Where(x => x.CartId == salesID).FirstOrDefault();
                //            if (Cartnewitems != null)
                //            {
                //                if (Cartnewitems.TicketId != null)
                //                {


                //                    var VcodeSplit = Cartnewitems.TicketId.Split('-');
                //                    long checknumber = Convert.ToInt64(VcodeSplit[1]);
                //                    long code = Convert.ToInt32(checknumber);

                //                    if (checknumber > 9)
                //                    {
                //                        Cartfullcode = "0000000-00" + Convert.ToString(code);
                //                    }
                //                    else if (checknumber > 99)
                //                    {
                //                        Cartfullcode = "0000000-0" + Convert.ToString(code);
                //                    }
                //                    else if (checknumber > 999)
                //                    {
                //                        Cartfullcode = "0000000-" + Convert.ToString(code);
                //                    }
                //                    else if (checknumber > 9999)
                //                    {
                //                        //10000
                //                        long ndcode = Convert.ToInt64(VcodeSplit[0]) + 1;

                //                        Cartfullcode = "000000" + Convert.ToString(ndcode) + "-" + "9999";
                //                    }
                //                    else
                //                    {
                //                        Cartfullcode = "0000000-000" + Convert.ToString(code);
                //                    }
                //                    //fullcode = "0000000" + "-" + Convert.ToString(code);
                //                }
                //                else
                //                {
                //                    Cartfullcode = "0000000" + "-" + "0001";
                //                }
                //            }
                //            else
                //            {
                //                Cartfullcode = "0000000" + "-" + "0001";
                //            }
                //        }
                //        else
                //        {
                //            Cartfullcode = "0000000" + "-" + "0001";
                //        }
                //    }
                //    else
                //    {
                //        Cartfullcode = "0000000" + "-" + "0001";
                //    }
                //    newva = Cartfullcode;

                //}
                Random rnd = new Random();
                var randomValue = rnd.Next();


                //  var newva = "000-" + (randomValue.ToString()).Substring(0, 4);
               // var newva = fullcode;

                var getCart = db.CartDetails.Where(x => x.UserId == obj1.UserId && x.PendingForApproval == false).ToList();

                CustomerOrder order = null;
                List<CustomerOrder> orderlist = new List<CustomerOrder>();
                if (getCart.Count() > 0)
                {
                    double GrossTotal = 0;
                    var GetCustomer = db.CustomerInformations.ToList().Where(x => x.UserId == obj1.UserId).FirstOrDefault();
                    for (int i = 0; i < getCart.Count(); i++)
                    {
                        order = new CustomerOrder();
                        if (GetCustomer != null)
                        {
                            order.CustomerCode = GetCustomer.CustomerCode;
                        }
                        double totalGet = Convert.ToDouble(getCart[i].Quantity) * Convert.ToDouble(getCart[i].Retail);
                        GrossTotal += totalGet;
                        order.UserId = obj1.UserId;
                        order.BillingAddress = obj1.BillingAddress;
                        order.City = obj1.City;
                        order.Country = obj1.Country;
                        order.CustomerName = obj1.CustomerName;
                        order.Email = obj1.Email;
                        if(obj1.OrderDate == null)
                        {
                            order.OrderDate = DateTime.Now; ;
                        }
                        else
                        {
                            order.OrderDate = obj1.OrderDate;
                        }
                        order.Phone = obj1.Phone;
                        order.Zipcode = obj1.Zipcode;
                        order.AdminStatus = obj1.AdminStatus;
                        order.Delivered = obj1.Delivered;
                        order.IsPulled = obj1.IsPulled;
                        order.IsRejected = obj1.IsRejected;
                        order.IsInvoiced = obj1.IsInvoiced;
                        order.Quantity = getCart[i].Quantity.ToString();
                        order.OrderAmount = obj1.OrderAmount;
                       // order.OrderAmount = GrossTotal.ToString();
                        order.CardTax = obj1.CardTax;
                        order.PaymentMode = obj1.PaymentMode;
                        order.NeedApproval = false;
                        order.AcceptApproval = false;
                        order.ChangeQuantity = null;
                        order.Comment = obj1.Comment;
                        order.BillingAddress = GetCustomer.BusinessAddress;

                        var amount = 0.0;
                        var tax = 0.0;
                        var product = db.Products.Where(x => x.Id == getCart[i].Id).FirstOrDefault();
                        if (product != null)
                        {
                            var productSt = db.Sttaxes.Where(x => x.ProductId == product.Id)?.FirstOrDefault();
                            if (productSt != null)
                            {
                                tax = (double)(tax + (double.Parse(productSt.Tax) * getCart[i].Quantity));
                            }
                            amount = (double)(amount + (double.Parse(product.Retail) * getCart[i].Quantity));
                            order.ProductName = product.Name;

                        }
                        order.TicketId = obj1.TicketId;
                        getCart[i].PendingForApproval = true;
                        order.Price = (Math.Round(amount, 2)).ToString();
                        order.TaxAmount = (Math.Round(tax, 2)).ToString();
                        db.CustomerOrders.Add(order);
                        await db.SaveChangesAsync();
                        orderlist.Add(order);

                    }
                    bool isEmailSent = emailService.SendNewOrderEmail("NewOrder", order.Email, orderlist);
                    if (isEmailSent)
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.Success_WithOutEmail, null, null);
                    }
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

        [HttpGet("RemoveItemFromCart")]
        public IActionResult RemoveItemFromCart(int id,string ticketId)
        {
            try
            {
                //if (!ticketId.Contains("00-"))
                //{
                //    ticketId = "00" + ticketId;
                //}
                var Response = ResponseBuilder.BuildWSResponse<CartDetail>();
                var record = db.CartDetails.Where(x => x.Id == id && x.TicketId == ticketId).FirstOrDefault();
                if (record != null)
                {
                    record.Quantity = record.Quantity - 1;
                    db.SaveChanges();
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
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("AddItemInCart")]
        public IActionResult AddItemInCart(int id, string ticketId)
        {
            try
            {
                //if (!ticketId.Contains("00-"))
                //{
                //    ticketId = "00" + ticketId;
                //}
                var Response = ResponseBuilder.BuildWSResponse<CartDetail>();
                var record = db.CartDetails.Where(x => x.Id == id && x.TicketId == ticketId)?.FirstOrDefault();
                if (record != null)
                {
                    record.Quantity = record.Quantity + 1;
                    db.SaveChanges();
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
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ContactCreate")]
        public async Task<IActionResult> ContactCreate(Contact model)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ItemCategory>();
                //bool checkname = db.ItemCategories.ToList().Exists(p => p.Name.Equals(model.UserName, StringComparison.CurrentCultureIgnoreCase));
                //if (checkname)
                //{
                //    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                //}
                db.Contacts.Add(model);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ItemCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("Send")]
        public async Task<string> Send(MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return ("Email Sent Successfully");
            }
            catch (Exception ex)
            {

                return (ex.Message.ToString());
            }

        }

        [AllowAnonymous]
        [HttpPost("RegisterCustomerECommerce")]
        public async Task<IActionResult> RegisterCustomerECommerce(CustomerInformation model)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ABC.EFCore.Repository.Edmx.CustomerInformation>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                //bool isvalid = db.Customers.ToList().Exists(x => x.Email.Equals(model.Customer.Email, StringComparison.CurrentCultureIgnoreCase));
                bool isvalid = false;
                if (isvalid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                else
                {
                    //if (model.Customer.ProfileImage != null)
                    //{
                    //    //  return BadRequest("Image Path Required Instead of Byte");
                    //    var imgPath = SaveImage(model.Customer.ProfileImage, Guid.NewGuid().ToString());
                    //    model.Customer.ImageByPath = imgPath;
                    //    model.Customer.ProfileImage = null;

                    //}
                    model.AdminApproved = false;
                    db.CustomerInformations.Add(model);
                    db.SaveChanges();

                    var getcustomer = db.CustomerInformations.AsQueryable().ToList().Where(x => x.Email == model.Email).FirstOrDefault();
                    if (getcustomer != null)
                    {
                        CertificateExemptionInstruction certificate = null;
                        certificate = new CertificateExemptionInstruction();
                        certificate.BusinessAddress = model.CertificateExemptionInstructions.BusinessAddress;
                        certificate.CertificateSinglePurchase = model.CertificateExemptionInstructions.CertificateSinglePurchase;
                        certificate.CountryIssue = model.CertificateExemptionInstructions.CountryIssue;
                        certificate.DrivingLicenseNo = model.CertificateExemptionInstructions.DrivingLicenseNo;
                        certificate.CreatedDate = DateTime.Now;
                        certificate.Fein = model.CertificateExemptionInstructions.Fein;
                        certificate.InvoicePurchaseOrderNo = model.CertificateExemptionInstructions.InvoicePurchaseOrderNo;
                        certificate.MultistateSupplementForm = model.CertificateExemptionInstructions.MultistateSupplementForm;
                        certificate.PostalAbbreviation = model.CertificateExemptionInstructions.PostalAbbreviation;
                        certificate.PurchaserCity = model.CertificateExemptionInstructions.PurchaserCity;
                        certificate.PurchaserName = model.CertificateExemptionInstructions.PurchaserName;
                        certificate.PurchaserState = model.CertificateExemptionInstructions.PurchaserState;
                        certificate.PurchaserZipCode = model.CertificateExemptionInstructions.PurchaserZipCode;
                        certificate.PurchaseTaxId = model.CertificateExemptionInstructions.PurchaseTaxId;
                        certificate.SellerAdress = model.CertificateExemptionInstructions.SellerAdress;
                        certificate.SellerCity = model.CertificateExemptionInstructions.SellerCity;
                        certificate.SellerName = model.CertificateExemptionInstructions.SellerName;
                        certificate.SellerState = model.CertificateExemptionInstructions.SellerState;
                        certificate.SellerZipCode = model.CertificateExemptionInstructions.SellerZipCode;
                        certificate.Signature = model.CertificateExemptionInstructions.Signature;
                        certificate.StateIssue = model.CertificateExemptionInstructions.StateIssue;
                        certificate.TermsCondition = model.CertificateExemptionInstructions.TermsCondition;
                        certificate.CustomerId = getcustomer.Id;
                        certificate.FeinCountry = model.CertificateExemptionInstructions.FeinCountry;
                        db.CertificateExemptionInstructions.Add(certificate);
                        await db.SaveChangesAsync();

                        var getCertificate = db.CertificateExemptionInstructions.AsQueryable().ToList().Where(x => x.CustomerId == getcustomer.Id).FirstOrDefault();
                        if (getCertificate != null)
                        {


                            CertificateIdentification identification = null;
                            for (int a = 0; a < model.CertificateIdentifications.Count(); a++)
                            {
                                identification = new CertificateIdentification();
                                identification.CertificateId = getCertificate.Ceiid;
                                identification.CustomerId = getcustomer.Id;
                                identification.IdentificationNumber = model.CertificateIdentifications[a].IdentificationNumber;
                                identification.ReasonExamption = model.CertificateIdentifications[a].ReasonExamption;
                                db.CertificateIdentifications.Add(identification);
                                await db.SaveChangesAsync();
                            }


                        }


                        //var getrole = db.AspNetRoles.ToList().Where(x => x.Name == "Customer").FirstOrDefault();
                        //if (getrole != null)
                        //{
                        //    AspNetUser makeuser = new AspNetUser();
                        //    makeuser.Email = getcustomer.Email;
                        //    makeuser.PasswordHash = encdec.Encrypt(getcustomer.Email);
                        //    makeuser.Deleted = false;
                        //    makeuser.AdminApproval = false;
                        //    makeuser.Address = getcustomer.Address;
                        //    makeuser.City = getcustomer.City;
                        //    makeuser.CreatedDate = DateTime.Now;
                        //    makeuser.DrivingLicense = getcustomer.DrivingLicense;
                        //    makeuser.EmployeeId = null;
                        //    makeuser.Firstname = getcustomer.FullName;
                        //    makeuser.FromScreen = "E-Commerce";
                        //    makeuser.IsActive = false;
                        //    makeuser.IsCancelled = false;
                        //    makeuser.Firstname = getcustomer.FullName;
                        //    makeuser.Lastname = "";
                        //    makeuser.Mobile = getcustomer.Mobile;
                        //    makeuser.PhoneNumber = getcustomer.Phone;
                        //    makeuser.RoleId = getrole.Id;
                        //    //makeuser.State = getcustomer.CutomerState;
                        //    makeuser.PhoneNumber = getcustomer.Phone;
                        //    makeuser.UserName = getcustomer.Email;
                        //    makeuser.ZipCode = getcustomer.PostalCode;

                        //    db.AspNetUsers.Add(makeuser);
                        //    await db.SaveChangesAsync();



                        //}
                        //else
                        //{
                        //    AspNetRole makerole = new AspNetRole();
                        //    makerole.Name = "Customer";
                        //    db.AspNetRoles.Add(makerole);
                        //    await db.SaveChangesAsync();

                        //    var getnewrole = db.AspNetRoles.AsQueryable().ToList().Where(x => x.Name == "Customer").FirstOrDefault();
                        //    if (getnewrole != null)
                        //    {
                        //        AspNetUser makeuser = new AspNetUser();
                        //        makeuser.Email = getcustomer.Email;
                        //        makeuser.PasswordHash = encdec.Encrypt(getcustomer.Email);
                        //        makeuser.Deleted = false;
                        //        makeuser.AdminApproval = false;
                        //        makeuser.Address = getcustomer.Address;
                        //        makeuser.City = getcustomer.City;
                        //        makeuser.CreatedDate = DateTime.Now;
                        //        makeuser.DrivingLicense = getcustomer.DrivingLicense;
                        //        makeuser.EmployeeId = null;
                        //        makeuser.Firstname = getcustomer.FullName;
                        //        makeuser.FromScreen = "E-Commerce";
                        //        makeuser.IsActive = false;
                        //        makeuser.IsCancelled = false;
                        //        makeuser.Firstname = getcustomer.FullName;
                        //        makeuser.Lastname = "";
                        //        makeuser.Mobile = getcustomer.Mobile;
                        //        makeuser.PhoneNumber = getcustomer.Phone;
                        //        makeuser.RoleId = getnewrole.Id;
                        //        //  makeuser.State = getcustomer.CutomerState;
                        //        makeuser.PhoneNumber = getcustomer.Phone;
                        //        makeuser.UserName = getcustomer.Email;
                        //        makeuser.ZipCode = getcustomer.PostalCode;


                        //        db.AspNetUsers.Add(makeuser);
                        //        await db.SaveChangesAsync();

                        //    }
                        //}
                    }


                    MailRequest request = new MailRequest();
                    request.ToEmail = getcustomer.Email;
                    request.Subject = "Welcome to ABCDiscounts";
                    var usermsg = "<div></h3>Dear Customer, Request for approval of your registration has received by admin. You will receive an confirmation email soon. For Login to system please use your email as username & Password.</h3><div>";
                    request.Body = usermsg;
                    var emailresponse = await Send(request);
                    if (emailresponse == "Email Sent Successfully")
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                        return Ok(Response);
                        // return CreatedAtAction("CustomersGetByID", new { id = getcustomer.CustomerId }, "Customer Profile created thank you, please check your email");
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.Success_WithOutEmail, null, null);
                        return Ok(Response);
                        //  return CreatedAtAction("CustomersGetByID", new { id = getcustomer.CustomerId }, "Customer Profile created thank you,For Login to system please use your email as username & Password.");
                    }
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ABC.EFCore.Repository.Edmx.Customer>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("CustomersGetByLoginUserID/{id}")]
        public IActionResult CustomersGetByLoginUserID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ABC.EFCore.Repository.Edmx.CustomerInformation>();
                var Getallrecord = db.CustomerInformations.ToList();
                if (Getallrecord.Count() > 0)
                {
                    var getallASPUsers = db.AspNetUsers.ToList();
                    for (int i = 0; i < Getallrecord.Count(); i++)
                    {
                        foreach (var item in getallASPUsers.ToList().Where(x => x.Email == Getallrecord[i].Email).ToList())
                        {
                            Getallrecord[i].AspNetUser = item;
                        }
                    }
                }
                var record = Getallrecord.ToList().Where(x => x.AspNetUser.Id == Convert.ToInt32(id)).FirstOrDefault();
                //var Getallrecord = db.Customers.ToList().Where(x=>x.user)
                if (record != null)
                {
                    if (record.AspNetUser == null)
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.Customer_Profile_Not_Found, null, null);
                        return Ok(Response);
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

                        //var getuser = db.AspNetUsers.ToList().Where(x => x.Email == record.Email).FirstOrDefault();
                        //if (getuser != null)
                        //{
                        //    record.AspNetUser = getuser;
                        //}
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ABC.EFCore.Repository.Edmx.Customer>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CustomersGetByID/{id}")]
        public IActionResult CustomersGetByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ABC.EFCore.Repository.Edmx.CustomerInformation>();
                var record = db.CustomerInformations.Find(id);

                if (record != null)
                {
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
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ABC.EFCore.Repository.Edmx.Customer>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CustomersblacklistByID/{accountid}")]
        public IActionResult CustomersblacklistByID(int accountid)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ABC.EFCore.Repository.Edmx.Customer>();
                var record = db.Customers.Find(accountid);

                if (record != null)
                {
                    record.Status = false;

                    db.Entry(record).State = EntityState.Modified;
                    db.SaveChanges();

                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ABC.EFCore.Repository.Edmx.Customer>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getcustomerdata/{accountid}")]
        public IActionResult getcustomerdata(int accountid)
        {
            var Response = ResponseBuilder.BuildWSResponse<List<ABC.EFCore.Repository.Edmx.Customer>>();

            var record = db.Customers.ToList().Where(x => x.CustomerId == accountid).ToList();

            if (record != null)
            {

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);

            }
            else
            {

                ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                return Ok(Response);
            }

        }


        [HttpGet("GetUserOrderFromCart/{id}")]
        public IActionResult GetUserOrderFromCart(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerOrder>>();
                var record = db.CustomerOrders.Where(x => x.UserId == id).ToList();
                if (record.Count() > 0 && record != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record.OrderByDescending(x=>x.TicketId).ToList());
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
        [HttpGet("GetOrderDetailFromCart/{id}")]
        public IActionResult GetOrderDetailFromCart(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerOrder>>();
                var record = db.CustomerOrders.Where(x => x.OrderId == id).ToList();
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

        [HttpGet("GetUserCartByAdminApproval/{ticketId}")]
        public IActionResult GetUserCartByAdminApproval(string ticketId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CartDetail>>();
                // var record = db.CartDetails.Where(x => x.UserId == id && x.PendingForApproval == true && x.IsDelivered == false).ToList();
                var record = db.CartDetails.Where(x => x.TicketId == ticketId).ToList();
                if (record != null)
                {

                    double Invtotals = 0; 
                    for (int i = 0; i < record.Count(); i++)
                    {
                        var aa = Convert.ToDouble(record[i].Total);
                        aa = Convert.ToDouble(record[i].Quantity) * Convert.ToDouble(record[i].Retail);
                        //Invtotals += Convert.ToDouble(record[i].Total);
                        Invtotals += aa;
                    }
                    record[0].InvTotal = Math.Round(Invtotals , 2).ToString();
                    var customerorderData = db.CustomerOrders.Where(x => x.TicketId == record[0].TicketId).ToList();
                    record[0].CustomerOrders = customerorderData;
                    var CurrentCustomer = db.CustomerInformations.ToList().Where(x => x.UserId == record[0].UserId).FirstOrDefault();
                    if (CurrentCustomer != null)
                    {

                        var billingData = db.CustomerBillingInfos.Where(x => x.CustomerInformationId == CurrentCustomer.Id && x.CustomerCode == CurrentCustomer.CustomerCode).FirstOrDefault();
                        var getpay = db.Receivables.ToList().Where(x => x.AccountId == CurrentCustomer.AccountId).FirstOrDefault();

                        if (billingData != null)
                        {
                            CurrentCustomer.CustomerBillingInfo = billingData;
                        }
                        if (getpay != null)
                        {
                            if (CurrentCustomer.CustomerBillingInfo != null)
                            {
                                if (Convert.ToDouble(CurrentCustomer.CustomerBillingInfo.CreditLimit) > Convert.ToDouble(getpay.Amount))
                                {
                                    record[0].Balance = "0";
                                }
                                else
                                {
                                    record[0].Balance = getpay.Amount;
                                }
                            }
                            else
                            {
                                record[0].Balance = "0";
                            }

                        }
                        else
                        {
                            record[0].Balance = "0";
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
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCustomerinfoo/{id}")]
        public IActionResult GetCustomerinfoo(int id)
        {
            try
            {
                var record = db.CustomerInformations.ToList().Where(x => x.UserId == id).FirstOrDefault();
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
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

        [HttpGet("GenerateCartInvoiceNo")]
        public IActionResult GenerateCartInvoiceNo()
        {
            try
            {
                CurrentInvoice invoiceData = new CurrentInvoice();
                var Response = ResponseBuilder.BuildWSResponse<CurrentInvoice>();
                var fullcode = "";
                var Cartfullcode = "";

                var CurrentgetCart = db.CartDetails.Where(x => x.PendingForApproval == false).ToList();
                var recordemp = db.CustomerOrders.ToList();
                if (CurrentgetCart != null && CurrentgetCart.Count() > 0)
                {
                    if (CurrentgetCart != null && CurrentgetCart.Count() > 0)
                    {
                        if (CurrentgetCart[0].TicketId != null && CurrentgetCart[0].TicketId != "string" && CurrentgetCart[0].TicketId != "")
                        {
                            int large, small;
                            int salesID = 0;
                            large = Convert.ToInt32(CurrentgetCart[0].TicketId.Split('-')[1]);
                            small = Convert.ToInt32(CurrentgetCart[0].TicketId.Split('-')[1]);
                            for (int i = 0; i < CurrentgetCart.Count(); i++)
                            {
                                if (CurrentgetCart[i].TicketId != null)
                                {
                                    var t = Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]);
                                    if (Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]) > large)
                                    {
                                        salesID = Convert.ToInt32(CurrentgetCart[i].CartId);
                                        large = Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]);
                                    }
                                    else if (Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]) < small)
                                    {
                                        small = Convert.ToInt32(CurrentgetCart[i].TicketId.Split('-')[1]);
                                    }
                                    else
                                    {
                                       
                                            salesID = Convert.ToInt32(CurrentgetCart[i].CartId);
                                        
                                    }
                                }
                            }
                            var Cartnewitems = CurrentgetCart.ToList().Where(x => x.CartId == salesID).FirstOrDefault();
                            if (Cartnewitems != null)
                            {
                                if (Cartnewitems.TicketId != null)
                                {


                                    var VcodeSplit = Cartnewitems.TicketId.Split('-');
                                    int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                    long checknumber = Convert.ToInt64(VcodeSplit[1]);
                                    if (checknumber > 9)
                                    {
                                        Cartfullcode = "0000000-00" + Convert.ToString(code);
                                    }
                                    else if (checknumber > 99)
                                    {
                                        Cartfullcode = "0000000-0" + Convert.ToString(code);
                                    }
                                    else if (checknumber > 999)
                                    {
                                        Cartfullcode = "0000000-" + Convert.ToString(code);
                                    }
                                    else if (checknumber > 9999)
                                    {
                                        //10000
                                        long ndcode = Convert.ToInt64(VcodeSplit[0]) + 1;

                                        Cartfullcode = "000000" + Convert.ToString(ndcode) + "-" + "9999";
                                    }
                                    else
                                    {
                                        Cartfullcode = "0000000-000" + Convert.ToString(code);
                                    }
                                    //fullcode = "0000000" + "-" + Convert.ToString(code);
                                }
                                else
                                {
                                    Cartfullcode = "0000000" + "-" + "0001";
                                }
                            }
                            else
                            {
                                Cartfullcode = "0000000" + "-" + "0001";
                            }
                        }
                        else
                        {
                            Cartfullcode = "0000000" + "-" + "0001";
                        }
                    }
                    else
                    {
                        Cartfullcode = "0000000" + "-" + "0001";
                    }
                    invoiceData.InvoiceNo = Cartfullcode;
                  
                }
                else if (recordemp != null && recordemp.Count() > 0)
                {
                    if (recordemp[0].TicketId != null && recordemp[0].TicketId != "string" && recordemp[0].TicketId != "")
                    {
                        int large, small;
                        int salesID = 0;
                        large = Convert.ToInt32(recordemp[0].TicketId.Split('-')[1]);
                        small = Convert.ToInt32(recordemp[0].TicketId.Split('-')[1]);
                        for (int i = 0; i < recordemp.Count(); i++)
                        {
                            if (recordemp[i].TicketId != null)
                            {
                                var t = Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]);
                                if (Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]) > large)
                                {
                                    salesID = Convert.ToInt32(recordemp[i].OrderId);
                                    large = Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]);
                                }
                                else if (Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]) < small)
                                {
                                    small = Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]);
                                }
                                else
                                {
                                    if (large <= 2)
                                    {
                                        salesID = Convert.ToInt32(recordemp[i].OrderId);
                                    }
                                }
                            }
                        }
                        var newitems = recordemp.ToList().Where(x => x.OrderId == salesID).FirstOrDefault();
                        if (newitems != null)
                        {
                            if (newitems.TicketId != null)
                            {


                                var VcodeSplit = newitems.TicketId.Split('-');
                                int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                long checknumber = Convert.ToInt64(VcodeSplit[1]);
                                if (checknumber > 9)
                                {
                                    fullcode = "0000000-00" + Convert.ToString(code);
                                }
                                else if (checknumber > 99)
                                {
                                    fullcode = "0000000-0" + Convert.ToString(code);
                                }
                                else if (checknumber > 999)
                                {
                                    fullcode = "0000000-" + Convert.ToString(code);
                                }
                                else if (checknumber > 9999)
                                {
                                    //10000
                                    long ndcode = Convert.ToInt64(VcodeSplit[0]) + 1;

                                    fullcode = "000000" + Convert.ToString(ndcode) + "-" + "9999";
                                }
                                else
                                {
                                    fullcode = "0000000-000" + Convert.ToString(code);
                                }
                                //fullcode = "0000000" + "-" + Convert.ToString(code);
                            }
                            else
                            {
                                fullcode = "0000000" + "-" + "0001";
                            }
                        }
                        else
                        {
                            fullcode = "0000000" + "-" + "0001";
                        }
                    }
                    else
                    {
                        fullcode = "0000000" + "-" + "0001";
                    }
                    invoiceData.InvoiceNo = fullcode;

                }
                else
                {
                    fullcode = "0000000" + "-" + "0001";
                    invoiceData.InvoiceNo = fullcode;
                }


                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, invoiceData);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<string>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AddQuantityInCart")]
        public IActionResult AddQuantityInCart(int id, string ticketId,string quantity)
        {
            try
            {
                //if (!ticketId.Contains("00-"))
                //{
                //    ticketId = "00" + ticketId;
                //}  
                
                if (!quantity.Contains("."))
                {
                    quantity = quantity.Split('.')[0];
                }

                var Response = ResponseBuilder.BuildWSResponse<CartDetail>();
                var record = db.CartDetails.Where(x => x.Id == id && x.TicketId == ticketId)?.FirstOrDefault();
                if (record != null)
                {
                    record.Quantity = Convert.ToInt32(quantity);
                    db.SaveChanges();
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
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetUserCartItems")]
        public async Task<IActionResult> GetUserCartItems(int id, string ticketId)
        {
            try
            {

                var record = db.CartDetails.ToList().Where(x => x.UserId == id && x.TicketId == ticketId).ToList();
                var Response = ResponseBuilder.BuildWSResponse<CartDetail>();
                var fullcode = "";
                var recordemp = db.CustomerOrders.ToList();
                if (recordemp != null && recordemp.Count() > 0)
                {
                    if (recordemp[0].TicketId != null && recordemp[0].TicketId != "string" && recordemp[0].TicketId != "")
                    {
                        int large, small;
                        int salesID = 0;
                        large = Convert.ToInt32(recordemp[0].TicketId.Split('-')[1]);
                        small = Convert.ToInt32(recordemp[0].TicketId.Split('-')[1]);
                        for (int i = 0; i < recordemp.Count(); i++)
                        {
                            if (recordemp[i].TicketId != null)
                            {
                                var t = Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]);
                                if (Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]) > large)
                                {
                                    salesID = Convert.ToInt32(recordemp[i].OrderId);
                                    large = Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]);
                                }
                                else if (Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]) < small)
                                {
                                    small = Convert.ToInt32(recordemp[i].TicketId.Split('-')[1]);
                                }
                                else
                                {
                                    if (large < 2)
                                    {
                                        salesID = Convert.ToInt32(recordemp[i].OrderId);
                                    }
                                }
                            }
                        }
                        var newitems = recordemp.ToList().Where(x => x.OrderId == salesID).FirstOrDefault();
                        if (newitems != null)
                        {
                            if (newitems.TicketId != null)
                            {
                                var VcodeSplit = newitems.TicketId.Split('-');
                                int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                if (Convert.ToDouble(VcodeSplit[1]) > 9)
                                {
                                    fullcode = "0000000-00" + Convert.ToString(code);
                                }
                                else if (Convert.ToDouble(VcodeSplit[1]) > 99)
                                {
                                    fullcode = "0000000-0" + Convert.ToString(code);
                                }
                                else if (Convert.ToDouble(VcodeSplit[1]) > 999)
                                {
                                    fullcode = "0000000-" + Convert.ToString(code);
                                }
                                else if (Convert.ToDouble(VcodeSplit[1]) > 9999)
                                {
                                    //10000
                                    long ndcode = Convert.ToInt64(VcodeSplit[0]) + 1;

                                    fullcode = "000000" + Convert.ToString(ndcode) + "-" + "9999";
                                }
                                else
                                {
                                    fullcode = "0000000-000" + Convert.ToString(code);
                                }
                                //fullcode = "0000000" + "-" + Convert.ToString(code);
                            }
                            else
                            {
                                fullcode = "0000000" + "-" + "0001";
                            }
                        }
                        else
                        {
                            fullcode = "0000000" + "-" + "0001";
                        }
                    }
                    else
                    {
                        fullcode = "0000000" + "-" + "0001";
                    }
                }
                else
                {
                    fullcode = "0000000" + "-" + "0001";
                }
                var checkproduct = db.CartDetails.Where(x => x.PendingForApproval == false && x.TicketId == fullcode).ToList();
                if (checkproduct.Count() > 0)
                {
                    for (int i = 0; i < checkproduct.Count(); i++)
                    {
                        db.CartDetails.Remove(checkproduct[i]);
                        await db.SaveChangesAsync();
                    }
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (record.Count() > 0 && record != null)
                {
                    foreach (var item in record)
                    {
                        CartDetail cartDetail = null;
                        cartDetail = new CartDetail();
                        cartDetail.TicketId = fullcode;
                        if (item.Quantity == null)
                        {
                            cartDetail.Quantity = 1;
                        }
                        else
                        {
                            cartDetail.Quantity = Convert.ToInt32(item.Quantity);
                        }
                        var getStock = db.InventoryStocks.ToList().Where(x => x.ProductId == item.Id)?.FirstOrDefault();
                        if (getStock != null)
                        {
                            //var checkproduct = db.CartDetails.Where(x => x.Id == item.Id && x.PendingForApproval == false && x.TicketId == item.TicketId).FirstOrDefault();
                            //if (checkproduct != null)
                            //{
                            //    cartDetail.Quantity = item.Quantity + checkproduct.Quantity;
                            //}
                            //if (int.Parse(getStock.Quantity) >= cartDetail.Quantity)
                            //{
                                //if (checkproduct != null)
                                //{
                                //    checkproduct.Quantity = item.Quantity;
                                //    //await db.SaveChangesAsync();
                                //    //ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                                //    //return Ok(Response);
                                //}

                                var tax = 0.0;
                                var productSt = db.Sttaxes.Where(x => x.ProductId == item.Id)?.FirstOrDefault();
                                if (productSt != null)
                                {
                                    tax = ((double)(tax + (double.Parse(productSt.Tax) * cartDetail.Quantity)));
                                    cartDetail.Tax = productSt.Tax;
                                }
                                var retailProduct = db.Products.Where(x => x.Id == item.Id)?.FirstOrDefault();
                                if (retailProduct != null)
                                {
                                    cartDetail.Name = retailProduct.Name;
                                    cartDetail.ProductCode = retailProduct.ProductCode;
                                    cartDetail.Retail = retailProduct.Retail.ToString();
                                    cartDetail.Total = (cartDetail.Quantity * decimal.Parse(item.Retail)).ToString();
                                    cartDetail.Id = item.Id;
                                }
                                cartDetail.TotalTaxes = tax.ToString();
                                cartDetail.PendingForApproval = false;
                                cartDetail.IsDelivered = false;
                                cartDetail.UserId = id;
                                cartDetail.ImagePath = item.ImagePath;
                                db.CartDetails.Add(cartDetail);
                                await db.SaveChangesAsync();
                            //}
                            //else
                            //{
                            //    cartDetail.Quantity = Convert.ToInt32(getStock.Quantity);
                            //}
                        }
                        else
                        {
                            var tax = 0.0;
                            var productSt = db.Sttaxes.Where(x => x.ProductId == item.Id)?.FirstOrDefault();
                            if (productSt != null)
                            {
                                tax = ((double)(tax + (double.Parse(productSt.Tax) * cartDetail.Quantity)));
                                cartDetail.Tax = "0";
                            }
                            var retailProduct = db.Products.Where(x => x.Id == item.Id)?.FirstOrDefault();
                            if (retailProduct != null)
                            {
                                cartDetail.Name = retailProduct.Name;
                                cartDetail.ProductCode = retailProduct.ProductCode;
                                cartDetail.Retail = retailProduct.Retail.ToString();
                                cartDetail.Total = (cartDetail.Quantity * decimal.Parse(item.Retail)).ToString();
                                cartDetail.Id = item.Id;
                            }
                            cartDetail.TotalTaxes = tax.ToString();
                            cartDetail.PendingForApproval = false;
                            cartDetail.IsDelivered = false;
                            cartDetail.UserId = id;
                            cartDetail.Quantity = 0;
                            db.CartDetails.Add(cartDetail);
                            await db.SaveChangesAsync();
                        }


                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    return Ok(Response);
                    //ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);

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
                    var Response = ResponseBuilder.BuildWSResponse<CartDetail>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("GetItemByBarcode")]
        public IActionResult GetItemByBarcode(string barcode)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                var record = db.Products.ToList().Where(x => x.BarCode == barcode).FirstOrDefault();
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
                    var Response = ResponseBuilder.BuildWSResponse<Product>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }






        ///////////////// replace this /////////

        [HttpPost("UpdateCustomerinfoo/{id}")]
        public IActionResult UpdateCustomerinfoo(int id, CustomerInformation obj)
        {
            try
            {
                var record = db.CustomerInformations.ToList().Where(x => x.UserId == id).FirstOrDefault();
                var recorddata = db.AspNetUsers.ToList().Where(x => x.Id == id).FirstOrDefault();
                if (record != null)
                {
                    if (obj.FirstName != null && obj.FirstName != "")
                    {
                        record.FirstName = obj.FirstName;
                    }
                    if (obj.LastName != null && obj.LastName != "")
                    {
                        record.LastName = obj.LastName;
                    }
                    if (obj.Cell != null && obj.Cell != "")
                    {
                        record.Cell = obj.Cell;
                    }
                    if (obj.State != null && obj.State != "")
                    {
                        record.State = obj.State;
                    }
                    if (obj.City != null && obj.City != "")
                    {
                        record.City = obj.City;
                    }
                    if (obj.Street != null && obj.Street != "")
                    {
                        record.Street = obj.Street;
                    }
                    if (obj.OwnerAddress != null && obj.OwnerAddress != "")
                    {
                        record.OwnerAddress = obj.OwnerAddress;
                    }
                    if (obj.FullName != null && obj.FullName != "")
                    {
                        record.FullName = obj.FullName;
                    }
                    if (obj.Phone != null && obj.Phone != "")
                    {
                        record.Phone = obj.Phone;
                    }
                    if (obj.Country != null && obj.Country != "")
                    {
                        record.Country = obj.Country;
                    }
                    if (obj.DrivingLicenseNumber != null && obj.DrivingLicenseNumber != "")
                    {
                        record.DrivingLicenseNumber = obj.DrivingLicenseNumber;
                    }
                    db.Entry(record).State = EntityState.Modified;
                    db.SaveChangesAsync();

                }

                if (recorddata != null)
                {
                    if (obj.FirstName != null && obj.FirstName != "")
                    {
                        recorddata.Firstname = obj.FirstName;
                    }
                    if (obj.LastName != null && obj.LastName != "")
                    {
                        recorddata.Lastname = obj.LastName;
                    }
                    if (obj.Cell != null && obj.Cell != "")
                    {
                        recorddata.PhoneNumber = obj.Cell;
                    }
                    if (obj.State != null && obj.State != "")
                    {
                        recorddata.State = obj.State;
                    }
                    if (obj.City != null && obj.City != "")
                    {
                        recorddata.City = obj.City;
                    }
                    if (obj.Street != null && obj.Street != "")
                    {
                        recorddata.Address = obj.Street;
                    }
                    if (obj.DrivingLicenseNumber != null && obj.DrivingLicenseNumber != "")
                    {
                        recorddata.DrivingLicense = obj.DrivingLicenseNumber;
                    }
                    db.Entry(record).State = EntityState.Modified;
                    db.SaveChangesAsync();

                }
                // return Ok("Profile Updated successfully");
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
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


        [HttpPost("AuthorizeQuantityInCart")]
        public async Task<IActionResult> AuthorizeQuantityInCart(AutherizeOrderLimit model)
        {
            try
            {
                //if (!model.TicketId.Contains("00-"))
                //{
                //    model.TicketId = "00" + model.TicketId;
                //}
                var productData = db.Products.ToList().Where(x => x.Id == model.ProductId).FirstOrDefault();
                if (productData != null)
                {
                    model.ProductName = productData.Name;
                    model.ProductCode = productData.ProductCode;
                }
                var customerData = db.CustomerInformations.ToList().Where(x => x.UserId == model.UserId).FirstOrDefault();
                if (customerData != null)
                {
                    model.CustomerId = customerData.Id;

                }
                var dateAndTime = DateTime.Now;
                var time = DateTime.Now.ToString("hh:mm tt");
                model.AccessTime = time;
                model.AccessDate = dateAndTime;
                var Response = ResponseBuilder.BuildWSResponse<AutherizeOrderLimit>();
                db.AutherizeOrderLimits.Add(model);
                await db.SaveChangesAsync();
                return Ok(Response);


            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<AutherizeOrderLimit>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("EmptyCurrentCart")]
        public async Task<IActionResult> EmptyCurrentCart(string ticketId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CartDetail>();
                var record = db.CartDetails.Where(x => x.TicketId == ticketId).ToList();
                if (record.Count() > 0)
                {
                    for (int i = 0; i < record.Count(); i++)
                    {
                        db.CartDetails.Remove(record[i]);
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
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCurrentOrderByTicketId/{TicketId}")]
        public IActionResult GetCurrentOrderByTicketId(string TicketId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerOrder>>();
                var record = db.CustomerOrders.ToList().Where(x => x.TicketId == TicketId).ToList();
                var Getproducts = db.Products.ToList();
                if (record != null)
                {
                    var GetCart = db.CartDetails.ToList().Where(x => x.TicketId == record[0].TicketId).ToList();
                    for (int i = 0; i < record.Count(); i++)
                    {
                        foreach (var item in GetCart.ToList().Where(x=>x.Name == record[i].ProductName).ToList())
                        {
                            var ProSKU = Getproducts.Where(x => x.Name == item.Name && x.ProductCode == item.ProductCode).FirstOrDefault();
                            record[i].ProCode = item.ProductCode;
                            record[i].ProSku = ProSKU.Sku;
                            record[i].ProImg = ProSKU.ItemImageByPath;
                            record[i].Retail = item.Retail;

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
                    var Response = ResponseBuilder.BuildWSResponse<Provider>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetitemCategories")]
        public IActionResult GetitemCategories()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<ItemCategory>>();
                var record = db.ItemCategories.ToList();
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
                    var Response = ResponseBuilder.BuildWSResponse<List<ItemCategory>>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("GetproductByCatID")]
        public IActionResult GetproductByCatID(int? id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                var record = db.Products.ToList().Where(x=>x.ItemCategoryId==id).ToList();
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
                    var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserOrderFromCartPrevious/{id}")]
        public IActionResult GetUserOrderFromCartPrevious(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerOrder>>();
                var record = db.CustomerOrders.Where(x => x.UserId == id).ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList();
                if (record.Count() > 0 && record != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record.OrderByDescending(x => x.TicketId).ToList());
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

        [HttpGet("GetAllFaqs")]
        public IActionResult GetAllFaqs()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Faq>>();
                var record = db.Faqs.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Faq>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetBrandsAll")]
        public IActionResult GetBrandsAll()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Brand>>();
                var record = db.Brands.ToList().OrderBy(x=>x.Name).ToList();
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

        [HttpGet("GetProductsByBrandId")]
        public IActionResult GetProductsByBrandId(int id, string name)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                var record = db.Products.ToList().Where(x=>x.BrandId == id || x.BrandName == name).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Product>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProductsByKeyword")]
        public IActionResult GetProductsByKeyword(string name)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                var record = db.Products.Where(x => x.Name.Contains(name)).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Product>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProductsByCategory")]
        public IActionResult GetProductsByCategory(string name , int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                var record = db.Products.Where(x => x.CategoryName == name || x.ItemCategoryId == id).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Product>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProductsBySubCatName/{name}")]
        public IActionResult GetProductsBySubCatName(string name)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                var record = db.Products.Where(x => x.SubCatName== name).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Product>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
    }
}
