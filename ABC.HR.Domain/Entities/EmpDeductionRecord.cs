﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class EmpDeductionRecord
    {
        public int EmployeeID { get; set; }
        public double TotalDeductions { get; set; }
    }
}
