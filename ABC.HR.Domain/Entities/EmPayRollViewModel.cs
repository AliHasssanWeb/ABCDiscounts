
//using ABC.HR.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class EmPayRollViewModel
    {
       public List<EmPayRollListModel> AllPayRoll { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        

    }
}
