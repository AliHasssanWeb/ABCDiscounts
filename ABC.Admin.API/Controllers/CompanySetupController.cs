using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanySetupController : Controller
    {
        protected readonly ABCDiscountsContext db;
        EncryptDecrypt encdec = new EncryptDecrypt();
        public CompanySetupController(ABCDiscountsContext _db)
        {
            db = _db;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("EmployerCreate")]
        public async Task<IActionResult> EmployerCreate(Employer obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employer>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Employers.ToList().Exists(p => p.EmployerName.Equals(obj.EmployerName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                else
                {
                    db.Employers.Add(obj);
                    await db.SaveChangesAsync();
                    return Ok(Response);
                }

                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employer>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("EmployerGet")]
        public IActionResult EmployerGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employer>();
                var GetEmployerrecord = db.Employers.FirstOrDefault();
                var getProducts = db.Products.ToList();
                if (GetEmployerrecord != null)
                {
                    GetEmployerrecord.product = new List<Product>();
                    GetEmployerrecord.product.AddRange(getProducts);
                    var GetSttax = db.Sttaxes.ToList().Where(x => x.CompanyId == GetEmployerrecord.EmployerId).ToList();
                    for (int i = 0; i < GetEmployerrecord.product.Count(); i++)
                    {
                        GetEmployerrecord.product[i].stTax = new Sttax();
                        foreach (var item in GetSttax.ToList().Where(x => x.ProductId == GetEmployerrecord.product[i].Id).ToList())
                        {
                            GetEmployerrecord.product[i].stTax = item;
                        }
                    }
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, GetEmployerrecord);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employer>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ItemGet")]
        public IActionResult ItemGet()
        {
            try
            {
                var record = db.Products.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
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






        [HttpGet("StTaxGet")]
        public IActionResult StTaxGet()
        {
            try
            {
                var record = db.Sttaxes.ToList();
                ///////make records list here/////
                var Response = ResponseBuilder.BuildWSResponse<List<Sttax>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);

                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Sttax>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("StTaxCreate")]
        public async Task<IActionResult> StTaxCreate(Sttax obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Sttax>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Sttaxes.ToList().Exists(p => p.TaxId == obj.TaxId);
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                else
                {
                    var emplID = db.Employers.ToList();
                    db.Sttaxes.Add(obj);
                    await db.SaveChangesAsync();
                    Response.Data = obj;
                    return Ok(Response);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Sttax>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("StTaxUpdate/{id}")]
        public async Task<IActionResult> StTaxUpdate(int id, Sttax data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Sttax>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.Sttaxes.ToList().Exists(x => x.TaxId==data.TaxId);
                if (isValid)
                {
                    var record = await db.Sttaxes.FindAsync(id);
                    if (data.Tax != null && data.Tax != "undefined")
                    {
                        record.Tax = data.Tax;
                    }
                    if (data.PerQty != null && data.PerQty != "undefined")
                    {
                        record.PerQty = data.PerQty;
                    }
                    if (data.PerOz != null && data.PerQty != "undefined")
                    {
                        record.PerOz = data.PerOz;
                    }
                    if (data.PerUcount != null && data.PerUcount != "undefined")
                    {
                        record.PerUcount = data.PerUcount;
                    }
                    data = record;
                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Ok(Response);
                    //ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
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

        [HttpGet("FaqsGet")]
        public IActionResult FaqsGet()
        {
            try
            {
                var record = db.Faqs.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Faq>>();
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


        [HttpPost("AddFaqs")]
        public async Task<IActionResult> AddFaqs(Faq addfaqs)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Faq>();
                if (ModelState.IsValid)
                {
                    db.Faqs.Add(addfaqs);
                    await db.SaveChangesAsync();
                    return Ok(Response);
                }
                else
                {
                    return BadRequest();
                }
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


        [HttpGet("DeleteFaq/{id}")]
        public async Task<IActionResult> DeleteFaq(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Faq>();
                Faq data = await db.Faqs.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.Faqs.Remove(data);
                await db.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Faq>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("FaqUpdate/{id}")]

        public async Task<IActionResult> FaqUpdate(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Faq>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    return BadRequest("Id Required");
                }

                var record = await db.Faqs.FindAsync(id);
                //db.Entry(record).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                return Ok(record);
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

        [HttpPost("Faqsdataupadte")]
        public async Task<IActionResult> Faqsdataupadte(Faq addfaqs)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Faq>();
                if (ModelState.IsValid)
                {
                    var data = db.Faqs.Where(x => x.Id == addfaqs.Id).FirstOrDefault();
                    if (data != null)
                    {
                        data.Description = addfaqs.Description;
                        data.Title = addfaqs.Title;
                        data.IsPublic = addfaqs.IsPublic;
                        db.Faqs.Update(data);
                        await db.SaveChangesAsync();
                        return Ok(Response);
                    }
                    //else
                    //{

                    //}
                    return null;

                }
                else
                {
                    return BadRequest();
                }
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


        [HttpGet("GetVisitors")]
        public IActionResult GetVisitors()
        {
            try
            {
                var record = db.Contacts.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Contact>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Contact>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EmployerUpdate")]
        public IActionResult EmployerUpdate(Employer obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employer>();
                var GetEmployerrecord = db.Employers.Where(x=>x.EmployerId == obj.EmployerId).FirstOrDefault();
                if (GetEmployerrecord != null)
                {
                    if(obj.EmployerName != null)
                    {
                        GetEmployerrecord.EmployerName = obj.EmployerName;
                    }
                    if (obj.Slogan != null)
                    {
                        GetEmployerrecord.Slogan = obj.Slogan;
                    }
                    if (obj.Contact != null)
                    {
                        GetEmployerrecord.Contact = obj.Contact;
                    }
                    if (obj.Branding != null)
                    {
                        GetEmployerrecord.Branding = obj.Branding;
                    }
                    if (obj.PayrolAddress != null)
                    {
                        GetEmployerrecord.PayrolAddress = obj.PayrolAddress;
                    }
                    if (obj.Suite != null)
                    {
                        GetEmployerrecord.Suite = obj.Suite;
                        
                    }
                    if (obj.City != null)
                    {
                        GetEmployerrecord.City = obj.City;
                    }
                    if (obj.EmployerState != null)
                    {
                        GetEmployerrecord.EmployerState = obj.EmployerState;
                    }
                    if (obj.Zip != null)
                    {
                        GetEmployerrecord.Zip = obj.Zip;
                    }
                    if (obj.EmployerPhone != null)
                    {
                        GetEmployerrecord.EmployerPhone = obj.EmployerPhone;
                    }
                    if (obj.Fax != null)
                    {
                        GetEmployerrecord.Fax = obj.Fax;
                    }
                    if (obj.EmployerEmail != null)
                    {
                        GetEmployerrecord.EmployerEmail = obj.EmployerEmail;
                    }
                    if (obj.FedTaxId != null)
                    {
                        GetEmployerrecord.FedTaxId = obj.FedTaxId;
                    }
                    if (obj.StateTaxId != null)
                    {
                        GetEmployerrecord.StateTaxId = obj.StateTaxId;
                    }
                    if (obj.DefaultSalesTax != null)
                    {
                        GetEmployerrecord.DefaultSalesTax = obj.DefaultSalesTax;
                    }
                    if (obj.DefaultCreditCharge != null)
                    {
                        GetEmployerrecord.DefaultCreditCharge = obj.DefaultCreditCharge;
                    }
                    if (obj.DefaultCreditCharge != null)
                    {
                        GetEmployerrecord.DefaultCreditCharge = obj.DefaultCreditCharge;
                    }
                    if (obj.PrintCopies != null)
                    {
                        GetEmployerrecord.PrintCopies = obj.PrintCopies;
                    }
                    if (obj.PrintPreview != null)
                    {
                        GetEmployerrecord.PrintPreview = obj.PrintPreview;
                    }
                    if (obj.OptionPrint != null)
                    {
                        GetEmployerrecord.OptionPrint = obj.OptionPrint;
                    }
                    db.Employers.Update(GetEmployerrecord);
                    db.SaveChanges();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, GetEmployerrecord);
                    
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employer>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
    }
}
