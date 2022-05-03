using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Models;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Ins
{
    public class InstructorController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public InstructorController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetInstructors")]
        public async Task<IActionResult> Get()
        {
            List<Instructor> lstInstructor = await _context.Instructors.OrderBy(x => x.InstructorId).ToListAsync();
            return Ok(lstInstructor);
        }

        [HttpGet]
        [Route("GetInstructors/{pInstructorId}")]
        public async Task<IActionResult> Get(int pInstructorId)
        {
            Instructor lstInstructor = await _context.Instructors.Where(x => x.InstructorId == pInstructorId).FirstOrDefaultAsync();
            return Ok(lstInstructor);
        }

        [HttpDelete]
        [Route("DeleteInstructors/{pInstructorId}")]
        public async Task<IActionResult> Delete(int pInstructorId)
        {
            Instructor lstInstructor = await _context.Instructors.Where(x => x.InstructorId == pInstructorId).FirstOrDefaultAsync();
            _context.Remove(lstInstructor);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Instructor _Instructor)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Inr = await _context.Instructors.Where(x => x.InstructorId == _Instructor.InstructorId).FirstOrDefaultAsync();
                if (_Inr == null)
                {
                    bExist = false;
                    _Inr = new Instructor();
                }
                else
                    bExist = true;

                _Inr.InstructorId = _Instructor.InstructorId;
                _Inr.Salutation = _Instructor.Salutation;
                _Inr.FirstName = _Instructor.FirstName;
                _Inr.LastName = _Instructor.LastName;
                _Inr.StreetAddress = _Instructor.StreetAddress;
                _Inr.Zip = _Instructor.Zip;
                _Inr.Phone = _Instructor.Phone;

                _Inr.SchoolId = _Instructor.SchoolId;
                if (bExist)
                {
                    _context.Instructors.Update(_Inr);
                }
                else
                    _context.Instructors.Add(_Inr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Instructor.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Instructor _Instructor)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Inr = await _context.Instructors.Where(x => x.InstructorId == _Instructor.InstructorId).FirstOrDefaultAsync();
                if (_Inr != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }
                _Inr = new Instructor();
                _Inr.InstructorId = _Instructor.InstructorId;
                _Inr.Salutation = _Instructor.Salutation;
                _Inr.FirstName = _Instructor.FirstName;
                _Inr.LastName = _Instructor.LastName;
                _Inr.StreetAddress = _Instructor.StreetAddress;
                _Inr.Zip = _Instructor.Zip;
                _Inr.Phone = _Instructor.Phone;
                _Inr.SchoolId = _Instructor.SchoolId;

                _context.Instructors.Add(_Inr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Instructor.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
