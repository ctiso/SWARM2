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

namespace SWARM.Server.Controllers.GrdTW
{
    public class GradeTypeWeightController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public GradeTypeWeightController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetGradeTW")]
        public async Task<IActionResult> Get()
        {
            List<GradeTypeWeight> lstGrade = await _context.GradeTypeWeights.OrderBy(x => x.GradeTypeCode).ToListAsync();
            return Ok(lstGrade);
        }

        [HttpGet]
        [Route("GetGradeTW/{pGrdTW}")]
        public async Task<IActionResult> Get(string pGrdTW)
        {
            GradeTypeWeight lstGrade = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == pGrdTW).FirstOrDefaultAsync();
            return Ok(lstGrade);
        }

        [HttpDelete]
        [Route("DeleteGradeTW/{pGrdTW}")]
        public async Task<IActionResult> Delete(string pGrdTW)
        {
            GradeTypeWeight lstGrade = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == pGrdTW).FirstOrDefaultAsync();
            _context.Remove(lstGrade);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight _GradeTW)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gdr = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == _GradeTW.GradeTypeCode).FirstOrDefaultAsync();
                if (_Gdr == null)
                {
                    bExist = false;
                    _Gdr = new GradeTypeWeight();
                }
                else
                    bExist = true;

                _Gdr.NumberPerSection = _GradeTW.NumberPerSection;
                _Gdr.SectionId = _GradeTW.SectionId;
                _Gdr.GradeTypeCode = _GradeTW.GradeTypeCode;
                _Gdr.PercentOfFinalGrade = _GradeTW.PercentOfFinalGrade;
                _Gdr.DropLowest = _GradeTW.DropLowest;


                _Gdr.SchoolId = _GradeTW.SchoolId;
                if (bExist)
                {
                    _context.GradeTypeWeights.Update(_Gdr);
                }
                else
                    _context.GradeTypeWeights.Add(_Gdr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeTW.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight _GradeTW)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gdr = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == _GradeTW.GradeTypeCode).FirstOrDefaultAsync();
                if (_Gdr != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }
                _Gdr = new GradeTypeWeight();
                _Gdr.NumberPerSection = _GradeTW.NumberPerSection;
                _Gdr.SectionId = _GradeTW.SectionId;
                _Gdr.GradeTypeCode = _GradeTW.GradeTypeCode;
                _Gdr.PercentOfFinalGrade = _GradeTW.PercentOfFinalGrade;
                _Gdr.DropLowest = _GradeTW.DropLowest;


                _Gdr.SchoolId = _GradeTW.SchoolId;
                _context.GradeTypeWeights.Add(_Gdr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeTW.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
