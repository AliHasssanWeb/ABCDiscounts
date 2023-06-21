using ABC.EFCore.Entities.POS;
using ABC.EFCore.Repository.Edmx;
using ABC.POS.Domain.DataConfig;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ABC.POS.Domain.DataConfig.RequestSender;
namespace ABC.POS.Website.Controllers
{
    public class CustomerbxController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewCustomer()
        {
            try
            {
                CustomerInformation model = new CustomerInformation();
                SResponse respcustomer = RequestSender.Instance.CallAPI("api", "Customer/CustomerInformationGet", "GET");
                if (respcustomer.Status && (respcustomer.Resp != null) && (respcustomer.Resp != ""))
                {
                    ResponseBack<List<CustomerInformation>> record = JsonConvert.DeserializeObject<ResponseBack<List<CustomerInformation>>>(respcustomer.Resp);
                    if (record != null && record.Data.Count() > 0)
                    {
                        CustomerInformation newcustomer = new CustomerInformation();
                        var fullcode = "";
                        if (record.Data[0].CustomerCode != null && record.Data[0].CustomerCode != "string" && record.Data[0].CustomerCode != "")
                        {
                            int large, small;
                            int CustomerInfoID = 0;
                            large = Convert.ToInt32(record.Data[0].CustomerCode.Split('-')[1]);
                            small = Convert.ToInt32(record.Data[0].CustomerCode.Split('-')[1]);
                            for (int i = 0; i < record.Data.Count; i++)
                            {
                                if (record.Data[i].CustomerCode != null)
                                {
                                    var t = Convert.ToInt32(record.Data[i].CustomerCode.Split('-')[1]);
                                    if (Convert.ToInt32(record.Data[i].CustomerCode.Split('-')[1]) > large)
                                    {
                                        CustomerInfoID = Convert.ToInt32(record.Data[i].Id);
                                        large = Convert.ToInt32(record.Data[i].CustomerCode.Split('-')[1]);

                                    }
                                    else if (Convert.ToInt32(record.Data[i].CustomerCode.Split('-')[1]) < small)
                                    {
                                        small = Convert.ToInt32(record.Data[i].CustomerCode.Split('-')[1]);
                                    }
                                    else
                                    {
                                        if (large < 2)
                                        {
                                            CustomerInfoID = Convert.ToInt32(record.Data[i].Id);
                                        }
                                    }
                                }
                            }
                            newcustomer = record.Data.ToList().Where(x => x.Id == CustomerInfoID).FirstOrDefault();
                            if (newcustomer != null)
                            {
                                if (newcustomer.CustomerCode != null)
                                {
                                    var VcodeSplit = newcustomer.CustomerCode.Split('-');
                                    int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                    fullcode = "00-" + Convert.ToString(code);
                                }
                                else
                                {
                                    fullcode = "00-" + "1";
                                }
                            }
                            else
                            {
                                fullcode = "00-" + "1";
                            }
                        }
                        else
                        {
                            fullcode = "00-" + "1";
                        }

                        ViewBag.CustomerCode = fullcode;
                    }
                    else
                    {
                        ViewBag.CustomerCode = "00-" + "1";
                    }
                    model.CustomerCode = ViewBag.CustomerCode;
                }
                else
                {
                    ViewBag.CustomerCode = "00-" + "1";
                    model.CustomerCode = ViewBag.CustomerCode;
                }

                List<SelectListItem> gender = new List<SelectListItem>
                {
                    new SelectListItem{Text="Mr.",Value="Mr."},
                    new SelectListItem{Text="Mrs.",Value="Mrs."}
                };
                ViewData["Gender"] = gender;
                SResponse CustomerState = RequestSender.Instance.CallAPI("api",
                "Customer/CustomerStateGet", "GET");
                if (CustomerState.Status && (CustomerState.Resp != null) && (CustomerState.Resp != ""))
                {
                    ResponseBack<List<CustomerState>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<CustomerState>>>(CustomerState.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<CustomerState> responseObject = response.Data;
                        ViewBag.CustomerState = new SelectList(responseObject.ToList(), "Id", "StateName");
                    }
                    else
                    {

                        List<CustomerState> responseObject = new List<CustomerState>();
                        ViewBag.CustomerState = new SelectList(responseObject.ToList(), "Id", "StateName");
                    }
                }
                else
                {
                    List<CustomerState> responseObject = new List<CustomerState>();
                    ViewBag.CustomerState = new SelectList(responseObject.ToList(), "Id", "StateName");
                }
                SResponse CustomerType = RequestSender.Instance.CallAPI("api",
                "Customer/CustomerTypeGet", "GET");
                if (CustomerType.Status && (CustomerType.Resp != null) && (CustomerType.Resp != ""))
                {
                    ResponseBack<List<CustomerType>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<CustomerType>>>(CustomerType.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<CustomerType> responseObject = response.Data;
                        ViewBag.CustomerType = new SelectList(responseObject.ToList(), "Id", "TypeName");
                    }
                    else
                    {

                        List<CustomerType> responseObject = new List<CustomerType>();
                        ViewBag.CustomerType = new SelectList(responseObject.ToList(), "Id", "TypeName");
                    }
                }
                else
                {
                    List<CustomerType> responseObject = new List<CustomerType>();
                    ViewBag.CustomerType = new SelectList(responseObject.ToList(), "Id", "TypeName");
                }
                SResponse Provider = RequestSender.Instance.CallAPI("api",
                "Customer/ProviderGet", "GET");
                if (Provider.Status && (Provider.Resp != null) && (Provider.Resp != ""))
                {
                    ResponseBack<List<Provider>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Provider>>>(Provider.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Provider> responseObject = response.Data;
                        ViewBag.Provider = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<Provider> responseObject = new List<Provider>();
                        ViewBag.Provider = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                }
                else
                {
                    List<Provider> responseObject = new List<Provider>();
                    ViewBag.Provider = new SelectList(responseObject.ToList(), "Id", "Name");
                }
                SResponse DrivingLicenseState = RequestSender.Instance.CallAPI("api",
                "Customer/DrivingLicenseStateGet", "GET");
                if (DrivingLicenseState.Status && (DrivingLicenseState.Resp != null) && (DrivingLicenseState.Resp != ""))
                {
                    ResponseBack<List<DrivingLicenseState>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<DrivingLicenseState>>>(DrivingLicenseState.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<DrivingLicenseState> responseObject = response.Data;
                        ViewBag.DrivingLicenseState = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {

                        List<DrivingLicenseState> responseObject = new List<DrivingLicenseState>();
                        ViewBag.DrivingLicenseState = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                }
                else
                {
                    List<DrivingLicenseState> responseObject = new List<DrivingLicenseState>();
                    ViewBag.DrivingLicenseState = new SelectList(responseObject.ToList(), "Id", "Name");
                }

                SResponse group = RequestSender.Instance.CallAPI("api",
               "Customer/GroupGet", "GET");
                if (group.Status && (group.Resp != null) && (group.Resp != ""))
                {
                    ResponseBack<List<Group>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Group>>>(group.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Group> responseObject = response.Data;
                        ViewBag.Group = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<Group> responseObject = new List<Group>();
                        ViewBag.Group = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                }
                else
                {
                    List<Group> responseObject = new List<Group>();
                    ViewBag.Group = new SelectList(responseObject.ToList(), "Id", "Name");
                }
                SResponse SubGroup = RequestSender.Instance.CallAPI("api",
                "Customer/SubGroupGet", "GET");
                if (SubGroup.Status && (SubGroup.Resp != null) && (SubGroup.Resp != ""))
                {
                    ResponseBack<List<SubGroup>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<SubGroup>>>(SubGroup.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<SubGroup> responseObject = response.Data;
                        ViewBag.SubGroup = new SelectList(responseObject.ToList(), "Id", "SubGroupName");
                    }
                    else
                    {
                        List<SubGroup> responseObject = new List<SubGroup>();
                        ViewBag.SubGroup = new SelectList(responseObject.ToList(), "Id", "SubGroupName");
                    }
                }
                else
                {
                    List<SubGroup> responseObject = new List<SubGroup>();
                    ViewBag.SubGroup = new SelectList(responseObject.ToList(), "Id", "SubGroupName");
                }
                SResponse Businesstype = RequestSender.Instance.CallAPI("api",
                "Customer/BusinessTypeGet", "GET");
                if (Businesstype.Status && (Businesstype.Resp != null) && (Businesstype.Resp != ""))
                {
                    ResponseBack<List<BusinessType>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<BusinessType>>>(Businesstype.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<BusinessType> responseObject = response.Data;
                        ViewBag.BusinessType = new SelectList(responseObject.ToList(), "Id", "TypeName");
                    }
                    else
                    {
                        List<BusinessType> responseObject = new List<BusinessType>();
                        ViewBag.BusinessType = new SelectList(responseObject.ToList(), "Id", "TypeName");
                    }
                }
                else
                {
                    List<BusinessType> responseObject = new List<BusinessType>();
                    ViewBag.BusinessType = new SelectList(responseObject.ToList(), "Id", "TypeName");
                }
                SResponse Salesman = RequestSender.Instance.CallAPI("api",
                "Customer/SalesmanGet", "GET");
                if (Salesman.Status && (Salesman.Resp != null) && (Salesman.Resp != ""))
                {
                    ResponseBack<List<Salesman>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Salesman>>>(Salesman.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Salesman> responseObject = response.Data;
                        ViewBag.Salesman = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<Salesman> responseObject = new List<Salesman>();
                        ViewBag.Salesman = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                }
                else
                {
                    List<Salesman> responseObject = new List<Salesman>();
                    ViewBag.Salesman = new SelectList(responseObject.ToList(), "Id", "Name");
                }
                SResponse Zone = RequestSender.Instance.CallAPI("api",
                "Customer/ZoneGet", "GET");
                if (Zone.Status && (Zone.Resp != null) && (Zone.Resp != ""))
                {
                    ResponseBack<List<Zone>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Zone>>>(Zone.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Zone> responseObject = response.Data;
                        ViewBag.Zone = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<Zone> responseObject = new List<Zone>();
                        ViewBag.Zone = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                }
                else
                {
                    List<Zone> responseObject = new List<Zone>();
                    ViewBag.Zone = new SelectList(responseObject.ToList(), "Id", "Name");
                }
                SResponse ShipmentPurchase = RequestSender.Instance.CallAPI("api",
                "Customer/ShipmentPurchaseGet", "GET");
                if (ShipmentPurchase.Status && (ShipmentPurchase.Resp != null) && (ShipmentPurchase.Resp != ""))
                {
                    ResponseBack<List<ShipmentPurchase>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<ShipmentPurchase>>>(ShipmentPurchase.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ShipmentPurchase> responseObject = response.Data;
                        ViewBag.ShipmentPurchase = new SelectList(responseObject.ToList(), "Id", "Type");
                    }
                    else
                    {
                        List<ShipmentPurchase> responseObject = new List<ShipmentPurchase>();
                        ViewBag.ShipmentPurchase = new SelectList(responseObject.ToList(), "Id", "Type");
                    }
                }
                else
                {
                    List<ShipmentPurchase> responseObject = new List<ShipmentPurchase>();
                    ViewBag.ShipmentPurchase = new SelectList(responseObject.ToList(), "Id", "Type");
                }
                SResponse Route = RequestSender.Instance.CallAPI("api",
                "Customer/RouteGet", "GET");
                if (Route.Status && (Route.Resp != null) && (Route.Resp != ""))
                {
                    ResponseBack<List<Route>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Route>>>(Route.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Route> responseObject = response.Data;
                        ViewBag.Route = new SelectList(responseObject.ToList(), "Id", "RouteName");
                    }
                    else
                    {
                        List<Route> responseObject = new List<Route>();
                        ViewBag.Route = new SelectList(responseObject.ToList(), "Id", "RouteName");
                    }
                }
                else
                {
                    List<Route> responseObject = new List<Route>();
                    ViewBag.Route = new SelectList(responseObject.ToList(), "Id", "RouteName");
                }
                SResponse Driver = RequestSender.Instance.CallAPI("api",
                "Customer/DriverGet", "GET");
                if (Driver.Status && (Driver.Resp != null) && (Driver.Resp != ""))
                {
                    ResponseBack<List<Driver>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Driver>>>(Driver.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Driver> responseObject = response.Data;
                        ViewBag.Driver = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<Driver> responseObject = new List<Driver>();
                        ViewBag.Driver = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                }
                else
                {
                    List<Driver> responseObject = new List<Driver>();
                    ViewBag.Driver = new SelectList(responseObject.ToList(), "Id", "Name");
                }
                SResponse ShiptoReference = RequestSender.Instance.CallAPI("api",
                "Customer/ShiptoReferenceGet", "GET");
                if (ShiptoReference.Status && (ShiptoReference.Resp != null) && (ShiptoReference.Resp != ""))
                {
                    ResponseBack<List<ShiptoReference>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<ShiptoReference>>>(ShiptoReference.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ShiptoReference> responseObject = response.Data;
                        ViewBag.ShiptoReference = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<ShiptoReference> responseObject = new List<ShiptoReference>();
                        ViewBag.ShiptoReference = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                }
                else
                {
                    List<ShiptoReference> responseObject = new List<ShiptoReference>();
                    ViewBag.ShiptoReference = new SelectList(responseObject.ToList(), "Id", "Name");
                }
                List<SelectListItem> RouteDeliveryDays = new List<SelectListItem>
                {
                    new SelectListItem{Text="Sunday",Value="Sunday"},
                    new SelectListItem{Text="Monday",Value="Monday"},
                    new SelectListItem{Text="Tuesday",Value="Tuesday"},
                    new SelectListItem{Text="Wednesday",Value="Wednesday"},
                    new SelectListItem{Text="Thursday",Value="Thursday"},
                    new SelectListItem{Text="Friday",Value="Friday"},
                    new SelectListItem{Text="Saturday",Value="Saturday"}
                };
                ViewBag.routedeliverydays = RouteDeliveryDays;
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult NewCustomer(CustomerInformation customer)
        {
            try
            {
                //if (customer.ProviderId != null)
                //{
                //    SResponse ress = RequestSender.Instance.CallAPI("api",
                //    "Customer/ProviderByID" + "/" + customer.ProviderId, "GET");
                //    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                //    {
                //        var response = JsonConvert.DeserializeObject<ResponseBack<Provider>>(ress.Resp);
                //        if (response.Data != null)
                //        {
                //            var responseObject = response.Data;
                //            customer.Provider = responseObject.Name;
                //        }
                //        else
                //        {
                //            customer.Provider = null;
                //        }
                //    }
                //}

                //if (customer.CustomerTypeId != null)
                //{
                //    SResponse ress = RequestSender.Instance.CallAPI("api",
                //    "Customer/CustomerTypeByID" + "/" + customer.CustomerTypeId, "GET");
                //    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                //    {
                //        var response = JsonConvert.DeserializeObject<ResponseBack<CustomerType>>(ress.Resp);
                //        if (response.Data != null)
                //        {
                //            var responseObject = response.Data;
                //            customer.CustomerType = responseObject.TypeName;
                //        }
                //        else
                //        {
                //            customer.CustomerType = null;
                //        }
                //    }
                //}

                //if (customer.DrivingLicenseStateId != null)
                //{
                //    SResponse ress = RequestSender.Instance.CallAPI("api",
                //    "Customer/DrivingLicenseStateByID" + "/" + customer.DrivingLicenseStateId, "GET");
                //    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                //    {
                //        var response = JsonConvert.DeserializeObject<ResponseBack<DrivingLicenseState>>(ress.Resp);
                //        if (response.Data != null)
                //        {
                //            var responseObject = response.Data;
                //            customer.DrivingLicenseState = responseObject.Name;
                //        }
                //        else
                //        {
                //            customer.DrivingLicenseState = null;
                //        }
                //    }
                //}
                //if (customer.StateId != null)
                //{
                //    SResponse ress = RequestSender.Instance.CallAPI("api",
                //    "Customer/CustomerStateByID" + "/" + customer.StateId, "GET");
                //    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                //    {
                //        var response = JsonConvert.DeserializeObject<ResponseBack<CustomerState>>(ress.Resp);
                //        if (response.Data != null)
                //        {
                //            var responseObject = response.Data;
                //            customer.State = responseObject.StateName;
                //        }
                //        else
                //        {
                //            customer.State = null;
                //        }
                //    }
                //}
                //if (customer.CustomerClassification.GroupId != null)
                //{
                //    SResponse ress = RequestSender.Instance.CallAPI("api",
                //    "Customer/GroupByID" + "/" + customer.CustomerClassification.GroupId, "GET");
                //    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                //    {
                //        var response = JsonConvert.DeserializeObject<ResponseBack<Group>>(ress.Resp);
                //        if (response.Data != null)
                //        {
                //            var responseObject = response.Data;
                //            customer.CustomerClassification.GroupName = responseObject.Name;
                //        }
                //        else
                //        {
                //            customer.CustomerClassification.GroupName = null;
                //        }
                //    }
                //}

                //if (customer.CustomerClassification.SubGroupId != null)
                //{
                //    SResponse ress = RequestSender.Instance.CallAPI("api",
                //    "Customer/SubGroupByID" + "/" + customer.CustomerClassification.SubGroupId, "GET");
                //    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                //    {
                //        var response = JsonConvert.DeserializeObject<ResponseBack<SubGroup>>(ress.Resp);
                //        if (response.Data != null)
                //        {
                //            var responseObject = response.Data;
                //            customer.CustomerClassification.SubGroupName = responseObject.SubGroupName;
                //        }
                //        else
                //        {
                //            customer.CustomerClassification.SubGroupName = null;
                //        }
                //    }
                //}

                //if (customer.CustomerClassification.BusinessTypeId != null)
                //{
                //    SResponse ress = RequestSender.Instance.CallAPI("api",
                //    "Customer/BusinessTypeByID" + "/" + customer.CustomerClassification.BusinessTypeId, "GET");
                //    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                //    {
                //        var response = JsonConvert.DeserializeObject<ResponseBack<BusinessType>>(ress.Resp);
                //        if (response.Data != null)
                //        {
                //            var responseObject = response.Data;
                //            customer.CustomerClassification.BusinessType = responseObject.TypeName;
                //        }
                //        else
                //        {
                //            customer.CustomerClassification.BusinessType = null;
                //        }
                //    }
                //}

                //if (customer.CustomerClassification.ZoneId != null)
                //{
                //    SResponse ress = RequestSender.Instance.CallAPI("api",
                //    "Customer/ZoneByID" + "/" + customer.CustomerClassification.ZoneId, "GET");
                //    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                //    {
                //        var response = JsonConvert.DeserializeObject<ResponseBack<Zone>>(ress.Resp);
                //        if (response.Data != null)
                //        {
                //            var responseObject = response.Data;
                //            customer.CustomerClassification.Zone = responseObject.Name;
                //        }
                //        else
                //        {
                //            customer.CustomerClassification.Zone = null;
                //        }
                //    }
                //}

                //if (customer.CustomerClassification.SalesmanId != null)
                //{
                //    SResponse ress = RequestSender.Instance.CallAPI("api",
                //    "Customer/SalesmanByID" + "/" + customer.CustomerClassification.SalesmanId, "GET");
                //    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                //    {
                //        var response = JsonConvert.DeserializeObject<ResponseBack<Salesman>>(ress.Resp);
                //        if (response.Data != null)
                //        {
                //            var responseObject = response.Data;
                //            customer.CustomerClassification.Salesman = responseObject.Name;
                //        }
                //        else
                //        {
                //            customer.CustomerClassification.Salesman = null;
                //        }
                //    }
                //}

                //if (customer.CustomerClassification.ShippedViaId != null)
                //{
                //    SResponse ress = RequestSender.Instance.CallAPI("api",
                //    "Customer/ShipmentPurchaseByID" + "/" + customer.CustomerClassification.ShippedViaId, "GET");
                //    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                //    {
                //        var response = JsonConvert.DeserializeObject<ResponseBack<ShipmentPurchase>>(ress.Resp);
                //        if (response.Data != null)
                //        {
                //            var responseObject = response.Data;
                //            customer.CustomerClassification.ShippedVia = responseObject.Type;
                //        }
                //        else
                //        {
                //            customer.CustomerClassification.ShippedVia = null;
                //        }
                //    }
                //}

                //if (customer.CustomerClassification.RouteId != null)
                //{
                //    SResponse ress = RequestSender.Instance.CallAPI("api",
                //    "Customer/RouteByID" + "/" + customer.CustomerClassification.RouteId, "GET");
                //    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                //    {
                //        var response = JsonConvert.DeserializeObject<ResponseBack<Route>>(ress.Resp);
                //        if (response.Data != null)
                //        {
                //            var responseObject = response.Data;
                //            customer.CustomerClassification.RouteName = responseObject.RouteName;
                //        }
                //        else
                //        {
                //            customer.CustomerClassification.RouteName = null;
                //        }
                //    }
                //}

                //if (customer.CustomerClassification.DriverId != null)
                //{
                //    SResponse ress = RequestSender.Instance.CallAPI("api",
                //    "Customer/DriverByID" + "/" + customer.CustomerClassification.DriverId, "GET");
                //    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                //    {
                //        var response = JsonConvert.DeserializeObject<ResponseBack<Driver>>(ress.Resp);
                //        if (response.Data != null)
                //        {
                //            var responseObject = response.Data;
                //            customer.CustomerClassification.DriverName = responseObject.Name;
                //        }
                //        else
                //        {
                //            customer.CustomerClassification.DriverName = null;
                //        }
                //    }
                //}

                //if (customer.CustomerClassification.ShiptoReferenceId != null)
                //{
                //    SResponse ress = RequestSender.Instance.CallAPI("api",
                //    "Customer/ShiptoReferenceByID" + "/" + customer.CustomerClassification.ShiptoReferenceId, "GET");
                //    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                //    {
                //        var response = JsonConvert.DeserializeObject<ResponseBack<ShiptoReference>>(ress.Resp);
                //        if (response.Data != null)
                //        {
                //            var responseObject = response.Data;
                //            customer.CustomerClassification.ShiptoReference = responseObject.Name;
                //        }
                //        else
                //        {
                //            customer.CustomerClassification.ShiptoReference = null;
                //        }
                //    }
                //}

                var customerinfobody = JsonConvert.SerializeObject(customer);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/CustomerInformationCreate", "POST", customerinfobody);

                return RedirectToAction("NewCustomer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult AddProvider(Provider obj)
        {
            if (obj != null)
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/ProviderCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json("true");
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult AddCustomerState(CustomerState obj)
        {
            if (obj != null)
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/CustomerStateCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json("true");
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult AddCustomerType(CustomerType obj)
        {
            if (obj != null)
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/CustomerTypeCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json("true");
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult AddDrivingLicenseState(DrivingLicenseState obj)
        {
            if (obj != null)
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/DrivingLicenseStateCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json("true");
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult AddGroup(Group obj)
        {
            if (obj != null)
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/GroupCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json("true");
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult AddSubGroup(SubGroup obj)
        {
            if (obj != null)
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/SubGroupCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json("true");
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult AddZone(Zone obj)
        {
            if (obj != null)
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/ZoneCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json("true");
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult AddBusinessType(BusinessType obj)
        {
            if (obj != null)
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/BusinessTypeCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json("true");
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult AddSalesman(Salesman obj)
        {
            if (obj != null)
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/SalesmanCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json("true");
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult AddShipmentPurchase(ShipmentPurchase obj)
        {
            if (obj != null)
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/ShipmentPurchaseCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json("true");
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult AddRoute(Route obj)
        {
            if (obj != null)
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/RouteCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json("true");
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult AddDriver(Driver obj)
        {
            if (obj != null)
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/DriverCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    return Json("true");
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult ShowGroup()
        {
            SResponse group = RequestSender.Instance.CallAPI("api",
               "Customer/GroupGet", "GET");
            if (group.Status && (group.Resp != null) && (group.Resp != ""))
            {
                ResponseBack<List<Group>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<Group>>>(group.Resp);
                if (response.Data.Count() > 0)
                {
                    List<Group> responseObject = response.Data;
                    var Group = new SelectList(responseObject.ToList(), "Id", "Name");
                    return Json(Group);
                }
                else
                {
                    List<Group> responseObject = new List<Group>();
                    var Group = new SelectList(responseObject.ToList(), "Id", "Name");
                    return Json(Group);
                }
            }
            else
            {
                List<Group> responseObject = new List<Group>();
                var Group = new SelectList(responseObject.ToList(), "Id", "Name");
                return Json(Group);
            }

        }
        public JsonResult SearchDataByCompanyName(string name)
        {
            if (name != null)
            {
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/CustomerInformationByCompany" + "/" + name, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ResponseBack<CustomerInformation> response =
                                 JsonConvert.DeserializeObject<ResponseBack<CustomerInformation>>(resp.Resp);
                    CustomerInformation cusinfo = response.Data;
                    return Json(cusinfo);
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult EditCustomerInformation(CustomerInformation customer)
        {
            if (customer != null)
            {
                var id = customer.Id;
                if (customer.ProviderId != null)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/ProviderByID" + "/" + customer.ProviderId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<Provider>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data;
                            customer.Provider = responseObject.Name;
                        }
                        else
                        {
                            customer.Provider = null;
                        }
                    }
                }

                if (customer.CustomerTypeId != null)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/CustomerTypeByID" + "/" + customer.CustomerTypeId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<CustomerType>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data;
                            customer.CustomerType = responseObject.TypeName;
                        }
                        else
                        {
                            customer.CustomerType = null;
                        }
                    }
                }

                if (customer.DrivingLicenseStateId != null)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/DrivingLicenseStateByID" + "/" + customer.DrivingLicenseStateId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<DrivingLicenseState>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data;
                            customer.DrivingLicenseState = responseObject.Name;
                        }
                        else
                        {
                            customer.DrivingLicenseState = null;
                        }
                    }
                }
                if (customer.StateId != null)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/CustomerStateByID" + "/" + customer.StateId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<CustomerState>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data;
                            customer.State = responseObject.StateName;
                        }
                        else
                        {
                            customer.State = null;
                        }
                    }
                }
                if (customer.CustomerClassification.GroupId != null)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/GroupByID" + "/" + customer.CustomerClassification.GroupId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<Group>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data;
                            customer.CustomerClassification.GroupName = responseObject.Name;
                        }
                        else
                        {
                            customer.CustomerClassification.GroupName = null;
                        }
                    }
                }

                if (customer.CustomerClassification.SubGroupId != null)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/SubGroupByID" + "/" + customer.CustomerClassification.SubGroupId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<SubGroup>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data;
                            customer.CustomerClassification.SubGroupName = responseObject.SubGroupName;
                        }
                        else
                        {
                            customer.CustomerClassification.SubGroupName = null;
                        }
                    }
                }

                if (customer.CustomerClassification.BusinessTypeId != null)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/BusinessTypeByID" + "/" + customer.CustomerClassification.BusinessTypeId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<BusinessType>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data;
                            customer.CustomerClassification.BusinessType = responseObject.TypeName;
                        }
                        else
                        {
                            customer.CustomerClassification.BusinessType = null;
                        }
                    }
                }

                if (customer.CustomerClassification.ZoneId != null)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/ZoneByID" + "/" + customer.CustomerClassification.ZoneId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<Zone>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data;
                            customer.CustomerClassification.Zone = responseObject.Name;
                        }
                        else
                        {
                            customer.CustomerClassification.Zone = null;
                        }
                    }
                }

                if (customer.CustomerClassification.SalesmanId != null)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/SalesmanByID" + "/" + customer.CustomerClassification.SalesmanId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<Salesman>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data;
                            customer.CustomerClassification.Salesman = responseObject.Name;
                        }
                        else
                        {
                            customer.CustomerClassification.Salesman = null;
                        }
                    }
                }

                if (customer.CustomerClassification.ShippedViaId != null)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/ShipmentPurchaseByID" + "/" + customer.CustomerClassification.ShippedViaId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<ShipmentPurchase>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data;
                            customer.CustomerClassification.ShippedVia = responseObject.Type;
                        }
                        else
                        {
                            customer.CustomerClassification.ShippedVia = null;
                        }
                    }
                }

                if (customer.CustomerClassification.RouteId != null)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/RouteByID" + "/" + customer.CustomerClassification.RouteId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<Route>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data;
                            customer.CustomerClassification.RouteName = responseObject.RouteName;
                        }
                        else
                        {
                            customer.CustomerClassification.RouteName = null;
                        }
                    }
                }

                if (customer.CustomerClassification.DriverId != null)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/DriverByID" + "/" + customer.CustomerClassification.DriverId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<Driver>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data;
                            customer.CustomerClassification.DriverName = responseObject.Name;
                        }
                        else
                        {
                            customer.CustomerClassification.DriverName = null;
                        }
                    }
                }

                if (customer.CustomerClassification.ShiptoReferenceId != null)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/ShiptoReferenceByID" + "/" + customer.CustomerClassification.ShiptoReferenceId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<ShiptoReference>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data;
                            customer.CustomerClassification.ShiptoReference = responseObject.Name;
                        }
                        else
                        {
                            customer.CustomerClassification.ShiptoReference = null;
                        }
                    }
                }

                if (customer != null)
                {
                    var body = JsonConvert.SerializeObject(customer);
                    SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/CustomerInformationUpdate" + "/" + id, "PUT", body);
                    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                    {
                        return Json("true");
                    }
                    else
                    {
                        return Json("false");
                    }
                }
            }
            return Json("false");
        }
        public IActionResult DeleteCustomerInformation(int id)
        {
            try
            {

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Customer/DeleteCustomerInformation" + "/" + id, "Delete");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    TempData["response"] = "Delete Successfully";
                    return Json(ress.Status);

                    //return RedirectToAction("NewCustomer");
                }
                else
                {
                    TempData["response"] = "Failed To Delete";
                    return Json(ress.Status);

                }
                return RedirectToAction("NewCustomer");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public JsonResult AutoCompleteSearchCustomer()
        {
            string Msg = "";

            SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/CustomersGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {

                ResponseBack<List<CustomerInformation>> response =
                              JsonConvert.DeserializeObject<ResponseBack<List<CustomerInformation>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<CustomerInformation> responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    return Json(Msg);
                }
            }

            return Json(Msg);
        }

        public JsonResult GetCustomerByID(int id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Customer/CustomerInformationByID" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<CustomerInformation>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    TempData["response"] = "No Record Found.";
                }
            }
            return Json(Msg);
        }
    }
}
