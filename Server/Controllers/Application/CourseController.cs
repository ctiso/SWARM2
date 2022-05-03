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

namespace SWARM.Server.Controllers.Crse
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public CourseController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetCourses")]
        public async Task<IActionResult> Get()
        {
            List<Course> lstCourses = await _context.Courses.OrderBy(x => x.CourseNo).ToListAsync();
            return Ok(lstCourses);
        }

        [HttpGet]
        [Route("GetCourses/{pCourseNo}")]
        public async Task<IActionResult> Get(int pCourseNo)
        {
            Course itmCourse = await _context.Courses.Where(x => x.CourseNo == pCourseNo).FirstOrDefaultAsync();
            return Ok(itmCourse);
        }

        [HttpDelete]
        [Route("DeleteCourse/{pCourseNo}")]
        public async Task<IActionResult> Delete(int pCourseNo)
        {
            Course itmCourse = await _context.Courses.Where(x => x.CourseNo == pCourseNo).FirstOrDefaultAsync();
            _context.Remove(itmCourse);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Course _Course)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Crse = await _context.Courses.Where(x => x.CourseNo == _Course.CourseNo).FirstOrDefaultAsync();
                if (_Crse == null)
                {
                    bExist = false;
                    _Crse = new Course();
                }
                else
                    bExist = true;

                _Crse.Cost = _Course.Cost;
                _Crse.Description = _Course.Description;
                _Crse.Prerequisite = _Course.Prerequisite;
                _Crse.PrerequisiteSchoolId = _Course.PrerequisiteSchoolId;
                _Crse.SchoolId = _Course.SchoolId;
                if (bExist)
                {
                    _context.Courses.Update(_Crse);
                }
                else
                    _context.Courses.Add(_Crse);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Course.CourseNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Course _Course)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Crse = await _context.Courses.Where(x => x.CourseNo == _Course.CourseNo).FirstOrDefaultAsync();
                if (_Crse != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }
                _Crse = new Course();
                _Crse.Cost = _Course.Cost;
                _Crse.Description = _Course.Description;
                _Crse.Prerequisite = _Course.Prerequisite;
                _Crse.PrerequisiteSchoolId = _Course.PrerequisiteSchoolId;
                _Crse.SchoolId = _Course.SchoolId;

                _context.Courses.Add(_Crse);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Course.CourseNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    } 
}
