using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class ValidateAllowance
    {
        [Required(ErrorMessage = "Please Select Employee")]
        [Display(Name = "Select Employee")]

        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Please Select Allowance")]
        [Display(Name = "Select Allownce Type")]
        public int AllowanceTypeId { get; set; }

        [Required]
        [Display(Name = "Amount of Allownce")]
        public string Amount { get; set; }

        [Required]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        //[Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        [Required]
        [Display(Name = "Reason Of Allownce")]
        //check space at start and at end also
        [RegularExpression(@"^(?:[a-zA-Z0-9]+\s?)+[a-zA-Z0-9]+$", ErrorMessage = "White Space Not Allowed")]
        public string Reason { get; set; }
        public bool IsApprove { get; set; }
    }
        //[ModelMetadataTypeAttribute(typeof(ValidateAllowance))]
        //public partial class EmpAllowance
        //{

        //}
    }

