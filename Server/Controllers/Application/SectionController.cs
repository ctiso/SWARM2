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

namespace SWARM.Server.Controllers.Sect
{
    public class SectionController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public SectionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetSection")]
        public async Task<IActionResult> Get()
        {
            List<Section> lstSection = await _context.Sections.OrderBy(x => x.SectionId).ToListAsync();
            return Ok(lstSection);
        }

        [HttpGet]
        [Route("GetSection/{pSectionId}")]
        public async Task<IActionResult> Get(int pSectionId)
        {
            Section itmSection = await _context.Sections.Where(x => x.SectionId == pSectionId).FirstOrDefaultAsync();
            return Ok(itmSection);
        }

        [HttpDelete]
        [Route("DeleteSection/{pStudentId}")]
        public async Task<IActionResult> Delete(int pSectionId)
        {
            Section imtSection = await _context.Sections.Where(x => x.SectionId == pSectionId).FirstOrDefaultAsync();
            _context.Remove(imtSection);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Section _Section)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Sec = await _context.Sections.Where(x => x.SectionId == _Section.SectionId).FirstOrDefaultAsync();
                if (_Sec == null)
                {
                    bExist = false;
                    _Sec = new Section();
                }
                else
                    bExist = true;

                _Sec.CourseNo = _Section.CourseNo;
                _Sec.SectionId = _Section.SectionId;
                _Sec.SectionNo = _Section.SectionNo;
                _Sec.StartDateTime = _Section.StartDateTime;
                _Sec.Location = _Section.Location;
                _Sec.InstructorId = _Section.InstructorId;
                _Sec.Capacity = _Section.Capacity;


                _Sec.SchoolId = _Section.SchoolId;
                if (bExist)
                {
                    _context.Sections.Update(_Sec);
                }
                else
                    _context.Sections.Add(_Sec);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Section.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Section _Section)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Sec = await _context.Sections.Where(x => x.SectionId == _Section.SectionId).FirstOrDefaultAsync();
                if (_Sec != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }
                _Sec = new Section();
                _Sec.CourseNo = _Section.CourseNo;
                _Sec.SectionId = _Section.SectionId;
                _Sec.SectionNo = _Section.SectionNo;
                _Sec.StartDateTime = _Section.StartDateTime;
                _Sec.Location = _Section.Location;
                _Sec.InstructorId = _Section.InstructorId;
                _Sec.Capacity = _Section.Capacity;
                _Sec.SchoolId = _Section.SchoolId;

                _context.Sections.Add(_Sec);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Section.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
