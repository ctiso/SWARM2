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

namespace SWARM.Server.Controllers.GrdT
{
    public class GradeTypeController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public GradeTypeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetGradeT")]
        public async Task<IActionResult> Get()
        {
            List<GradeType> lstGrade = await _context.GradeTypes.OrderBy(x => x.GradeTypeCode).ToListAsync();
            return Ok(lstGrade);
        }

        [HttpGet]
        [Route("GetGradeT/{pGrdT}")]
        public async Task<IActionResult> Get(string pGrdT)
        {
            GradeType lstGrade = await _context.GradeTypes.Where(x => x.GradeTypeCode == pGrdT).FirstOrDefaultAsync();
            return Ok(lstGrade);
        }

        [HttpDelete]
        [Route("DeleteGradeT/{pGrdT}")]
        public async Task<IActionResult> Delete(string pGrdT)
        {
            GradeType lstGrade = await _context.GradeTypes.Where(x => x.GradeTypeCode == pGrdT).FirstOrDefaultAsync();
            _context.Remove(lstGrade);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeType _GradeT)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gdr = await _context.GradeTypes.Where(x => x.GradeTypeCode == _GradeT.GradeTypeCode).FirstOrDefaultAsync();
                if (_Gdr == null)
                {
                    bExist = false;
                    _Gdr = new GradeType();
                }
                else
                    bExist = true;

                _Gdr.Description = _GradeT.Description;
                _Gdr.GradeTypeCode = _GradeT.GradeTypeCode;

                _Gdr.SchoolId = _GradeT.SchoolId;
                if (bExist)
                {
                    _context.GradeTypes.Update(_Gdr);
                }
                else
                    _context.GradeTypes.Add(_Gdr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeT.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeType _GradeT)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gdr = await _context.GradeTypes.Where(x => x.GradeTypeCode == _GradeT.GradeTypeCode).FirstOrDefaultAsync();
                if (_Gdr != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }
                _Gdr = new GradeType();
                _Gdr.Description = _GradeT.Description;
                _Gdr.GradeTypeCode = _GradeT.GradeTypeCode;

                _Gdr.SchoolId = _GradeT.SchoolId;
                _context.GradeTypes.Add(_Gdr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeT.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
