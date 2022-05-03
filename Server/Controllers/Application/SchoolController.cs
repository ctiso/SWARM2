using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Schl
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public SchoolController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetSchools")]
        public async Task<IActionResult> Get()
        {
            List<School> lstCourses = await _context.Schools.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstCourses);
        }

        [HttpGet]
        [Route("GetSchools/{pSchoolId}")]
        public async Task<IActionResult> Get(int pSchoolId)
        {
            School itmSchool = await _context.Schools.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            return Ok(itmSchool);
        }

        [HttpDelete]
        [Route("DeleteSchool/{pSchoolId}")]
        public async Task<IActionResult> Delete(int pSchoolId)
        {
            School itmSchool = await _context.Schools.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            _context.Remove(itmSchool);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] School _School)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Sch = await _context.Schools.Where(x => x.SchoolId == _School.SchoolId).FirstOrDefaultAsync();
                if (_Sch == null)
                {
                    bExist = false;
                    _Sch = new School();
                }
                else
                    bExist = true;

                _Sch.SchoolName = _School.SchoolName;
                
                _Sch.SchoolId = _School.SchoolId;
                if (bExist)
                {
                    _context.Schools.Update(_Sch);
                }
                else
                    _context.Schools.Add(_Sch);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_School.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] School _School)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Sch = await _context.Schools.Where(x => x.SchoolId == _School.SchoolId).FirstOrDefaultAsync();
                if (_Sch != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }
                _Sch = new School();
                _Sch.SchoolName = _School.SchoolName;
               
                _Sch.SchoolId = _School.SchoolId;

                _context.Schools.Add(_Sch);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_School.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    } 
}
