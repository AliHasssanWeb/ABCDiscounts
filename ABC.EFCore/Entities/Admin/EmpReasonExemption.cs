using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.EFCore.Entities.Admin
{
    public class EmpReasonExemption
    {
        public string FederalGovernment { get; set; }
        public string FederalGovernmentText { get; set; }
        public bool IsSelectedFederalGovernment { get; set; }
        public string StateOrLocalGovernment { get; set; }
        public string StateOrLocalGovernmentText { get; set; }
        public bool IsSelectedStateOrLocalGovernment { get; set; }
        public string TribalGovernment { get; set; }
        public string TribalGovernmentText { get; set; }
        public bool IsSelectedTribalGovernment { get; set; }
        public string ForeignDiplomat { get; set; }
        public string ForeignDiplomatText { get; set; }
        public bool IsSelectedForeignDiplomat { get; set; }
        public string AgriculturalProduction { get; set; }
        public string AgriculturalProductionText { get; set; }
        public bool IsSelectedAgriculturalProduction { get; set; }
        public string IndustrialProductionManufacturing { get; set; }
        public string IndustrialProductionManufacturingText { get; set; }
        public bool IsSelectedIndustrialProductionManufacturing { get; set; }
        public string DirectPayPermit { get; set; }
        public string DirectPayPermitText { get; set; }
        public bool IsSelectedDirectPayPermit { get; set; }
        public string DirectMail { get; set; }
        public string DirectMailText { get; set; }
        public bool IsSelectedDirectMail { get; set; }
        public string Resale { get; set; }
        public string ResaleText { get; set; }
        public bool IsSelectedResale { get; set; }
        public string others { get; set; }
        public string othersText { get; set; }
        public bool IsSelectedothers { get; set; }
    }
}
