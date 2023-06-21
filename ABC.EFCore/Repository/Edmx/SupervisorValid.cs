using ABC.EFCore.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class SupervisorValid
    {
        [NotMapped]
        public AspNetUser AspNetUser { get; set; }
        [NotMapped]
        public string FullName { get; set; }
        [NotMapped]
        public string AvailedCredit { get; set; }

        [NotMapped]
        public bool AvailedSupervisor { get; set; }

        [NotMapped]
        public bool AvailedManager { get; set; }

        [NotMapped]
        public string CustomerName { get; set; }
    }
    [MetadataType(typeof(SupervisorValid))]
    public partial class Supervisor
    {
        [NotMapped]
        public AspNetUser AspNetUser { get; set; }
        [NotMapped]
        public string FullName { get; set; }
        [NotMapped]
        public string AvailedCredit { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }

        [NotMapped]
        public bool AvailedSupervisor { get; set; }

        [NotMapped]
        public bool AvailedManager { get; set; }

        
    }
}
