using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.EFCore.Entities.Admin
{
    public partial class EmpBusiness
    {
        public string AccommodationAndFoodServices { get; set; }
        public bool IsSelectedAccommodationAndFoodServices { get; set; }
        public string AgriculturalforestryFishingAndHunting { get; set; }
        public bool IsSelectedAgriculturalforestryFishingAndHunting { get; set; }
        public string Construction { get; set; }
        public bool IsSelectedConstruction { get; set; }
        public string FinanceAndInsurance { get; set; }
        public bool IsSelectedFinanceAndInsurance { get; set; }
        public string InformationPublishingAndCommunications { get; set; }
        public bool IsSelectedInformationPublishingAndCommunications { get; set; }
        public string Manufacturing { get; set; }
        public bool IsSelectedManufacturing { get; set; }
        public string Mining { get; set; }
        public bool IsSelectedMining { get; set; }
        public string RealEstate { get; set; }
        public bool IsSelectedRealEstate { get; set; }
        public string RentalAndLeasing { get; set; }
        public bool IsSelectedRentalAndLeasing { get; set; }
        public string RetailTrade { get; set; }
        public bool IsSelectedRetailTrade { get; set; }
        public string TransportationAndWarehousing { get; set; }
        public bool IsSelectedTransportationAndWarehousing { get; set; }
        public string Utilities { get; set; }
        public bool IsSelectedUtilities { get; set; }
        public string WholesaleTrade { get; set; }
        public bool IsSelectedWholesaleTrade { get; set; }
        public string BusinessServices { get; set; }
        public bool IsSelectedBusinessServices { get; set; }
        public string ProfessionalServices { get; set; }
        public bool IsSelectedProfessionalServices { get; set; }
        public string EducationAndHealth { get; set; }
        public bool IsSelectedEducationAndHealth { get; set; }
        public string NonprofitOrganization { get; set; }
        public bool IsSelectedNonprofitOrganization { get; set; }
        public string Government { get; set; }
        public bool IsSelectedGovernment { get; set; }
        public string NotABusiness { get; set; }
        public bool IsSelectedNotABusiness { get; set; }
        public bool IsSelectedothers { get; set; }

        public string others { get; set; }
    }
}
