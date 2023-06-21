using ABC.Admin.Domain.DataConfig;
using ABC.EFCore.Entities.Admin;
using ABC.EFCore.Entities.HR;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ABC.Admin.Domain.DataConfig.RequestSender;
using CustomerPaperWork = ABC.EFCore.Repository.Edmx.CustomerPaperWork;

namespace ABC.Admin.Website.Controllers
{
    public class CustomersController : Controller
    {

        private readonly ISession session;
        public CustomersController(IHttpContextAccessor httpContextAccessor)
        {
            this.session = httpContextAccessor.HttpContext.Session;

        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult CustomerRegistration()
        {
            try
            {
                List<SelectListItem> CustomerType = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="WholeSaler",Value="WholeSaler"},
                    new SelectListItem(){Text="Retailer",Value="Retailer"}
                };
                ViewData["CustomerType"] = CustomerType;
                List<SelectListItem> Customertate = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="North Carolina",Value="North Carolina"}

                };
                ViewData["Customertate"] = Customertate;
                List<SelectListItem> RegistrationType = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="Customer",Value="Customer"}

                };
                ViewData["RegistrationType"] = RegistrationType;
                List<SelectListItem> LicenseState = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="Alabama",Value="Alabama"},
                    new SelectListItem(){Text="Arizona",Value="Arizona"},
                    new SelectListItem(){Text="Alaska",Value="Alaska"},
                    new SelectListItem(){Text="California",Value="California"},
                    new SelectListItem(){Text="Colorado",Value="Colorado"},
                    new SelectListItem(){Text="Connecticut",Value="Connecticut"},
                    new SelectListItem(){Text="Delaware",Value="Delaware"},
                    new SelectListItem(){Text="Florida",Value="Florida"},
                    new SelectListItem(){Text="Georgia",Value="Georgia"},
                    new SelectListItem(){Text="Hawaii",Value="Hawaii"},
                    new SelectListItem(){Text="Idaho",Value="Idaho"},
                    new SelectListItem(){Text="Illinois",Value="Illinois"},
                    new SelectListItem(){Text="Indiana",Value="Indiana"},
                    new SelectListItem(){Text="Iowa",Value="Iowa"},
                    new SelectListItem(){Text="Kansas",Value="Kansas"},
                    new SelectListItem(){Text="Kentucky",Value="Kentucky"},
                    new SelectListItem(){Text="Maine",Value="Maine"},
                    new SelectListItem(){Text="Maryland",Value="Maryland"},
                    new SelectListItem(){Text="Massachusetts",Value="Massachusetts"},
                    new SelectListItem(){Text="Michigan",Value="Michigan"},
                    new SelectListItem(){Text="Mississippi",Value="Mississippi"},
                    new SelectListItem(){Text="Missouri",Value="Missouri"},
                    new SelectListItem(){Text="Montana",Value="Montana"},
                    new SelectListItem(){Text="Nevada",Value="Nevada"},
                    new SelectListItem(){Text="New Hampshire",Value="New Hampshire"},
                    new SelectListItem(){Text="New York",Value="New York"},
                    new SelectListItem(){Text="North Carolina",Value="North Carolina"},
                    new SelectListItem(){Text="North Dakota",Value="North Dakota"},
                    new SelectListItem(){Text="Ohio",Value="Ohio"},
                    new SelectListItem(){Text="Oklahoma",Value="Oklahoma"},
                    new SelectListItem(){Text="Oregon",Value="Oregon"},
                    new SelectListItem(){Text="Pennsylvania",Value="Pennsylvania"},
                    new SelectListItem(){Text="Rhode Island",Value="Rhode Island"},
                    new SelectListItem(){Text="South Dakota",Value="South Dakota"},
                    new SelectListItem(){Text="Tennessee",Value="Tennessee"},
                    new SelectListItem(){Text="Texas",Value="Texas"},
                    new SelectListItem(){Text="Utah",Value="Utah"},
                    new SelectListItem(){Text="Vermont",Value="Vermont"},
                    new SelectListItem(){Text="Virginia",Value="Virginia"},
                    new SelectListItem(){Text="Washington",Value="Washington"},
                    new SelectListItem(){Text="Vest Virginia",Value="Vest Virginia"},
                    new SelectListItem(){Text="Wisconsin",Value="Wisconsin"},
                    new SelectListItem(){Text="Wyoming",Value="Wyoming"}
                };
                ViewData["LicenseState"] = LicenseState;
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }
        [HttpPost]
        public IActionResult CustomerRegistration(RegisterCustomer Customer, IFormFile FederalForm, IFormFile SalesTaxID, IFormFile DrivingLicenseID, IFormFile ProfileImage)
        {
            try
            {

                if (Customer.EmpBusinesss != null)
                {
                    List<CertificateBusinessType> businesslist = null;
                    businesslist = new List<CertificateBusinessType>();


                    if (Customer.EmpBusinesss.IsSelectedAccommodationAndFoodServices == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Accommodation And Food Services";
                        businesslist.Add(business);
                    }

                    if (Customer.EmpBusinesss.IsSelectedAgriculturalforestryFishingAndHunting == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Agricultural forestry Fishing And Hunting";
                        businesslist.Add(business);
                    }

                    if (Customer.EmpBusinesss.IsSelectedConstruction == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Construction";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedFinanceAndInsurance == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Finance And Insurance";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedInformationPublishingAndCommunications == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Information Publishing And Communications";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedManufacturing == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Manufacturing";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedMining == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Mining";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedRealEstate == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "RealEstate";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedRentalAndLeasing == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Rental And Leasing";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedRetailTrade == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Retail Trade";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedTransportationAndWarehousing == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Transportation And Warehousing";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedUtilities == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Utilities";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedWholesaleTrade == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Whole sale Trade";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedBusinessServices == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Business Services";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedProfessionalServices == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Professional Services";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedEducationAndHealth == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Education And Health";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedNonprofitOrganization == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Non Profit Organization";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedGovernment == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Government";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedNotABusiness == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Not A Business";
                        businesslist.Add(business);
                    }
                    if (Customer.EmpBusinesss.IsSelectedothers == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        //business.Type = Customer.EmpBusinesss.others ;
                        business.Type = "Others";
                        businesslist.Add(business);
                    }

                    Customer.Customer.CertificateBusinessTypes = businesslist;
                }
                if (Customer.EmpIdentification != null)
                {
                    List<CertificateIdentification> Identificationlist = null;
                    Identificationlist = new List<CertificateIdentification>();
                    if (Customer.EmpIdentification.IdentificationNumberAR != null && Customer.EmpIdentification.IdentificationNumberAR != "" && Customer.EmpIdentification.ReasonExamptionAR != null && Customer.EmpIdentification.ReasonExamptionAR != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "AR";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionAR;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberAR;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberIA != null && Customer.EmpIdentification.IdentificationNumberIA != "" && Customer.EmpIdentification.ReasonExamptionIA != null && Customer.EmpIdentification.ReasonExamptionIA != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "IA";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionIA;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberIA;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberIN != null && Customer.EmpIdentification.IdentificationNumberIN != "" && Customer.EmpIdentification.ReasonExamptionIN != null && Customer.EmpIdentification.ReasonExamptionIN != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "IN";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionIN;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberIN;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberKS != null && Customer.EmpIdentification.IdentificationNumberKS != "" && Customer.EmpIdentification.ReasonExamptionKS != null && Customer.EmpIdentification.ReasonExamptionKS != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "KS";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionKS;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberKS;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberKY != null && Customer.EmpIdentification.IdentificationNumberKY != "" && Customer.EmpIdentification.ReasonExamptionKY != null && Customer.EmpIdentification.ReasonExamptionKY != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "KY";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionKY;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberKY;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberMI != null && Customer.EmpIdentification.IdentificationNumberMI != "" && Customer.EmpIdentification.ReasonExamptionMI != null && Customer.EmpIdentification.ReasonExamptionMI != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "MI";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionKY;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberKY;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberMN != null && Customer.EmpIdentification.IdentificationNumberMN != "" && Customer.EmpIdentification.ReasonExamptionMN != null && Customer.EmpIdentification.ReasonExamptionMN != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "MN";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionMN;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberMN;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberNC != null && Customer.EmpIdentification.IdentificationNumberNC != "" && Customer.EmpIdentification.ReasonExamptionNC != null && Customer.EmpIdentification.ReasonExamptionNC != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "NC";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionNC;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberNC;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberND != null && Customer.EmpIdentification.IdentificationNumberND != "" && Customer.EmpIdentification.ReasonExamptionND != null && Customer.EmpIdentification.ReasonExamptionND != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "ND";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionND;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberND;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberNE != null && Customer.EmpIdentification.IdentificationNumberNE != "" && Customer.EmpIdentification.ReasonExamptionNE != null && Customer.EmpIdentification.ReasonExamptionNE != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "NE";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionNE;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberNE;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberNJ != null && Customer.EmpIdentification.IdentificationNumberNJ != "" && Customer.EmpIdentification.ReasonExamptionNJ != null && Customer.EmpIdentification.ReasonExamptionNJ != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "NJ";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionNJ;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberNJ;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberNV != null && Customer.EmpIdentification.IdentificationNumberNV != "" && Customer.EmpIdentification.ReasonExamptionNV != null && Customer.EmpIdentification.ReasonExamptionNV != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "NV";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionNV;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberNV;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberOH != null && Customer.EmpIdentification.IdentificationNumberOH != "" && Customer.EmpIdentification.ReasonExamptionOH != null && Customer.EmpIdentification.ReasonExamptionOH != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "OH";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionOH;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberOH;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberRI != null && Customer.EmpIdentification.IdentificationNumberRI != "" && Customer.EmpIdentification.ReasonExamptionRI != null && Customer.EmpIdentification.ReasonExamptionRI != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "RI";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionRI;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberRI;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberOK != null && Customer.EmpIdentification.IdentificationNumberOK != "" && Customer.EmpIdentification.ReasonExamptionOK != null && Customer.EmpIdentification.ReasonExamptionOK != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "OK";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionOK;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberOK;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberSD != null && Customer.EmpIdentification.IdentificationNumberSD != "" && Customer.EmpIdentification.ReasonExamptionSD != null && Customer.EmpIdentification.ReasonExamptionSD != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "SD";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionSD;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberSD;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberTN != null && Customer.EmpIdentification.IdentificationNumberTN != "" && Customer.EmpIdentification.ReasonExamptionTN != null && Customer.EmpIdentification.ReasonExamptionTN != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "TN";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionTN;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberTN;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberUT != null && Customer.EmpIdentification.IdentificationNumberUT != "" && Customer.EmpIdentification.ReasonExamptionUT != null && Customer.EmpIdentification.ReasonExamptionUT != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "UT";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionUT;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberUT;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberVT != null && Customer.EmpIdentification.IdentificationNumberVT != "" && Customer.EmpIdentification.ReasonExamptionVT != null && Customer.EmpIdentification.ReasonExamptionVT != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "VT";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionVT;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberVT;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberWV != null && Customer.EmpIdentification.IdentificationNumberWV != "" && Customer.EmpIdentification.ReasonExamptionWV != null && Customer.EmpIdentification.ReasonExamptionWV != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "WV";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionWV;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberWV;
                        Identificationlist.Add(Identification);
                    }
                    if (Customer.EmpIdentification.IdentificationNumberWY != null && Customer.EmpIdentification.IdentificationNumberWY != "" && Customer.EmpIdentification.ReasonExamptionWY != null && Customer.EmpIdentification.ReasonExamptionWY != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "WY";
                        Identification.ReasonExamption = Customer.EmpIdentification.ReasonExamptionWY;
                        Identification.IdentificationNumber = Customer.EmpIdentification.IdentificationNumberWY;
                        Identificationlist.Add(Identification);
                    }


                    Customer.Customer.CertificateIdentifications = Identificationlist;
                }
                if (Customer.EmpReasonExemption != null)
                {
                    List<CertificateReasonExemption> ExemptionList = null;
                    ExemptionList = new List<CertificateReasonExemption>();

                    if (Customer.EmpReasonExemption.IsSelectedFederalGovernment == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = Customer.EmpReasonExemption.FederalGovernmentText;
                        Exemption.Reason = Customer.EmpReasonExemption.FederalGovernment;
                        ExemptionList.Add(Exemption);
                    }
                    if (Customer.EmpReasonExemption.IsSelectedStateOrLocalGovernment == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = Customer.EmpReasonExemption.StateOrLocalGovernmentText;
                        Exemption.Reason = Customer.EmpReasonExemption.StateOrLocalGovernment;
                        ExemptionList.Add(Exemption);
                    }
                    if (Customer.EmpReasonExemption.IsSelectedTribalGovernment == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = Customer.EmpReasonExemption.TribalGovernmentText;
                        Exemption.Reason = Customer.EmpReasonExemption.TribalGovernment;
                        ExemptionList.Add(Exemption);
                    }
                    if (Customer.EmpReasonExemption.IsSelectedForeignDiplomat == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = Customer.EmpReasonExemption.ForeignDiplomatText;
                        Exemption.Reason = Customer.EmpReasonExemption.ForeignDiplomat;
                        ExemptionList.Add(Exemption);
                    }
                    if (Customer.EmpReasonExemption.IsSelectedAgriculturalProduction == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = Customer.EmpReasonExemption.AgriculturalProductionText;
                        Exemption.Reason = Customer.EmpReasonExemption.AgriculturalProduction;
                        ExemptionList.Add(Exemption);
                    }
                    if (Customer.EmpReasonExemption.IsSelectedIndustrialProductionManufacturing == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = Customer.EmpReasonExemption.IndustrialProductionManufacturingText;
                        Exemption.Reason = Customer.EmpReasonExemption.IndustrialProductionManufacturing;
                        ExemptionList.Add(Exemption);
                    }
                    if (Customer.EmpReasonExemption.IsSelectedDirectPayPermit == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = Customer.EmpReasonExemption.DirectPayPermitText;
                        Exemption.Reason = Customer.EmpReasonExemption.DirectPayPermit;
                        ExemptionList.Add(Exemption);
                    }
                    if (Customer.EmpReasonExemption.IsSelectedDirectMail == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = Customer.EmpReasonExemption.DirectMailText;
                        Exemption.Reason = Customer.EmpReasonExemption.DirectMail;
                        ExemptionList.Add(Exemption);
                    }
                    if (Customer.EmpReasonExemption.IsSelectedResale == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = Customer.EmpReasonExemption.ResaleText;
                        Exemption.Reason = Customer.EmpReasonExemption.Resale;
                        ExemptionList.Add(Exemption);
                    }
                    if (Customer.EmpReasonExemption.IsSelectedothers == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = Customer.EmpReasonExemption.othersText;
                        Exemption.Reason = Customer.EmpReasonExemption.others;
                        ExemptionList.Add(Exemption);
                    }

                    Customer.Customer.CertificateReasonExemptions = ExemptionList;
                }
                Customer.Customer.paperWork = new CustomerPaperWork();
                if (FederalForm != null)
                {
                    var input = FederalForm.OpenReadStream();
                    byte[] byteData = null, buffer = new byte[input.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        byteData = ms.ToArray();
                    }

                    Customer.Customer.paperWork.FederalForm = byteData;

                }

                if (SalesTaxID != null)
                {
                    var input = SalesTaxID.OpenReadStream();
                    byte[] byteData = null, buffer = new byte[input.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        byteData = ms.ToArray();
                    }
                    Customer.Customer.paperWork.SalesTaxId = byteData;
                }

                if (DrivingLicenseID != null)
                {
                    var input = DrivingLicenseID.OpenReadStream();
                    byte[] byteData = null, buffer = new byte[input.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        byteData = ms.ToArray();
                    }
                    Customer.Customer.paperWork.DrivingLicenseId = byteData;
                }
                if (ProfileImage != null)
                {
                    var input = ProfileImage.OpenReadStream();
                    byte[] byteData = null, buffer = new byte[input.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        byteData = ms.ToArray();
                    }
                    
                }

               // Customer.Customer.FromScreen = "POS";
                CustomerInformation registernewCustomer = null;
                registernewCustomer = new CustomerInformation();

                
                registernewCustomer = Customer.Customer;

                


                var body = JsonConvert.SerializeObject(registernewCustomer);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/RegisterCustomerECommerce", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["Msg"] = "Thank you for registration, your request has send to administration for approval";
                    return RedirectToAction("ManageCustomer");
                }
                else
                {
                    TempData["Msg"] = "Request not completed right now";
                    return RedirectToAction("CustomerRegistration");
                }


            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }

        }
        public IActionResult ManageCustomer()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Customers/CustomersGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<CustomerInformation>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<CustomerInformation>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<CustomerInformation> responseObject = response.Data.ToList().Where(x=>x.Pending == true && x.Approved == false && x.Rejected == false).ToList();
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable to get Customers.";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to get Customer." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }      
        
        public IActionResult ApprovedCustomer()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Customers/CustomersGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<CustomerInformation>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<CustomerInformation>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<CustomerInformation> responseObject = response.Data.ToList().Where(x=>x.Pending == false && x.Approved == true && x.Rejected == false).ToList();
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable to get Customer.";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to get Customer." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult RejectedCustomer()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Customers/CustomersGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<CustomerInformation>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<CustomerInformation>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<CustomerInformation> responseObject = response.Data.ToList().Where(x => x.Pending == false && x.Approved == false && x.Rejected == true).ToList();
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable to get Customers.";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to get Customer." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult CustomerDetail(int id)
        {
            try
            {

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Customers/CustomerInformationByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<CustomerInformation> response = JsonConvert.DeserializeObject<ResponseBack<CustomerInformation>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable to get employee record.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }
        public IActionResult ApproveThisCustomer(int? id,string CreditLimit)
        {
            try
            {
                string AccessToken = string.Empty;
                string AccessName = string.Empty;
                string userData = session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDto.RefreshToken;
                    AccessName = userDto.UserName;
                }

                SResponse ress = RequestSender.Instance.CallAPI("api", "Customers/ApproveCustomerByID/?id=" + id + "&CreditLimit=" + CreditLimit, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<CustomerInformation> response = JsonConvert.DeserializeObject<ResponseBack<CustomerInformation>>(ress.Resp);

                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = AccessName.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Customers";
                    activity.NewDetails = "Approved Customer Profile on " + DateTime.Now.Date + " " + "by " + AccessName.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);

                    return Json("true");
                    
                }
                else
                {
                    TempData["response"] = "Unable to approve the customer.";
                }
                return Json("false");
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return Json("false");
            }
        }

        public IActionResult RejectThisCustomer(int? id)
        {
            try
            {
                string AccessToken = string.Empty;
                string AccessName = string.Empty;
                string userData = session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDto.RefreshToken;
                    AccessName = userDto.UserName;
                }
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Customers/RejectCustomerByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<CustomerInformation> response = JsonConvert.DeserializeObject<ResponseBack<CustomerInformation>>(ress.Resp);

                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = AccessName.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Customers";
                    activity.NewDetails = "Rejected Customer Profile on " + DateTime.Now.Date + " " + "by " + AccessName.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);

                    return Json("true");
                }
                else
                {
                    TempData["response"] = "Unable to reject the customer.";
                }
                return Json("false");
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return Json("false");
            }
        }
    }
}




  