using ABC.EFCore.Repository.Edmx;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        public FinanceController(ABCDiscountsContext _db)
        {
            db = _db;
        }


        [HttpGet("GetPayables")]
        public IActionResult GetPayables()
        {
            try
            {
                var accounts = db.Payables.ToList();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetReceivables")]
        public IActionResult GetReceivables()
        {
            try
            {
                var accounts = db.Receivables.ToList();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("FinanceTransactionGet")]
        public IActionResult FinanceTransactionGet()
        {
            try
            {
                var record = db.Transactions.ToList();
                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
