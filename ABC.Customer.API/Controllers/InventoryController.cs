using ABC.EFCore.Entities.POS;
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


namespace ABC.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin, Customer")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public InventoryController(ABCDiscountsContext _db, IWebHostEnvironment webHostEnvironment)
        {
            db = _db;
            _webHostEnvironment = webHostEnvironment;

        }

        [HttpGet("ItemCategoryGet")]
        public IActionResult ItemCategoryGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<ItemCategory>>();
                var record = db.ItemCategories.ToList().OrderBy(x => x.Name).ToList();
                //   return Ok(record);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
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

        [HttpPost("ItemCategoryCreate")]
        public async Task<IActionResult> ItemCategoryCreate(ItemCategory users)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ItemCategory>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.ItemCategories.ToList().Exists(p => p.Name.Equals(users.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                db.ItemCategories.Add(users);
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
        [HttpPut("ItemCategoryUpdate/{id}")]
        public async Task<IActionResult> ItemCategoryUpdate(int id, ItemCategory data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ItemCategory>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                bool isValid = db.ItemCategories.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                var record = await db.ItemCategories.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<ItemCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ItemCategoryUpdateByID/{id}")]
        public IActionResult ItemCategoryUpdateByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ItemCategory>();

                var record = db.ItemCategories.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<ItemCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteItemCategory/{id}")]
        public async Task<IActionResult> DeleteItemCategory(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ItemCategory>();
                ItemCategory data = await db.ItemCategories.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.ItemCategories.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
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

        [HttpGet("ItemCategoryByID/{id}")]
        public IActionResult ItemCategoryByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ItemCategory>();

                var record = db.ItemCategories.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<ItemCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //ItemCategoryEnd


        //ItemCategory Sub Start
        [HttpGet("ItemSubCategoryGet")]
        public IActionResult ItemSubCategoryGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<ItemSubCategory>>();
                var record = db.ItemSubCategories.ToList().OrderBy(x => x.SubCategory).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ItemSubCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ItemSubCategoryGetByCategoryID/{id}")]
        public IActionResult ItemSubCategoryGetByCategoryID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<ItemSubCategory>>();
                var record = db.ItemSubCategories.ToList().Where(x=>x.CategoryId == id).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ItemSubCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ItemSubCategoryCreate")]
        public async Task<IActionResult> ItemSubCategoryCreate(ItemSubCategory users)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ItemSubCategory>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.ItemSubCategories.ToList().Exists(p => p.SubCategory.Equals(users.SubCategory, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                db.ItemSubCategories.Add(users);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ItemSubCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ItemSubCategoryUpdate/{id}")]
        public async Task<IActionResult> ItemSubCategoryUpdate(int id, ItemSubCategory data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ItemSubCategory>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                bool isValid = db.ItemSubCategories.ToList().Exists(x => x.SubCategory.Equals(data.SubCategory, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                var record = await db.ItemSubCategories.FindAsync(id);
                if (data.SubCategory != null && data.SubCategory != "undefined")
                {
                    record.SubCategory = data.SubCategory;
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
                    var Response = ResponseBuilder.BuildWSResponse<ItemSubCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ItemSubCategoryUpdateByID/{id}")]
        public IActionResult ItemSubCategoryUpdateByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ItemSubCategory>();

                var record = db.ItemSubCategories.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<ItemSubCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteSubItemCategory/{id}")]
        public async Task<IActionResult> DeleteSubItemCategory(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ItemSubCategory>();
                ItemSubCategory data = await db.ItemSubCategories.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.ItemSubCategories.Remove(data);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ItemSubCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //ItemCategoryEnd

        //Store Start

        [HttpGet("StoreGet")]
        public IActionResult StoreGet()
        {
            try
            {
                var record = db.Stores.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Store>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Store>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("StoreCreate")]
        public async Task<IActionResult> StoreCreate(Store obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Store>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }


                bool isValid = db.Stores.ToList().Exists(p => p.StoreName.Equals(obj.StoreName, StringComparison.CurrentCultureIgnoreCase));
                if (isValid)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }

                db.Stores.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Store>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("StoreUpdate/{id}")]
        public async Task<IActionResult> StoreUpdate(int id, Store data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Store>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                bool isValid = db.Stores.ToList().Exists(x => x.StoreName.Equals(data.StoreName, StringComparison.CurrentCultureIgnoreCase));
                if (isValid)
                {
                    return BadRequest("Store Already Exists");

                }

                else
                {
                    var record = await db.Stores.FindAsync(id);
                    if (data.StoreName != null && data.StoreName != "undefined")
                    {
                        record.StoreName = data.StoreName;
                    }
                    data = record;
                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Ok(Response);
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Store>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteStore/{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Store>();
                Store data = await db.Stores.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Stores.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Store>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("StoreGetByID/{id}")]
        public IActionResult StoreGetByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Store>();

                var record = db.Stores.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Store>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //Store End


        //Vendor Start

        [HttpGet("VendorGet")]
        public IActionResult VendorGet()
        {
            try
            {
                var record = db.Vendors.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Vendor>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("VendorCreate")]
        public async Task<IActionResult> VendorCreate(Vendor obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (obj.AccountId != null && obj.AccountId != "undefined")
                {
                    var actrecord = db.Accounts.Find(obj.AccountId);
                    obj.AccountNumber = actrecord.AccountId;
                    obj.AccountTitle = actrecord.Title;
                }
                else
                {

                    return Ok("AccountId required");
                }
                bool isValid = db.Vendors.ToList().Exists(p => p.Email.Equals(obj.Email, StringComparison.CurrentCultureIgnoreCase));
                if (isValid)

                {
                    return BadRequest("Vendor Already Exists");
                }
                else
                {


                    if (obj.ProfileImage != null)
                    {
                        //  return BadRequest("Image Path Required Instead of Byte");
                        var imgPath = SaveImage(obj.ProfileImage, Guid.NewGuid().ToString());
                        obj.ImageByPath = imgPath;
                        obj.ProfileImage = null;

                    }

                    db.Vendors.Add(obj);
                    await db.SaveChangesAsync();
                    return Ok(Response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("VendorUpdate/{id}")]
        public async Task<IActionResult> VendorUpdate(int id, Vendor data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != data.VendorId)
                {
                    return BadRequest();
                }

                bool isValid = db.Vendors.ToList().Exists(x => x.FullName.Equals(data.FullName, StringComparison.CurrentCultureIgnoreCase) && x.VendorId != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);

                }

                else
                {
                    var record = await db.Vendors.FindAsync(id);
                    if (data.FullName != null && data.FullName != "undefined")
                    {
                        record.FullName = data.FullName;
                    }
                    if (data.Address != null && data.Address != "undefined")
                    {
                        record.Address = data.Address;
                    }
                    if (data.Discount != null && data.Discount != "undefined")
                    {
                        record.Discount = data.Discount;
                    }
                    if (data.DrivingLicense != null && data.DrivingLicense != "undefined")
                    {
                        record.DrivingLicense = data.DrivingLicense;
                    }
                    if (data.DrivingLicenseState != null && data.DrivingLicenseState != "undefined")
                    {
                        record.DrivingLicenseState = data.DrivingLicenseState;
                    }
                    if (data.Email != null && data.Email != "undefined")
                    {
                        record.Email = data.Email;
                    }
                    if (data.FullName != null && data.FullName != "undefined")
                    {
                        record.FullName = data.FullName;
                    }
                    if (data.ImageByPath != null && data.ImageByPath != "undefined")
                    {
                        record.ImageByPath = data.ImageByPath;
                    }
                    if (data.Irs != null && data.Irs != "undefined")
                    {
                        record.Irs = data.Irs;
                    }
                    if (data.Mobile != null && data.Mobile != "undefined")
                    {
                        record.Mobile = data.Mobile;
                    }
                    if (data.Phone != null && data.Phone != "undefined")
                    {
                        record.Phone = data.Phone;
                    }
                    if (data.StateId != null)
                    {
                        record.StateId = data.StateId;
                    }
                    if (data.TaxExempt != null)
                    {
                        record.TaxExempt = data.TaxExempt;
                    }
                    if (data.TaxId != null)
                    {
                        record.TaxId = data.TaxId;
                    }
                    if (data.StateName != null && data.StateName != "undefined")
                    {
                        record.StateName = data.StateName;
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
                    var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteVendor/{id}")]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                Vendor data = await db.Vendors.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Vendors.Remove(data);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("VendorGetByID/{id}")]
        public IActionResult VendorGetByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Vendor>();

                var record = db.Vendors.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //Vendor End


        ////Item Start

        //[HttpGet("ItemGet")]
        //public IActionResult ItemGet()
        //{
        //    try
        //    {
        //        var record = db.Products.ToList();
        //        var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
        //        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
        //        return Ok(Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<Product>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost("ItemCreate")]
        //public async Task<IActionResult> ItemCreate(Product obj)
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<Product>();
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest();
        //        }

        //        bool checkname = db.Products.ToList().Exists(p => p.Name.Equals(obj.Name, StringComparison.CurrentCultureIgnoreCase));
        //        if (checkname)

        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
        //        }
        //        else
        //        {

        //            if (obj.ItemImage != null)
        //            {
        //                //  return BadRequest("Image Path Required Instead of Byte");
        //                var imgPath = SaveImage(obj.ItemImage, Guid.NewGuid().ToString());
        //                obj.ItemImageByPath = imgPath;
        //                obj.ItemImage = null;
        //            }

        //            var getcategory = db.ItemCategories.Find(obj.ItemCategoryId);
        //            if (getcategory != null)
        //            {
        //                obj.CategoryName = getcategory.Name;
        //            }
        //            var getsubcat = db.ItemSubCategories.Find(obj.ItemSubCategoryId);
        //            if (getsubcat != null)
        //            {
        //                obj.SubCatName = getsubcat.SubCategory;
        //            }
        //            var getmodel = db.Models.Find(obj.ModelId);
        //            if (getmodel != null)
        //            {
        //                obj.ModelName = getmodel.Name;
        //            }

        //            var getbrand = db.Brands.Find(obj.BrandId);
        //            if (getbrand != null)
        //            {
        //                obj.BrandName = getbrand.Name;
        //            }
        //            var getcolor = db.Colors.Find(obj.ColorId);
        //            if (getcolor != null)
        //            {
        //                obj.ColorName = getcolor.Name;
        //            }
        //            db.Products.Add(obj);
        //            await db.SaveChangesAsync();
        //            return Ok(Response);
        //        }
        //        return Ok(Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<Product>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[HttpGet("ItemGetByID/{id}")]
        //public IActionResult ItemGetByID(int id)
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<Product>();

        //        var record = db.Products.Find(id);
        //        if (record != null)
        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);

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
        //            var Response = ResponseBuilder.BuildWSResponse<Product>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpGet("ItemGetByIDWithStock/{id}")]
        //public IActionResult ItemGetByIDWithStock(int id)
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<Product>();
        //        var record = db.Products.Find(id);
        //        if (record != null)
        //        {
        //            var getStock = db.InventoryStocks.ToList().Where(x => x.ProductId == id).FirstOrDefault();
        //            if (getStock != null)
        //            {
        //                record.Stock.ProductId = getStock.ProductId;
        //                record.Stock.ItemCode = getStock.ItemCode;
        //                record.Stock.Quantity = getStock.Quantity;
        //                record.Stock.ItemName = getStock.ItemName;
        //                record.Stock.Sku = getStock.Sku;
        //                record.Stock.ItemBarCode = getStock.ItemBarCode;
        //                record.Stock.StockId = getStock.StockId;
        //            }
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
        //            return Ok(Response);
        //        }
        //        else
        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
        //        }
        //        return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<Product>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[HttpPut("ItemUpdate/{id}")]
        //public async Task<IActionResult> ItemUpdate(int id, Product data)
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<Product>();
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        if (id != data.Id)
        //        {
        //            return BadRequest();
        //        }

        //        bool isValid = db.Products.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
        //        if (isValid)
        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);

        //        }
        //        else
        //        {
        //            var record = await db.Products.FindAsync(id);
        //            if (data.Name != null && data.Name != "undefined")
        //            {
        //                record.Name = data.Name;
        //            }
        //            if (data.ProductCode != null && data.ProductCode != "undefined")
        //            {
        //                record.ProductCode = data.ProductCode;
        //            }
        //            if (data.BarCode != null && data.BarCode != "undefined")
        //            {
        //                record.BarCode = data.BarCode;
        //            }
        //            if (data.Size != null && data.Size != "undefined")
        //            {
        //                record.Size = data.Size;
        //            }
        //            if (data.Sku != null && data.Sku != "undefined")
        //            {
        //                record.Sku = data.Sku;
        //            }
        //            if (data.TaxOnPurchase != null)
        //            {
        //                record.TaxOnPurchase = data.TaxOnPurchase;
        //            }
        //            if (data.QtyinStock != null && data.QtyinStock != "undefined")
        //            {
        //                record.QtyinStock = data.QtyinStock;
        //            }
        //            if (data.ItemNumber != null && data.ItemNumber != "undefined")
        //            {
        //                record.ItemNumber = data.ItemNumber;
        //            }
        //            if (data.UnitCharge != null && data.UnitCharge != "undefined")
        //            {
        //                record.UnitCharge = data.UnitCharge;
        //            }
        //            if (data.OutofstateCost != null && data.OutofstateCost != "undefined")
        //            {
        //                record.OutofstateCost = data.OutofstateCost;
        //            }
        //            if (data.AddtoCostPercenatge != null && data.AddtoCostPercenatge != "undefined")
        //            {
        //                record.AddtoCostPercenatge = data.AddtoCostPercenatge;
        //            }
        //            if (data.UnitsInPack != null && data.UnitsInPack != "undefined")
        //            {
        //                record.UnitsInPack = data.UnitsInPack;
        //            }
        //            if (data.RetailPackPrice != null && data.RetailPackPrice != "undefined")
        //            {
        //                record.RetailPackPrice = data.RetailPackPrice;
        //            }
        //            if (data.SalesLimit != null && data.SalesLimit != "undefined")
        //            {
        //                record.SalesLimit = data.SalesLimit;
        //            }
        //            if (data.Cost != null && data.Cost != "undefined")
        //            {
        //                record.Cost = data.Cost;
        //            }
        //            if (data.UnitRetail != null && data.UnitRetail != "undefined")
        //            {
        //                record.UnitRetail = data.UnitRetail;
        //            }
        //            if (data.TaxExempt != null)
        //            {
        //                record.TaxExempt = data.TaxExempt;
        //            }
        //            if (data.ShippingEnable != null)
        //            {
        //                record.ShippingEnable = data.ShippingEnable;
        //            }
        //            if (data.AllowECommerce != null)
        //            {
        //                record.AllowECommerce = data.AllowECommerce;
        //            }
        //            if (data.ShippingEnable != null)
        //            {
        //                record.ShippingEnable = data.ShippingEnable;
        //            }
        //            if (data.ItemCategoryId != null && data.ItemCategoryId > 0)
        //            {
        //                var getcategory = db.ItemCategories.Find(data.ItemCategoryId);
        //                if (getcategory != null)
        //                {
        //                    data.CategoryName = getcategory.Name;
        //                }
        //                record.ItemCategoryId = data.ItemCategoryId;
        //            }

        //            if (data.ItemSubCategoryId != null && data.ItemSubCategoryId > 0)
        //            {
        //                var getsubcat = db.ItemSubCategories.Find(data.ItemSubCategoryId);
        //                if (getsubcat != null)
        //                {
        //                    data.SubCatName = getsubcat.SubCategory;
        //                }
        //            }
        //            if (data.ModelId != null && data.ModelId > 0)
        //            {
        //                var getmodel = db.Models.Find(data.ModelId);
        //                if (getmodel != null)
        //                {
        //                    data.ModelName = getmodel.Name;
        //                }
        //            }
        //            if (data.BrandId != null && data.BrandId > 0)
        //            {
        //                var getbrand = db.Brands.Find(data.BrandId);
        //                if (getbrand != null)
        //                {
        //                    data.BrandName = getbrand.Name;
        //                }
        //            }
        //            if (data.BrandId != null && data.BrandId > 0)
        //            {
        //                var getcolor = db.Colors.Find(data.ColorId);
        //                if (getcolor != null)
        //                {
        //                    data.ColorName = getcolor.Name;
        //                }
        //            }
        //            data = record;
        //            db.Entry(data).State = EntityState.Modified;
        //            await db.SaveChangesAsync();
        //            return Ok(Response);
        //        }
        //        return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<Product>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpDelete("DeleteItem/{id}")]
        //public async Task<IActionResult> DeleteItem(int id)
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<Product>();
        //        Product data = await db.Products.FindAsync(id);
        //        if (data == null)
        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
        //        }
        //        db.Products.Remove(data);
        //        await db.SaveChangesAsync();
        //        return Ok(Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<Product>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}
        ////Item End
        [HttpGet("SaveImage/{str,ImgName}")]
        public string SaveImage(byte[] str, string ImgName)
        {
            string hostRootPath = _webHostEnvironment.WebRootPath;
            string webRootPath = _webHostEnvironment.ContentRootPath;
            string imgPath = string.Empty;

            if (!string.IsNullOrEmpty(webRootPath))
            {
                string path = webRootPath + "\\images\\";
                string imageName = ImgName + ".jpg";
                imgPath = Path.Combine(path, imageName);
                //imgPath = path + imageName;
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                imgPath = "http://45.35.97.246:5595/abcdwebapi/images/" + imageName;
                return imgPath;
            }
            else if (!string.IsNullOrEmpty(hostRootPath))
            {
                string path = hostRootPath + "\\images\\";
                string imageName = ImgName + ".jpg";
                imgPath = Path.Combine(path, imageName);
                //imgPath = path + imageName;
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                imgPath = "http://45.35.97.246:5595/abcdwebapi/images/" + imageName;
                return imgPath;
            }
            imgPath = imgPath.Replace(" ", "");
            return imgPath;
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
                imgPath = "http://45.35.97.246:5595/abcdwebapi/images/documents/" + imageName;
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
                imgPath = "http://45.35.97.246:5595/abcdwebapi/images/documents/" + imageName;
                return imgPath;
            }
            imgPath = imgPath.Replace(" ", "");
            return imgPath;
        }
        //Color Sub Start
        [HttpGet("ColorGet")]
        public IActionResult ColorGet()
        {
            try
            {
                var record = db.Colors.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Color>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Color>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ColorCreate")]
        public async Task<IActionResult> ColorCreate(Color users)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Colors.ToList().Exists(p => p.Name.Equals(users.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    return BadRequest("Color Already Exists");
                }
                db.Colors.Add(users);
                await db.SaveChangesAsync();
                return CreatedAtAction("ColorGet", new { id = users.Id }, users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ColorUpdate/{id}")]
        public async Task<IActionResult> ColorUpdate(int id, Color data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.Colors.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    return BadRequest("Color Already Exists");
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.Colors.FindAsync(id);
                if (data.Name != null && data.Name != "undefined")
                {
                    record.Name = data.Name;
                }
                data = record;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(data);
        }
        [HttpGet("ColorByID/{id}")]
        public IActionResult ColorByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Color>();
                var record = db.Colors.Find(id);
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
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteColor/{id}")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Color>();
                Color data = await db.Colors.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Colors.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Color>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //Color End
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
        //Group End

        // Group Start
        [HttpGet("ModelGet")]
        public IActionResult ModelGet()
        {
            try
            {
                var record = db.Models.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Model>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Model>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ModelCreate")]
        public async Task<IActionResult> ModelCreate(Model users)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Models.ToList().Exists(p => p.Name.Equals(users.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    return BadRequest("Model Already Exists");
                }
                db.Models.Add(users);
                await db.SaveChangesAsync();
                return CreatedAtAction("GroupGet", new { id = users.Id }, users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ModelUpdate/{id}")]
        public async Task<IActionResult> ModelUpdate(int id, Model data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.Models.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    return BadRequest("Group Already Exists");
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.Models.FindAsync(id);
                if (data.Name != null && data.Name != "undefined")
                {
                    record.Name = data.Name;
                }
                data = record;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(data);
        }
        [HttpGet("ModelByID/{id}")]
        public IActionResult ModelByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Model>();
                var record = db.Models.Find(id);
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
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteModel/{id}")]
        public async Task<IActionResult> DeleteModel(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Model>();
                Model data = await db.Models.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Models.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Model End
        //Sales Start
        //[HttpGet("SaleGet")]
        //public IActionResult SaleGet()
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<List<Sale>>();
        //        var record = db.Sales.ToList();
        //        for (int i = 0; i < record.Count(); i++)
        //        {
        //            if (record[i].SupervisorId != null)
        //            {
        //                var GetSupervisors = db.Supervisors.Find(record[i].SupervisorId);
        //                if (GetSupervisors != null)
        //                {
        //                    var getCurrentUser = db.AspNetUsers.Find(GetSupervisors.UserId);
        //                    if (getCurrentUser != null)
        //                    {
        //                        GetSupervisors.AspNetUser = getCurrentUser;
        //                    }
        //                    record[i].Supervisor = GetSupervisors;
        //                }
        //            }

        //            if (record[i].SalesManagerId != null)
        //            {
        //                var GetSalesManager = db.SalesManagers.Find(record[i].SalesManagerId);
        //                if (GetSalesManager != null)
        //                {
        //                    var getCurrentUser = db.AspNetUsers.Find(GetSalesManager.UserId);
        //                    if (getCurrentUser != null)
        //                    {
        //                        GetSalesManager.AspNetUser = getCurrentUser;
        //                    }
        //                    record[i].SalesManager = GetSalesManager;

        //                }
        //            }
        //        }
        //        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
        //        return Ok(Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<Sale>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost("SaleCreate")]
        //public async Task<IActionResult> SaleCreate(List<Sale> obj)
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<List<Sale>>();

        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest();
        //        }
        //        InventoryStock stock = null;
        //        double grossamount = 0;
        //        for (int a = 0; a < obj.Count(); a++)
        //        {
        //            grossamount += Convert.ToDouble(obj[a].TotalAmount);
        //        }
        //        for (int i = 0; i < obj.Count(); i++)
        //        {
        //            stock = new InventoryStock();
        //            obj[i].GrossAmount = grossamount.ToString();
        //            obj[i].SaleDate = DateTime.Now;
        //            if (obj[i].CustomerId != null)
        //            {
        //                var getcustomername = db.Customers.Find(obj[i].CustomerId);
        //                {
        //                    obj[i].CustomerName = getcustomername.FullName;
        //                }
        //            }
        //            db.Sales.Add(obj[i]);
        //            db.SaveChanges();
        //            var getstock = db.InventoryStocks.ToList().Where(x => x.ProductId == obj[i].ItemId).FirstOrDefault();
        //            if (getstock != null)
        //            {
        //                getstock.Quantity = (Convert.ToDouble(getstock.Quantity) - Convert.ToDouble(obj[i].Quantity)).ToString();
        //                db.Entry(getstock).State = EntityState.Modified;
        //                db.SaveChanges();
        //            }
        //        }
        //        var getvendor = db.Customers.Find(obj[0].CustomerId);
        //        if (getvendor != null)
        //        {
        //            var getaccount = db.Accounts.ToList().Where(a => a.Title == getvendor.AccountTitle && a.AccountId == getvendor.AccountId).FirstOrDefault();
        //            var getCHaccount = db.Accounts.ToList().Where(a => a.Title == "Net Sales").FirstOrDefault();
        //            var getCInHaccount = db.Accounts.ToList().Where(a => a.Title == "Cash in hand").FirstOrDefault();

        //            for (int i = 0; i < 2; i++)
        //            {
        //                Transaction transaction = null;
        //                transaction = new Transaction();
        //                if (i == 0)
        //                {
        //                    transaction.AccountName = getaccount.Title;
        //                    transaction.AccountNumber = getaccount.AccountId;
        //                    transaction.DetailAccountId = getaccount.AccountId;
        //                    transaction.Credit = "0.00";
        //                    transaction.Debit = grossamount.ToString();
        //                    transaction.InvoiceNumber = obj[0].InvoiceNumber;
        //                    transaction.Date = DateTime.Now;
        //                    transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
        //                    db.Transactions.Add(transaction);
        //                    db.SaveChanges();

        //                }
        //                else
        //                {
        //                    transaction.AccountName = getCHaccount.Title;
        //                    transaction.AccountNumber = getCHaccount.AccountId;
        //                    transaction.DetailAccountId = getCHaccount.AccountId;
        //                    transaction.Credit = grossamount.ToString();
        //                    transaction.Debit = "0.00";
        //                    transaction.InvoiceNumber = obj[0].InvoiceNumber;
        //                    transaction.Date = DateTime.Now;
        //                    transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
        //                    db.Transactions.Add(transaction);
        //                    db.SaveChanges();
        //                }
        //            }

        //            Receivable pay = null;
        //            if (obj[0].OnCash == false)
        //            {
        //                pay = new Receivable();
        //                if (getaccount != null)
        //                {
        //                    var getpay = db.Receivables.ToList().Where(x => x.AccountId == getaccount.AccountId).FirstOrDefault();
        //                    if (getpay != null)
        //                    {
        //                        getpay.Amount = (Convert.ToDouble(getpay.Amount) + Convert.ToDouble(grossamount)).ToString();
        //                        db.Entry(getpay).State = EntityState.Modified;
        //                        db.SaveChanges();

        //                    }
        //                    else
        //                    {
        //                        pay.AccountId = getaccount.AccountId;
        //                        pay.AccountNumber = getaccount.AccountId;
        //                        pay.Amount = grossamount.ToString();
        //                        pay.AccountName = getaccount.Title;
        //                        db.Receivables.Add(pay);
        //                        db.SaveChanges();
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                //var getFGaccount = db.Accounts.ToList().Where(a => a.AccountId == ).FirstOrDefault();
        //                if (getaccount != null)
        //                {

        //                    var fullcode = "";
        //                    Receiving newitems = new Receiving();
        //                    var recordemp = db.Receivings.ToList();
        //                    if (recordemp.Count() > 0)
        //                    {
        //                        if (recordemp[0].InvoiceNumber != null && recordemp[0].InvoiceNumber != "string" && recordemp[0].InvoiceNumber != "")
        //                        {
        //                            int large, small;
        //                            int salesID = 0;
        //                            large = Convert.ToInt32(recordemp[0].InvoiceNumber.Split('-')[1]);
        //                            small = Convert.ToInt32(recordemp[0].InvoiceNumber.Split('-')[1]);
        //                            for (int i = 0; i < recordemp.Count; i++)
        //                            {
        //                                if (recordemp[i].InvoiceNumber != null)
        //                                {
        //                                    var t = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);
        //                                    if (Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]) > large)
        //                                    {
        //                                        salesID = Convert.ToInt32(recordemp[i].ReceivingId);
        //                                        large = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);

        //                                    }
        //                                    else if (Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]) < small)
        //                                    {
        //                                        small = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);
        //                                    }
        //                                    else
        //                                    {
        //                                        if (large < 2)
        //                                        {
        //                                            salesID = Convert.ToInt32(recordemp[i].ReceivingId);
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            newitems = recordemp.ToList().Where(x => x.ReceivingId == salesID).FirstOrDefault();
        //                            if (newitems != null)
        //                            {
        //                                if (newitems.InvoiceNumber != null)
        //                                {
        //                                    var VcodeSplit = newitems.InvoiceNumber.Split('-');
        //                                    int code = Convert.ToInt32(VcodeSplit[1]) + 1;
        //                                    fullcode = "RE00" + "-" + Convert.ToString(code);
        //                                }
        //                                else
        //                                {
        //                                    fullcode = "RE00" + "-" + "1";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                fullcode = "RE00" + "-" + "1";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            fullcode = "RE00" + "-" + "1";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        fullcode = "RE00" + "-" + "1";
        //                    }

        //                    Receiving receiving = null;
        //                    receiving = new Receiving();
        //                    receiving.Date = DateTime.Now;
        //                    receiving.DueDate = DateTime.Now;
        //                    receiving.AccountId = getaccount.AccountId;
        //                    receiving.AccountName = getaccount.Title;
        //                    receiving.AccountNumber = getaccount.AccountId;
        //                    receiving.InvoiceNumber = fullcode;
        //                    receiving.Debit = "0.00";
        //                    receiving.Credit = grossamount.ToString();
        //                    receiving.CashBalance = grossamount.ToString();
        //                    receiving.PaymentType = "Cash";
        //                    receiving.Note = "";
        //                    receiving.NetAmount = grossamount.ToString();
        //                    db.Receivings.Add(receiving);
        //                    await db.SaveChangesAsync();
        //                    for (int i = 0; i < 2; i++)
        //                    {
        //                        Transaction transaction = null;
        //                        transaction = new Transaction();
        //                        if (i == 0)
        //                        {
        //                            transaction.AccountName = getaccount.Title;
        //                            transaction.AccountNumber = getaccount.AccountId;
        //                            transaction.DetailAccountId = getaccount.AccountId;
        //                            transaction.Credit = grossamount.ToString();
        //                            transaction.Debit = "0.00";
        //                            transaction.InvoiceNumber = fullcode;
        //                            transaction.Date = DateTime.Now;
        //                            transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
        //                            db.Transactions.Add(transaction);
        //                            db.SaveChanges();

        //                        }
        //                        else
        //                        {
        //                            transaction.AccountName = getCInHaccount.Title;
        //                            transaction.AccountNumber = getCInHaccount.AccountId;
        //                            transaction.DetailAccountId = getCInHaccount.AccountId;
        //                            transaction.Credit = "0.00";
        //                            transaction.Debit = grossamount.ToString();
        //                            transaction.InvoiceNumber = fullcode;
        //                            transaction.Date = DateTime.Now;
        //                            transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
        //                            db.Transactions.Add(transaction);
        //                            db.SaveChanges();
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //        await db.SaveChangesAsync();
        //        return Ok(Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<Sale>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}



        //[HttpDelete("DeleteSale/{invoice}")]
        //public async Task<IActionResult> DeleteSale(string invoice)
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<Sale>();
        //        var data = db.Sales.ToList().Where(x => x.InvoiceNumber == invoice).ToList();

        //        if (data.Count() > 0)
        //        {
        //            var gettransactions = db.Transactions.ToList().Where(x => x.InvoiceNumber == invoice).ToList();
        //            for (int a = 0; a < gettransactions.Count(); a++)
        //            {
        //                db.Transactions.Remove(gettransactions[a]);
        //                db.SaveChanges();
        //            }
        //            if (data[0].CustomerName != null)
        //            {
        //                var getAcc = db.Accounts.ToList().Where(x => x.Title == data[0].CustomerName).FirstOrDefault();
        //                if (getAcc != null)
        //                {
        //                    var getrecv = db.Receivables.ToList().Where(x => x.AccountId == getAcc.AccountId).FirstOrDefault();
        //                    if (getrecv != null)
        //                    {
        //                        if (Convert.ToDouble(getrecv.Amount) > Convert.ToDouble(data[0].GrossAmount))
        //                        {
        //                            double rem = Convert.ToDouble(getrecv.Amount) - Convert.ToDouble(data[0].GrossAmount);
        //                            getrecv.Amount = rem.ToString();
        //                            db.Entry(getrecv).State = EntityState.Modified;
        //                            db.SaveChanges();
        //                        }
        //                        else
        //                        {
        //                            db.Receivables.Remove(getrecv);
        //                            db.SaveChanges();
        //                        }
        //                    }
        //                }
        //            }
        //            else if (data[0].CustomerId != null && data[0].CustomerName == null)
        //            {

        //                var getcus = db.Customers.Find(data[0].CustomerId);
        //                var getIAcc = db.Accounts.ToList().Where(x => x.Title == getcus.AccountTitle).FirstOrDefault();
        //                if (getIAcc != null)
        //                {
        //                    var getrecv = db.Receivables.ToList().Where(x => x.AccountId == getIAcc.AccountId).FirstOrDefault();
        //                    if (getrecv != null)
        //                    {
        //                        if (Convert.ToDouble(getrecv.Amount) > Convert.ToDouble(data[0].GrossAmount))
        //                        {
        //                            double rem = Convert.ToDouble(getrecv.Amount) - Convert.ToDouble(data[0].GrossAmount);
        //                            getrecv.Amount = rem.ToString();
        //                            db.Entry(getrecv).State = EntityState.Modified;
        //                            db.SaveChanges();
        //                        }
        //                        else
        //                        {
        //                            db.Receivables.Remove(getrecv);
        //                            db.SaveChanges();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        for (int i = 0; i < data.Count(); i++)
        //        {
        //            db.Sales.Remove(data[i]);
        //            db.SaveChanges();
        //        }
        //        if (data.Count() < 1)
        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
        //        }

        //        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
        //        return Ok(Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<Brand>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }

        //}
        //Sales END
        //Article
        [HttpGet("ArticleGet")]
        public IActionResult ArticleGet()
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
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ArticleGetByID/{id}")]
        public IActionResult ArticleGetByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ArticleType>();
                var record = db.ArticleTypes.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<ArticleType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ArticleCreate")]
        public async Task<IActionResult> ArticleCreate(ArticleType obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ArticleType>();

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.ArticleTypes.ToList().Exists(p => p.ArticleTypeName.Equals(obj.ArticleTypeName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                db.ArticleTypes.Add(obj);
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

        [HttpPut("ArticleUpdate/{id}")]
        public async Task<IActionResult> ArticleUpdate(int id, ArticleType data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }
                var record = await db.ArticleTypes.FindAsync(id);
                if (data.ArticleTypeName != null && data.ArticleTypeName != "undefined")
                {
                    record.ArticleTypeName = data.ArticleTypeName;
                }
                data = record;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(data);
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
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.ArticleTypes.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
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
        //Article

        //Brand Manufacturer
        [HttpGet("BrandGet")]
        public IActionResult BrandGet()
        {
            try
            {
                var record = db.Brands.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Brand>>();
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
                    var Response = ResponseBuilder.BuildWSResponse<Brand>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateBrand/{id}")]
        public async Task<IActionResult> UpdateBrand(int id, Brand model)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Brand>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (id != model.Id)
                {
                    return BadRequest();
                }
                bool isValid = db.Brands.ToList().Exists(x => x.Name.Equals(model.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                var record = await db.Brands.FindAsync(id);
                if (model.Name != null && model.Name != "undefined")
                {
                    record.Name = model.Name;
                }
                model = record;
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
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
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Brands.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
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
        [HttpGet("BrandGetByID/{id}")]
        public IActionResult BrandGetByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Brand>();

                var record = db.Brands.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Brand>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        //Brand ENd

        // Stock Evaluation
        [HttpGet("STockEvaluationGet")]
        public IActionResult STockEvaluationGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<StockEvaluation>>();
                var record = db.InventoryStocks.ToList();
                //var items = db.Products.ToList();

                List<StockEvaluation> liststock = null;
                liststock = new List<StockEvaluation>();

                StockEvaluation obj = null;


                double TotalGross = 0;
                double TotalPrice = 0;
                double TotalQuantiy = 0;
                for (int i = 0; i < record.Count(); i++)
                {
                    obj = new StockEvaluation();
                    obj.ItemBarCode = record[i].ItemBarCode;
                    obj.ItemCode = record[i].ItemCode;
                    obj.ItemName = record[i].ItemName;
                    obj.Quantity = Convert.ToDouble(record[i].Quantity);
                    TotalQuantiy += obj.Quantity;
                    obj.TotalQuantityInHand = TotalQuantiy;
                    obj.Sku = record[i].Sku;
                    obj.StockId = record[i].StockId;
                    obj.ProductId = record[i].ProductId;
                    var items = db.Products.ToList().Where(x => x.Id == record[i].ProductId).FirstOrDefault();
                    if (items != null)
                    {
                        TotalPrice = Convert.ToDouble(record[i].Quantity) * Convert.ToDouble(items.UnitCharge);

                        obj.TotalAmount = TotalPrice;
                        TotalGross += TotalPrice;
                        obj.GrossAmount = TotalGross;
                    }
                    liststock.Add(obj);
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, liststock);
                //   return Ok(liststock);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<StockEvaluation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("CheckCreditSalesAvailableForCustomerID/{id}")]
        public IActionResult CheckCreditSalesAvailableForCustomerID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<string>();
                if (id == 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, "CustomerIDRequired");
                    return Ok(Response);
                }
                var record = db.PosSales.ToList().Where(x => x.CustomerId == id).ToList();
                var getCustomer = db.Customers.Find(id);
                if (record != null && getCustomer != null)
                {
                    for (int i = 0; i < record.Count(); i++)
                    {
                        if (record[i].SalesManagerId != null)
                        {
                            var GetSalesManager = db.SalesManagers.Find(record[i].SalesManagerId);
                            if (GetSalesManager != null)
                            {
                                var getCurrentUser = db.AspNetUsers.Find(GetSalesManager.UserId);
                                if (getCurrentUser != null)
                                {
                                    GetSalesManager.AspNetUser = getCurrentUser;
                                }
                                record[i].SalesManager = GetSalesManager;

                            }
                        }

                        if (record[i].SupervisorId != null)
                        {
                            var GetSupervisors = db.Supervisors.Find(record[i].SupervisorId);
                            if (GetSupervisors != null)
                            {
                                var getCurrentUser = db.AspNetUsers.Find(GetSupervisors.UserId);
                                if (getCurrentUser != null)
                                {
                                    GetSupervisors.AspNetUser = getCurrentUser;
                                }
                                record[i].Supervisor = GetSupervisors;
                            }
                        }
                    }
                    double GrossCheckAmount = 0;
                    var lastSale = record.ToList().Where(x => x.OnCredit == true).LastOrDefault();
                    if (lastSale != null)
                    {
                        if (lastSale.OnCredit == true)
                        {
                            GrossCheckAmount = Convert.ToDouble(lastSale.InvoiceTotal);
                            var getAccount = db.Accounts.ToList().Where(x => x.AccountId == getCustomer.AccountId).FirstOrDefault();
                            if (getAccount != null)
                            {
                                var getRec = db.Receivables.ToList().Where(x => x.AccountId == getAccount.AccountId).FirstOrDefault();
                                if (getRec != null)
                                {
                                    if (Convert.ToDouble(getRec.Amount) > GrossCheckAmount)
                                    {
                                        ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, "CreditNotAllow");
                                    }
                                    else
                                    {
                                        if (Convert.ToDouble(getRec.Amount) == GrossCheckAmount)
                                        {
                                            ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, "AskSalesManager");
                                        }
                                    }
                                }
                                else
                                {
                                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, "AskSupervisor");
                                }
                            }
                            else
                            {
                                ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, "");
                            }
                        }
                    }
                }
                // ResponseBuilder.SetWSResponse(Response, Infrastructure.Configuration.StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<string>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("AuthenticateLogin")]
        public IActionResult AuthenticateSupervisor(Supervisor data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Supervisor>();

                var record = db.Supervisors.AsQueryable().Where(x => x.AccessPin == data.AccessPin).FirstOrDefault();
                if (record == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                else
                {
                    if (Convert.ToDouble(data.CreditLimit) > Convert.ToDouble(record.CreditLimit))
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.Exceed_Credit_Limit, null, null);
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    }
                }

                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("SaleGetBySupervisorID/{id}")]
        public IActionResult SaleGetBySupervisorID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                var record = db.PosSales.ToList().Where(x => x.SupervisorId == id).ToList();
                for (int i = 0; i < record.Count(); i++)
                {
                    if (record[i].SupervisorId != null)
                    {
                        var GetSupervisors = db.Supervisors.Find(record[i].SupervisorId);
                        if (GetSupervisors != null)
                        {
                            var getCurrentUser = db.AspNetUsers.Find(GetSupervisors.UserId);
                            if (getCurrentUser != null)
                            {
                                GetSupervisors.AspNetUser = getCurrentUser;
                            }
                            record[i].Supervisor = GetSupervisors;
                        }
                    }

                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Sale>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SaleGetBySalesManagerID/{id}")]
        public IActionResult SaleGetBySalesManagerID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                var record = db.PosSales.ToList().Where(x => x.SalesManagerId == id).ToList();
                for (int i = 0; i < record.Count(); i++)
                {
                    if (record[i].SalesManagerId != null)
                    {
                        var GetSalesManager = db.SalesManagers.Find(record[i].SalesManagerId);
                        if (GetSalesManager != null)
                        {
                            var getCurrentUser = db.AspNetUsers.Find(GetSalesManager.UserId);
                            if (getCurrentUser != null)
                            {
                                GetSalesManager.AspNetUser = getCurrentUser;
                            }
                            record[i].SalesManager = GetSalesManager;

                        }
                    }
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Sale>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("SaleGetByInvoiceNumber/{invoice}")]
        public IActionResult SaleGetByInvoiceNumber(string invoice)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                var record = db.PosSales.ToList().Where(x => x.InvoiceNumber == invoice).ToList();
                for (int i = 0; i < record.Count(); i++)
                {
                    if (record[i].SupervisorId != null)
                    {
                        var GetSupervisors = db.Supervisors.Find(record[i].SupervisorId);
                        if (GetSupervisors != null)
                        {
                            var getCurrentUser = db.AspNetUsers.Find(GetSupervisors.UserId);
                            if (getCurrentUser != null)
                            {
                                GetSupervisors.AspNetUser = getCurrentUser;
                            }
                            record[i].Supervisor = GetSupervisors;
                        }
                    }

                    if (record[i].SalesManagerId != null)
                    {
                        var GetSalesManager = db.SalesManagers.Find(record[i].SalesManagerId);
                        if (GetSalesManager != null)
                        {
                            var getCurrentUser = db.AspNetUsers.Find(GetSalesManager.UserId);
                            if (getCurrentUser != null)
                            {
                                GetSalesManager.AspNetUser = getCurrentUser;
                            }
                            record[i].SalesManager = GetSalesManager;

                        }
                    }

                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Sale>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("StockGet")]
        public IActionResult StockGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<InventoryStock>>();
                var record = db.InventoryStocks.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<InventoryStock>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        //New Vendor

        [HttpGet("NewVendorGet")]
        public IActionResult NewVendorGet()
        {
            try
            {
                var record = db.Vendors.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Vendor>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("NewVendorCreate")]
        public async Task<IActionResult> NewVendorCreate(Vendor obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool isValid = db.Vendors.ToList().Exists(p => p.Email.Equals(obj.Email, StringComparison.CurrentCultureIgnoreCase));
                if (isValid)

                {
                    return BadRequest("Vendor Already Exists");
                }
                else
                {

                    //if (obj.Attachment != null)
                    //{
                    //    //  return BadRequest("Image Path Required Instead of Byte");
                    //    var imgPath = SaveImage(obj.Attachment, Guid.NewGuid().ToString());
                    //    obj.AttachmentByPath = imgPath;
                    //    obj.Attachment = null;

                    //}

                    if (obj.AccountNumber != null)
                    {
                        Account objacount = null;
                        objacount = new Account();

                        objacount.AccountId = obj.AccountNumber;
                        objacount.Title = obj.FullName;
                        objacount.Status = 1;
                        string num1 = obj.AccountNumber.Split("-")[0];
                        string num2 = obj.AccountNumber.Split("-")[1];
                        string num3 = obj.AccountNumber.Split("-")[2];
                        objacount.AccountSubGroupId = num1 + "-" + num2 + "-" + num3;

                        db.Accounts.Add(objacount);
                        db.SaveChanges();

                    }
                    else
                    {
                        Account objacount = null;
                        objacount = new Account();
                        var record = db.AccountSubGroups.ToList().Where(x => x.Title == "Supplier").FirstOrDefault();
                        if (record != null)
                        {
                            var getAccount = db.Accounts.ToList().Where(x => x.AccountSubGroupId == record.AccountSubGroupId).LastOrDefault();
                            if (getAccount != null)
                            {
                                var code = getAccount.AccountId.Split("-")[3];
                                int getcode = 0;
                                if (code != null)
                                {

                                    getcode = Convert.ToInt32(code) + 1;
                                }
                                objacount.AccountId = record.AccountSubGroupId + "000" + Convert.ToString(getcode);
                                objacount.Title = obj.FullName;
                                objacount.Status = 1;
                                objacount.AccountSubGroupId = record.AccountSubGroupId;
                                db.Accounts.Add(objacount);
                                db.SaveChanges();
                            }
                            else
                            {
                            }
                        }

                    }
                    db.Vendors.Add(obj);
                    await db.SaveChangesAsync();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    return Ok(Response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("NewVendorUpdate/{id}")]
        public async Task<IActionResult> NewVendorUpdate(int id, Vendor data)
        {
            try
            {
                //data.FullName = data.FirstName + " " + data.LastName;
                var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != data.VendorId)
                {
                    return BadRequest();
                }

                bool isValid = db.Vendors.ToList().Exists(x => x.FullName.Equals(data.FullName, StringComparison.CurrentCultureIgnoreCase) && x.VendorId != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);

                }
                else
                {
                    var record = await db.Vendors.FindAsync(id);
                    if(record != null && data != null)
                    {
                        if (data.Company != null && data.Company != "undefined")
                        {
                            record.Company = data.Company;
                        }
                        if (data.Title != null && data.Title != "undefined")
                        {
                            record.Title = data.Title;
                        }
                        if (data.FirstName != null && data.FirstName != "undefined")
                        {
                            record.FirstName = data.FirstName;
                        }

                        if (data.LastName != null && data.LastName != "undefined")
                        {
                            record.LastName = data.LastName;
                        }
                        if (data.FullName != null && data.FullName != "undefined" && data.FullName != " ")
                        {
                            record.FullName = data.FullName;
                        }
                        if (data.SupplierChecked != null)
                        {
                            record.SupplierChecked = data.SupplierChecked;
                        }
                        if (data.Street != null && data.Street != "undefined")
                        {
                            record.Street = data.Street;
                        }
                        if (data.City != null && data.City != "undefined")
                        {
                            record.City = data.City;
                        }
                        if (data.State != null && data.State != "undefined")
                        {
                            record.State = data.State;
                        }
                        if (data.ZipCode != null && data.ZipCode != "undefined")
                        {
                            record.ZipCode = data.ZipCode;
                        }
                        if (data.Suite != null && data.Suite != "undefined")
                        {
                            record.Suite = data.Suite;
                        }
                        if (data.Country != null && data.Country != "undefined")
                        {
                            record.Country = data.Country;
                        }
                        if (data.Phone != null && data.Phone != "undefined")
                        {
                            record.Phone = data.Phone;
                        }
                        if (data.Fax != null && data.Fax != "undefined")
                        {
                            record.Fax = data.Fax;
                        }
                        if (data.PayTerms != null && data.PayTerms != "undefined")
                        {
                            record.PayTerms = data.PayTerms;
                        }
                        if (data.Discount != null && data.Discount != "undefined")
                        {
                            record.Discount = data.Discount;
                        }
                        if (data.CreditLimit != null && data.CreditLimit != "undefined")
                        {
                            record.CreditLimit = data.CreditLimit;
                        }
                        if (data.FedTaxId != null && data.FedTaxId != "undefined")
                        {
                            record.FedTaxId = data.FedTaxId;
                        }
                        if (data.StateTaxId != null && data.StateTaxId != "undefined")
                        {
                            record.StateTaxId = data.StateTaxId;
                        }
                        if (data.Email != null && data.Email != "undefined")
                        {
                            record.Email = data.Email;
                        }
                        if (data.Website != null && data.Website != "undefined")
                        {
                            record.Website = data.Website;
                        }
                        if (data.Ledger != null && data.Ledger != "undefined")
                        {
                            record.Ledger = data.Ledger;
                        }
                        if (data.LedgerCode != null && data.LedgerCode != "undefined")
                        {
                            record.LedgerCode = data.LedgerCode;
                        }
                        if (data.CheckMemo != null && data.CheckMemo != "undefined")
                        {
                            record.CheckMemo = data.CheckMemo;
                        }
                        if (data.PrintYtdonChecks != null)
                        {
                            record.PrintYtdonChecks = data.PrintYtdonChecks;
                        }
                        if (data.Comments != null && data.Comments != "undefined")
                        {
                            record.Comments = data.Comments;
                        }
                        if (data.AccountNumber != null && data.AccountNumber != "undefined")
                        {
                            record.AccountNumber = data.AccountNumber;
                        }
                        if (data.VenderType != null && data.VenderType != "undefined")
                        {
                            record.VenderType = data.VenderType;
                        }
                        if (data.StateDiscount != null && data.StateDiscount != "undefined")
                        {
                            record.StateDiscount = data.StateDiscount;
                        }
                        if (data.OutOfStateSupplier != null)
                        {
                            record.OutOfStateSupplier = data.OutOfStateSupplier;
                        }
                        if (data.LocalTextPaidBySupplier != null)
                        {
                            record.LocalTextPaidBySupplier = data.LocalTextPaidBySupplier;
                        }
                        if (data.ReportTaxesToStateNc != null)
                        {
                            record.ReportTaxesToStateNc = data.ReportTaxesToStateNc;
                        }
                        if (data.StateDiscount != null && data.StateDiscount != "undefined")
                        {
                            record.StateDiscount = data.StateDiscount;
                        }
                        data = record;
                        db.Entry(data).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return Ok(Response);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteNewVendor/{id}")]
        public async Task<IActionResult> DeleteNewVendor(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                Vendor data = await db.Vendors.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Vendors.Remove(data);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("NewVendorGetByID/{id}")]
        public IActionResult NewVendorGetByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Vendor>();

                var record = db.Vendors.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //New Vendor End


        //Supplier Documents Type
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
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SupplierDocumentType>();
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
        [HttpPut("SupplierDocumentTypeUpdate/{id}")]
        public async Task<IActionResult> SupplierDocumentTypeUpdate(int id, SupplierDocumentType data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SupplierDocumentType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != data.DocumentTypeId)
                {
                    return BadRequest();
                }

                bool isValid = db.SupplierDocumentTypes.ToList().Exists(x => x.DocumentType.Equals(data.DocumentType, StringComparison.CurrentCultureIgnoreCase) && x.DocumentTypeId != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);

                }

                else
                {
                    var record = await db.SupplierDocumentTypes.FindAsync(id);
                    if (data.DocumentType != null && data.DocumentType != "undefined")
                    {
                        record.DocumentType = data.DocumentType;
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
                    var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("SupplierDocumentTypeDelete/{id}")]
        public async Task<IActionResult> SupplierDocumentTypeDelete(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SupplierDocumentType>();
                Vendor data = await db.Vendors.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Vendors.Remove(data);
                await db.SaveChangesAsync();
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
        [HttpGet("SupplierDocumentTypeGetByID/{id}")]
        public IActionResult SupplierDocumentTypeGetByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SupplierDocumentType>();

                var record = db.SupplierDocumentTypes.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<SupplierDocumentType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        //SupplierDocuments


        [HttpGet("SupplierDocumentsGet")]
        public IActionResult SupplierDocumentGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<SupplierDocument>>();
                var record = db.SupplierDocuments.ToList();
                //   return Ok(record);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
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
        [HttpPost("SupplierDocumentCreate")]
        public IActionResult SupplierDocumentCreate(SupplierDocument document)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SupplierDocument>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.SupplierDocuments.ToList().Exists(p => p.DocumentName.Equals(document.DocumentName, StringComparison.CurrentCultureIgnoreCase));
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

                if(document.DocumentTypeId != null)
                {
                    var foundtype = db.SupplierDocumentTypes.Find(Convert.ToInt32(document.DocumentTypeId));
                    if(foundtype != null)
                    {
                        document.DocumentType = foundtype.DocumentType;
                    }
                }
                db.SupplierDocuments.Add(document);
                db.SaveChanges();
                return Ok(Response);
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
        [HttpPut("SupplierDocumentUpdate/{id}")]
        public async Task<IActionResult> SupplierDocumentUpdate(int id, SupplierDocument data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SupplierDocument>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != data.DocumentId)
                {
                    return BadRequest();
                }
                bool isValid = db.SupplierDocuments.ToList().Exists(x => x.DocumentName.Equals(data.DocumentName, StringComparison.CurrentCultureIgnoreCase) && x.DocumentId != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);

                }

                else
                {
                    var record = await db.SupplierDocuments.FindAsync(id);
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
        [HttpDelete("SupplierDocumentDelete/{id}")]
        public async Task<IActionResult> SupplierDocumentDelete(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SupplierDocument>();
                SupplierDocument data = await db.SupplierDocuments.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.SupplierDocuments.Remove(data);
                await db.SaveChangesAsync();
                return Ok(Response);
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
        [HttpGet("SupplierDocumentGetByID/{id}")]
        public IActionResult SupplierDocumentGetByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SupplierDocument>();

                var record = db.SupplierDocuments.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<SupplierDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SupplierDocumentGetBySupplierID/{id}")]
        public IActionResult SupplierDocumentGetBySupplierID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<SupplierDocument>>();

                var record = db.SupplierDocuments.ToList().Where(x => x.SupplierId == id).ToList();
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
                    var Response = ResponseBuilder.BuildWSResponse<SupplierDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }





        //VENDOR TYPE
        //SUPPLIER DOCUMENT TYPE
        [HttpGet("SupplierTypeGet")]
        public IActionResult SupplierTypeGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<SupplierType>>();
                var record = db.SupplierTypes.ToList();
                //   return Ok(record);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SupplierType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SupplierTypeCreate")]
        public async Task<IActionResult> SupplierTypeCreate(SupplierType document)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SupplierType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.SupplierTypes.ToList().Exists(p => p.VendorType.Equals(document.VendorType, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                db.SupplierTypes.Add(document);
                await db.SaveChangesAsync();
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





        //ACCOUNT NUMBER GENERATION
        [HttpGet("LastSupplierAccountGet")]
        public IActionResult LastSupplierAccountGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Account>();

                var record = db.AccountSubGroups.ToList().Where(x => x.Title == "Suppliers").FirstOrDefault();
                if (record != null)
                {
                    var getAccount = db.Accounts.ToList().Where(x => x.AccountSubGroupId == record.AccountSubGroupId).LastOrDefault();
                    if (getAccount != null)
                    {
                        if(getAccount.AccountSubGroup != null)
                        {
                            getAccount.AccountSubGroup = null;
                        }
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, getAccount);
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);

                    }

                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Account>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("LastSupplierSubGroupGet")]
        public IActionResult LastSupplierSubGroupGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AccountSubGroup>();

                var record = db.AccountSubGroups.ToList().Where(x => x.Title == "Supplier").FirstOrDefault();
                if (record != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);

                }

                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<AccountSubGroup>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }


        }


        //MisPick Start Manufacturer
        [HttpGet("MisPickGet")]
        public IActionResult MisPickGet()
        {
            try
            {
                var record = db.MisPicks.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<MisPick>>();
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
                ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<MisPick>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("MisPickCreate")]
        public async Task<IActionResult> MisPickCreate(MisPick obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<MisPick>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.MisPicks.ToList().Exists(p => p.MisPickName.Equals(obj.MisPickName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                db.MisPicks.Add(obj);
                await db.SaveChangesAsync();
                //       var mispick= db.Entry(obj).GetDatabaseValues();
                //    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, mispick);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<MisPick>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateMisPick/{id}")]
        public async Task<IActionResult> UpdateMisPick(int id, MisPick model)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<MisPick>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (id != model.MisPickId)
                {
                    return BadRequest();
                }
                bool isValid = db.MisPicks.ToList().Exists(x => x.MisPickName.Equals(model.MisPickName, StringComparison.CurrentCultureIgnoreCase) && x.MisPickId != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                var record = await db.MisPicks.FindAsync(id);
                if (model.MisPickName != null && model.MisPickName != "undefined")
                {
                    record.MisPickName = model.MisPickName;
                }
                model = record;
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<MisPick>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteMisPick/{id}")]
        public async Task<IActionResult> DeleteMisPick(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<MisPick>();
                MisPick data = await db.MisPicks.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.MisPicks.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<MisPick>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("MisPickGetByID/{id}")]
        public IActionResult MisPickGetByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<MisPick>();

                var record = db.MisPicks.Find(id);
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
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<MisPick>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FAILURE_CODE, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //MisPick END Manufacturer
        [HttpGet("CheckCheapProduct/{id}")]
        public IActionResult CheckCheapProduct(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                var record = db.Products.Find(id);
                List<Product> listproductitems = null;
                listproductitems = new List<Product>();


                var itemsList = db.Products.ToList().Where(x => x.Id != id && x.CategoryName == record.CategoryName && x.SubCatName == record.SubCatName).ToList();
                for (int i = 0; i < itemsList.Count(); i++)
                {
                    Product obj = null;
                    if (Convert.ToDouble(itemsList[i].Retail) < Convert.ToDouble(record.Retail))
                    {
                        obj = new Product();
                        obj = itemsList[i];
                        listproductitems.Add(obj);
                    }

                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, listproductitems);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Product>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FAILURE_CODE, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateFinancial/{id}")]
        public async Task<IActionResult> UpdateFinancial(int id, Financial data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Financial>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != data.FinancialId)
                {
                    return BadRequest();
                }
                else
                {
                    var record = await db.Financials.FindAsync(id);
                    if (data.Cost != null && data.Cost != "undefined")
                    {
                        record.Cost = data.Cost;
                    }
                    if (data.Profit != null && data.Profit != "undefined")
                    {
                        record.Profit = data.Profit;
                    }
                    if (data.MsgPromotion != null && data.MsgPromotion != "undefined")
                    {
                        record.MsgPromotion = data.MsgPromotion;
                    }
                    if (data.AddToCost != null && data.AddToCost != "undefined")
                    {
                        record.AddToCost = data.AddToCost;
                    }
                    if (data.UnitCharge != null && data.UnitCharge != "undefined")
                    {
                        record.UnitCharge = data.UnitCharge;
                    }
                    if (data.FixedCost != null)
                    {
                        record.FixedCost = data.FixedCost;
                    }
                    else
                    {
                        record.FixedCost = false;
                    }
                    if (data.CostPerQuantity != null)
                    {
                        record.CostPerQuantity = data.CostPerQuantity;
                    }
                    else
                    {

                        record.CostPerQuantity = false;
                    }

                    if (data.St != null && data.St != "undefined")
                    {
                        record.St = data.St;
                    }
                    if (data.Tax != null && data.Tax != "undefined")
                    {
                        record.Tax = data.Tax;
                    }
                    if (data.OutOfStateCost != null && data.OutOfStateCost != "undefined")
                    {
                        record.OutOfStateCost = data.OutOfStateCost;
                    }
                    if (data.OutOfStateRetail != null && data.OutOfStateRetail != "undefined")
                    {
                        record.OutOfStateRetail = data.OutOfStateRetail;
                    }
                    if (data.Price != null && data.Price != "undefined")
                    {
                        record.Price = data.Price;
                    }
                    if (data.Quantity != null && data.Quantity != "undefined")
                    {
                        record.Quantity = data.Quantity;
                    }
                    if (data.QuantityPrice != null && data.QuantityPrice != "undefined")
                    {
                        record.QuantityPrice = data.QuantityPrice;
                    }
                    if (data.SuggestedRetailPrice != null && data.SuggestedRetailPrice != "undefined")
                    {
                        record.SuggestedRetailPrice = data.SuggestedRetailPrice;
                    }
                    if (data.AutoSetSrp != null)
                    {
                        record.AutoSetSrp = data.AutoSetSrp;
                    }
                    else
                    {
                        record.AutoSetSrp = false;
                    }
                    if (data.QuantityInStock != null && data.QuantityInStock != "undefined")
                    {
                        record.QuantityInStock = data.QuantityInStock;
                    }

                    if (data.Adjustment != null && data.Adjustment != "undefined")
                    {
                        record.Adjustment = data.Adjustment;
                    }
                    if (data.OutOfState != null && data.OutOfState != "undefined")
                    {
                        record.OutOfState = data.OutOfState;
                    }


                    if (data.AskForPricing != null)
                    {
                        record.AskForPricing = data.AskForPricing;
                    }
                    else
                    {
                        record.AskForPricing = false;
                    }
                    if (data.AskForDescrip != null)
                    {
                        record.AskForDescrip = data.AskForDescrip;
                    }
                    else
                    {
                        record.AskForDescrip = false;
                    }
                    if (data.Serialized != null)
                    {
                        record.Serialized = data.Serialized;
                    }
                    else
                    {
                        record.Serialized = false;
                    }
                    if (data.TaxOnSales != null)
                    {
                        record.TaxOnSales = data.TaxOnSales;
                    }
                    else
                    {
                        record.TaxOnSales = false;
                    }
                    if (data.Purchase != null)
                    {
                        record.Purchase = data.Purchase;
                    }
                    else
                    {
                        record.Purchase = false;
                    }

                    if (data.SellBelowCost != null)
                    {
                        record.SellBelowCost = data.SellBelowCost;
                    }
                    else
                    {
                        record.SellBelowCost = false;
                    }
                    if (data.NoSuchDiscount != null)
                    {
                        record.NoSuchDiscount = data.NoSuchDiscount;
                    }
                    if (data.NoReturns != null)
                    {
                        record.NoReturns = data.NoReturns;
                    }
                    else
                    {
                        record.NoReturns = false;
                    }
                    if (data.CodeA != null)
                    {
                        record.CodeA = data.CodeA;
                    }
                    else
                    {
                        record.CodeA = false;
                    }
                    if (data.CodeB != null)
                    {
                        record.CodeB = data.CodeB;
                    }
                    else
                    {
                        record.CodeB = false;
                    }
                    if (data.CodeC != null)
                    {
                        record.CodeC = data.CodeC;
                    }
                    else
                    {
                        record.CodeC = false;
                    }
                    if (data.CodeD != null)
                    {
                        record.CodeD = data.CodeD;
                    }
                    else
                    {
                        record.CodeD = false;
                    }

                    if (data.AddCustomersDiscount != null)
                    {
                        record.AddCustomersDiscount = data.AddCustomersDiscount;
                    }
                    else
                    {
                        record.AddCustomersDiscount = false;
                    }
                    if (data.Retail != null && data.Retail != "undefined")
                    {
                        record.Retail = data.Retail;
                    }
                    data = record;
                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Ok(Response);
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Financial>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("ItemFinancialCreate")]
        //public async Task<IActionResult> ItemFinancialCreate(Product data)
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<Product>();
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest();
        //        }


        //        bool checkname = db.Products.ToList().Exists(p => p.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase));
        //        if (checkname)

        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
        //            return Ok(Response);
        //        }
        //        else
        //        {
        //            var fullcode = "";
        //            Product obj = new Product();
        //            //ItemRelationFinancial itemfinancialobj = new ItemRelationFinancial();



        //            //obj.Id= itemfinancialobj.Id;
        //            //obj.Name = itemfinancialobj.Name;
        //            //itemfinancialobj.ItemCategoryId= itemfinancialobj.ItemCategoryId;
        //            //itemfinancialobj.BrandId=  itemfinancialobj.BrandId;
        //            //itemfinancialobj.BrandId= itemfinancialobj.ArticleId;
        //            //itemfinancialobj.StoreId= itemfinancialobj.StoreId;
        //            //itemfinancialobj.Unit= itemfinancialobj.Unit;
        //            //itemfinancialobj.ProductCode= itemfinancialobj.ProductCode;
        //            //itemfinancialobj.BarCode= itemfinancialobj.BarCode;
        //            //itemfinancialobj.Size= itemfinancialobj.Size;
        //            //itemfinancialobj.ColorId= itemfinancialobj.ColorId;
        //            //itemfinancialobj.Sku= itemfinancialobj.Sku;
        //            //itemfinancialobj.Description= itemfinancialobj.Description;
        //            //itemfinancialobj.UnitRetail= itemfinancialobj.UnitRetail;
        //            //itemfinancialobj.SaleRetail= itemfinancialobj.SaleRetail;
        //            //itemfinancialobj.TaxExempt= itemfinancialobj.TaxExempt;
        //            //itemfinancialobj.ShippingEnable= itemfinancialobj.ShippingEnable;
        //            //itemfinancialobj.AllowECommerce= itemfinancialobj.AllowECommerce;
        //            //itemfinancialobj.CreatedDate= itemfinancialobj.CreatedDate;
        //            //itemfinancialobj.CreatedBy= itemfinancialobj.CreatedBy;
        //            //itemfinancialobj.ModifiedBy= itemfinancialobj.ModifiedBy;
        //            //itemfinancialobj.ModifiedDate= itemfinancialobj.ModifiedDate;
        //            //itemfinancialobj.OldPrice= itemfinancialobj.OldPrice;
        //            //itemfinancialobj.MsareportAs= itemfinancialobj.MsareportAs;
        //            //itemfinancialobj.StateReportAs= itemfinancialobj.StateReportAs;
        //            //itemfinancialobj.ReportingWeight= itemfinancialobj.ReportingWeight;
        //            //itemfinancialobj.FamilyId= itemfinancialobj.FamilyId;
        //            //itemfinancialobj.Family= itemfinancialobj.Family;
        //            //itemfinancialobj.QtyUnit= itemfinancialobj.QtyUnit;
        //            //itemfinancialobj.UnitsInPack= itemfinancialobj.UnitsInPack;
        //            //itemfinancialobj.RetailPackPrice= itemfinancialobj.RetailPackPrice;
        //            //itemfinancialobj.SalesLimit= itemfinancialobj.SalesLimit;
        //            //itemfinancialobj.Adjustment= itemfinancialobj.Adjustment;
        //            //itemfinancialobj.ProfitPercentage= itemfinancialobj.ProfitPercentage;
        //            //itemfinancialobj.Cost= itemfinancialobj.Cost;
        //            //itemfinancialobj.MfgPromotion= itemfinancialobj.MfgPromotion;
        //            //itemfinancialobj.AddtoCostPercenatge= itemfinancialobj.AddtoCostPercenatge;
        //            //itemfinancialobj.UnitCharge= itemfinancialobj.UnitCharge;
        //            //itemfinancialobj.OutofstateCost= itemfinancialobj.OutofstateCost;
        //            //itemfinancialobj.OutofstateRetail= itemfinancialobj.OutofstateRetail;
        //            //itemfinancialobj.TaxonSale= itemfinancialobj.TaxonSale;
        //            //itemfinancialobj.TaxOnPurchase= itemfinancialobj.TaxOnPurchase;
        //            //itemfinancialobj.Location=itemfinancialobj.Location;
        //            //itemfinancialobj.GroupId= itemfinancialobj.GroupId;
        //            //itemfinancialobj.ItemNumber= itemfinancialobj.ItemNumber;
        //            //itemfinancialobj.QtyinStock= itemfinancialobj.QtyinStock;
        //            //itemfinancialobj.ItemSubCategoryId=itemfinancialobj.ItemSubCategoryId;
        //            //itemfinancialobj.ModelId= itemfinancialobj.ModelId;
        //            //itemfinancialobj.ModelName= itemfinancialobj.ModelName;
        //            //itemfinancialobj.CategoryName= itemfinancialobj.CategoryName;
        //            //itemfinancialobj.SubCatName= itemfinancialobj.SubCatName;
        //            //itemfinancialobj.GroupName= itemfinancialobj.GroupName;
        //            //itemfinancialobj.BrandName= itemfinancialobj.BrandName;
        //            //itemfinancialobj.ColorName= itemfinancialobj.ColorName;
        //            //itemfinancialobj.ItemImage= itemfinancialobj.ItemImage;
        //            //itemfinancialobj.ItemImageByPath= itemfinancialobj.ItemImageByPath;
        //            //itemfinancialobj.Variations= itemfinancialobj.Variations;
        //            //itemfinancialobj.DiscountPrice= itemfinancialobj.DiscountPrice;
        //            //itemfinancialobj.Rating= itemfinancialobj.Rating;
        //            //itemfinancialobj.MinOrderQty= itemfinancialobj.MinOrderQty;
        //            //itemfinancialobj.MaxOrderQty= itemfinancialobj.MaxOrderQty;
        //            //itemfinancialobj.Retail= itemfinancialobj.Retail;
        //            //itemfinancialobj.QuantityCase= itemfinancialobj.QuantityCase;
        //            //itemfinancialobj.QuantityPallet= itemfinancialobj.QuantityPallet;
        //            //itemfinancialobj.SingleUnitMsa= itemfinancialobj.SingleUnitMsa;
        //            //itemfinancialobj.MisPickId=itemfinancialobj.MisPickId;
        //            //itemfinancialobj.MisPickName= itemfinancialobj.MisPickName;
        //            //itemfinancialobj.OrderQuantity= itemfinancialobj.OrderQuantity;
        //            //itemfinancialobj.Units=itemfinancialobj.Units;
        //            //itemfinancialobj.WeightOz= itemfinancialobj.WeightOz;
        //            //itemfinancialobj.WeightLb= itemfinancialobj.WeightLb;
        //            //itemfinancialobj.LocationTwo= itemfinancialobj.LocationTwo;
        //            //itemfinancialobj.LocationThree= itemfinancialobj.LocationThree;
        //            //itemfinancialobj.LocationFour= itemfinancialobj.LocationFour;
        //            //itemfinancialobj.MaintainStockForDays= itemfinancialobj.MaintainStockForDays;
        //            //Financial financialobj = new Financial();

        //            //itemfinancialobj.Financial.FinancialId; itemfinancialobj.Financial.FinancialId;
        //            //itemfinancialobj.Financial.Cost; itemfinancialobj.Financial.Cost;
        //            //itemfinancialobj.Financial.Profit; itemfinancialobj.Financial.Profit;
        //            //itemfinancialobj.Financial.MsgPromotion; itemfinancialobj.Financial. MsgPromotion;
        //            //itemfinancialobj.Financial.AddToCost; itemfinancialobj.Financial. AddToCost;
        //            //itemfinancialobj.Financial.UnitCharge; itemfinancialobj.Financial. UnitCharge;
        //            //itemfinancialobj.Financial.FixedCost; itemfinancialobj.Financial. FixedCost;
        //            //itemfinancialobj.Financial.CostPerQuantity; itemfinancialobj.Financial. CostPerQuantity;
        //            //itemfinancialobj.Financial.St; itemfinancialobj.Financial.St;
        //            //itemfinancialobj.Financial.Tax; itemfinancialobj.Financial. Tax;
        //            //itemfinancialobj.Financial.OutOfStateCost; itemfinancialobj.Financial.OutOfStateCost;
        //            //itemfinancialobj.Financial.OutOfStateRetail; itemfinancialobj.Financial.OutOfStateRetail;
        //            //itemfinancialobj.Financial.Price; itemfinancialobj.Financial.Price;
        //            //itemfinancialobj.Financial.Quantity; itemfinancialobj.Financial.Quantity;
        //            //itemfinancialobj.Financial.QuantityPrice; itemfinancialobj.Financial.QuantityPrice;
        //            //itemfinancialobj.Financial.SuggestedRetailPrice; itemfinancialobj.Financial.SuggestedRetailPrice;
        //            //itemfinancialobj.Financial.AutoSetSrp; itemfinancialobj.Financial.AutoSetSrp;
        //            //itemfinancialobj.Financial.ItemNumber; itemfinancialobj.Financial.ItemNumber;
        //            //itemfinancialobj.Financial.QuantityInStock; itemfinancialobj.Financial.QuantityInStock;
        //            //itemfinancialobj.Financial.Adjustment; itemfinancialobj.Financial.Adjustment;
        //            //itemfinancialobj.Financial.AskForPricing; itemfinancialobj.Financial.AskForPricing;
        //            //itemfinancialobj.Financial.AskForDescrip; itemfinancialobj.Financial.AskForDescrip;
        //            //itemfinancialobj.Financial.Serialized; itemfinancialobj.Financial.Serialized;
        //            //itemfinancialobj.Financial.TaxOnSales; itemfinancialobj.Financial.TaxOnSales;
        //            //itemfinancialobj.Financial.Purchase; itemfinancialobj.Financial.Purchase;
        //            //itemfinancialobj.Financial.NoSuchDiscount; itemfinancialobj.Financial.NoSuchDiscount;
        //            //itemfinancialobj.Financial.NoReturns; itemfinancialobj.Financial.NoReturns;
        //            //itemfinancialobj.Financial.SellBelowCost; itemfinancialobj.Financial.SellBelowCost;
        //            //itemfinancialobj.Financial.OutOfState; itemfinancialobj.Financial.OutOfState;
        //            //itemfinancialobj.Financial.CodeA; itemfinancialobj.Financial.CodeA;
        //            //itemfinancialobj.Financial.CodeB; itemfinancialobj.Financial.CodeB;
        //            //itemfinancialobj.Financial.CodeC; itemfinancialobj.Financial.CodeC;
        //            //itemfinancialobj.Financial.CodeD; itemfinancialobj.Financial.CodeD;
        //            //itemfinancialobj.Financial.AddCustomersDiscount; itemfinancialobj.Financial.AddCustomersDiscount;
        //            //itemfinancialobj.Financial.ItemName; itemfinancialobj.Financial.ItemName;
        //            //itemfinancialobj.Financial.Retail; itemfinancialobj.Financial.Retail;
        //            //  itemfinancialobj.Financial.ItemId;

        //var recordemp = db.Products.ToList();
        //            if (recordemp.Count() > 0)
        //            {
        //                if (recordemp[0].ItemNumber != null && recordemp[0].ItemNumber != "string" && recordemp[0].ItemNumber != "")
        //                {
        //                    int large, small;
        //                    int salesID = 0;
        //                    large = Convert.ToInt32(recordemp[0].ItemNumber.Split('-')[1]);
        //                    small = Convert.ToInt32(recordemp[0].ItemNumber.Split('-')[1]);
        //                    for (int i = 0; i < recordemp.Count(); i++)
        //                    {
        //                        if (recordemp[i].ItemNumber != null)
        //                        {
        //                            var t = Convert.ToInt32(recordemp[i].ItemNumber.Split('-')[1]);
        //                            if (Convert.ToInt32(recordemp[i].ItemNumber.Split('-')[1]) > large)
        //                            {
        //                                salesID = Convert.ToInt32(recordemp[i].Id);
        //                                large = Convert.ToInt32(recordemp[i].ItemNumber.Split('-')[1]);

        //                            }
        //                            else if (Convert.ToInt32(recordemp[i].ItemNumber.Split('-')[1]) < small)
        //                            {
        //                                small = Convert.ToInt32(recordemp[i].ItemNumber.Split('-')[1]);
        //                            }
        //                            else
        //                            {
        //                                if (large < 2)
        //                                {
        //                                    salesID = Convert.ToInt32(recordemp[i].Id);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    newitems = recordemp.ToList().Where(x => x.Id == salesID).FirstOrDefault();
        //                    if (newitems != null)
        //                    {
        //                        if (newitems.ItemNumber != null)
        //                        {
        //                            var VcodeSplit = newitems.ItemNumber.Split('-');
        //                            int code = Convert.ToInt32(VcodeSplit[1]) + 1;
        //                            fullcode = "IT00" + "-" + Convert.ToString(code);
        //                        }
        //                        else
        //                        {
        //                            fullcode = "IT00" + "-" + "1";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        fullcode = "IT00" + "-" + "1";
        //                    }
        //                }
        //                else
        //                {
        //                    fullcode = "IT00" + "-" + "1";
        //                }
        //            }
        //            else
        //            {
        //                fullcode = "IT00" + "-" + "1";
        //            }
        //            if (obj.ItemImage != null)
        //            {
        //                //  return BadRequest("Image Path Required Instead of Byte");
        //                var imgPath = SaveImage(obj.ItemImage, Guid.NewGuid().ToString());
        //                obj.ItemImageByPath = imgPath;
        //                obj.ItemImage = null;
        //            }

        //            var getcategory = db.ItemCategories.Find(obj.ItemCategoryId);
        //            if (getcategory != null)
        //            {
        //                obj.CategoryName = getcategory.Name;
        //            }
        //            var getsubcat = db.ItemSubCategories.Find(obj.ItemSubCategoryId);
        //            if (getsubcat != null)
        //            {
        //                obj.SubCatName = getsubcat.SubCategory;
        //            }
        //            var getmispick = db.MisPicks.Find(obj.MisPickId);
        //            if (getmispick != null)
        //            {
        //                obj.MisPickName = getmispick.MisPickName;
        //            }
        //            var getmodel = db.Models.Find(obj.ModelId);
        //            if (getmodel != null)
        //            {
        //                obj.ModelName = getmodel.Name;
        //            }
        //            var getbrand = db.Brands.Find(obj.BrandId);
        //            if (getbrand != null)
        //            {
        //                obj.BrandName = getbrand.Name;
        //            }
        //            var getcolor = db.Colors.Find(obj.ColorId);
        //            if (getcolor != null)
        //            {
        //                obj.ColorName = getcolor.Name;
        //            }
        //            obj.ItemNumber = fullcode;

        //            db.Products.Add(obj);
        //            await db.SaveChangesAsync();
        //            var getcurrentitem = db.Products.ToList().Where(x => x.ItemNumber == fullcode && x.Name == obj.Name).FirstOrDefault();
        //            if (getcurrentitem != null)
        //            {
        //                Financial savefinancial = new Financial();
        //                savefinancial.ItemNumber = getcurrentitem.ItemNumber;
        //                savefinancial.ItemName = getcurrentitem.Name;
        //                savefinancial.Retail = getcurrentitem.Retail;
        //                db.Financials.Add(savefinancial);
        //                //  savefinancial.i = getcurrentitem.Id;


        //            }
        //            var getProductnew = db.Products.ToList().Where(x => x.ItemNumber == fullcode && x.Name == obj.Name).FirstOrDefault();
        //            if (getProductnew != null)
        //            {
        //                ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, getProductnew);
        //                return Ok(Response);
        //            }

        //        }
        //        return Ok(Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<ItemRelationFinancial>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}



        [HttpGet("ItemGetByIDWithStockAndFinancial/{id}")]
        public IActionResult ItemGetByIDWithStockAndFinancial(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                var record = db.Products.Find(id);
                if (record != null)
                {
                    var getStock = db.InventoryStocks.ToList().Where(x => x.ProductId == id).FirstOrDefault();
                    if (getStock != null)
                    {
                        record.Stock.ProductId = getStock.ProductId;
                        record.Stock.ItemCode = getStock.ItemCode;
                        record.Stock.Quantity = getStock.Quantity;
                        record.Stock.ItemName = getStock.ItemName;
                        record.Stock.Sku = getStock.Sku;
                        record.Stock.ItemBarCode = getStock.ItemBarCode;
                        record.Stock.StockId = getStock.StockId;
                    }
                    var getFinancial = db.Financials.ToList().Where(x => x.ItemId == id).FirstOrDefault();
                    if (getFinancial != null)

                    {
                        record.Financial = new Financial();
                        record.Financial.ItemName = getFinancial.ItemName;
                        record.Financial.ItemId = getFinancial.ItemId;
                        record.Financial.ItemNumber = getFinancial.ItemNumber;
                        record.Financial.Quantity = getFinancial.Quantity;
                        record.Financial.Cost = getFinancial.Cost;
                        record.Financial.Profit = getFinancial.Profit;
                        record.Financial.MsgPromotion = getFinancial.MsgPromotion;
                        record.Financial.AddToCost = getFinancial.AddToCost;
                        record.Financial.ItemId = getFinancial.ItemId;
                        record.Financial.FixedCost = getFinancial.FixedCost;
                        record.Financial.CostPerQuantity = getFinancial.CostPerQuantity;
                        record.Financial.St = getFinancial.St;
                        record.Financial.Tax = getFinancial.Tax;
                        record.Financial.OutOfStateCost = getFinancial.OutOfStateCost;
                        record.Financial.OutOfStateRetail = getFinancial.OutOfStateRetail;
                        record.Financial.Price = getFinancial.Price;
                        record.Financial.Quantity = getFinancial.Quantity;
                        record.Financial.QuantityPrice = getFinancial.QuantityPrice;
                        record.Financial.SuggestedRetailPrice = getFinancial.SuggestedRetailPrice;
                        record.Financial.AutoSetSrp = getFinancial.AutoSetSrp;
                        record.Financial.QuantityInStock = getFinancial.MsgPromotion;
                        record.Financial.Adjustment = getFinancial.Adjustment;
                        record.Financial.AskForPricing = getFinancial.AskForPricing;
                        record.Financial.AskForDescrip = getFinancial.AskForDescrip;
                        record.Financial.Serialized = getFinancial.Serialized;
                        record.Financial.TaxOnSales = getFinancial.TaxOnSales;
                        record.Financial.Purchase = getFinancial.Purchase;
                        record.Financial.NoSuchDiscount = getFinancial.NoSuchDiscount;
                        record.Financial.NoReturns = getFinancial.NoReturns;
                        record.Financial.SellBelowCost = getFinancial.SellBelowCost;
                        record.Financial.OutOfState = getFinancial.OutOfState;
                        record.Financial.CodeA = getFinancial.CodeA;
                        record.Financial.CodeB = getFinancial.CodeB;
                        record.Financial.CodeC = getFinancial.CodeC;
                        record.Financial.CodeD = getFinancial.CodeD;
                        record.Financial.AddCustomersDiscount = getFinancial.AddCustomersDiscount;
                        record.Financial.Retail = getFinancial.Retail;

                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest();
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


        [HttpGet("SaleGetByProductID/{id}")]
        public IActionResult SaleGetByProductID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                var getproduct = db.Products.Find(id);
                if (getproduct == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                var record = db.PosSales.ToList().Where(x => x.ItemId == id).ToList();
                for (int i = 0; i < record.Count(); i++)
                {
                    if (record[i].SupervisorId != null)
                    {
                        var GetSupervisors = db.Supervisors.Find(record[i].SupervisorId);
                        if (GetSupervisors != null)
                        {
                            var getCurrentUser = db.AspNetUsers.Find(GetSupervisors.UserId);
                            if (getCurrentUser != null)
                            {
                                GetSupervisors.AspNetUser = getCurrentUser;
                            }
                            record[i].Supervisor = GetSupervisors;
                        }
                    }

                    if (record[i].SalesManagerId != null)
                    {
                        var GetSalesManager = db.SalesManagers.Find(record[i].SalesManagerId);
                        if (GetSalesManager != null)
                        {
                            var getCurrentUser = db.AspNetUsers.Find(GetSalesManager.UserId);
                            if (getCurrentUser != null)
                            {
                                GetSalesManager.AspNetUser = getCurrentUser;
                            }
                            record[i].SalesManager = GetSalesManager;

                        }
                    }
                }

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Sale>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        //Item Start

        [HttpGet("ItemGet")]
        public IActionResult ItemGet()
        {
            try
            {
                var record = db.Products.ToList();
                for (int i = 0; i < record.Count(); i++)
                {
                    var getStock = db.InventoryStocks.ToList().Where(x => x.ProductId == record[i].Id).FirstOrDefault();
                    if (getStock != null)
                    {
                        record[i].Stock.ProductId = getStock.ProductId;
                        record[i].Stock.ItemCode = getStock.ItemCode;
                        record[i].Stock.Quantity = getStock.Quantity;
                        record[i].Stock.ItemName = getStock.ItemName;
                        record[i].Stock.Sku = getStock.Sku;
                        record[i].Stock.ItemBarCode = getStock.ItemBarCode;
                        record[i].Stock.StockId = getStock.StockId;
                    }
                    var getFinancial = db.Financials.AsQueryable().Where(x => x.ItemId == record[i].Id).FirstOrDefault();
                    if (getFinancial != null)
                    {
                        record[i].Financial = new Financial();
                        record[i].Financial.ItemName = getFinancial.ItemName;
                        record[i].Financial.ItemId = getFinancial.ItemId;
                        record[i].Financial.ItemNumber = getFinancial.ItemNumber;
                        record[i].Financial.Quantity = getFinancial.Quantity;
                        record[i].Financial.Cost = getFinancial.Cost;
                        record[i].Financial.Profit = getFinancial.Profit;
                        record[i].Financial.MsgPromotion = getFinancial.MsgPromotion;
                        record[i].Financial.AddToCost = getFinancial.AddToCost;
                        record[i].Financial.ItemId = getFinancial.ItemId;
                        record[i].Financial.FixedCost = getFinancial.FixedCost;
                        record[i].Financial.CostPerQuantity = getFinancial.CostPerQuantity;
                        record[i].Financial.St = getFinancial.St;
                        record[i].Financial.Tax = getFinancial.Tax;
                        record[i].Financial.OutOfStateCost = getFinancial.OutOfStateCost;
                        record[i].Financial.OutOfStateRetail = getFinancial.OutOfStateRetail;
                        record[i].Financial.Price = getFinancial.Price;
                        record[i].Financial.Quantity = getFinancial.Quantity;
                        record[i].Financial.QuantityPrice = getFinancial.QuantityPrice;
                        record[i].Financial.SuggestedRetailPrice = getFinancial.SuggestedRetailPrice;
                        record[i].Financial.AutoSetSrp = getFinancial.AutoSetSrp;
                        record[i].Financial.QuantityInStock = getFinancial.MsgPromotion;
                        record[i].Financial.Adjustment = getFinancial.Adjustment;
                        record[i].Financial.AskForPricing = getFinancial.AskForPricing;
                        record[i].Financial.AskForDescrip = getFinancial.AskForDescrip;
                        record[i].Financial.Serialized = getFinancial.Serialized;
                        record[i].Financial.TaxOnSales = getFinancial.TaxOnSales;
                        record[i].Financial.Purchase = getFinancial.Purchase;
                        record[i].Financial.NoSuchDiscount = getFinancial.NoSuchDiscount;
                        record[i].Financial.NoReturns = getFinancial.NoReturns;
                        record[i].Financial.SellBelowCost = getFinancial.SellBelowCost;
                        record[i].Financial.OutOfState = getFinancial.OutOfState;
                        record[i].Financial.CodeA = getFinancial.CodeA;
                        record[i].Financial.CodeB = getFinancial.CodeB;
                        record[i].Financial.CodeC = getFinancial.CodeC;
                        record[i].Financial.CodeD = getFinancial.CodeD;
                        record[i].Financial.AddCustomersDiscount = getFinancial.AddCustomersDiscount;
                        record[i].Financial.Retail = getFinancial.Retail;

                    }

                }
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record.ToList().OrderBy(x => x.Name).ToList());
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

        [HttpGet("ItemFetch")]
        public IActionResult ItemFetch()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                var record = db.Products.ToList().OrderBy(x=>x.Name).ToList();
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

        [HttpPost("ItemCreate")]
        public async Task<IActionResult> ItemCreate(Product obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }


                bool checkname = db.Products.ToList().Exists(p => p.Name.Equals(obj.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                else
                {
                    var fullcode = "";
                    Product newitems = new Product();
                    var recordemp = db.Products.ToList();
                    if (recordemp.Count() > 0)
                    {
                        if (recordemp[0].ItemNumber != null && recordemp[0].ItemNumber != "string" && recordemp[0].ItemNumber != "")
                        {
                            int large, small;
                            int salesID = 0;
                            large = Convert.ToInt32(recordemp[0].ItemNumber.Split('-')[1]);
                            small = Convert.ToInt32(recordemp[0].ItemNumber.Split('-')[1]);
                            for (int i = 0; i < recordemp.Count(); i++)
                            {
                                if (recordemp[i].ItemNumber != null)
                                {
                                    var t = Convert.ToInt32(recordemp[i].ItemNumber.Split('-')[1]);
                                    if (Convert.ToInt32(recordemp[i].ItemNumber.Split('-')[1]) > large)
                                    {
                                        salesID = Convert.ToInt32(recordemp[i].Id);
                                        large = Convert.ToInt32(recordemp[i].ItemNumber.Split('-')[1]);

                                    }
                                    else if (Convert.ToInt32(recordemp[i].ItemNumber.Split('-')[1]) < small)
                                    {
                                        small = Convert.ToInt32(recordemp[i].ItemNumber.Split('-')[1]);
                                    }
                                    else
                                    {
                                        if (large < 2)
                                        {
                                            salesID = Convert.ToInt32(recordemp[i].Id);
                                        }
                                    }
                                }
                            }
                            newitems = recordemp.ToList().Where(x => x.Id == salesID).FirstOrDefault();
                            if (newitems != null)
                            {
                                if (newitems.ItemNumber != null)
                                {
                                    var VcodeSplit = newitems.ItemNumber.Split('-');
                                    int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                    fullcode = "00" + "-" + Convert.ToString(code);
                                }
                                else
                                {
                                    fullcode = "00" + "-" + "1";
                                }
                            }
                            else
                            {
                                fullcode = "00" + "-" + "1";
                            }
                        }
                        else
                        {
                            fullcode = "00" + "-" + "1";
                        }
                    }
                    else
                    {
                        fullcode = "00" + "-" + "1";
                    }
                    if (obj.ItemImage != null)
                    {
                        //  return BadRequest("Image Path Required Instead of Byte");
                        var imgPath = SaveImage(obj.ItemImage, Guid.NewGuid().ToString());
                        obj.ItemImageByPath = imgPath;
                        obj.ItemImage = null;
                    }

                    var getcategory = db.ItemCategories.Find(obj.ItemCategoryId);
                    if (getcategory != null)
                    {
                        obj.CategoryName = getcategory.Name;
                    }
                    var getsubcat = db.ItemSubCategories.Find(obj.ItemSubCategoryId);
                    if (getsubcat != null)
                    {
                        obj.SubCatName = getsubcat.SubCategory;
                    }
                    var getmispick = db.MisPicks.Find(obj.MisPickId);
                    if (getmispick != null)
                    {
                        obj.MisPickName = getmispick.MisPickName;
                    }
                    var getmodel = db.Models.Find(obj.ModelId);
                    if (getmodel != null)
                    {
                        obj.ModelName = getmodel.Name;
                    }
                    var getbrand = db.Brands.Find(obj.BrandId);
                    if (getbrand != null)
                    {
                        obj.BrandName = getbrand.Name;
                    }
                    var getcolor = db.Colors.Find(obj.ColorId);
                    if (getcolor != null)
                    {
                        obj.ColorName = getcolor.Name;
                    }

                    if (obj.TaxExempt == null)
                    {
                        obj.TaxExempt = false;
                    }
                    if (obj.ShippingEnable == null)
                    {
                        obj.ShippingEnable = false;
                    }
                    if (obj.AllowECommerce == null)
                    {
                        obj.AllowECommerce = false;
                    }
                    if (obj.TaxonSale == null)
                    {
                        obj.TaxonSale = false;
                    }
                    if (obj.TaxOnPurchase == null)
                    {
                        obj.TaxOnPurchase = false;
                    }
                    obj.ItemNumber = fullcode;
                    db.Products.Add(obj);
                    await db.SaveChangesAsync();
                    var getcurrentitem = db.Products.ToList().Where(x => x.ItemNumber == fullcode && x.Name == obj.Name).FirstOrDefault();
                    if (getcurrentitem != null)
                    {
                        Financial savefinancial = new Financial();
                        savefinancial.ItemNumber = getcurrentitem.ItemNumber;
                        savefinancial.ItemName = getcurrentitem.Name;
                        savefinancial.Retail = getcurrentitem.Retail;
                        savefinancial.ItemId = getcurrentitem.Id;
                        savefinancial.FinancialId = obj.Financial.FinancialId;
                        savefinancial.Cost = obj.Financial.Cost;
                        savefinancial.Profit = obj.Financial.Profit;
                        savefinancial.MsgPromotion = obj.Financial.MsgPromotion;
                        savefinancial.AddToCost = obj.Financial.AddToCost;
                        savefinancial.UnitCharge = obj.Financial.UnitCharge;
                        if (obj.Financial.FixedCost != null)
                        {
                            savefinancial.FixedCost = obj.Financial.FixedCost;

                        }
                        else
                        {
                            savefinancial.FixedCost = false;
                        }
                        savefinancial.St = obj.Financial.St;
                        savefinancial.Tax = obj.Financial.Tax;
                        savefinancial.OutOfStateCost = obj.Financial.OutOfStateCost;
                        savefinancial.OutOfStateRetail = obj.Financial.OutOfStateRetail;
                        savefinancial.Price = obj.Financial.Price;
                        savefinancial.Quantity = obj.Financial.Quantity;
                        savefinancial.QuantityPrice = obj.Financial.QuantityPrice;
                        savefinancial.SuggestedRetailPrice = obj.Financial.SuggestedRetailPrice;

                        if (obj.Financial.AutoSetSrp != null)
                        {
                            savefinancial.AutoSetSrp = obj.Financial.AutoSetSrp;
                        }
                        else
                        {
                            savefinancial.AutoSetSrp = false;
                        }

                        //     savefinancial.ItemNumber = obj.Financial.ItemNumber;
                        savefinancial.QuantityInStock = obj.Financial.QuantityInStock;
                        savefinancial.Adjustment = obj.Financial.Adjustment;

                        if (obj.Financial.AskForPricing != null)
                        {
                            savefinancial.AskForPricing = obj.Financial.AskForPricing;
                        }
                        else
                        {
                            savefinancial.AskForPricing = false;
                        }


                        if (obj.Financial.AskForDescrip != null)
                        {
                            savefinancial.AskForDescrip = obj.Financial.AskForDescrip;
                        }
                        else
                        {
                            savefinancial.AskForDescrip = false;
                        }

                        if (obj.Financial.Serialized != null)
                        {
                            savefinancial.Serialized = obj.Financial.Serialized;
                        }
                        else
                        {
                            savefinancial.Serialized = false;
                        }

                        if (obj.Financial.TaxOnSales != null)
                        {
                            savefinancial.TaxOnSales = obj.Financial.TaxOnSales;
                        }
                        else
                        {
                            savefinancial.TaxOnSales = false;
                        }
                        if (obj.Financial.Purchase != null)
                        {
                            savefinancial.Purchase = obj.Financial.Purchase;
                        }
                        else
                        {
                            savefinancial.Purchase = false;
                        }
                        if (obj.Financial.NoSuchDiscount != null)
                        {
                            savefinancial.NoSuchDiscount = obj.Financial.NoSuchDiscount;
                        }
                        else
                        {
                            savefinancial.NoSuchDiscount = false;
                        }

                        if (obj.Financial.NoReturns != null)
                        {
                            savefinancial.NoReturns = obj.Financial.NoReturns;
                        }
                        else
                        {
                            savefinancial.NoReturns = false;
                        }

                        if (obj.Financial.SellBelowCost != null)
                        {
                            savefinancial.SellBelowCost = obj.Financial.SellBelowCost;
                        }
                        else
                        {
                            savefinancial.SellBelowCost = false;
                        }

                        savefinancial.OutOfState = obj.Financial.OutOfState;


                        if (obj.Financial.CodeA != null)
                        {
                            savefinancial.CodeA = obj.Financial.CodeA;
                        }
                        else
                        {
                            savefinancial.CodeA = false;
                        }
                        if (obj.Financial.CodeB != null)
                        {
                            savefinancial.CodeB = obj.Financial.CodeB;
                        }
                        else
                        {
                            savefinancial.CodeB = false;
                        }
                        if (obj.Financial.CodeC != null)
                        {
                            savefinancial.CodeC = obj.Financial.CodeC;
                        }
                        else
                        {
                            savefinancial.CodeC = false;
                        }
                        if (obj.Financial.CodeD != null)
                        {
                            savefinancial.CodeD = obj.Financial.CodeD;
                        }
                        else
                        {
                            savefinancial.CodeD = false;
                        }
                        if (obj.Financial.AddCustomersDiscount != null)
                        {
                            savefinancial.AddCustomersDiscount = obj.Financial.AddCustomersDiscount;
                        }
                        else
                        {
                            savefinancial.AddCustomersDiscount = false;
                        }


                        savefinancial.AddCustomersDiscount = obj.Financial.AddCustomersDiscount;


                        db.Financials.Add(savefinancial);
                        await db.SaveChangesAsync();
                    }
                    var getProductnew = db.Products.ToList().Where(x => x.ItemNumber == fullcode && x.Name == obj.Name).FirstOrDefault();
                    if (getProductnew != null)
                    {

                        ItemGetByIDWithStockAndFinancial(getProductnew.Id);

                    }

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
        [HttpGet("ItemGetByID/{id}")]
        public IActionResult ItemGetByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();

                var record = db.Products.Find(id);
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

        [HttpGet("ItemGetByIDWithStock/{id}")]
        public IActionResult ItemGetByIDWithStock(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                var record = db.Products.Find(id);
                if (record != null)
                {
                    var getStock = db.InventoryStocks.ToList().Where(x => x.ProductId == id).FirstOrDefault();
                    if (getStock != null)
                    {
                        record.Stock.ProductId = getStock.ProductId;
                        record.Stock.ItemCode = getStock.ItemCode;
                        record.Stock.Quantity = getStock.Quantity;
                        record.Stock.ItemName = getStock.ItemName;
                        record.Stock.Sku = getStock.Sku;
                        record.Stock.ItemBarCode = getStock.ItemBarCode;
                        record.Stock.StockId = getStock.StockId;
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest();
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


        [HttpPut("ItemUpdate/{id}")]
        public async Task<IActionResult> ItemUpdate(int id, Product data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != data.Id)
                {
                    return BadRequest();
                }

                bool isValid = db.Products.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);

                }
                else
                {
                    Financial newobjfinancial = null;
                    newobjfinancial = new Financial();

                    newobjfinancial = data.Financial;
                    var record = await db.Products.FindAsync(id);
                    if (data.Name != null && data.Name != "undefined")
                    {
                        record.Name = data.Name;
                    }
                    if (data.ProductCode != null && data.ProductCode != "undefined")
                    {
                        record.ProductCode = data.ProductCode;
                    }
                    if (data.BarCode != null && data.BarCode != "undefined")
                    {
                        record.BarCode = data.BarCode;
                    }
                    if (data.Size != null && data.Size != "undefined")
                    {
                        record.Size = data.Size;
                    }
                    if (data.Sku != null && data.Sku != "undefined")
                    {
                        record.Sku = data.Sku;
                    }
                    if (data.TaxOnPurchase != null)
                    {
                        record.TaxOnPurchase = data.TaxOnPurchase;
                    }
                    if (data.QtyinStock != null && data.QtyinStock != "undefined")
                    {
                        record.QtyinStock = data.QtyinStock;
                    }
                    if (data.ItemNumber != null && data.ItemNumber != "undefined")
                    {
                        record.ItemNumber = data.ItemNumber;
                    }
                    if (data.UnitCharge != null && data.UnitCharge != "undefined")
                    {
                        record.UnitCharge = data.UnitCharge;
                    }
                    if (data.OutofstateCost != null && data.OutofstateCost != "undefined")
                    {
                        record.OutofstateCost = data.OutofstateCost;
                    }
                    if (data.AddtoCostPercenatge != null && data.AddtoCostPercenatge != "undefined")
                    {
                        record.AddtoCostPercenatge = data.AddtoCostPercenatge;
                    }
                    if (data.UnitsInPack != null && data.UnitsInPack != "undefined")
                    {
                        record.UnitsInPack = data.UnitsInPack;
                    }
                    if (data.RetailPackPrice != null && data.RetailPackPrice != "undefined")
                    {
                        record.RetailPackPrice = data.RetailPackPrice;
                    }
                    if (data.SalesLimit != null && data.SalesLimit != "undefined")
                    {
                        record.SalesLimit = data.SalesLimit;
                    }
                    if (data.Cost != null && data.Cost != "undefined")
                    {
                        record.Cost = data.Cost;
                    }
                    if (data.UnitRetail != null && data.UnitRetail != "undefined")
                    {
                        record.UnitRetail = data.UnitRetail;
                    }
                    if (data.TaxExempt != null)
                    {
                        record.TaxExempt = data.TaxExempt;
                    }
                    if (data.ShippingEnable != null)
                    {
                        record.ShippingEnable = data.ShippingEnable;
                    }
                    if (data.AllowECommerce != null)
                    {
                        record.AllowECommerce = data.AllowECommerce;
                    }
                    if (data.ShippingEnable != null)
                    {
                        record.ShippingEnable = data.ShippingEnable;
                    }
                    if (data.ItemCategoryId != null && data.ItemCategoryId > 0)
                    {
                        var getcategory = db.ItemCategories.Find(data.ItemCategoryId);
                        if (getcategory != null)
                        {
                            data.CategoryName = getcategory.Name;
                        }
                        record.ItemCategoryId = data.ItemCategoryId;
                    }

                    if (data.ItemSubCategoryId != null && data.ItemSubCategoryId > 0)
                    {
                        var getsubcat = db.ItemSubCategories.Find(data.ItemSubCategoryId);
                        if (getsubcat != null)
                        {
                            data.SubCatName = getsubcat.SubCategory;
                        }
                    }
                    if (data.ModelId != null && data.ModelId > 0)
                    {
                        var getmodel = db.Models.Find(data.ModelId);
                        if (getmodel != null)
                        {
                            data.ModelName = getmodel.Name;
                        }
                    }
                    if (data.BrandId != null && data.BrandId > 0)
                    {
                        var getbrand = db.Brands.Find(data.BrandId);
                        if (getbrand != null)
                        {
                            data.BrandName = getbrand.Name;
                        }
                    }
                    if (data.BrandId != null && data.BrandId > 0)
                    {
                        var getcolor = db.Colors.Find(data.ColorId);
                        if (getcolor != null)
                        {
                            data.ColorName = getcolor.Name;
                        }
                    }
                    if (data.MisPickId != null && data.MisPickId > 0)
                    {
                        var getmispick = db.MisPicks.Find(data.MisPickId);
                        if (getmispick != null)
                        {
                            data.MisPickName = getmispick.MisPickName;
                        }
                    }
                    if (data.OrderQuantity != null && data.OrderQuantity != "undefined")
                    {
                        record.OrderQuantity = data.OrderQuantity;
                    }
                    if (data.Units != null && data.Units != "undefined")
                    {
                        record.Units = data.Units;
                    }
                    if (data.WeightOz != null && data.WeightOz != "undefined")
                    {
                        record.WeightOz = data.WeightOz;
                    }
                    if (data.WeightLb != null && data.WeightLb != "undefined")
                    {
                        record.WeightLb = data.WeightLb;
                    }
                    if (data.LocationTwo != null && data.LocationTwo != "undefined")
                    {
                        record.LocationTwo = data.LocationTwo;
                    }
                    if (data.LocationThree != null && data.LocationThree != "undefined")
                    {
                        record.LocationThree = data.LocationThree;
                    }
                    if (data.MaintainStockForDays != null && data.MaintainStockForDays != "undefined")
                    {
                        record.MaintainStockForDays = data.MaintainStockForDays;
                    }
                    if (data.DiscountPrice != null && data.DiscountPrice != "undefined")
                    {
                        record.DiscountPrice = data.DiscountPrice;
                    }
                    if (data.Rating != null && data.Rating != "undefined")
                    {
                        record.Rating = data.Rating;
                    }
                    if (data.MinOrderQty != null && data.MinOrderQty != "undefined")
                    {
                        record.MinOrderQty = data.MinOrderQty;
                    }
                    if (data.MaxOrderQty != null && data.MaxOrderQty != "undefined")
                    {
                        record.MaxOrderQty = data.MaxOrderQty;
                    }
                    if (data.MaxOrderQty != null && data.MaxOrderQty != "undefined")
                    {
                        record.MaxOrderQty = data.MaxOrderQty;
                    }
                    if (data.SingleUnitMsa != null && data.SingleUnitMsa != "undefined")
                    {
                        record.SingleUnitMsa = data.SingleUnitMsa;
                    }
                    if (data.Variations != null && data.Variations != "undefined")
                    {
                        record.Variations = data.Variations;
                    }
                    if (data.Retail != null && data.Retail != "undefined")
                    {
                        if (data.Retail != record.Retail)
                        {
                            var oldrecord = db.Financials.ToList().Where(x => x.ItemId == data.Id).FirstOrDefault();
                            if(oldrecord != null)
                            {
                                oldrecord.Retail = data.Retail;
                                db.Entry(oldrecord).State = EntityState.Modified;
                            }
                        }
                        record.Retail = data.Retail;
                    }
                    data = record;
                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    var check = db.Financials.ToList();
                    var savefinancial = db.Financials.ToList().Where(x => x.ItemId == data.Id && x.ItemNumber == data.ItemNumber).FirstOrDefault();
                    //      data.Financial = new Financial();
                    if (newobjfinancial.Cost != null && newobjfinancial.Cost != "undefined")
                    {
                        savefinancial.Cost = newobjfinancial.Cost;
                    }
                    if (newobjfinancial.Profit != null && newobjfinancial.Profit != "undefined")
                    {
                        savefinancial.Profit = newobjfinancial.Profit;
                    }
                    if (newobjfinancial.MsgPromotion != null && newobjfinancial.MsgPromotion != "undefined")
                    {
                        savefinancial.MsgPromotion = newobjfinancial.MsgPromotion;
                    }
                    if (newobjfinancial.AddToCost != null && newobjfinancial.AddToCost != "undefined")
                    {
                        savefinancial.AddToCost = newobjfinancial.AddToCost;
                    }
                    if (newobjfinancial.UnitCharge != null && newobjfinancial.UnitCharge != "undefined")
                    {
                        savefinancial.UnitCharge = newobjfinancial.UnitCharge;
                    }
                    if (newobjfinancial.FixedCost != null)
                    {
                        savefinancial.FixedCost = newobjfinancial.FixedCost;
                    }


                    if (newobjfinancial.St != null && newobjfinancial.St != "undefined")
                    {
                        savefinancial.St = newobjfinancial.St;
                    }
                    if (newobjfinancial.Tax != null && newobjfinancial.Tax != "undefined")
                    {
                        savefinancial.Tax = newobjfinancial.Tax;
                    }
                    if (newobjfinancial.OutOfStateCost != null && newobjfinancial.OutOfStateCost != "undefined")
                    {
                        savefinancial.OutOfStateCost = newobjfinancial.OutOfStateCost;
                    }
                    if (newobjfinancial.OutOfStateRetail != null && newobjfinancial.OutOfStateRetail != "undefined")
                    {
                        savefinancial.OutOfStateRetail = newobjfinancial.OutOfStateRetail;
                    }
                    if (newobjfinancial.Price != null && newobjfinancial.Price != "undefined")
                    {
                        savefinancial.Price = newobjfinancial.Price;
                    }
                    if (newobjfinancial.Quantity != null && newobjfinancial.Quantity != "undefined")
                    {
                        savefinancial.Quantity = newobjfinancial.Quantity;
                    }
                    if (newobjfinancial.QuantityPrice != null && newobjfinancial.QuantityPrice != "undefined")
                    {
                        savefinancial.QuantityPrice = newobjfinancial.QuantityPrice;
                    }
                    if (newobjfinancial.SuggestedRetailPrice != null && newobjfinancial.SuggestedRetailPrice != "undefined")
                    {
                        savefinancial.SuggestedRetailPrice = newobjfinancial.SuggestedRetailPrice;
                    }

                    if (newobjfinancial.AutoSetSrp != null)
                    {
                        savefinancial.AutoSetSrp = newobjfinancial.AutoSetSrp;
                    }
                    if (newobjfinancial.QuantityInStock != null && newobjfinancial.QuantityInStock != "undefined")
                    {
                        savefinancial.QuantityInStock = newobjfinancial.QuantityInStock;
                    }
                    if (newobjfinancial.Adjustment != null && newobjfinancial.Adjustment != "undefined")
                    {
                        savefinancial.Adjustment = newobjfinancial.Adjustment;
                    }

                    if (newobjfinancial.AskForPricing != null)
                    {
                        savefinancial.AskForPricing = newobjfinancial.AskForPricing;
                    }


                    if (newobjfinancial.AskForDescrip != null)
                    {
                        savefinancial.AskForDescrip = newobjfinancial.AskForDescrip;
                    }


                    if (newobjfinancial.Serialized != null)
                    {
                        savefinancial.Serialized = newobjfinancial.Serialized;
                    }


                    if (newobjfinancial.TaxOnSales != null)
                    {
                        savefinancial.TaxOnSales = newobjfinancial.TaxOnSales;
                    }

                    if (newobjfinancial.Purchase != null)
                    {
                        savefinancial.Purchase = newobjfinancial.Purchase;
                    }

                    if (newobjfinancial.NoSuchDiscount != null)
                    {
                        savefinancial.NoSuchDiscount = newobjfinancial.NoSuchDiscount;
                    }


                    if (newobjfinancial.NoReturns != null)
                    {
                        savefinancial.NoReturns = newobjfinancial.NoReturns;
                    }


                    if (newobjfinancial.SellBelowCost != null)
                    {
                        savefinancial.SellBelowCost = newobjfinancial.SellBelowCost;
                    }


                    savefinancial.OutOfState = newobjfinancial.OutOfState;


                    if (newobjfinancial.CodeA != null)
                    {
                        savefinancial.CodeA = newobjfinancial.CodeA;
                    }

                    if (newobjfinancial.CodeB != null)
                    {
                        savefinancial.CodeB = newobjfinancial.CodeB;
                    }

                    if (newobjfinancial.CodeC != null)
                    {
                        savefinancial.CodeC = newobjfinancial.CodeC;
                    }

                    if (newobjfinancial.CodeD != null)
                    {
                        savefinancial.CodeD = newobjfinancial.CodeD;
                    }

                    if (newobjfinancial.AddCustomersDiscount != null)
                    {
                        savefinancial.AddCustomersDiscount = newobjfinancial.AddCustomersDiscount;
                    }
                    data.Financial = savefinancial;
                    db.Entry(data.Financial).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Ok(Response);
                }
                return BadRequest();
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

        [HttpDelete("DeleteItem/{id}")]
        public IActionResult DeleteItem(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                Product data = db.Products.Find(id);
                Financial getfinancial = db.Financials.ToList().Where(x => x.ItemId == id).FirstOrDefault();
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                if (getfinancial != null)
                {
                    db.Financials.Remove(getfinancial);
                    db.SaveChanges();

                }
                db.Products.Remove(data);
                db.SaveChanges();
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









        [HttpPost("SaleCreate")]
        public async Task<IActionResult> SaleCreate(List<PosSale> obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                InventoryStock stock = null;
                double grossamount = 0;
                //for (int a = 0; a < obj.Count(); a++)
                //{
                //    grossamount += Convert.ToDouble(obj[a].Total);
                //}

                grossamount = Convert.ToDouble(obj[0].InvoiceTotal);
                for (int i = 0; i < obj.Count(); i++)
                {
                    stock = new InventoryStock();
                    obj[i].InvoiceTotal = grossamount.ToString();
                    obj[i].InvoiceDate = DateTime.Now;
                    if (obj[i].CustomerId != null)
                    {
                        var getcustomername = db.CustomerInformations.Find(obj[i].CustomerId);
                        {
                            obj[i].CustomerName = getcustomername.FirstName;
                            obj[i].CustomerAccountNumber = getcustomername.AccountNumber;
                        }
                    }
                    db.PosSales.Add(obj[i]);
                    db.SaveChanges();
                    var getstock = db.InventoryStocks.ToList().Where(x => x.ProductId == obj[i].ItemId).FirstOrDefault();
                    if (getstock != null)
                    {
                        getstock.Quantity = (Convert.ToDouble(getstock.Quantity) - Convert.ToDouble(obj[i].Quantity)).ToString();
                        db.Entry(getstock).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                var getvendor = db.CustomerInformations.Find(obj[0].CustomerId);
                if (getvendor != null)
                {
                    var getaccount = db.Accounts.ToList().Where(a => a.Title == getvendor.AccountTitle && a.AccountId == getvendor.AccountId).FirstOrDefault();
                    var getCHaccount = db.Accounts.ToList().Where(a => a.Title == "Net Sales").FirstOrDefault();
                    var getCInHaccount = db.Accounts.ToList().Where(a => a.Title == "Cash in hand").FirstOrDefault();

                    for (int i = 0; i < 2; i++)
                    {
                        Transaction transaction = null;
                        transaction = new Transaction();
                        if (i == 0)
                        {
                            transaction.AccountName = getaccount.Title;
                            transaction.AccountNumber = getaccount.AccountId;
                            transaction.DetailAccountId = getaccount.AccountId;
                            transaction.Credit = "0.00";
                            transaction.Debit = grossamount.ToString();
                            transaction.InvoiceNumber = obj[0].InvoiceNumber;
                            transaction.Date = DateTime.Now;
                            transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                            db.Transactions.Add(transaction);
                            db.SaveChanges();

                        }
                        else
                        {
                            transaction.AccountName = getCHaccount.Title;
                            transaction.AccountNumber = getCHaccount.AccountId;
                            transaction.DetailAccountId = getCHaccount.AccountId;
                            transaction.Credit = grossamount.ToString();
                            transaction.Debit = "0.00";
                            transaction.InvoiceNumber = obj[0].InvoiceNumber;
                            transaction.Date = DateTime.Now;
                            transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                            db.Transactions.Add(transaction);
                            db.SaveChanges();
                        }
                    }

                    Receivable pay = null;
                    if (obj[0].OnCash == false)
                    {
                        pay = new Receivable();
                        if (getaccount != null)
                        {
                            var getpay = db.Receivables.ToList().Where(x => x.AccountId == getaccount.AccountId).FirstOrDefault();
                            if (getpay != null)
                            {
                                getpay.Amount = (Convert.ToDouble(getpay.Amount) + Convert.ToDouble(grossamount)).ToString();
                                db.Entry(getpay).State = EntityState.Modified;
                                db.SaveChanges();

                            }
                            else
                            {
                                pay.AccountId = getaccount.AccountId;
                                pay.AccountNumber = getaccount.AccountId;
                                pay.Amount = grossamount.ToString();
                                pay.AccountName = getaccount.Title;
                                db.Receivables.Add(pay);
                                db.SaveChanges();
                            }
                        }

                    }
                    else
                    {

                    }
                }
                await db.SaveChangesAsync();
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

        [HttpGet("SalePyment/{id}")]
        public async Task<IActionResult> SalePyment(int id)
        {
            try
            {
                double grossamount = 0;
                var getvendor = db.Customers.Find(id);
                if (getvendor != null)
                {
                    var getaccount = db.Accounts.ToList().Where(a => a.Title == getvendor.AccountTitle && a.AccountId == getvendor.AccountId).FirstOrDefault();
                    var getCHaccount = db.Accounts.ToList().Where(a => a.Title == "Net Sales").FirstOrDefault();
                    var getCInHaccount = db.Accounts.ToList().Where(a => a.Title == "Cash in hand").FirstOrDefault();
                    if (getaccount != null)
                    {

                        var fullcode = "";
                        Receiving newitems = new Receiving();
                        var recordemp = db.Receivings.ToList();
                        if (recordemp.Count() > 0)
                        {
                            if (recordemp[0].InvoiceNumber != null && recordemp[0].InvoiceNumber != "string" && recordemp[0].InvoiceNumber != "")
                            {
                                int large, small;
                                int salesID = 0;
                                large = Convert.ToInt32(recordemp[0].InvoiceNumber.Split('-')[1]);
                                small = Convert.ToInt32(recordemp[0].InvoiceNumber.Split('-')[1]);
                                for (int i = 0; i < recordemp.Count; i++)
                                {
                                    if (recordemp[i].InvoiceNumber != null)
                                    {
                                        var t = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);
                                        if (Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]) > large)
                                        {
                                            salesID = Convert.ToInt32(recordemp[i].ReceivingId);
                                            large = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);

                                        }
                                        else if (Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]) < small)
                                        {
                                            small = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);
                                        }
                                        else
                                        {
                                            if (large < 2)
                                            {
                                                salesID = Convert.ToInt32(recordemp[i].ReceivingId);
                                            }
                                        }
                                    }
                                }
                                newitems = recordemp.ToList().Where(x => x.ReceivingId == salesID).FirstOrDefault();
                                if (newitems != null)
                                {
                                    if (newitems.InvoiceNumber != null)
                                    {
                                        var VcodeSplit = newitems.InvoiceNumber.Split('-');
                                        int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                        fullcode = "RE00" + "-" + Convert.ToString(code);
                                    }
                                    else
                                    {
                                        fullcode = "RE00" + "-" + "1";
                                    }
                                }
                                else
                                {
                                    fullcode = "RE00" + "-" + "1";
                                }
                            }
                            else
                            {
                                fullcode = "RE00" + "-" + "1";
                            }
                        }
                        else
                        {
                            fullcode = "RE00" + "-" + "1";
                        }

                        Receiving receiving = null;
                        receiving = new Receiving();
                        receiving.Date = DateTime.Now;
                        receiving.DueDate = DateTime.Now;
                        receiving.AccountId = getaccount.AccountId;
                        receiving.AccountName = getaccount.Title;
                        receiving.AccountNumber = getaccount.AccountId;
                        receiving.InvoiceNumber = fullcode;
                        receiving.Debit = "0.00";
                        receiving.Credit = grossamount.ToString();
                        receiving.CashBalance = grossamount.ToString();
                        receiving.PaymentType = "Cash";
                        receiving.Note = "";
                        receiving.NetAmount = grossamount.ToString();
                        db.Receivings.Add(receiving);
                        await db.SaveChangesAsync();
                        for (int i = 0; i < 2; i++)
                        {
                            Transaction transaction = null;
                            transaction = new Transaction();
                            if (i == 0)
                            {
                                transaction.AccountName = getaccount.Title;
                                transaction.AccountNumber = getaccount.AccountId;
                                transaction.DetailAccountId = getaccount.AccountId;
                                transaction.Credit = grossamount.ToString();
                                transaction.Debit = "0.00";
                                transaction.InvoiceNumber = fullcode;
                                transaction.Date = DateTime.Now;
                                transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                db.Transactions.Add(transaction);
                                db.SaveChanges();

                            }
                            else
                            {
                                transaction.AccountName = getCInHaccount.Title;
                                transaction.AccountNumber = getCInHaccount.AccountId;
                                transaction.DetailAccountId = getCInHaccount.AccountId;
                                transaction.Credit = "0.00";
                                transaction.Debit = grossamount.ToString();
                                transaction.InvoiceNumber = fullcode;
                                transaction.Date = DateTime.Now;
                                transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                db.Transactions.Add(transaction);
                                db.SaveChanges();
                            }
                        }

                    }

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
        [HttpGet("SaleGet")]
        public IActionResult SaleGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                var record = db.PosSales.ToList();
                for (int i = 0; i < record.Count(); i++)
                {
                    if (record[i].SupervisorId != null)
                    {
                        var GetSupervisors = db.Supervisors.Find(record[i].SupervisorId);
                        if (GetSupervisors != null)
                        {
                            var getCurrentUser = db.AspNetUsers.Find(GetSupervisors.UserId);
                            if (getCurrentUser != null)
                            {
                                GetSupervisors.AspNetUser = getCurrentUser;
                            }
                            record[i].Supervisor = GetSupervisors;
                        }
                    }

                    if (record[i].SalesManagerId != null)
                    {
                        var GetSalesManager = db.SalesManagers.Find(record[i].SalesManagerId);
                        if (GetSalesManager != null)
                        {
                            var getCurrentUser = db.AspNetUsers.Find(GetSalesManager.UserId);
                            if (getCurrentUser != null)
                            {
                                GetSalesManager.AspNetUser = getCurrentUser;
                            }
                            record[i].SalesManager = GetSalesManager;

                        }
                    }
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
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


        [HttpDelete("DeleteSale/{invoice}")]
        public  IActionResult DeleteSale(string invoice)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Sale>();
                var data = db.Sales.ToList().Where(x => x.InvoiceNumber == invoice).ToList();

                if (data.Count() > 0)
                {
                    var gettransactions = db.Transactions.ToList().Where(x => x.InvoiceNumber == invoice).ToList();
                    for (int a = 0; a < gettransactions.Count(); a++)
                    {
                        db.Transactions.Remove(gettransactions[a]);
                        db.SaveChanges();
                    }
                    if (data[0].CustomerName != null)
                    {
                        var getAcc = db.Accounts.ToList().Where(x => x.Title == data[0].CustomerName).FirstOrDefault();
                        if (getAcc != null)
                        {
                            var getrecv = db.Receivables.ToList().Where(x => x.AccountId == getAcc.AccountId).FirstOrDefault();
                            if (getrecv != null)
                            {
                                if (Convert.ToDouble(getrecv.Amount) > Convert.ToDouble(data[0].GrossAmount))
                                {
                                    double rem = Convert.ToDouble(getrecv.Amount) - Convert.ToDouble(data[0].GrossAmount);
                                    getrecv.Amount = rem.ToString();
                                    db.Entry(getrecv).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    db.Receivables.Remove(getrecv);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    else if (data[0].CustomerId != null && data[0].CustomerName == null)
                    {

                        var getcus = db.Customers.Find(data[0].CustomerId);
                        var getIAcc = db.Accounts.ToList().Where(x => x.Title == getcus.AccountTitle).FirstOrDefault();
                        if (getIAcc != null)
                        {
                            var getrecv = db.Receivables.ToList().Where(x => x.AccountId == getIAcc.AccountId).FirstOrDefault();
                            if (getrecv != null)
                            {
                                if (Convert.ToDouble(getrecv.Amount) > Convert.ToDouble(data[0].GrossAmount))
                                {
                                    double rem = Convert.ToDouble(getrecv.Amount) - Convert.ToDouble(data[0].GrossAmount);
                                    getrecv.Amount = rem.ToString();
                                    db.Entry(getrecv).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    db.Receivables.Remove(getrecv);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < data.Count(); i++)
                {
                    db.Sales.Remove(data[i]);
                    db.SaveChanges();
                }
                if (data.Count() < 1)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
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



        [HttpDelete("DeletePosSale/{invoice}")]
        public IActionResult DeletePosSale(string invoice)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Sale>();
                var data = db.PosSales.ToList().Where(x => x.InvoiceNumber == invoice).ToList();

                if (data.Count() > 0)
                {
                    var gettransactions = db.Transactions.ToList().Where(x => x.InvoiceNumber == invoice).ToList();
                    for (int a = 0; a < gettransactions.Count(); a++)
                    {
                        db.Transactions.Remove(gettransactions[a]);
                        db.SaveChanges();
                    }
                    if (data[0].CustomerName != null)
                    {
                        var getAcc = db.Accounts.ToList().Where(x => x.Title == data[0].CustomerName).FirstOrDefault();
                        if (getAcc != null)
                        {
                            var getrecv = db.Receivables.ToList().Where(x => x.AccountId == getAcc.AccountId).FirstOrDefault();
                            if (getrecv != null)
                            {
                                if (Convert.ToDouble(getrecv.Amount) > Convert.ToDouble(data[0].InvoiceTotal))
                                {
                                    double rem = Convert.ToDouble(getrecv.Amount) - Convert.ToDouble(data[0].InvoiceTotal);
                                    getrecv.Amount = rem.ToString();
                                    db.Entry(getrecv).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    db.Receivables.Remove(getrecv);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    else if (data[0].CustomerId != null && data[0].CustomerName == null)
                    {

                        var getcus = db.Customers.Find(data[0].CustomerId);
                        var getIAcc = db.Accounts.ToList().Where(x => x.Title == getcus.AccountTitle).FirstOrDefault();
                        if (getIAcc != null)
                        {
                            var getrecv = db.Receivables.ToList().Where(x => x.AccountId == getIAcc.AccountId).FirstOrDefault();
                            if (getrecv != null)
                            {
                                if (Convert.ToDouble(getrecv.Amount) > Convert.ToDouble(data[0].InvoiceTotal))
                                {
                                    double rem = Convert.ToDouble(getrecv.Amount) - Convert.ToDouble(data[0].InvoiceTotal);
                                    getrecv.Amount = rem.ToString();
                                    db.Entry(getrecv).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    db.Receivables.Remove(getrecv);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < data.Count(); i++)
                {
                    db.PosSales.Remove(data[i]);
                    db.SaveChanges();
                }
                if (data.Count() < 1)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
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


        [HttpGet("ItemGetWithStockAndFinancialByItemnumber/{itemnumber}")]
        public IActionResult ItemGetWithStockAndFinancialByItemnumber(string itemnumber)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                var record = db.Products.ToList().Where(x => x.ItemNumber == itemnumber).FirstOrDefault();
                if (record != null)
                {
                    var getStock = db.InventoryStocks.ToList().Where(x => x.ProductId == record.Id).FirstOrDefault();
                    if (getStock != null)
                    {
                        record.Stock.ProductId = getStock.ProductId;
                        record.Stock.ItemCode = getStock.ItemCode;
                        record.Stock.Quantity = getStock.Quantity;
                        record.Stock.ItemName = getStock.ItemName;
                        record.Stock.Sku = getStock.Sku;
                        record.Stock.ItemBarCode = getStock.ItemBarCode;
                        record.Stock.StockId = getStock.StockId;
                    }
                    var getFinancial = db.Financials.ToList().Where(x => x.ItemId == record.Id).FirstOrDefault();
                    if (getFinancial != null)

                    {
                        record.Financial = new Financial();
                        record.Financial.ItemName = getFinancial.ItemName;
                        record.Financial.ItemId = getFinancial.ItemId;
                        record.Financial.ItemNumber = getFinancial.ItemNumber;
                        record.Financial.Quantity = getFinancial.Quantity;
                        record.Financial.Cost = getFinancial.Cost;
                        record.Financial.Profit = getFinancial.Profit;
                        record.Financial.MsgPromotion = getFinancial.MsgPromotion;
                        record.Financial.AddToCost = getFinancial.AddToCost;
                        record.Financial.ItemId = getFinancial.ItemId;
                        record.Financial.FixedCost = getFinancial.FixedCost;
                        record.Financial.CostPerQuantity = getFinancial.CostPerQuantity;
                        record.Financial.St = getFinancial.St;
                        record.Financial.Tax = getFinancial.Tax;
                        record.Financial.OutOfStateCost = getFinancial.OutOfStateCost;
                        record.Financial.OutOfStateRetail = getFinancial.OutOfStateRetail;
                        record.Financial.Price = getFinancial.Price;
                        record.Financial.Quantity = getFinancial.Quantity;
                        record.Financial.QuantityPrice = getFinancial.QuantityPrice;
                        record.Financial.SuggestedRetailPrice = getFinancial.SuggestedRetailPrice;
                        record.Financial.AutoSetSrp = getFinancial.AutoSetSrp;
                        record.Financial.QuantityInStock = getFinancial.MsgPromotion;
                        record.Financial.Adjustment = getFinancial.Adjustment;
                        record.Financial.AskForPricing = getFinancial.AskForPricing;
                        record.Financial.AskForDescrip = getFinancial.AskForDescrip;
                        record.Financial.Serialized = getFinancial.Serialized;
                        record.Financial.TaxOnSales = getFinancial.TaxOnSales;
                        record.Financial.Purchase = getFinancial.Purchase;
                        record.Financial.NoSuchDiscount = getFinancial.NoSuchDiscount;
                        record.Financial.NoReturns = getFinancial.NoReturns;
                        record.Financial.SellBelowCost = getFinancial.SellBelowCost;
                        record.Financial.OutOfState = getFinancial.OutOfState;
                        record.Financial.CodeA = getFinancial.CodeA;
                        record.Financial.CodeB = getFinancial.CodeB;
                        record.Financial.CodeC = getFinancial.CodeC;
                        record.Financial.CodeD = getFinancial.CodeD;
                        record.Financial.AddCustomersDiscount = getFinancial.AddCustomersDiscount;
                        record.Financial.Retail = getFinancial.Retail;

                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest();
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
                            ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, infodata);
                        }
                        else
                        {
                            infodata.CustomerClassification = null;
                            ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                        }
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
                    var getpay = db.Receivables.ToList().Where(x => x.AccountId == record.AccountId).FirstOrDefault();
                    var classificationdata = db.CustomerClassifications.Where(x => x.CustomerInfoId == record.Id && x.CustomerCode == record.CustomerCode).FirstOrDefault();
                    if (classificationdata != null)
                    {
                        record.CustomerClassification = classificationdata;
                        if (getpay != null)
                        {

                            record.Balance = getpay.Amount;
                        }

                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    }
                    else
                    {
                        record.CustomerClassification = null;
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CustomerInformationGet")]
        public IActionResult CustomerInformationGet()
        {
            try
            {
                var inforecord = db.CustomerInformations.ToList();
                List<CustomerInformation> record = new List<CustomerInformation>();
                CustomerInformation customerinformation = new CustomerInformation();
                if (inforecord != null)
                {
                    foreach (var item in inforecord)
                    {
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
                        customerinformation.AccountNumber = item.AccountNumber;
                        customerinformation.AccountId = item.AccountId;
                        customerinformation.AccountTitle = item.AccountTitle;
                        var classificationrecord = db.CustomerClassifications.Where(x => x.CustomerInfoId == customerinformation.Id).FirstOrDefault();
                        customerinformation.CustomerClassification = classificationrecord;
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



        //Item End

        [HttpPost("OngoingSaleCreate")]
        public async Task<IActionResult> OngoingSaleCreate(List<OngoingSaleInvoice> obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<OngoingSaleInvoice>>();

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var getpossales = db.OngoingSaleInvoices.ToList().Where(x => x.CurrentInvoiceNumber == obj[0].CurrentInvoiceNumber).ToList();
                if (getpossales.Count() > 0)
                {

                    for (int a = 0; a < getpossales.Count(); a++)
                    {
                        db.OngoingSaleInvoices.Remove(getpossales[a]);
                        db.SaveChanges();
                    }
                }

                //    InventoryStock stock = null;
                double grossamount = 0;
                grossamount = Convert.ToDouble(obj[0].InvoiceTotal);
                for (int i = 0; i < obj.Count(); i++)
                {
                    // stock = new InventoryStock();
                    obj[i].InvoiceTotal = grossamount.ToString();
                    obj[i].InvoiceDate = DateTime.Now;
                    if (obj[i].CustomerId != null)
                    {
                        var getcustomername = db.CustomerInformations.Find(obj[i].CustomerId);
                        {
                            obj[i].CustomerName = getcustomername.FirstName;
                        }
                    }
                    db.OngoingSaleInvoices.Add(obj[i]);
                    db.SaveChanges();
                }
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<OngoingSaleInvoice>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        //OngoingSale

        [HttpGet("OnGoingSaleGet")]
        public IActionResult OnGoingSaleGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<OngoingSaleInvoice>>();
                var record = db.OngoingSaleInvoices.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<OngoingSaleInvoice>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("ItemGetWithStockAndFinancialByItemName/{itemname}")]
        public IActionResult ItemGetWithStockAndFinancialByItemName(string itemname)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();

                var record = db.Products.ToList().Where(x => x.Name.Equals(itemname, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (record != null)
                {
                    var getStock = db.InventoryStocks.ToList().Where(x => x.ProductId == record.Id).FirstOrDefault();
                    if (getStock != null)
                    {
                        record.Stock.ProductId = getStock.ProductId;
                        record.Stock.ItemCode = getStock.ItemCode;
                        record.Stock.Quantity = getStock.Quantity;
                        record.Stock.ItemName = getStock.ItemName;
                        record.Stock.Sku = getStock.Sku;
                        record.Stock.ItemBarCode = getStock.ItemBarCode;
                        record.Stock.StockId = getStock.StockId;
                    }
                    var getFinancial = db.Financials.ToList().Where(x => x.ItemId == record.Id).FirstOrDefault();
                    if (getFinancial != null)

                    {
                        record.Financial = new Financial();
                        record.Financial.ItemName = getFinancial.ItemName;
                        record.Financial.ItemId = getFinancial.ItemId;
                        record.Financial.ItemNumber = getFinancial.ItemNumber;
                        record.Financial.Quantity = getFinancial.Quantity;
                        record.Financial.Cost = getFinancial.Cost;
                        record.Financial.Profit = getFinancial.Profit;
                        record.Financial.MsgPromotion = getFinancial.MsgPromotion;
                        record.Financial.AddToCost = getFinancial.AddToCost;
                        record.Financial.ItemId = getFinancial.ItemId;
                        record.Financial.FixedCost = getFinancial.FixedCost;
                        record.Financial.CostPerQuantity = getFinancial.CostPerQuantity;
                        record.Financial.St = getFinancial.St;
                        record.Financial.Tax = getFinancial.Tax;
                        record.Financial.OutOfStateCost = getFinancial.OutOfStateCost;
                        record.Financial.OutOfStateRetail = getFinancial.OutOfStateRetail;
                        record.Financial.Price = getFinancial.Price;
                        record.Financial.Quantity = getFinancial.Quantity;
                        record.Financial.QuantityPrice = getFinancial.QuantityPrice;
                        record.Financial.SuggestedRetailPrice = getFinancial.SuggestedRetailPrice;
                        record.Financial.AutoSetSrp = getFinancial.AutoSetSrp;
                        record.Financial.QuantityInStock = getFinancial.MsgPromotion;
                        record.Financial.Adjustment = getFinancial.Adjustment;
                        record.Financial.AskForPricing = getFinancial.AskForPricing;
                        record.Financial.AskForDescrip = getFinancial.AskForDescrip;
                        record.Financial.Serialized = getFinancial.Serialized;
                        record.Financial.TaxOnSales = getFinancial.TaxOnSales;
                        record.Financial.Purchase = getFinancial.Purchase;
                        record.Financial.NoSuchDiscount = getFinancial.NoSuchDiscount;
                        record.Financial.NoReturns = getFinancial.NoReturns;
                        record.Financial.SellBelowCost = getFinancial.SellBelowCost;
                        record.Financial.OutOfState = getFinancial.OutOfState;
                        record.Financial.CodeA = getFinancial.CodeA;
                        record.Financial.CodeB = getFinancial.CodeB;
                        record.Financial.CodeC = getFinancial.CodeC;
                        record.Financial.CodeD = getFinancial.CodeD;
                        record.Financial.AddCustomersDiscount = getFinancial.AddCustomersDiscount;
                        record.Financial.Retail = getFinancial.Retail;

                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest();
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

        [HttpGet("GetStockByItemNumber/{itemnumber}")]
        public IActionResult GetStockByItemNumber(string itemnumber)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<InventoryStock>();

                var record = db.InventoryStocks.ToList().Where(x => x.StockItemNumber.Equals(itemnumber, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (record != null)
                {
                    var getitems = db.Products.ToList().Where(x => x.Id == record.ProductId).FirstOrDefault();
                    if (getitems != null)
                    {
                        record.items = new Product();
                        record.items.Id = getitems.Id;
                        record.items.ItemNumber = getitems.ItemNumber;
                        record.items.Name = getitems.Name;
                        record.items.Sku = getitems.Sku;
                        record.items.BarCode = getitems.BarCode;
                        record.items.Retail = getitems.Retail;
                        record.items.Cost = getitems.Cost;

                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    return Ok(Response);
                }

                return BadRequest();


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
        [HttpPost("SaveSupplierItemNumber")]
        public IActionResult SaveSupplierItemNumber(SupplierItemNumber obj)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<SupplierItemNumber>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.SupplierItemNumbers.ToList().Exists(p => p.SupplierItemNum.Equals(obj.SupplierItemNum, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                obj.CreatedDate = DateTime.Now;
                db.SupplierItemNumbers.Add(obj);
                db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SupplierItemNumber>();
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
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
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

        [HttpGet("ItemGetByCategoryID/{id}")]
        public IActionResult ItemGetByCategoryID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();

                var record = db.Products.ToList().Where(x=>x.ItemCategoryId == id);
                if (record != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record.ToList());
                    return Ok(Response);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }

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

        [HttpGet("ItemGetBySubCategoryID/{id}")]
        public IActionResult ItemGetBySubCategoryID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();

                var record = db.Products.ToList().Where(x => x.ItemSubCategoryId == id);
                if (record != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record.ToList());
                    return Ok(Response);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }

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

        //[HttpGet("ItemGetByName")]
        //public IActionResult ItemGetByName(string name)
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<List<Product>>();

        //        var record = db.Products.ToList().Where(x => x.Name.Contains("'%" + name + "%'")).ToList();
        //        if (record != null)
        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record.ToList());
        //            return Ok(Response);

        //        }
        //        else
        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
        //            return Ok(Response);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet("ItemGetByName")]
        public IActionResult ItemGetByName(string name)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();

                var record = db.Products.ToList().Where(x => x.Name.ToUpper().Contains(name.ToUpper())).ToList();
                if (record != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record.ToList());
                    return Ok(Response);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }

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

        [HttpGet("ItemGetByNamePagination")]
        public IActionResult ItemGetByNamePagination(int skipcount, int getcount)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();


                var getrecord = db.Products.ToList().Skip(skipcount).Take(getcount).ToList();

                if (getrecord != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, getrecord.ToList().OrderBy(x=>x.Name).ToList());
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }

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
        /////

    }

}

