using ABC.EFCore.Entities.POS;
using ABC.EFCore.Repository.Edmx;
using ABC.POS.API.ViewModel;
using ABC.Shared;
using ABC.Shared.DataConfig;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBarcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using StatusCodes = ABC.Shared.DataConfig.StatusCodes;
using Vendor = ABC.EFCore.Repository.Edmx.Vendor;

namespace ABC.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class InventoryController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        protected readonly EmailService emailService = new EmailService();

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
                var record = db.ItemCategories.ToList();
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
                    return Ok("Already Exists");
                }
                else
                {
                    db.ItemCategories.Add(users);
                    await db.SaveChangesAsync();
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
                var record = db.ItemSubCategories.ToList();
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
                    return Ok("Already Exists");
                }
                else
                {
                    db.ItemSubCategories.Add(users);
                    await db.SaveChangesAsync();
                    return Ok(Response);
                }

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
                    obj.AccountId = actrecord.AccountId;
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
                //imgPath = "https://localhost:5001/images/" + imageName;
                //imgPath = "http://45.35.97.246:5595/abcdposapi/images/" + imageName;
                imgPath = "http://172.107.33.98:5595/abcdposapi/images/" + imageName;
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
                // imgPath = "https://localhost:5001/images/" + imageName;
                // imgPath = "http://45.35.97.246:5595/abcdposapi/images/" + imageName;
                imgPath = "http://172.107.33.98:5595/abcdposapi/images/" + imageName;

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
                //imgPath = "http://45.35.97.246/abcdposapi/images/documents/" + imageName;
                //imgPath = "http://172.107.33.98:5595/pos/images/documents/" + imageName;
                imgPath = "http://172.107.33.98:5595/abcdposapi/images/documents/" + imageName;
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
                //imgPath = "http://45.35.97.246/abcdposapi/images/documents/" + imageName;
                imgPath = "http://172.107.33.98:5595/abcdposapi/images/documents/" + imageName;
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
                var Response = ResponseBuilder.BuildWSResponse<List<Vendor>>();
                var record = db.Vendors.ToList();
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
                var list = db.Vendors.ToList().Where(x => x.Email == obj.Email)?.FirstOrDefault();
                var isValid = false;
                if (list != null)
                {
                    isValid = true;
                }
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
                        obj.AccountId = obj.AccountNumber;
                        obj.AccountTitle = obj.FullName;
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
                                obj.AccountId = obj.AccountNumber;
                                obj.AccountTitle = obj.FullName;
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
                    if (record != null && data != null)
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
                    var getnote = db.CustomerNotes.ToList().Where(x => x.CustomerId == Convert.ToInt32(record.VendorCode)).FirstOrDefault();
                    if (getnote != null)
                    {
                        record.CustomerNote = getnote.CustomerNote1;
                        record.NoteDate = Convert.ToString(getnote.NoteDate);
                    }
                    var getpaying = db.Payings.ToList().Where(x => x.AccountId == record.AccountId && x.PaymentType == "Cheque").LastOrDefault();
                    if (getpaying != null)
                    {
                        if (getpaying.CheckNumber == null)
                        {
                            record.LastChequeNum = "";
                        }
                        else
                        {
                            record.LastChequeNum = getpaying.CheckNumber;
                        }
                    }
                    else
                    {
                        record.LastChequeNum = "";
                    }


                    if (record.VenderType != null)
                    {
                        var GetType = db.SupplierTypes.ToList().Where(x => x.VendorType == record.VenderType).FirstOrDefault();
                        if (GetType != null)
                        {
                            record.VenderType = GetType.VendorId.ToString();
                        }
                    }
                    if (record.Ledger != null)
                    {
                        var GetType = db.LedgerCategories.ToList().Where(x => x.Category == record.Ledger).FirstOrDefault();
                        if (GetType != null)
                        {
                            record.Ledger = GetType.Ledger.ToString();
                        }
                    }
                    if (record.AccountId == null)
                    {
                        Account objacount = null;
                        objacount = new Account();

                        Account customeracc = null;
                        customeracc = new Account();
                        var subaccrecord = db.AccountSubGroups.ToList().Where(x => x.Title == "Suppliers").FirstOrDefault();
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
                                if (getcode > 99)
                                {
                                    objacount.AccountId = subaccrecord.AccountSubGroupId + "-0" + Convert.ToString(getcode);

                                }
                                else if (getcode > 9)
                                {
                                    objacount.AccountId = subaccrecord.AccountSubGroupId + "-00" + Convert.ToString(getcode);
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
                        record.PayablesFound = "0.00";
                    }
                    else
                    {
                        var getpayables = db.Payables.ToList().Where(x => x.AccountId == record.AccountId).LastOrDefault();
                        if (getpayables != null)
                        {
                            record.PayablesFound = getpayables.Amount;
                        }
                        else
                        {
                            record.PayablesFound = "0.00";
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
                    var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("OptimizedVendors")]
        public IActionResult OptimizedVendors()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Vendor>>();
                List<Vendor> dateObject = new List<Vendor>();
                dateObject = (from data in db.Vendors
                             orderby data.Company
                             select new Vendor  { VendorId = data.VendorId, Company= data.Company }).ToList();
              ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, dateObject);
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

                if (document.DocumentTypeId != null)
                {
                    var foundtype = db.SupplierDocumentTypes.Find(Convert.ToInt32(document.DocumentTypeId));
                    if (foundtype != null)
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
        //[HttpGet("LastSupplierAccountGet")]
        //public IActionResult LastSupplierAccountGet()
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<Account>();

        //        var record = db.AccountSubGroups.ToList().Where(x => x.Title == "Suppliers").FirstOrDefault();
        //        if (record != null)
        //        {
        //            var getAccount = db.Accounts.ToList().Where(x => x.AccountSubGroupId == record.AccountSubGroupId).LastOrDefault();
        //            if (getAccount != null)
        //            {
        //                if (getAccount.AccountSubGroup != null)
        //                {
        //                    getAccount.AccountSubGroup = null;
        //                }
        //                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, getAccount);
        //            }
        //            else
        //            {
        //                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);

        //            }

        //        }
        //        return Ok(Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<Account>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }

        //}

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
                    if (record.CategoryName != null)
                    {
                        var GetCat = db.ItemCategories.ToList().Where(x => x.Name == record.CategoryName).FirstOrDefault();
                        if(GetCat != null)
                        {
                            record.ItemCategoryId = GetCat.Id;
                        }
                    }
                    if (record.SubCatName != null)
                    {
                        var GetCat = db.ItemSubCategories.ToList().Where(x => x.SubCategory == record.SubCatName).FirstOrDefault();
                        if(GetCat != null)
                        {
                            record.ItemSubCategoryId = GetCat.Id;
                        }
                    }
                    if (record.BrandName != null)
                    {
                        var GetCat = db.Brands.ToList().Where(x => x.Name == record.BrandName).FirstOrDefault();
                        if (GetCat != null)
                        {
                            record.BrandId = GetCat.Id;
                        }
                    }
                    if (record.Family != null)
                    {
                        var GetCat = db.ArticleTypes.ToList().Where(x => x.ArticleTypeName == record.Family).FirstOrDefault();
                        if (GetCat != null)
                        {
                            record.FamilyId = GetCat.Id;
                        }
                    }
                    if (record.GroupName != null)
                    {
                        var GetCat = db.Groups.ToList().Where(x => x.Name == record.GroupName).FirstOrDefault();
                        if (GetCat != null)
                        {
                            record.GroupId = GetCat.Id;
                        }
                    }
                    if (record.MisPickName != null)
                    {
                        var GetCat = db.MisPicks.ToList().Where(x => x.MisPickName == record.MisPickName).FirstOrDefault();
                        if (GetCat != null)
                        {
                            record.MisPickId = GetCat.MisPickId;
                        }
                    }
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

        [HttpGet("GetproductByStockName/{id}")]
        public IActionResult GetproductByStockName(string id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                var record = db.Products.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                if (record != null)
                {
                    var CheckQty = db.InventoryStocks.Where(x => x.ItemCode == record.ProductCode).FirstOrDefault();
                    if (CheckQty != null)
                    {
                        record.ItemQuantity = CheckQty.Quantity;
                    }
                    else
                    {
                        record.ItemQuantity = "0.00";
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
        [HttpGet("ItemFetch")]
        public IActionResult ItemFetch()
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

                var allproducts = db.Products.ToList().Where(x => x.Name != null).ToList();
                bool checkname = allproducts.Exists(p => p.Name.Equals(obj.Name, StringComparison.CurrentCultureIgnoreCase));
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
                            large = Convert.ToInt32(recordemp[0].ItemNumber);
                            small = Convert.ToInt32(recordemp[0].ItemNumber);
                            for (int i = 0; i < recordemp.Count(); i++)
                            {
                                if (recordemp[i].ItemNumber != null)
                                {
                                    var t = Convert.ToInt32(recordemp[i].ItemNumber);
                                    if (Convert.ToInt32(recordemp[i].ItemNumber) > large)
                                    {
                                        salesID = Convert.ToInt32(recordemp[i].Id);
                                        large = Convert.ToInt32(recordemp[i].ItemNumber);

                                    }
                                    else if (Convert.ToInt32(recordemp[i].ItemNumber) < small)
                                    {
                                        small = Convert.ToInt32(recordemp[i].ItemNumber);
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
                                    var VcodeSplit = newitems.ItemNumber;
                                    int code = Convert.ToInt32(VcodeSplit) + 1;

                                    if (Convert.ToInt32(VcodeSplit) < 9)
                                    {
                                        fullcode = "0000" + Convert.ToString(code);

                                    }
                                    else if (Convert.ToInt32(VcodeSplit) < 99)
                                    {
                                        fullcode = "000" + Convert.ToString(code);

                                    }
                                    else if (Convert.ToInt32(VcodeSplit) < 999)
                                    {
                                        fullcode = "00" + Convert.ToString(code);

                                    }
                                    else if (Convert.ToInt32(VcodeSplit) < 9999)
                                    {
                                        fullcode = "0" + Convert.ToString(code);
                                    }
                                    else if (Convert.ToInt32(VcodeSplit) < 99999)
                                    {
                                        fullcode = Convert.ToString(code);
                                    }
                                }
                                else
                                {
                                    fullcode = "0000" + "1";
                                }
                            }
                            else
                            {
                                fullcode = "0000" + "1";
                            }
                        }
                        else
                        {
                            fullcode = "0000" + "1";
                        }
                    }
                    else
                    {
                        fullcode = "0000" + "1";
                    }

                    bool checknumber = allproducts.Any(x => x.ItemNumber == fullcode);
                    if (checkname)
                    {
                        var VcodeSplit = fullcode;
                        int code = Convert.ToInt32(VcodeSplit) + 1;

                        if (Convert.ToInt32(VcodeSplit) < 9)
                        {
                            fullcode = "0000" + Convert.ToString(code);

                        }
                        else if (Convert.ToInt32(VcodeSplit) < 99)
                        {
                            fullcode = "000" + Convert.ToString(code);

                        }
                        else if (Convert.ToInt32(VcodeSplit) < 999)
                        {
                            fullcode = "00" + Convert.ToString(code);

                        }
                        else if (Convert.ToInt32(VcodeSplit) < 9999)
                        {
                            fullcode = "0" + Convert.ToString(code);
                        }
                        else if (Convert.ToInt32(VcodeSplit) < 99999)
                        {
                            fullcode = Convert.ToString(code);
                        }
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
                        obj.ItemCategoryId = null;
                    }
                    else
                    {
                        obj.ItemCategoryId = null;
                    }
                    var getsubcat = db.ItemSubCategories.Find(obj.ItemSubCategoryId);
                    if (getsubcat != null)
                    {
                        obj.SubCatName = getsubcat.SubCategory;
                        obj.ItemSubCategoryId = null;
                    }
                    else
                    {
                        obj.ItemSubCategoryId = null;
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

                    if (obj.HighlimitOn == null)
                    {
                        obj.HighlimitOn = "";
                    }
                    string hostRootPath = _webHostEnvironment.WebRootPath;
                    string webRootPath = _webHostEnvironment.ContentRootPath;
                    if (obj.BarCode != null)
                    {
                        string filePathbar;
                        string uniqueFileName = null;
                        string uploadsFolder = webRootPath + "\\images\\";
                        uniqueFileName = Guid.NewGuid().ToString() + "" + ".PNG";
                        filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
                        var barcode = new Barcode(obj.BarCode, NetBarcode.Type.EAN13, true);
                        var ProductBarCodePath = $"Images/{uniqueFileName}";
                        barcode.SaveImageFile(filePathbar);

                        obj.BarCodePath = ProductBarCodePath;
                    }



                    obj.ItemNumber = fullcode;
                    obj.ProductCode = fullcode;
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

                Response.Data = obj;
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

        [HttpGet("OptimizeItems")]
        public IActionResult OptimizeItems()
        {
            try
            {
               // var record = db.Products.ToList();
                List<Product> dataObject = new List<Product>();
                dataObject = (from e in db.Products
                              orderby e.Description
                              select new Product { Id = e.Id, Name = e.Name, Retail = e.Retail }
                              ).ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, dataObject);
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
        //            return Ok(Response);
        //        }
        //        else
        //        {
        //            var fullcode = "";
        //            Product newitems = new Product();
        //            var recordemp = db.Products.ToList();
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
        //                            fullcode = "00" + "-" + Convert.ToString(code);
        //                        }
        //                        else
        //                        {
        //                            fullcode = "00" + "-" + "1";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        fullcode = "00" + "-" + "1";
        //                    }
        //                }
        //                else
        //                {
        //                    fullcode = "00" + "-" + "1";
        //                }
        //            }
        //            else
        //            {
        //                fullcode = "00" + "-" + "1";
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

        //            if (obj.TaxExempt == null)
        //            {
        //                obj.TaxExempt = false;
        //            }
        //            if (obj.ShippingEnable == null)
        //            {
        //                obj.ShippingEnable = false;
        //            }
        //            if (obj.AllowECommerce == null)
        //            {
        //                obj.AllowECommerce = false;
        //            }
        //            if (obj.TaxonSale == null)
        //            {
        //                obj.TaxonSale = false;
        //            }
        //            if (obj.TaxOnPurchase == null)
        //            {
        //                obj.TaxOnPurchase = false;
        //            }

        //            if (obj.HighlimitOn == null)
        //            {
        //                obj.HighlimitOn = "";
        //            }
        //            string hostRootPath = _webHostEnvironment.WebRootPath;
        //            string webRootPath = _webHostEnvironment.ContentRootPath;
        //            if (obj.BarCode != null)
        //            {
        //                string filePathbar;
        //                string uniqueFileName = null;
        //                string uploadsFolder = webRootPath + "\\images\\";
        //                uniqueFileName = Guid.NewGuid().ToString() + "" + ".PNG";
        //                filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
        //                var barcode = new Barcode(obj.BarCode, NetBarcode.Type.EAN13, true);
        //                var ProductBarCodePath = $"Images/{uniqueFileName}";
        //                barcode.SaveImageFile(filePathbar);

        //                obj.BarCodePath = ProductBarCodePath;
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
        //                savefinancial.ItemId = getcurrentitem.Id;
        //                savefinancial.FinancialId = obj.Financial.FinancialId;
        //                savefinancial.Cost = obj.Financial.Cost;
        //                savefinancial.Profit = obj.Financial.Profit;
        //                savefinancial.MsgPromotion = obj.Financial.MsgPromotion;
        //                savefinancial.AddToCost = obj.Financial.AddToCost;
        //                savefinancial.UnitCharge = obj.Financial.UnitCharge;
        //                if (obj.Financial.FixedCost != null)
        //                {
        //                    savefinancial.FixedCost = obj.Financial.FixedCost;

        //                }
        //                else
        //                {
        //                    savefinancial.FixedCost = false;
        //                }
        //                savefinancial.St = obj.Financial.St;
        //                savefinancial.Tax = obj.Financial.Tax;
        //                savefinancial.OutOfStateCost = obj.Financial.OutOfStateCost;
        //                savefinancial.OutOfStateRetail = obj.Financial.OutOfStateRetail;
        //                savefinancial.Price = obj.Financial.Price;
        //                savefinancial.Quantity = obj.Financial.Quantity;
        //                savefinancial.QuantityPrice = obj.Financial.QuantityPrice;
        //                savefinancial.SuggestedRetailPrice = obj.Financial.SuggestedRetailPrice;

        //                if (obj.Financial.AutoSetSrp != null)
        //                {
        //                    savefinancial.AutoSetSrp = obj.Financial.AutoSetSrp;
        //                }
        //                else
        //                {
        //                    savefinancial.AutoSetSrp = false;
        //                }

        //                //     savefinancial.ItemNumber = obj.Financial.ItemNumber;
        //                savefinancial.QuantityInStock = obj.Financial.QuantityInStock;
        //                savefinancial.Adjustment = obj.Financial.Adjustment;

        //                if (obj.Financial.AskForPricing != null)
        //                {
        //                    savefinancial.AskForPricing = obj.Financial.AskForPricing;
        //                }
        //                else
        //                {
        //                    savefinancial.AskForPricing = false;
        //                }


        //                if (obj.Financial.AskForDescrip != null)
        //                {
        //                    savefinancial.AskForDescrip = obj.Financial.AskForDescrip;
        //                }
        //                else
        //                {
        //                    savefinancial.AskForDescrip = false;
        //                }

        //                if (obj.Financial.Serialized != null)
        //                {
        //                    savefinancial.Serialized = obj.Financial.Serialized;
        //                }
        //                else
        //                {
        //                    savefinancial.Serialized = false;
        //                }

        //                if (obj.Financial.TaxOnSales != null)
        //                {
        //                    savefinancial.TaxOnSales = obj.Financial.TaxOnSales;
        //                }
        //                else
        //                {
        //                    savefinancial.TaxOnSales = false;
        //                }
        //                if (obj.Financial.Purchase != null)
        //                {
        //                    savefinancial.Purchase = obj.Financial.Purchase;
        //                }
        //                else
        //                {
        //                    savefinancial.Purchase = false;
        //                }
        //                if (obj.Financial.NoSuchDiscount != null)
        //                {
        //                    savefinancial.NoSuchDiscount = obj.Financial.NoSuchDiscount;
        //                }
        //                else
        //                {
        //                    savefinancial.NoSuchDiscount = false;
        //                }

        //                if (obj.Financial.NoReturns != null)
        //                {
        //                    savefinancial.NoReturns = obj.Financial.NoReturns;
        //                }
        //                else
        //                {
        //                    savefinancial.NoReturns = false;
        //                }

        //                if (obj.Financial.SellBelowCost != null)
        //                {
        //                    savefinancial.SellBelowCost = obj.Financial.SellBelowCost;
        //                }
        //                else
        //                {
        //                    savefinancial.SellBelowCost = false;
        //                }

        //                savefinancial.OutOfState = obj.Financial.OutOfState;


        //                if (obj.Financial.CodeA != null)
        //                {
        //                    savefinancial.CodeA = obj.Financial.CodeA;
        //                }
        //                else
        //                {
        //                    savefinancial.CodeA = false;
        //                }
        //                if (obj.Financial.CodeB != null)
        //                {
        //                    savefinancial.CodeB = obj.Financial.CodeB;
        //                }
        //                else
        //                {
        //                    savefinancial.CodeB = false;
        //                }
        //                if (obj.Financial.CodeC != null)
        //                {
        //                    savefinancial.CodeC = obj.Financial.CodeC;
        //                }
        //                else
        //                {
        //                    savefinancial.CodeC = false;
        //                }
        //                if (obj.Financial.CodeD != null)
        //                {
        //                    savefinancial.CodeD = obj.Financial.CodeD;
        //                }
        //                else
        //                {
        //                    savefinancial.CodeD = false;
        //                }
        //                if (obj.Financial.AddCustomersDiscount != null)
        //                {
        //                    savefinancial.AddCustomersDiscount = obj.Financial.AddCustomersDiscount;
        //                }
        //                else
        //                {
        //                    savefinancial.AddCustomersDiscount = false;
        //                }


        //                savefinancial.AddCustomersDiscount = obj.Financial.AddCustomersDiscount;


        //                db.Financials.Add(savefinancial);
        //                await db.SaveChangesAsync();
        //            }
        //            var getProductnew = db.Products.ToList().Where(x => x.ItemNumber == fullcode && x.Name == obj.Name).FirstOrDefault();
        //            if (getProductnew != null)
        //            {

        //                ItemGetByIDWithStockAndFinancial(getProductnew.Id);

        //            }

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
                var allfoundpro = db.Products.ToList().Where(x => x.Name != null).ToList();
                bool isValid = allfoundpro.Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
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

                    if (data.ItemImage != null)
                    {
                        //  return BadRequest("Image Path Required Instead of Byte");
                        var imgPath = SaveImage(data.ItemImage, Guid.NewGuid().ToString());
                        record.ItemImageByPath = imgPath;
                        record.ItemImage = null;
                    }
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

                        if (data.BarCode != record.BarCode)
                        {
                            string webRootPath = _webHostEnvironment.ContentRootPath;
                            //var saveimg = record.BarCodePath.Split("/");
                            if (record.BarCodePath != null)
                            {
                                System.IO.File.Delete(Path.Combine(webRootPath, record.BarCodePath));
                            }
                            string uniqueFileName = null;
                            var filePathbar = "";
                            string uploadsFolder = webRootPath + "\\images\\";
                            uniqueFileName = Guid.NewGuid().ToString() + "" + ".PNG";
                            filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
                            var barcode = new Barcode(data.BarCode, NetBarcode.Type.EAN13, true);
                            var ProductBarCodePath = $"Images/{uniqueFileName}";
                            barcode.SaveImageFile(filePathbar);

                            record.BarCodePath = ProductBarCodePath;
                        }

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
                    if (data.ShipmentLimit != null && data.ShipmentLimit != "undefined")
                    {
                        record.ShipmentLimit = data.ShipmentLimit;
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
                    if (data.ShipmentLimit != null && data.ShipmentLimit != "undefined")
                    {
                        record.ShipmentLimit = data.ShipmentLimit;
                    }
                    if (data.NeedHighAuthorization != null)
                    {
                        record.NeedHighAuthorization = data.NeedHighAuthorization;
                    }
                    if (data.HighlimitOn == null)
                    {
                        record.HighlimitOn = "";
                    }
                    else
                    {
                        record.HighlimitOn = data.HighlimitOn;
                    }
                    if (data.Retail != null && data.Retail != "undefined")
                    {
                        if (data.Retail != record.Retail)
                        {
                            var oldrecord = db.Financials.ToList().Where(x => x.ItemId == data.Id).FirstOrDefault();
                            if (oldrecord != null)
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
                    if(savefinancial != null)
                    {
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
                    }
                   
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
        //            return Ok(Response);

        //        }
        //        else
        //        {
        //            Financial newobjfinancial = null;
        //            newobjfinancial = new Financial();

        //            newobjfinancial = data.Financial;
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

        //                if (data.BarCode != record.BarCode)
        //                {
        //                    string webRootPath = _webHostEnvironment.ContentRootPath;
        //                    //var saveimg = record.BarCodePath.Split("/");
        //                    System.IO.File.Delete(Path.Combine(webRootPath, record.BarCodePath));

        //                    string uniqueFileName = null;
        //                    var filePathbar = "";
        //                    string uploadsFolder = webRootPath + "\\images\\";
        //                    uniqueFileName = Guid.NewGuid().ToString() + "" + ".PNG";
        //                    filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
        //                    var barcode = new Barcode(data.BarCode, NetBarcode.Type.EAN13, true);
        //                    var ProductBarCodePath = $"Images/{uniqueFileName}";
        //                    barcode.SaveImageFile(filePathbar);

        //                    record.BarCodePath = ProductBarCodePath;
        //                }

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
        //            if (data.ShipmentLimit != null && data.ShipmentLimit != "undefined")
        //            {
        //                record.ShipmentLimit = data.ShipmentLimit;
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
        //            if (data.MisPickId != null && data.MisPickId > 0)
        //            {
        //                var getmispick = db.MisPicks.Find(data.MisPickId);
        //                if (getmispick != null)
        //                {
        //                    data.MisPickName = getmispick.MisPickName;
        //                }
        //            }
        //            if (data.OrderQuantity != null && data.OrderQuantity != "undefined")
        //            {
        //                record.OrderQuantity = data.OrderQuantity;
        //            }
        //            if (data.Units != null && data.Units != "undefined")
        //            {
        //                record.Units = data.Units;
        //            }
        //            if (data.WeightOz != null && data.WeightOz != "undefined")
        //            {
        //                record.WeightOz = data.WeightOz;
        //            }
        //            if (data.WeightLb != null && data.WeightLb != "undefined")
        //            {
        //                record.WeightLb = data.WeightLb;
        //            }
        //            if (data.LocationTwo != null && data.LocationTwo != "undefined")
        //            {
        //                record.LocationTwo = data.LocationTwo;
        //            }
        //            if (data.LocationThree != null && data.LocationThree != "undefined")
        //            {
        //                record.LocationThree = data.LocationThree;
        //            }
        //            if (data.MaintainStockForDays != null && data.MaintainStockForDays != "undefined")
        //            {
        //                record.MaintainStockForDays = data.MaintainStockForDays;
        //            }
        //            if (data.DiscountPrice != null && data.DiscountPrice != "undefined")
        //            {
        //                record.DiscountPrice = data.DiscountPrice;
        //            }
        //            if (data.Rating != null && data.Rating != "undefined")
        //            {
        //                record.Rating = data.Rating;
        //            }
        //            if (data.MinOrderQty != null && data.MinOrderQty != "undefined")
        //            {
        //                record.MinOrderQty = data.MinOrderQty;
        //            }
        //            if (data.MaxOrderQty != null && data.MaxOrderQty != "undefined")
        //            {
        //                record.MaxOrderQty = data.MaxOrderQty;
        //            }
        //            if (data.MaxOrderQty != null && data.MaxOrderQty != "undefined")
        //            {
        //                record.MaxOrderQty = data.MaxOrderQty;
        //            }
        //            if (data.SingleUnitMsa != null && data.SingleUnitMsa != "undefined")
        //            {
        //                record.SingleUnitMsa = data.SingleUnitMsa;
        //            }
        //            if (data.Variations != null && data.Variations != "undefined")
        //            {
        //                record.Variations = data.Variations;
        //            }
        //            if (data.ShipmentLimit != null && data.ShipmentLimit != "undefined")
        //            {
        //                record.ShipmentLimit = data.ShipmentLimit;
        //            }
        //            if (data.NeedHighAuthorization != null)
        //            {
        //                record.NeedHighAuthorization = data.NeedHighAuthorization;
        //            }
        //            if (data.HighlimitOn == null)
        //            {
        //                record.HighlimitOn = "";
        //            }
        //            else
        //            {
        //                record.HighlimitOn = data.HighlimitOn;
        //            }
        //            if (data.Retail != null && data.Retail != "undefined")
        //            {
        //                if (data.Retail != record.Retail)
        //                {
        //                    var oldrecord = db.Financials.ToList().Where(x => x.ItemId == data.Id).FirstOrDefault();
        //                    if (oldrecord != null)
        //                    {
        //                        oldrecord.Retail = data.Retail;
        //                        db.Entry(oldrecord).State = EntityState.Modified;
        //                    }
        //                }
        //                record.Retail = data.Retail;
        //            }
        //            data = record;
        //            db.Entry(data).State = EntityState.Modified;
        //            await db.SaveChangesAsync();

        //            var check = db.Financials.ToList();
        //            var savefinancial = db.Financials.ToList().Where(x => x.ItemId == data.Id && x.ItemNumber == data.ItemNumber).FirstOrDefault();
        //            //      data.Financial = new Financial();
        //            if (newobjfinancial.Cost != null && newobjfinancial.Cost != "undefined")
        //            {
        //                savefinancial.Cost = newobjfinancial.Cost;
        //            }
        //            if (newobjfinancial.Profit != null && newobjfinancial.Profit != "undefined")
        //            {
        //                savefinancial.Profit = newobjfinancial.Profit;
        //            }
        //            if (newobjfinancial.MsgPromotion != null && newobjfinancial.MsgPromotion != "undefined")
        //            {
        //                savefinancial.MsgPromotion = newobjfinancial.MsgPromotion;
        //            }
        //            if (newobjfinancial.AddToCost != null && newobjfinancial.AddToCost != "undefined")
        //            {
        //                savefinancial.AddToCost = newobjfinancial.AddToCost;
        //            }
        //            if (newobjfinancial.UnitCharge != null && newobjfinancial.UnitCharge != "undefined")
        //            {
        //                savefinancial.UnitCharge = newobjfinancial.UnitCharge;
        //            }
        //            if (newobjfinancial.FixedCost != null)
        //            {
        //                savefinancial.FixedCost = newobjfinancial.FixedCost;
        //            }


        //            if (newobjfinancial.St != null && newobjfinancial.St != "undefined")
        //            {
        //                savefinancial.St = newobjfinancial.St;
        //            }
        //            if (newobjfinancial.Tax != null && newobjfinancial.Tax != "undefined")
        //            {
        //                savefinancial.Tax = newobjfinancial.Tax;
        //            }
        //            if (newobjfinancial.OutOfStateCost != null && newobjfinancial.OutOfStateCost != "undefined")
        //            {
        //                savefinancial.OutOfStateCost = newobjfinancial.OutOfStateCost;
        //            }
        //            if (newobjfinancial.OutOfStateRetail != null && newobjfinancial.OutOfStateRetail != "undefined")
        //            {
        //                savefinancial.OutOfStateRetail = newobjfinancial.OutOfStateRetail;
        //            }
        //            if (newobjfinancial.Price != null && newobjfinancial.Price != "undefined")
        //            {
        //                savefinancial.Price = newobjfinancial.Price;
        //            }
        //            if (newobjfinancial.Quantity != null && newobjfinancial.Quantity != "undefined")
        //            {
        //                savefinancial.Quantity = newobjfinancial.Quantity;
        //            }
        //            if (newobjfinancial.QuantityPrice != null && newobjfinancial.QuantityPrice != "undefined")
        //            {
        //                savefinancial.QuantityPrice = newobjfinancial.QuantityPrice;
        //            }
        //            if (newobjfinancial.SuggestedRetailPrice != null && newobjfinancial.SuggestedRetailPrice != "undefined")
        //            {
        //                savefinancial.SuggestedRetailPrice = newobjfinancial.SuggestedRetailPrice;
        //            }

        //            if (newobjfinancial.AutoSetSrp != null)
        //            {
        //                savefinancial.AutoSetSrp = newobjfinancial.AutoSetSrp;
        //            }
        //            if (newobjfinancial.QuantityInStock != null && newobjfinancial.QuantityInStock != "undefined")
        //            {
        //                savefinancial.QuantityInStock = newobjfinancial.QuantityInStock;
        //            }
        //            if (newobjfinancial.Adjustment != null && newobjfinancial.Adjustment != "undefined")
        //            {
        //                savefinancial.Adjustment = newobjfinancial.Adjustment;
        //            }

        //            if (newobjfinancial.AskForPricing != null)
        //            {
        //                savefinancial.AskForPricing = newobjfinancial.AskForPricing;
        //            }


        //            if (newobjfinancial.AskForDescrip != null)
        //            {
        //                savefinancial.AskForDescrip = newobjfinancial.AskForDescrip;
        //            }


        //            if (newobjfinancial.Serialized != null)
        //            {
        //                savefinancial.Serialized = newobjfinancial.Serialized;
        //            }


        //            if (newobjfinancial.TaxOnSales != null)
        //            {
        //                savefinancial.TaxOnSales = newobjfinancial.TaxOnSales;
        //            }

        //            if (newobjfinancial.Purchase != null)
        //            {
        //                savefinancial.Purchase = newobjfinancial.Purchase;
        //            }

        //            if (newobjfinancial.NoSuchDiscount != null)
        //            {
        //                savefinancial.NoSuchDiscount = newobjfinancial.NoSuchDiscount;
        //            }


        //            if (newobjfinancial.NoReturns != null)
        //            {
        //                savefinancial.NoReturns = newobjfinancial.NoReturns;
        //            }


        //            if (newobjfinancial.SellBelowCost != null)
        //            {
        //                savefinancial.SellBelowCost = newobjfinancial.SellBelowCost;
        //            }


        //            savefinancial.OutOfState = newobjfinancial.OutOfState;


        //            if (newobjfinancial.CodeA != null)
        //            {
        //                savefinancial.CodeA = newobjfinancial.CodeA;
        //            }

        //            if (newobjfinancial.CodeB != null)
        //            {
        //                savefinancial.CodeB = newobjfinancial.CodeB;
        //            }

        //            if (newobjfinancial.CodeC != null)
        //            {
        //                savefinancial.CodeC = newobjfinancial.CodeC;
        //            }

        //            if (newobjfinancial.CodeD != null)
        //            {
        //                savefinancial.CodeD = newobjfinancial.CodeD;
        //            }

        //            if (newobjfinancial.AddCustomersDiscount != null)
        //            {
        //                savefinancial.AddCustomersDiscount = newobjfinancial.AddCustomersDiscount;
        //            }
        //            data.Financial = savefinancial;
        //            db.Entry(data.Financial).State = EntityState.Modified;
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

        [HttpGet("SaleByproductID/{id}")]
        public IActionResult SaleByproductID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                var tt = db.Sales.ToList();
                var record = db.PosSales.ToList().Where(x => x.ItemId == id).ToList();
                if (record != null)
                {
                    var allsuppliers = db.Customers.ToList();
                    for (int i = 0; i < record.Count(); i++)
                    {
                        foreach (var item in allsuppliers.ToList().Where(x => x.CustomerId == record[i].CustomerId).ToList())
                        {
                            record[i].CustomerName = item.FullName;
                        }
                    }
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
                    var Response = ResponseBuilder.BuildWSResponse<PosSale>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
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


                var possale = db.PosSales.Where(x => x.InvoiceNumber == obj[0].InvoiceNumber).ToList();


                //var recordemp = db.CustomerOrders.ToList();
                if (possale != null && possale.Count() > 0)
                {
                    var fullcode = "";
                    var possaleList = db.PosSales.ToList();

                    if (possaleList != null && possaleList.Count() > 0)
                    {
                        PosSale newcustomer = new PosSale();


                        if (possaleList[0].InvoiceNumber != null && possaleList[0].InvoiceNumber != "string" && possaleList[0].InvoiceNumber != "")
                        {
                            int large, small;
                            int POSSALEID = 0;
                            large = Convert.ToInt32(possaleList[0].InvoiceNumber.Split('-')[1]);
                            small = Convert.ToInt32(possaleList[0].InvoiceNumber.Split('-')[1]);
                            for (int i = 0; i < possaleList.Count(); i++)
                            {
                                if (possaleList[i].InvoiceNumber != null)
                                {
                                    var t = Convert.ToInt32(possaleList[i].InvoiceNumber.Split('-')[1]);
                                    if (Convert.ToInt32(possaleList[i].InvoiceNumber.Split('-')[1]) > large)
                                    {
                                        POSSALEID = Convert.ToInt32(possaleList[i].PossaleId);
                                        large = Convert.ToInt32(possaleList[i].InvoiceNumber.Split('-')[1]);
                                    }
                                    else if (Convert.ToInt32(possaleList[i].InvoiceNumber.Split('-')[1]) < small)
                                    {
                                        small = Convert.ToInt32(possaleList[i].InvoiceNumber.Split('-')[1]);
                                    }
                                    else
                                    {
                                        //if (large < 2)
                                        //{
                                        POSSALEID = Convert.ToInt32(possaleList[i].PossaleId);
                                        //}
                                    }
                                }
                            }
                            newcustomer = possaleList.ToList().Where(x => x.PossaleId == POSSALEID).FirstOrDefault();
                            if (newcustomer != null)
                            {
                                if (newcustomer.InvoiceNumber != null)
                                {

                                    int code = 0;
                                    var VcodeSplit = newcustomer.InvoiceNumber.Split('-');

                                    code = Convert.ToInt32(VcodeSplit[1]) + 1;

                                    long checknumber = Convert.ToInt64(VcodeSplit[1]);
                                    if (checknumber > 9999)
                                    {
                                        //10000
                                        long ndcode = Convert.ToInt64(VcodeSplit[0]) + 1;

                                        fullcode = "000000" + Convert.ToString(ndcode) + "-" + "9999";
                                    }
                                    else if (checknumber > 999)
                                    {
                                        fullcode = "0000000-" + Convert.ToString(code);
                                    }
                                    else if (checknumber > 99)
                                    {
                                        fullcode = "0000000-0" + Convert.ToString(code);
                                    }
                                    else if (checknumber > 9)
                                    {
                                        fullcode = "0000000-00" + Convert.ToString(code);
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
                        //foreach (var item in obj)
                        //{
                        //    item.InvoiceNumber= fullcode;
                        //}
                    }

                    foreach (var item in obj)
                    {
                        item.InvoiceNumber = fullcode;
                    }


                }

                var supervisorId = obj[0].SupervisorId;
                var record = db.Supervisors.Where(x => x.SupervisorId == supervisorId && x.CustomerId == obj[0].CustomerId)?.FirstOrDefault();
                if (record != null)
                {

                    SupervisorCredit credit = new SupervisorCredit();


                    var GetCustomer = db.CustomerInformations.ToList().Where(x => x.Id == record.CustomerId).FirstOrDefault();

                    if (record.RoleName != null)
                    {
                        credit.RoleName = record.RoleName;

                    }
                    else
                    {
                        var GetRole = db.AspNetRoles.Find(record.RoleId);
                        if (GetRole != null)
                        {
                            credit.RoleName = GetRole.Name;
                        }
                    }
                    credit.SupervisorId = record.SupervisorId;
                    credit.RoleId = record.RoleId;
                    credit.RoleName = record.RoleName;
                    credit.CustomerId = obj[0].CustomerId;
                    credit.CreditDate = DateTime.Now;
                    credit.PaymentStatus = false;
                    credit.CreditAmount = obj[0].Total;
                    credit.SaleId = obj[0].InvoiceNumber;

                    if (record.UserId != null)
                    {
                        var getUserInfo = db.AspNetUsers.Find(record.UserId);
                        if (getUserInfo != null)
                        {
                            if (getUserInfo.UserName != null)
                            {
                                credit.FullName = getUserInfo.UserName;
                            }
                        }
                    }
                    if (record.EmployeeId != null)
                    {
                        var getEmployeeInfo = db.Employees.Find(record.EmployeeId);
                        if (getEmployeeInfo != null)
                        {
                            credit.EmployeeCode = getEmployeeInfo.EmployeeCode;
                        }
                    }
                    db.SupervisorCredits.Add(credit);
                    db.SaveChanges();
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
                    //if (obj[0].IsClose == true)
                    //{
                        if (getstock != null)
                        {
                            getstock.Quantity = (Convert.ToDouble(getstock.Quantity) - Convert.ToDouble(obj[i].RingerQuantity)).ToString();
                            db.Entry(getstock).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                   // }
                }


                var getvendor = db.CustomerInformations.Find(obj[0].CustomerId);
                if (getvendor != null)
                {
                    var getaccount = db.Accounts.ToList().Where(a => a.Title == getvendor.AccountTitle && a.AccountId == getvendor.AccountId).FirstOrDefault();
                    var getCHaccount = db.Accounts.ToList().Where(a => a.Title == "Net Sales").FirstOrDefault();
                    //var getCInHaccount = db.Accounts.ToList().Where(a => a.Title == "Cash in hand").FirstOrDefault();
                    if (getaccount != null)
                    {
                        //if (obj[0].OnCredit == true)
                        if (obj[0].IsPaid == false || obj[0].IsPaid == null)
                        {
                            Receivable objRec = new Receivable();
                          //  var checcck = db.Receivables.ToList();
                            var GetRec = db.Receivables.ToList().Where(x => x.AccountNumber == getvendor.AccountId).ToList().FirstOrDefault();
                            if (GetRec != null)
                            {
                                double last = Convert.ToDouble(GetRec.Amount);
                                GetRec.Amount = (last +  grossamount).ToString();
                                db.Entry(GetRec).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                Receivable receive = null;
                                receive = new Receivable();
                                receive.AccountId = obj[0].CustomerAccountNumber;
                                receive.AccountNumber = obj[0].CustomerAccountNumber;
                                receive.AccountName = obj[0].CustomerName;
                                receive.Amount = obj[0].InvoiceTotal;

                                db.Receivables.Add(receive);
                                db.SaveChanges();
                            }

                            for (int i = 0; i < 2; i++)
                            {
                                Transaction transaction = null;
                                transaction = new Transaction();
                                transaction.AccountName = getaccount.Title;
                                transaction.AccountNumber = getaccount.AccountId;
                                transaction.DetailAccountId = getaccount.AccountId;
                                transaction.InvoiceNumber = obj[0].InvoiceNumber;
                                transaction.Date = DateTime.Now;

                                if (i == 0)
                                {
                                    transaction.Credit = "0.00";
                                    transaction.Debit = grossamount.ToString();
                                }
                                else
                                {
                                    transaction.Credit = grossamount.ToString();
                                    transaction.Debit = "0.00";
                                }

                                transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                db.Transactions.Add(transaction);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            var getCInHaccount = db.Accounts.ToList().Where(a => a.Title == "Cash in hand").FirstOrDefault();
                            if (getCInHaccount != null)
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
                                                fullcode = "CP00" + "-" + Convert.ToString(code);
                                            }
                                            else
                                            {
                                                fullcode = "CP00" + "-" + "1";
                                            }
                                        }
                                        else
                                        {
                                            fullcode = "CP00" + "-" + "1";
                                        }
                                    }
                                    else
                                    {
                                        fullcode = "CP00" + "-" + "1";
                                    }
                                }
                                else
                                {
                                    fullcode = "CP00" + "-" + "1";
                                }

                                Receiving receiving = null;
                                receiving = new Receiving();
                                receiving.AccountName = getaccount.Title;
                                receiving.AccountNumber = getaccount.AccountId;
                                receiving.AccountId = getaccount.AccountId;

                                if (obj[0].PaymentTerms == "Cheque")
                                {
                                    //receiving.CheckDate = obj[0].CheckDate;
                                    //receiving.CheckNumber = obj[0].CheckNumber;
                                    //receiving.CheckTitle = obj[0].CheckTitle;
                                    //receiving.PaymentType = "Wire";

                                }
                                else if(obj[0].PaymentTerms == "Cash")
                                {
                                    receiving.PaymentType = "Cash";
                                }
                                receiving.Note = "";
                                receiving.InvoiceNumber = fullcode;
                                receiving.Debit = grossamount.ToString();
                                receiving.Credit = "0.00";
                                receiving.Date = DateTime.Now;
                                db.Receivings.Add(receiving);
                                await db.SaveChangesAsync();

                                for (int i = 0; i < 1; i++)
                                {
                                    Transaction transaction = null;
                                    transaction = new Transaction();
                                    transaction.AccountName = getaccount.Title;
                                    transaction.AccountNumber = getaccount.AccountId;
                                    transaction.DetailAccountId = getaccount.AccountId;
                                    transaction.InvoiceNumber = fullcode;
                                    transaction.Date = DateTime.Now;

                                    if (i == 0)
                                    {
                                        transaction.Credit = "0.00";
                                        transaction.Debit = grossamount.ToString();
                                    }
                                    else
                                    {
                                        transaction.Credit = grossamount.ToString();
                                        transaction.Debit = "0.00";
                                    }

                                    transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                    db.Transactions.Add(transaction);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                       // Account objAcc = new Account();
                        Account objAcc = null;
                        objAcc = new Account();

                        Account customeracc = null;
                        customeracc = new Account();

                        var subaccrecord = db.AccountSubGroups.ToList().Where(x => x.Title == "Suppliers").FirstOrDefault();
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
                                if (getcode > 99)
                                {
                                    objAcc.AccountId = subaccrecord.AccountSubGroupId + "-0" + Convert.ToString(getcode);

                                }
                                else if (getcode > 9)
                                {
                                    objAcc.AccountId = subaccrecord.AccountSubGroupId + "-00" + Convert.ToString(getcode);
                                }
                                else
                                {
                                    objAcc.AccountId = subaccrecord.AccountSubGroupId + "-000" + Convert.ToString(getcode);
                                }
                                objAcc.Title = getvendor.Company;
                                objAcc.Status = 1;
                                objAcc.AccountSubGroupId = subaccrecord.AccountSubGroupId;
                                customeracc = db.Accounts.Add(objAcc).Entity;
                                db.SaveChanges();
                            }
                            else
                            {
                                objAcc.AccountId = subaccrecord.AccountSubGroupId + "-0001";
                                objAcc.Title = obj[0].CustomerName;
                                objAcc.Status = 1;
                                objAcc.AccountSubGroupId = subaccrecord.AccountSubGroupId;
                                customeracc = db.Accounts.Add(objAcc).Entity;
                                db.SaveChanges();
                            }
                            if (customeracc != null)
                            {
                                var foundSOrder = db.CustomerInformations.Find(obj[0].CustomerId);
                                if (foundSOrder != null)
                                {
                                    // foundSOrder.StockItemNumber = fullcode;
                                    foundSOrder.AccountId = customeracc.AccountId;
                                    foundSOrder.AccountNumber = customeracc.AccountId;
                                    foundSOrder.AccountTitle = customeracc.Title;
                                    db.Entry(foundSOrder).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                if (obj[0].OnCredit == true)
                                {
                                    Receivable objRec = new Receivable();
                                    var GetRec = db.Receivables.ToList().Where(x => x.AccountId == customeracc.AccountId).FirstOrDefault();
                                    if (GetRec != null)
                                    {
                                        double last = Convert.ToDouble(GetRec.Amount);
                                        GetRec.Amount = (last + grossamount).ToString();
                                        db.Entry(GetRec).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        Receivable receive = null;
                                        receive = new Receivable();
                                        receive.AccountId = obj[0].CustomerAccountNumber;
                                        receive.AccountNumber = obj[0].CustomerAccountNumber;
                                        receive.AccountName = obj[0].CustomerName;
                                        receive.Amount = obj[0].InvoiceTotal;

                                        db.Receivables.Add(receive);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }


                var customerData = db.CustomerInformations.ToList().Where(x => x.Id == obj[0].CustomerId).FirstOrDefault();

                for (int i = 0; i < obj.Count(); i++)
                {
                    var productdetail = db.Products.Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                    if (productdetail != null)
                    {
                        if (productdetail.NeedHighAuthorization == true)
                        {
                            if (int.Parse(obj[i].RingerQuantity) > int.Parse(productdetail.HighlimitOn))
                            {
                                AutherizeOrderLimit autherizeOrderLimit = new AutherizeOrderLimit();
                                if (obj[i].plateNo != null)
                                {
                                    autherizeOrderLimit.PlateNumber = obj[i].plateNo;
                                }
                                else
                                {
                                    autherizeOrderLimit.PlateNumber = "NA";
                                }
                                if (obj[i].drivingLicense != null)
                                {
                                    autherizeOrderLimit.DrivingLicenseNumber = obj[i].drivingLicense;
                                }
                                else
                                {
                                    autherizeOrderLimit.DrivingLicenseNumber = "NA";
                                }
                                if (obj[i].InvoiceNumber != null)
                                {
                                    autherizeOrderLimit.TicketId = obj[i].InvoiceNumber;
                                }
                                else
                                {
                                    autherizeOrderLimit.TicketId = "NA";
                                }

                                if (customerData != null)
                                {
                                    autherizeOrderLimit.UserId = customerData.UserId;
                                    autherizeOrderLimit.CustomerId = customerData.Id;
                                }

                                autherizeOrderLimit.ProductId = productdetail.Id;
                                autherizeOrderLimit.ProductName = productdetail.Name;
                                autherizeOrderLimit.ProductCode = productdetail.ProductCode;


                                var dateAndTime = DateTime.Now;
                                var time = DateTime.Now.ToString("hh:mm tt");
                                autherizeOrderLimit.AccessTime = time;
                                autherizeOrderLimit.AccessDate = dateAndTime;
                                db.AutherizeOrderLimits.Add(autherizeOrderLimit);
                            }
                        }
                    }
                }


                db.SaveChanges();
                //var recordTicket = db.CustomerOrders.Where(x => x.TicketId == obj[0].InvoiceNumber).ToList();
                //var cartdetail = db.CartDetails.Where(x => x.TicketId == obj[0].InvoiceNumber).ToList();

                //if (cartdetail.Count() > 0)
                //{
                //    for (int i = 0; i < cartdetail.Count(); i++)
                //    {
                //        cartdetail[i].Quantity = int.Parse(obj[i].Quantity);
                //        cartdetail[i].Total = obj[i].Total;
                //        db.Entry(cartdetail[i]).State = EntityState.Modified;
                //        db.SaveChanges();
                //    }
                //}
                //await db.SaveChangesAsync();

                //if (recordTicket.Count() > 0)
                //{
                //    for (int i = 0; i < recordTicket.Count(); i++)
                //    {
                //        recordTicket[i].InvoicedDate = DateTime.Now;
                //        recordTicket[i].InvoicedTime = DateTime.Now.ToString("hh:mm tt");
                //        recordTicket[i].IsInvoiced = true;
                //        recordTicket[i].InvoicedBy = "cashier@gmail.com";
                //        recordTicket[i].OrderAmount = obj[i].Total;
                //        recordTicket[i].Quantity = obj[i].Quantity;
                //        db.Entry(recordTicket[i]).State = EntityState.Modified;
                //        db.SaveChanges();
                //    }
                //}
                await db.SaveChangesAsync();

                //bool isEmailSent = emailService.SendNewOrderEmail("NewOrder", recordTicket[0].Email, recordTicket, cartdetail, obj);

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




        [HttpPost("SaleUpdate")]
        public async Task<IActionResult> SaleUpdate(List<PosSale> obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }


                var oldpossalelist = db.PosSales.Where(x => x.InvoiceNumber == obj[0].InvoiceNumber).ToList();
                foreach (var oldpos in oldpossalelist)
                {
                    var existpos = obj.Where(x => x.ItemId == oldpos.ItemId).FirstOrDefault();
                    if (existpos == null)
                    {
                        db.PosSales.Remove(oldpos);
                    }
                }

                for (int j = 0; j < obj.Count; j++)
                {
                    var existpos = oldpossalelist.Where(x => x.ItemId == obj[j].ItemId).FirstOrDefault();
                    if (existpos != null)
                    {
                        if (obj[j].PossaleId == 0)
                        {
                            obj[j].PossaleId = existpos.PossaleId;
                        }
                    }
                }

                db.SaveChanges();

                var possale = db.PosSales.Where(x => x.InvoiceNumber == obj[0].InvoiceNumber).ToList();



                var supervisorId = obj[0].SupervisorId;
                var record = db.Supervisors.Where(x => x.SupervisorId == supervisorId)?.FirstOrDefault();
                if (record != null)
                {

                    SupervisorCredit credit = new SupervisorCredit();


                    var GetCustomer = db.CustomerInformations.ToList().Where(x => x.UserId == record.UserId).FirstOrDefault();

                    if (record.RoleName != null)
                    {
                        credit.RoleName = record.RoleName;

                    }
                    else
                    {
                        var GetRole = db.AspNetRoles.Find(record.RoleId);
                        if (GetRole != null)
                        {
                            credit.RoleName = GetRole.Name;
                        }
                    }
                    credit.SupervisorId = record.SupervisorId;
                    credit.RoleId = record.RoleId;
                    credit.RoleName = record.RoleName;
                    credit.CustomerId = obj[0].CustomerId;
                    credit.CreditDate = DateTime.Now;
                    credit.PaymentStatus = false;
                    credit.CreditAmount = obj[0].Total;
                    credit.SaleId = obj[0].InvoiceNumber;

                    if (record.UserId != null)
                    {
                        var getUserInfo = db.AspNetUsers.Find(record.UserId);
                        if (getUserInfo != null)
                        {
                            if (getUserInfo.UserName != null)
                            {
                                credit.FullName = getUserInfo.UserName;
                            }
                        }
                    }
                    if (record.EmployeeId != null)
                    {
                        var getEmployeeInfo = db.Employees.Find(record.EmployeeId);
                        if (getEmployeeInfo != null)
                        {
                            credit.EmployeeCode = getEmployeeInfo.EmployeeCode;
                        }
                    }
                    // db.SupervisorCredits.Add(credit);
                    // db.SaveChanges();
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
                    if (obj[i].PossaleId == 0)
                    {
                        db.PosSales.Add(obj[i]);
                    }
                    else
                    {
                        var possalerecord = db.PosSales.Where(x => x.PossaleId == obj[i].PossaleId).FirstOrDefault();
                        db.Entry(possalerecord).CurrentValues.SetValues(obj[i]);
                    }
                    db.SaveChanges();
                    var getstock = db.InventoryStocks.ToList().Where(x => x.ProductId == obj[i].ItemId).FirstOrDefault();
                    if (obj[0].IsClose == true)
                    {
                        if (getstock != null)
                        {
                            getstock.Quantity = (Convert.ToDouble(getstock.Quantity) - Convert.ToDouble(obj[i].Quantity)).ToString();
                            db.Entry(getstock).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                var getvendor = db.CustomerInformations.Find(obj[0].CustomerId);
                if (getvendor != null)
                {
                    var getaccount = db.Accounts.ToList().Where(a => a.Title == getvendor.AccountTitle && a.AccountId == getvendor.AccountId).FirstOrDefault();
                    var getCHaccount = db.Accounts.ToList().Where(a => a.Title == "Net Sales").FirstOrDefault();
                    var getCInHaccount = db.Accounts.ToList().Where(a => a.Title == "Cash in hand").FirstOrDefault();
                    if (getaccount != null)
                    {
                        if (obj[0].IsClose == true)
                        {
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
                        }
                    }



                }


                //var customerData = db.CustomerInformations.ToList().Where(x => x.Id == obj[0].CustomerId).FirstOrDefault();

                //for (int i = 0; i < obj.Count(); i++)
                //{
                //    var productdetail = db.Products.Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                //    if (productdetail != null)
                //    {
                //        if (productdetail.NeedHighAuthorization == true)
                //        {
                //            if (int.Parse(obj[i].RingerQuantity) > int.Parse(productdetail.HighlimitOn))
                //            {
                //                AutherizeOrderLimit autherizeOrderLimit = new AutherizeOrderLimit();
                //                if (obj[i].plateNo != null)
                //                {
                //                    autherizeOrderLimit.PlateNumber = obj[i].plateNo;
                //                }
                //                else
                //                {
                //                    autherizeOrderLimit.PlateNumber = "NA";
                //                }
                //                if (obj[i].drivingLicense != null)
                //                {
                //                    autherizeOrderLimit.DrivingLicenseNumber = obj[i].drivingLicense;
                //                }
                //                else
                //                {
                //                    autherizeOrderLimit.DrivingLicenseNumber = "NA";
                //                }
                //                if (obj[i].InvoiceNumber != null)
                //                {
                //                    autherizeOrderLimit.TicketId = obj[i].InvoiceNumber;
                //                }
                //                else
                //                {
                //                    autherizeOrderLimit.TicketId = "NA";
                //                }

                //                if (customerData != null)
                //                {
                //                    autherizeOrderLimit.UserId = customerData.UserId;
                //                    autherizeOrderLimit.CustomerId = customerData.Id;
                //                }

                //                autherizeOrderLimit.ProductId = productdetail.Id;
                //                autherizeOrderLimit.ProductName = productdetail.Name;
                //                autherizeOrderLimit.ProductCode = productdetail.ProductCode;


                //                var dateAndTime = DateTime.Now;
                //                var time = DateTime.Now.ToString("hh:mm tt");
                //                autherizeOrderLimit.AccessTime = time;
                //                autherizeOrderLimit.AccessDate = dateAndTime;
                //                db.AutherizeOrderLimits.Add(autherizeOrderLimit);
                //            }
                //        }
                //    }
                //}


                db.SaveChanges();
                //var recordTicket = db.CustomerOrders.Where(x => x.TicketId == obj[0].InvoiceNumber).ToList();
                //var cartdetail = db.CartDetails.Where(x => x.TicketId == obj[0].InvoiceNumber).ToList();

                //if (cartdetail.Count() > 0)
                //{
                //    for (int i = 0; i < cartdetail.Count(); i++)
                //    {
                //        cartdetail[i].Quantity = int.Parse(obj[i].Quantity);
                //        cartdetail[i].Total = obj[i].Total;
                //        db.Entry(cartdetail[i]).State = EntityState.Modified;
                //        db.SaveChanges();
                //    }
                //}
                //await db.SaveChangesAsync();

                //if (recordTicket.Count() > 0)
                //{
                //    for (int i = 0; i < recordTicket.Count(); i++)
                //    {
                //        recordTicket[i].InvoicedDate = DateTime.Now;
                //        recordTicket[i].InvoicedTime = DateTime.Now.ToString("hh:mm tt");
                //        recordTicket[i].IsInvoiced = true;
                //        recordTicket[i].InvoicedBy = "cashier@gmail.com";
                //        recordTicket[i].OrderAmount = obj[i].Total;
                //        recordTicket[i].Quantity = obj[i].Quantity;
                //        db.Entry(recordTicket[i]).State = EntityState.Modified;
                //        db.SaveChanges();
                //    }
                //}
                await db.SaveChangesAsync();

                //bool isEmailSent = emailService.SendNewOrderEmail("NewOrder", recordTicket[0].Email, recordTicket, cartdetail, obj);

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

        [HttpPost("SaleCreateOFI")]
        public async Task<IActionResult> SaleCreateOFI(List<PosSale> obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var supervisorId = obj[0].SupervisorId;
                var record = db.Supervisors.Where(x => x.SupervisorId == supervisorId)?.FirstOrDefault();
                if (record != null)
                {
                    SupervisorCredit credit = new SupervisorCredit();
                    var GetCustomer = db.CustomerInformations.ToList().Where(x => x.UserId == record.UserId).FirstOrDefault();
                    if (record.RoleName != null)
                    {
                        credit.RoleName = record.RoleName;
                    }
                    else
                    {
                        var GetRole = db.AspNetRoles.Find(record.RoleId);
                        if (GetRole != null)
                        {
                            credit.RoleName = GetRole.Name;
                        }
                    }
                    credit.SupervisorId = record.SupervisorId;
                    credit.RoleId = record.RoleId;
                    credit.RoleName = record.RoleName;
                    credit.CustomerId = obj[0].CustomerId;
                    credit.CreditDate = DateTime.Now;
                    credit.PaymentStatus = false;
                    credit.CreditAmount = obj[0].Total;
                    credit.SaleId = obj[0].InvoiceNumber;

                    if (record.UserId != null)
                    {
                        var getUserInfo = db.AspNetUsers.Find(record.UserId);
                        if (getUserInfo != null)
                        {
                            if (getUserInfo.UserName != null)
                            {
                                credit.FullName = getUserInfo.UserName;
                            }
                        }
                    }
                    if (record.EmployeeId != null)
                    {
                        var getEmployeeInfo = db.Employees.Find(record.EmployeeId);
                        if (getEmployeeInfo != null)
                        {
                            credit.EmployeeCode = getEmployeeInfo.EmployeeCode;
                        }
                    }
                    db.SupervisorCredits.Add(credit);
                    db.SaveChanges();
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
                    if (obj[0].FromScreen != null && obj[0].FromScreen == "Online")
                    {
                        if (getstock != null)
                        {
                            getstock.Quantity = (Convert.ToDouble(getstock.Quantity) - Convert.ToDouble(obj[i].RingerQuantity)).ToString();
                            db.Entry(getstock).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                var getvendor = db.CustomerInformations.Find(obj[0].CustomerId);
                if (getvendor != null)
                {
                    var getaccount = db.Accounts.ToList().Where(a => a.Title == getvendor.AccountTitle && a.AccountId == getvendor.AccountId).FirstOrDefault();
                    var getCHaccount = db.Accounts.ToList().Where(a => a.Title == "Net Sales").FirstOrDefault();
                    var getCInHaccount = db.Accounts.ToList().Where(a => a.Title == "Cash in hand").FirstOrDefault();
                    if (getaccount != null)
                    {
                        if (obj[0].FromScreen != null && obj[0].FromScreen == "Online")
                        {
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
                        }
                    }


                    Receivable pay = null;
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

                    //else
                    //{

                    //}
                }

                var recordTicket = db.CustomerOrders.Where(x => x.TicketId == obj[0].InvoiceNumber).ToList();
                var cartdetail = db.CartDetails.Where(x => x.TicketId == obj[0].InvoiceNumber).ToList();

                if (cartdetail.Count() > 0)
                {
                    for (int i = 0; i < cartdetail.Count(); i++)
                    {
                        cartdetail[i].Quantity = int.Parse(obj[i].Quantity);
                        cartdetail[i].Total = obj[i].Total;
                        db.Entry(cartdetail[i]).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                await db.SaveChangesAsync();
                double amount = 0;
                if (recordTicket.Count() > 0)
                {
                    for (int i = 0; i < recordTicket.Count(); i++)
                    {
                        amount += Convert.ToDouble(recordTicket[i].Price);
                            //Convert.ToDouble(obj[i].Quantity) * Convert.ToDouble(recordTicket[i].Price);
                    }
                    for (int i = 0; i < recordTicket.Count(); i++)
                    {
                        //amount += Convert.ToDouble(obj[i].Quantity) * Convert.ToDouble(recordTicket[i].Price);
                        if (obj[i] != null && recordTicket[i] != null)
                        {
                            recordTicket[i].InvoicedDate = DateTime.Now;
                            recordTicket[i].InvoicedTime = DateTime.Now.ToString("hh:mm tt");
                            recordTicket[i].IsInvoiced = true;
                            recordTicket[i].InvoicedBy = "cashier@gmail.com";
                            recordTicket[i].OrderAmount = Math.Round(amount, 2).ToString();
                            recordTicket[i].Quantity = obj[i].Quantity;
                            db.Entry(recordTicket[i]).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                await db.SaveChangesAsync();

                //bool isEmailSent = emailService.SendNewOrderEmail("NewOrder", recordTicket[0].Email, recordTicket, cartdetail, obj);

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


        [HttpPost("SaleHistoryCreate")]
        public async Task<IActionResult> SaleHistoryCreate(List<PosSale> obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<SaleHistory>>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                //var supervisorId = obj[0].SupervisorId;
                //var record = db.Supervisors.Where(x => x.SupervisorId == supervisorId)?.FirstOrDefault();



                SaleHistory salehistory = null;
                double grossamount = 0;
                //for (int a = 0; a < obj.Count(); a++)
                //{
                //    grossamount += Convert.ToDouble(obj[a].Total);
                //}
                var cartdetail = db.CartDetails.Where(x => x.TicketId == obj[0].InvoiceNumber).ToList();
                var possalelist = db.PosSales.Where(x => x.InvoiceNumber == obj[0].InvoiceNumber).ToList();
                grossamount = Convert.ToDouble(obj[0].InvoiceTotal);
                for (int i = 0; i < obj.Count(); i++)
                {
                    int QuantityDifference = 0;
                    if (obj[i].ShipmentLimit == null)
                    {
                        obj[i].ShipmentLimit = "100";
                    }
                    if (obj[i].Quantity == null)
                    {
                        obj[i].Quantity = "1";
                    }
                    if (obj[i].RingerQuantity == null)
                    {
                        obj[i].RingerQuantity = "1";
                    }
                    if (int.Parse(obj[i].RingerQuantity) > int.Parse(obj[i].ShipmentLimit))
                    {
                        QuantityDifference = int.Parse(obj[i].ShipmentLimit) - int.Parse(obj[i].RingerQuantity);
                    }
                    else
                    {

                        QuantityDifference = int.Parse(obj[i].Quantity) - int.Parse(obj[i].RingerQuantity);
                    }
                    if (QuantityDifference > 0)
                    {
                        salehistory = new SaleHistory();
                        salehistory.ProductId = obj[i].ItemId;
                        salehistory.CartDetailId = cartdetail[i].CartId.ToString();
                        salehistory.ItemNumber = obj[i].ItemNumber;
                        salehistory.OrderQuantity = int.Parse(obj[i].Quantity);
                        salehistory.RingerQuantity = int.Parse(obj[i].RingerQuantity);
                        salehistory.ShipmentLimit = int.Parse(obj[i].ShipmentLimit);
                        salehistory.PosSaleId = possalelist[i].PossaleId.ToString();
                        salehistory.TicketId = obj[0].InvoiceNumber;
                        salehistory.QuantityDifference = QuantityDifference;
                        salehistory.CustomerId = obj[i].CustomerId;
                        db.SaleHistories.Add(salehistory);
                    }
                    //db.SaveChanges();
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
                var recordss = db.PosSales.AsQueryable();
                var record = db.PosSales.AsQueryable().ToList();
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
        public IActionResult DeleteSale(string invoice)
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
                    var zipData = db.ZipCodes.ToList().Where(x => x.Id == Convert.ToInt32(record.Zip)).FirstOrDefault();
                    var getpay = db.Receivables.ToList().Where(x => x.AccountId == record.AccountId).FirstOrDefault();
                    var classificationdata = db.CustomerClassifications.Where(x => x.CustomerInfoId == record.Id && x.CustomerCode == record.CustomerCode).FirstOrDefault();
                    var billingData = db.CustomerBillingInfos.Where(x => x.CustomerInformationId == record.Id && x.CustomerCode == record.CustomerCode).AsQueryable()?.FirstOrDefault();
                    var recieveableData = db.Receivables.Where(x => x.AccountId == record.AccountId && x.AccountNumber == record.AccountNumber)?.FirstOrDefault();
                    if (zipData != null)
                    {
                        record.ZipCode = zipData;
                    }
                    else
                    {
                        record.ZipCode = new ZipCode();
                        record.ZipCode.Code = "";
                    }
                    if (billingData != null)
                    {
                        record.CustomerBillingInfo = billingData;
                    }
                    if (recieveableData != null)
                    {
                        if (recieveableData.Amount == null)
                        {
                            recieveableData.Amount = "0.00";
                        }
                        else
                        {
                            var amount = Convert.ToDouble(recieveableData.Amount);
                            recieveableData.Amount = Math.Round(amount, 2).ToString();
                        }
                        record.Receivable = recieveableData;
                    }
                    if (classificationdata != null)
                    {
                        record.CustomerClassification = classificationdata;
                        if (getpay != null)
                        {

                            record.Balance = getpay.Amount;
                        }



                      
                    }
                    else
                    {
                        record.CustomerClassification = null;
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
                                objacount.Title = (record.FullName != null ? record.Company : record.FirstName + record.LastName);
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
        [HttpGet("CustomerInformationGet")]
        public IActionResult CustomerInformationGet()
        {
            try
            {
                var inforecord = db.CustomerInformations.ToList();
                List<CustomerInformation> record = new List<CustomerInformation>();
                CustomerInformation customerinformation = null;

                foreach (var item in inforecord)
                {
                    customerinformation = new CustomerInformation();
                    if (inforecord != null)
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

                //else
                //{
                //    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                //    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                //    return Ok(Response);
                //}

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
                var record = db.InventoryStocks.ToList().Where(x => x.StockItemNumber == itemnumber)?.FirstOrDefault();
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


        [HttpGet("GetItemByItemNumber/{itemnumber}")]
        public IActionResult GetItemByItemNumber(string itemnumber)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                var record = db.Products.ToList().Where(x => x.ItemNumber == itemnumber)?.FirstOrDefault();
                if (record != null)
                {
                    //var getitems = db.Products.ToList().Where(x => x.Id == record.ProductId).FirstOrDefault();

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
                    var Response = ResponseBuilder.BuildWSResponse<Product>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("GetItemBySupplierItemNumber")]
        public IActionResult GetItemBySupplierItemNumber(SupplierItemNumber obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                var record = db.SupplierItemNumbers.ToList().Where(x => x.SupplierItemNum == obj.SupplierItemNum && x.VendorId == obj.VendorId)?.FirstOrDefault();
                if (record != null)
                {
                    //var getitems = db.Products.ToList().Where(x => x.Id == record.ProductId).FirstOrDefault();

                    var product = db.Products.Where(x => x.Id == record.ProductId).FirstOrDefault();
                    //if (product != null)
                    //{
                    //    var vendor = db.Vendors.Where(x => x.VendorId == record.VendorId).FirstOrDefault();
                    //    product.vendor = vendor;
                    //}
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, product);
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
                    var Response = ResponseBuilder.BuildWSResponse<Product>();
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
                bool checkname = db.SupplierItemNumbers.ToList().Exists(p => p.SupplierItemNum.Equals(obj.SupplierItemNum, StringComparison.CurrentCultureIgnoreCase) && p.VendorId.Equals(obj.VendorId));
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


        /////

        [HttpGet("GetAllStock")]
        public IActionResult GetAllStock()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<InventoryStock>>();
                var record = db.InventoryStocks.AsQueryable().ToList();
                //   return Ok(record);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
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

        [HttpGet("GetStockLevel/{id}/{price}")]
        public IActionResult GetStockLevel(int id, string price)
        {
            try
            {
                var Getid = id;
                double Getprince = Convert.ToDouble(price);
                var count = 0;
                var Response = ResponseBuilder.BuildWSResponse<string>();
                List<PurchaseOrder> obj = null;
                obj = new List<PurchaseOrder>();
                var GetSupliersPrices = db.PurchaseOrders.ToList().Where(x => x.ItemId == id).ToList();
                if (GetSupliersPrices != null)
                {

                    for (int i = 0; i < GetSupliersPrices.Count(); i++)
                    {
                        PurchaseOrder poobj = null;


                        if (Convert.ToDouble(GetSupliersPrices[i].Price) < Getprince)
                        {
                            poobj = new PurchaseOrder();
                            poobj = GetSupliersPrices[i];
                            obj.Add(poobj);
                            count = obj.Count();

                        }
                    }

                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, count.ToString());
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
        [HttpGet("GetStockLevellist/{id}/{price}")]
        public IActionResult GetStockLevellist(int id, string price)
        {
            try
            {
                var Getid = id;
                double Getprice = Convert.ToDouble(price);
                var count = 0;
                var Response = ResponseBuilder.BuildWSResponse<List<PurchaseOrder>>();
                List<PurchaseOrder> obj = null;
                obj = new List<PurchaseOrder>();
                var GetSupliersPrices = db.PurchaseOrders.ToList().Where(x => x.ItemId == id).ToList();
                if (GetSupliersPrices != null)
                {

                    for (int i = 0; i < GetSupliersPrices.Count(); i++)
                    {
                        PurchaseOrder poobj = null;


                        if (Convert.ToDouble(GetSupliersPrices[i].Price) < Getprice)
                        {
                            poobj = new PurchaseOrder();
                            poobj = GetSupliersPrices[i];
                            obj.Add(poobj);
                            //  count = obj.Count();

                        }
                    }

                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, obj);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PurchaseOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }


        }
        [HttpGet("GetStockAlertCount")]
        public IActionResult GetStockAlertCount()
        {
            try
            {
                double counter = 0;
                var allitems = db.InventoryStocks.ToList();

                var Response = ResponseBuilder.BuildWSResponse<string>();
                List<InventoryStock> obj = null;
                obj = new List<InventoryStock>();
                for (int i = 0; i < allitems.Count(); i++)
                {
                    var fifteenDaysAgo = DateTime.Today.AddDays(-15);
                    InventoryStock Inobj = null;
                    var allsale = db.PosSales.ToList().Where(x => x.InvoiceDate >= fifteenDaysAgo && x.ItemId == allitems[i].ProductId).ToList();
                    double saleqty = 0;
                    for (int x = 0; x < allsale.Count(); x++)
                    {
                        saleqty += Convert.ToDouble(allsale[x].Quantity);
                        double remainingqty = Convert.ToDouble(allitems[i].Quantity);
                        if (remainingqty < saleqty)
                        {
                            Inobj = allitems[i];
                            counter = counter + 1;
                            // obj.Add(Inobj);
                        }
                    }

                }

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, counter.ToString());
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
        [HttpGet("GetStockAlertList")]
        public IActionResult GetStockAlertList()
        {
            try
            {
                double counter = 0;
                var allitems = db.InventoryStocks.ToList();

                var Response = ResponseBuilder.BuildWSResponse<List<InventoryStock>>();
                List<InventoryStock> obj = null;
                obj = new List<InventoryStock>();
                for (int i = 0; i < allitems.Count(); i++)
                {
                    double DayAges = 0;
                    var currentItem = db.Products.Find(allitems[i].ProductId);
                    //if (currentItem.MaintainStockForDays == null)
                    //{
                    //    DayAges = -14;
                    //}
                    //else
                    //{

                    //    if (currentItem.MaintainStockForDays == "7-14 Days")
                    //    {
                    //        DayAges = -14;
                    //    }
                    //    else if (currentItem.MaintainStockForDays == "14-21 Days")
                    //    {
                    //        DayAges = -21;
                    //    }
                    //    else if (currentItem.MaintainStockForDays == "21-28 Days")
                    //    {
                    //        DayAges = -28;
                    //    }
                    //    else if (currentItem.MaintainStockForDays == "OneMonth")
                    //    {
                    //        DayAges = -30;
                    //    }
                    //    else if (currentItem.MaintainStockForDays == "TwoMonths")
                    //    {
                    //        DayAges = -60;
                    //    }
                    //    else if (currentItem.MaintainStockForDays == "ThreeMohtns")
                    //    {
                    //        DayAges = -90;
                    //    }
                    //}
                    InventoryStock Inobj = null;
                    Inobj = new InventoryStock();
                    var fifteenDaysAgo = DateTime.Today.AddDays(DayAges);
                    var allsale = db.PosSales.ToList().Where(x => x.InvoiceDate >= fifteenDaysAgo && x.ItemId == allitems[i].ProductId).ToList();
                    double saleqty = 0;
                    double remainingqty = 0;
                    for (int x = 0; x < allsale.Count(); x++)
                    {
                        saleqty += Convert.ToDouble(allsale[x].Quantity);
                        remainingqty = Convert.ToDouble(allitems[i].Quantity);
                    }
                    if (remainingqty <= saleqty)
                    {
                        var remainingdays = DayAges.ToString();
                        if (remainingdays != null && remainingdays != "undefined")
                        {
                            string trimmed = (remainingdays as string).Trim('-');
                            remainingdays = trimmed;
                        }
                        allitems[i].RemainingDays = remainingdays;
                        Inobj = allitems[i];
                        counter = counter + 1;
                        obj.Add(Inobj);
                    }
                }

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, obj);
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
        [HttpGet("GetStockAlertByID/{id}")]
        public IActionResult GetStockAlertByID(int id)
        {
            try
            {
                double counter = 0;
                var allitems = db.InventoryStocks.ToList().Where(x => x.ProductId == id).FirstOrDefault();

                var Response = ResponseBuilder.BuildWSResponse<List<InventoryStock>>();
                List<InventoryStock> obj = null;
                obj = new List<InventoryStock>();
                if (allitems != null)
                {


                    double DayAges = 0;
                    var currentItem = db.Products.Find(allitems.ProductId);
                    if (currentItem.MaintainStockForDays == null)
                    {
                        DayAges = -14;
                    }
                    else
                    {

                        if (currentItem.MaintainStockForDays == "7-14 Days")
                        {
                            DayAges = -14;
                        }
                        else if (currentItem.MaintainStockForDays == "14-21 Days")
                        {
                            DayAges = -21;
                        }
                        else if (currentItem.MaintainStockForDays == "21-28 Days")
                        {
                            DayAges = -28;
                        }
                        else if (currentItem.MaintainStockForDays == "OneMonth")
                        {
                            DayAges = -30;
                        }
                        else if (currentItem.MaintainStockForDays == "TwoMonths")
                        {
                            DayAges = -60;
                        }
                        else if (currentItem.MaintainStockForDays == "ThreeMohtns")
                        {
                            DayAges = -90;
                        }
                    }
                    InventoryStock Inobj = null;
                    Inobj = new InventoryStock();
                    var fifteenDaysAgo = DateTime.Today.AddDays(DayAges);
                    var allsale = db.PosSales.ToList().Where(x => x.InvoiceDate >= fifteenDaysAgo && x.ItemId == allitems.ProductId).ToList();
                    if (allsale.Count != 0)
                    {
                        double saleqty = 0;
                        double remainingqty = 0;
                        double remainstock = 0;
                        for (int x = 0; x < allsale.Count(); x++)
                        {
                            saleqty += Convert.ToDouble(allsale[x].Quantity);
                            remainingqty = Convert.ToDouble(allitems.Quantity);
                        }
                        remainstock = saleqty / 100 * remainingqty;
                        var remainingdays = DayAges.ToString();
                        if (remainingdays != null && remainingdays != "undefined")
                        {
                            string trimmed = (remainingdays as string).Trim('-');
                            remainingdays = trimmed;
                        }
                        //there
                        allitems.items = new Product();
                        allitems.Quantity = Convert.ToString(remainstock);
                        allitems.items.SalesLimit = Convert.ToString(saleqty);
                        allitems.items.MaxOrderQty = Convert.ToString(saleqty);
                        allitems.items.MinOrderQty = Convert.ToString(remainingqty);
                        allitems.RemainingDays = remainingdays;
                        Inobj = allitems;
                        obj.Add(Inobj);
                    }
                    else
                    {
                        var remainingdays = DayAges.ToString();
                        if (remainingdays != null && remainingdays != "undefined")
                        {
                            string trimmed = (remainingdays as string).Trim('-');
                            remainingdays = trimmed;
                        }
                        allitems.items = new Product();
                        allitems.Quantity = Convert.ToString(currentItem.QtyinStock);
                        allitems.items.SalesLimit = Convert.ToString(currentItem.SalesLimit);
                        allitems.items.MaxOrderQty = Convert.ToString(currentItem.MaxOrderQty);
                        allitems.items.MinOrderQty = Convert.ToString(currentItem.MinOrderQty);
                        allitems.RemainingDays = remainingdays;
                        Inobj = allitems;
                        obj.Add(Inobj);
                    }
                }

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, obj);
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




        [HttpGet("InventoryPurchaseGetByInvoiceNumber/{invoice}")]
        public IActionResult InventoryPurchaseGetByInvoiceNumber(string invoice)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PurchaseOrder>>();
                var record = db.PurchaseOrders.ToList().Where(x => x.InvoiceNumber == invoice).ToList();

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PurchaseOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("InventorySaleGetByInvoiceNumber/{invoice}")]
        public IActionResult InventorySaleGetByInvoiceNumber(string invoice)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                var record = db.PosSales.ToList().Where(x => x.InvoiceNumber == invoice).ToList();

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

        [HttpGet("InventoryVendorGetbyProduct/{id}")]
        public IActionResult InventoryVendorGetbyProduct(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Vendor>>();
                List<Vendor> allVendors = null;
                allVendors = new List<Vendor>();
                var productFound = db.Products.Find(id);
                if (productFound != null)
                {
                    var recordPurchase = db.PurchaseOrders.ToList().Where(x => x.ItemId == productFound.Id).ToList();
                    for (int i = 0; i < recordPurchase.Count(); i++)
                    {
                        var record = db.Vendors.ToList().Where(x => x.VendorId == recordPurchase[i].SupplierId).FirstOrDefault();
                        if (record != null)
                        {
                            var itemnumrecord = db.SupplierItemNumbers.ToList().Where(x => x.VendorId == recordPurchase[i].SupplierId).FirstOrDefault();
                            if (itemnumrecord != null)
                            {
                                record.supplierItemNumber = itemnumrecord;
                            }
                            record.purchaseOrder = recordPurchase[i];
                            allVendors.Add(record);
                        }
                    }

                    if (allVendors.Count() < 0)
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, allVendors);

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
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("InventorySupplierGetByVendorid/{id}")]
        public IActionResult InventorySupplierGetByVendorid(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Vendor>>();
                var record = db.Vendors.ToList().Where(x => x.VendorId == id).ToList();

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Vendor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ItemGetWithItemName/{itemname}")]
        public IActionResult ItemGetWithItemName(string itemname)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<IEnumerable<Product>>();

                var record = db.Products.ToList().Where(x => x.Name.Equals(itemname, StringComparison.CurrentCultureIgnoreCase));
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
        [HttpGet("GetAllItem")]
        public IActionResult GetAllItem(string itemname)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<IEnumerable<Product>>();

                var record = db.Products.ToList();
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
        [HttpGet("GetItemWithVendorId/{vendorId}")]
        public IActionResult GetItemWithVendorId(int? vendorId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<IEnumerable<Product>>();

                var supplierRecord = db.SupplierItemNumbers.Where(x => x.VendorId == vendorId).Distinct().ToList();
                var productRecord = db.Products.ToList();
                List<Product> products = new List<Product>();
                for (int i = 0; i < supplierRecord.Count(); i++)
                {
                    for (int x = 0; x < productRecord.Count(); x++)
                    {
                        if (supplierRecord[i]?.ProductId != null)
                        {
                            if (supplierRecord[i].ProductId == productRecord[x].Id)
                            {
                                products.Add(productRecord[x]);
                            }
                        }
                    }

                }
                if (products != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, products);
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
        [HttpGet("GetVendorOutStateTax/{vendorId}")]
        public IActionResult GetVendorOutStateTax(int? vendorId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Vendor>();

                var record = db.Vendors.Where(x => x.VendorId == vendorId)?.FirstOrDefault();
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


  




        [HttpGet("GetSaleHistory")]
        public IActionResult GetSaleHistory()
        {
            try
            {
                var productlist = db.Products.ToList();
                var customerlist = db.CustomerInformations.ToList();
                var saleshisitorylist = db.SaleHistories.OrderBy(x => x.TicketId).ToList();
                for (int i = 0; i < saleshisitorylist.Count(); i++)
                {
                    var product = productlist.ToList().Where(x => x.Id == saleshisitorylist[i].ProductId).FirstOrDefault();
                    var customer = customerlist.ToList().Where(x => x.Id == saleshisitorylist[i].CustomerId).FirstOrDefault();
                    if (product != null)
                    {
                        saleshisitorylist[i].ProductName = product.Name;

                    }
                    else
                    {
                        saleshisitorylist[i].ProductName = "N/A";

                    }
                    if (customer != null)
                    {
                        saleshisitorylist[i].CustomerName = customer.Company;

                    }
                    else
                    {
                        saleshisitorylist[i].CustomerName = "N/A";

                    }
                }
                var Response = ResponseBuilder.BuildWSResponse<List<SaleHistory>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, saleshisitorylist);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SaleHistory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("GetSaleHistotyById/{ticketId}")]
        public IActionResult GetSaleHistotyById(string ticketId)
        {
            try
            {
                var productlist = db.Products.ToList();
                var customerlist = db.CustomerInformations.ToList();
                var saleshisitorylist = db.SaleHistories.Where(x => x.TicketId == ticketId).ToList();
                for (int i = 0; i < saleshisitorylist.Count(); i++)
                {
                    //if (!Convert.ToString(saleshisitorylist[i].ProductId).Contains('-'))
                    //{
                    //    var ProID = "00-" + Convert.ToString(saleshisitorylist[i].ProductId);
                    //}
                    var product = productlist.Where(x => x.Id == saleshisitorylist[i].ProductId).FirstOrDefault();
                    var customer = customerlist.Where(x => x.Id == saleshisitorylist[i].CustomerId).FirstOrDefault();
                    saleshisitorylist[i].ProductName = product.Name;
                    saleshisitorylist[i].CustomerName = customer.Company;
                }
                var Response = ResponseBuilder.BuildWSResponse<List<SaleHistory>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, saleshisitorylist);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SaleHistory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ActivitylogCreate")]
        public async Task<IActionResult> ActivitylogCreate(ActivityLog obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ActivityLog>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                obj.NewDetails = obj.CreatedBy + " " + obj.OperationName + " on " + obj.CreatedDate;
                db.ActivityLogs.Add(obj);

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



        [HttpPost("SendRequestforApproval")]
        public async Task<IActionResult> SendRequestforApproval(List<PosSale> obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }



                var customeridlist = db.CustomerOrders.Where(x => x.TicketId == obj[0].InvoiceNumber).ToList();
                for (int i = 0; i < obj.Count(); i++)
                {
                    var comparequantity = 0;
                    var recordTicket = customeridlist.Where(x => x.ProductName == obj[i].ItemName).FirstOrDefault();
                    if (recordTicket != null)
                    {
                        if (int.Parse(obj[i].Quantity) > int.Parse(obj[i].ShipmentLimit))
                        {
                            comparequantity = int.Parse(obj[i].ShipmentLimit);
                        }
                        else
                        {
                            comparequantity = int.Parse(obj[i].Quantity);
                        }
                        if (int.Parse(obj[i].RingerQuantity) > comparequantity)
                        {
                            recordTicket.NeedApproval = true;
                            recordTicket.AcceptApproval = false;
                        }
                        else
                        {
                            recordTicket.NeedApproval = false;
                            recordTicket.AcceptApproval = false;
                        }

                        recordTicket.ChangeQuantity = int.Parse(obj[i].RingerQuantity);
                        db.Entry(recordTicket).State = EntityState.Modified;
                        db.SaveChanges();
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


        [HttpPost("AuthorizeQuantityInCart")]
        public async Task<IActionResult> AuthorizeQuantityInCart(AutherizeOrderLimit model)
        {
            try
            {
                if (!model.TicketId.Contains("00-"))
                {
                    model.TicketId = "00" + model.TicketId;
                }
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


        [HttpGet("GetItemCostChangeList/{invoicenumber}")]
        public IActionResult GetItemCostChangeList(string invoicenumber)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<ItemCostChange>>();
                var record = db.ItemCostChanges.ToList().Where(x => x.PoInvoiceNumber == invoicenumber).ToList();

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                //if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                //{
                var Response = ResponseBuilder.BuildWSResponse<ItemCostChange>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                return Ok(Response);
                //}
                //return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteAttachement/{id}")]
        public async Task<IActionResult> DeleteAttachement(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SupplierDocument>();
                var data = db.SupplierDocuments.ToList().Where(x => x.DocumentId == id).FirstOrDefault();
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

        //ACCOUNT NUMBER GENERATION
        [HttpGet("LastSupplierAccountGet/{id}")]
        public IActionResult LastSupplierAccountGet(string id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Account>();
                var record = db.AccountSubGroups.ToList().Where(x => x.AccountSubGroupId == id).FirstOrDefault();
                if (record != null)
                {
                    var getAccount = db.Accounts.ToList().Where(x => x.AccountSubGroupId == record.AccountSubGroupId).LastOrDefault();
                    if (getAccount != null)
                    {
                        if (getAccount.AccountSubGroup != null)
                        {
                            getAccount.AccountSubGroup = null;
                        }
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, getAccount);
                    }
                    else
                    {
                        Account objacount = null;
                        objacount = new Account();
                        objacount.AccountId = record.AccountSubGroupId + "-0001";
                        objacount.Status = 1;
                        objacount.AccountSubGroupId = record.AccountSubGroupId;
                        objacount.Title = "Supplier";
                        db.Accounts.Add(objacount);
                        db.SaveChanges();
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, objacount);
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

        [HttpGet("AccountSubGroupsGet")]
        public IActionResult AccountSubGroupsGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<AccountSubGroup>>();
                var record = db.AccountSubGroups.ToList();
                if (record.Count() > 0)
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
                    var Response = ResponseBuilder.BuildWSResponse<AccountSubGroup>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("LedgerCategoryGet")]
        public IActionResult LedgerCategoryGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<LedgerCategory>>();
                var record = db.LedgerCategories.ToList();
                if (record.Count() > 0)
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
                    var Response = ResponseBuilder.BuildWSResponse<LedgerCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetproductByStockNo/{id}")]
        public IActionResult GetproductByStockNo(string id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                var record = db.Products.Where(x => x.ItemNumber == id).FirstOrDefault();
                if (record != null)
                {
                    var CheckQty = db.InventoryStocks.Where(x => x.ItemCode == id || x.ItemName == id).FirstOrDefault();
                    if (CheckQty != null)
                    {
                        record.ItemQuantity = CheckQty.Quantity;
                    }
                    else
                    {
                        record.ItemQuantity = "0.00";
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

        [HttpGet("GetproductByStockDesc/{Name}")]
        public IActionResult GetproductByStockDesc(string Name)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                var record = db.Products.Where(x => x.Description == Name).FirstOrDefault();
                if (record != null)
                {
                    var CheckQty = db.InventoryStocks.Where(x => x.ItemName == Name).FirstOrDefault();
                    if (CheckQty != null)
                    {
                        record.ItemQuantity = CheckQty.Quantity;
                    }
                    else
                    {
                        record.ItemQuantity = "Nill";
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



        [HttpGet("CustomerInformationfetch")]
        public IActionResult CustomerInformationfetch()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerInformation>>();
                var record = db.CustomerInformations.ToList();
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CustomerDataByID/{id}")]
        public IActionResult CustomerDataByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                var record = db.CustomerInformations.ToList().Where(x => x.UserId == id).FirstOrDefault();
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CustomerDataByCode/{id}")]
        public IActionResult CustomerDataByCode(string id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                var record = db.CustomerInformations.ToList().Where(x => x.CustomerCode == id).FirstOrDefault();
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
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("GetOpenSales")]
        public IActionResult GetOpenSales()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                var record = db.PosSales.Where(x => x.IsOpen == true).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
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


        [HttpGet("GetPostedsales")]
        public IActionResult GetPostedsales()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                var record = db.PosSales.Where(x => x.IsClose == true).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
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

        [HttpGet("NewVendorGetByCode/{id}")]
        public IActionResult NewVendorGetByCode(string id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Vendor>();

                var record = db.Vendors.ToList().Where(x => x.VendorCode == id).FirstOrDefault();
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


        [HttpGet("CreateCodeForCheque")]
        public IActionResult CreateCodeForCheque()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<int>();

                Random rnd = new Random();
                int myRandomNo = rnd.Next(10000000, 99999999);
                if (myRandomNo != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, myRandomNo);
                    return Ok(Response);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("CreateCreditSupplier")]
        public async Task<IActionResult> CreateCreditSupplier(CreditAdjustment obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<int>();

                CreditAdjustment creditAdjustment = new CreditAdjustment();
                creditAdjustment.Type = "Supplier";
                creditAdjustment.UserId = obj.UserId;
                creditAdjustment.UserName = obj.UserName;
                creditAdjustment.CompanyName = obj.CompanyName;
                creditAdjustment.Comments = obj.Comments;
                creditAdjustment.CreditAmount = obj.CreditAmount;
                var remainingAmount = (Convert.ToDouble(obj.RemainingAmount) - Convert.ToDouble(obj.CreditAmount));
                creditAdjustment.RemainingAmount = Math.Round(remainingAmount, 2).ToString();
                creditAdjustment.CreditDate = DateTime.Now;
                creditAdjustment.InvoiceNo = obj.InvoiceNo;


                var result = db.CreditAdjustments.Add(creditAdjustment);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, result.Entity.AdjustmentId);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CreditAdjustment>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCreditSupplierById/{id}/{invNo}")]
        public IActionResult GetCreditSupplierById(int id, string invNo)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CreditAdjustment>>();
                var record = db.CreditAdjustments.Where(x => x.UserId == id && x.InvoiceNo == invNo).ToList();
                foreach (var item in record)
                {
                    item.createdDateonly = item.CreditDate.Value.ToString("dd/MM/yyyy");
                    item.amount = Convert.ToDouble(item.CreditAmount);
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CreditAdjustment>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ChangePayment")]
        public IActionResult ChangePayment(SalesInvoice obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SalesInvoice>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var FoundInvoice = db.PosSales.ToList().Where(x => x.InvoiceNumber == obj.InvoiceNumber).ToList();
                for (int i = 0; i < FoundInvoice.Count(); i++)
                {
                    if (obj.IsInvoicedPaid == true)
                    {
                        FoundInvoice[i].IsPaid = true;

                    }
                    FoundInvoice[i].IsClose = true;
                    FoundInvoice[i].IsOpen = false;
                    db.Entry(FoundInvoice[i]).State = EntityState.Modified;
                    db.SaveChanges();
                }
                db.SalesInvoices.Add(obj);
                db.SaveChanges();
                var getacc = db.CustomerInformations.Where(x => x.CustomerCode == obj.CustomerCode).FirstOrDefault();
                //if (getacc != null)
                //{
                   
                //}
                var getaccount = db.Accounts.ToList().Where(a => a.AccountId == getacc.AccountNumber).FirstOrDefault();
                if(getaccount != null)
                {
                   // var getCHaccount = db.Accounts.ToList().Where(a => a.Title == "Cash in hand").FirstOrDefault();
                    for (int i = 0; i < 2; i++)
                    {
                        Receiving transaction = null;
                        transaction = new Receiving();

                        transaction.AccountName = getaccount.Title;
                        transaction.AccountNumber = getaccount.AccountId;
                        transaction.NetAmount = obj.TotalPaid;
                        transaction.CashBalance = obj.TotalAmount;
                        transaction.InvoiceNumber = obj.InvoiceNumber;
                        transaction.Change = obj.Change;
                        transaction.Date = DateTime.Now;

                        if (i == 0) { transaction.Credit = obj.Balance; transaction.Debit = "0.00"; }
                        else { transaction.Credit = "0.00"; transaction.Debit = obj.Balance; }

                        db.Receivings.Add(transaction);
                        db.SaveChanges();
                    }


                    var receive = db.Receivables.Where(x => x.AccountNumber == getaccount.AccountId).FirstOrDefault();
                    if (receive != null)
                    {
                        double num1 = Convert.ToDouble(receive.Amount);
                        double num2 = Convert.ToDouble(obj.TotalAmount);
                        var num3 = num1 - num2;
                        receive.Amount = Convert.ToString(num3);
                        db.Entry(receive).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    Account objAcc = new Account();
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
                                objAcc.AccountId = subaccrecord.AccountSubGroupId + "-00" + Convert.ToString(getcode);

                            }
                            else if (getcode > 99)
                            {
                                objAcc.AccountId = subaccrecord.AccountSubGroupId + "-0" + Convert.ToString(getcode);
                            }
                            else
                            {
                                objAcc.AccountId = subaccrecord.AccountSubGroupId + "-000" + Convert.ToString(getcode);
                            }
                            objAcc.Title = obj.CustomerName;
                            objAcc.Status = 1;
                            objAcc.AccountSubGroupId = subaccrecord.AccountSubGroupId;
                            var customeracc = db.Accounts.Add(objAcc);
                            db.SaveChanges();

                            var receive = db.Receivables.Where(x => x.AccountNumber == customeracc.Entity.AccountId).FirstOrDefault();
                            if (receive != null)
                            {
                                double num1 = Convert.ToDouble(receive.Amount);
                                double num2 = Convert.ToDouble(obj.TotalAmount);
                                var num3 = num1 - num2;
                                receive.Amount = Convert.ToString(num3);
                                db.Entry(receive).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        for (int i = 0; i < 2; i++)
                        {
                            Receiving transaction = null;
                            transaction = new Receiving();

                            transaction.AccountName = getAccount.Title;
                            transaction.AccountNumber = getAccount.AccountId;
                            transaction.NetAmount = obj.TotalPaid;
                            transaction.CashBalance = obj.TotalAmount;
                            transaction.InvoiceNumber = obj.InvoiceNumber;
                            transaction.Change = obj.Change;
                            transaction.Date = DateTime.Now;
                            
                            if (i == 0){ transaction.Credit = obj.Balance; transaction.Debit = "0.00"; }
                            else{ transaction.Credit = "0.00"; transaction.Debit = obj.Balance; }

                            db.Receivings.Add(transaction);
                            db.SaveChanges();
                        }
                    }

                }


                
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SalesInvoice>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SalesInvoiceTransactions")]
        public async Task<IActionResult> SalesInvoiceTransactions(List<SalesInvTransaction> obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SalesInvTransaction>();
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest();
                //}
                for (int i = 0; i < obj.Count(); i++)
                {
                    var SaleInvId = db.SalesInvoices.Where(x => x.InvoiceNumber == obj[0].InvoiceNumber).FirstOrDefault();
                    if (SaleInvId != null)
                    {
                        obj[i].SalesInvoiceId = SaleInvId.Id;
                    }
                   await db.SalesInvTransactions.AddAsync(obj[i]);
                   await db.SaveChangesAsync();
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SalesInvTransaction>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("NextItemCode")]
        public IActionResult NextItemCode()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<string>();
                var record = db.Products.ToList();
                Product newitems = new Product();
                var fullcode = "";
                if (record[0].ItemNumber != null && record[0].ItemNumber != "string" && record[0].ItemNumber != "")
                {
                    int large, small;
                    int salesID = 0;
                    large = Convert.ToInt32(record[0].ItemNumber);
                    small = Convert.ToInt32(record[0].ItemNumber);
                    for (int i = 0; i < record.Count(); i++)
                    {
                        if (record[i].ItemNumber != null)
                        {
                            var t = Convert.ToInt32(record[i].ItemNumber);
                            if (Convert.ToInt32(record[i].ItemNumber) > large)
                            {
                                salesID = record[i].Id;
                                large = Convert.ToInt32(record[i].ItemNumber);

                            }
                            else if (Convert.ToInt32(record[i].ItemNumber) < small)
                            {
                                small = Convert.ToInt32(record[i].ItemNumber);
                            }
                            else
                            {
                                if (large < 2)
                                {
                                    salesID = record[i].Id;
                                }
                            }
                        }
                    }
                    newitems = record.ToList().Where(x => x.Id == Convert.ToInt32(salesID)).FirstOrDefault();
                    if (newitems != null)
                    {
                        if (newitems.ItemNumber != null)
                        {
                            var VcodeSplit = newitems.ItemNumber;
                            int code = Convert.ToInt32(VcodeSplit) + 1;
                            fullcode = "0000" + Convert.ToString(code);
                        }
                        else
                        {
                            fullcode = "0000" + "1";
                        }
                    }
                    else
                    {
                        fullcode = "0000" + "1";
                    }
                }
                else
                {
                    fullcode = "0000" + "1";
                }

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, fullcode);
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


        [HttpGet("GetAllProductsCounter")]
        public IActionResult GetAllProductsCounter()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<int>();

                int counter = db.Products.ToList().Count();
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

        [HttpGet("GetAllPayables")]
        public IActionResult GetAllPayables()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<double>();

                double pay = 0;
                var counter = db.Payables.ToList();
                if (counter.Count() > 0)
                {
                    for (int i = 0; i < counter.Count(); i++)
                    {
                        if (counter[i].Amount != null)
                        {
                            pay += Convert.ToDouble(counter[i].Amount);
                        }

                    }

                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, pay);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, pay);
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


        [HttpGet("GetAllReceivables")]
        public IActionResult GetAllReceivables()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<double>();

                double pay = 0;
                var counter = db.Receivables.ToList();
                if (counter.Count() > 0)
                {
                    for (int i = 0; i < counter.Count(); i++)
                    {
                        pay += Convert.ToDouble(counter[i].Amount);
                    }

                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, pay);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, pay);
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


        [HttpPost("ItemDocumentCreate")]
        public IActionResult ItemDocumentCreate(ItemDocument document)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ItemDocument>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.ItemDocuments.ToList().Exists(p => p.DocumentName.Equals(document.DocumentName, StringComparison.CurrentCultureIgnoreCase));
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
                db.ItemDocuments.Add(document);
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

        [HttpDelete("DeleteItemsAttachement/{id}")]
        public async Task<IActionResult> DeleteItemsAttachement(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ItemDocument>();
                var data = db.ItemDocuments.ToList().Where(x => x.DocumentId == id).FirstOrDefault();
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.ItemDocuments.Remove(data);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ItemDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("DocumentGetByItemID/{id}")]
        public IActionResult DocumentGetByItemID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<ItemDocument>>();

                var record = db.ItemDocuments.ToList().Where(x => x.ItemId == id).ToList();
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
                    var Response = ResponseBuilder.BuildWSResponse<ItemDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
    }

}

