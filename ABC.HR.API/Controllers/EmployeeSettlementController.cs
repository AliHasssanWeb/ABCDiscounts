using ABC.EFCore.Entities.HR;
using ABC.EFCore.Repository.Edmx;
using ABC.HR.Domain.Entities;
using ABC.Shared;
using ABC.Shared.DataConfig;
using ABC.Shared.Interface;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.HR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeSettlementController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        EncryptDecrypt encdec = new EncryptDecrypt();
        private readonly IMailService mailService;


        public EmployeeSettlementController(ABCDiscountsContext _db, IMailService mailService)
        {
            db = _db;
      
            this.mailService = mailService;
        }





       // Employee Settlement Start

      [HttpGet("EmployeeSettlementGet")]
        public IActionResult EmployeeSettlementGet()
        {
            try
            {
                var record = db.EmployeeSettlements.ToList();
                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("EmployeeSettlementCreate")]
        public async Task<IActionResult> EmployeeSettlementCreate(EmployeeSettlement data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                } 
                if (data.EmployeeId==null || data.EmployeeId==0)
                {
                    return Ok("Employee ID Required");
                }
                bool isValid = db.EmployeeSettlements.ToList().Exists(x => x.SettlementId == data.SettlementId);
                if (isValid)
                {
                    return BadRequest("EmployeeSettlement Already Exists");

                }

                data.CreatedDate = DateTime.Now;

                db.EmployeeSettlements.Add(data);
                await db.SaveChangesAsync();
                return CreatedAtAction("EmployeeSettlementGet", new { id = data.SettlementId }, data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("EmployeeSettlementUpdate/{id}")]
        public async Task<IActionResult> EmployeeSettlementUpdate(int id, EmployeeSettlement data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    return Ok("Id Required");
                } 
                if (data.EmployeeId == 0 || data.EmployeeId == null)
                {
                    return Ok("Employee Id Required");
                }

                bool isValid = db.EmployeeSettlements.ToList().Exists(x => x.EmployeeEmail == data.EmployeeEmail && x.SettlementId != Convert.ToInt32(id));
                if (isValid)
                {
                    return BadRequest("EmployeeSettlement Already Exists");
                }

                var record = await db.EmployeeSettlements.FindAsync(id);

                if (data.EmployeeName != null && data.EmployeeName != "")
                {
                    record.EmployeeName = data.EmployeeName;
                }
                if (data.EmployeeEmail != null && data.EmployeeEmail != "")
                {
                    record.EmployeeEmail = data.EmployeeEmail;
                }
                if (data.PendingSalary != null && data.PendingSalary != "")
                {
                    record.PendingSalary = data.PendingSalary;
                }
                if (data.PendingLeave != null && data.PendingLeave != "")
                {
                    record.PendingLeave = data.PendingLeave;
                }
                if (data.EmployeeEmail != null && data.EmployeeEmail != "")
                {
                    record.EmployeeEmail = data.EmployeeEmail;
                }
                if (data.TerminationNode != null && data.TerminationNode !="")
                {
                    record.TerminationNode = data.TerminationNode;
                }
                if (data.TerminationDate != null)
                {
                    record.TerminationDate = data.TerminationDate;
                }
                if (data.IsLoan != null)
                {
                    record.IsLoan = data.IsLoan;
                }
                if (data.ConfidentialClear != null)
                {
                    record.ConfidentialClear = data.ConfidentialClear;
                }
                if (data.AssetsReturn != null)
                {
                    record.AssetsReturn = data.AssetsReturn;
                }
                data = record;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(data);
        }
        [HttpGet("EmployeeSettlementByID/{id}")]
        public IActionResult EmployeeSettlementByID(int id)
        {
            try
            {
                var record = db.EmployeeSettlements.Find(id);
                if (record != null)
                    return Ok(record);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("EmployeeSettlementEmployeeID/{id}")]
        public IActionResult EmployeeSettlementEmployeeID(int id)
        {
            try
            {
                var record = db.EmployeeSettlements.Where(x => x.SettlementId == id).FirstOrDefault();
                if (record != null)
                    return Ok(record);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteEmployeeSettlement/{id}")]
        public async Task<IActionResult> DeleteEmployeeSettlement(int id)
        {
            try
            {
                EmployeeSettlement data = await db.EmployeeSettlements.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.EmployeeSettlements.Remove(data);
                await db.SaveChangesAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Employee Settlement End
    }
}
