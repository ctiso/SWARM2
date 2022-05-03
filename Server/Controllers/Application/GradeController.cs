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

namespace SWARM.Server.Controllers.Grd
{
    public class GradeController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public GradeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetGrades")]
        public async Task<IActionResult> Get()
        {
            List<Grade> lstGrade = await _context.Grades.OrderBy(x => x.GradeCodeOccurrence).ToListAsync();
            return Ok(lstGrade);
        }

        [HttpGet]
        [Route("GetGrades/{pGrdOcc}")]
        public async Task<IActionResult> Get(int pGrdOcc)
        {
            Grade lstGrade = await _context.Grades.Where(x => x.GradeCodeOccurrence == pGrdOcc).FirstOrDefaultAsync();
            return Ok(lstGrade);
        }

        [HttpDelete]
        [Route("DeleteGrades/{pGrdOcc}")]
        public async Task<IActionResult> Delete(int pGrdOcc)
        {
            Grade lstGrade = await _context.Grades.Where(x => x.GradeCodeOccurrence == pGrdOcc).FirstOrDefaultAsync();
            _context.Remove(lstGrade);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Grade _Grade)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gdr = await _context.Grades.Where(x => x.GradeCodeOccurrence == _Grade.GradeCodeOccurrence).FirstOrDefaultAsync();
                if (_Gdr == null)
                {
                    bExist = false;
                    _Gdr = new Grade();
                }
                else
                    bExist = true;

                _Gdr.StudentId = _Grade.StudentId;
                _Gdr.SectionId = _Grade.SectionId;
                _Gdr.GradeTypeCode = _Grade.GradeTypeCode;
                _Gdr.NumericGrade = _Grade.NumericGrade;
                _Gdr.Comments = _Grade.Comments;

                _Gdr.SchoolId = _Grade.SchoolId;
                if (bExist)
                {
                    _context.Grades.Update(_Gdr);
                }
                else
                    _context.Grades.Add(_Gdr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Grade.GradeCodeOccurrence);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Grade _Grade)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gdr = await _context.Grades.Where(x => x.GradeCodeOccurrence == _Grade.GradeCodeOccurrence).FirstOrDefaultAsync();
                if (_Gdr != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }
                _Gdr = new Grade();
                _Gdr.StudentId = _Grade.StudentId;
                _Gdr.SectionId = _Grade.SectionId;
                _Gdr.GradeTypeCode = _Grade.GradeTypeCode;
                _Gdr.NumericGrade = _Grade.NumericGrade;
                _Gdr.Comments = _Grade.Comments;

                _Gdr.SchoolId = _Grade.SchoolId;

                _context.Grades.Add(_Gdr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Grade.GradeCodeOccurrence);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
