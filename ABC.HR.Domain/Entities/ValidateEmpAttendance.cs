using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
   public class ValidateEmpAttendance
    {
        [Required]
        [Display(Name = "Select Employee")]
        public int EmployeeId { get; set; }

        [Required]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? AttendanceDate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public string TimeIn { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public string TimeOut { get; set; }
        [Required]
        [MaxLength(30)]
        //[DisplayFormat(ApplyFormatInEditMode = true)]
        //[DataType(DataType.Time)]
        public string Late { get; set; }
        [Required]
        [MaxLength(30)]
        //[DisplayFormat(ApplyFormatInEditMode = true)]
        //[DataType(DataType.Time)]
        public string OverTime { get; set; }
        public bool IsApprove { get; set; }

        [Required]
        public IFormFile AttendanceFile { get; set; }


    }
}
