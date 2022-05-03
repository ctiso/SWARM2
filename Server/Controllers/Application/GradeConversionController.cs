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

namespace SWARM.Server.Controllers.GrdC
{
    public class GradeConversionController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public GradeConversionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetGradeConversion")]
        public async Task<IActionResult> Get()
        {
            List<GradeConversion> lstGrade = await _context.GradeConversions.OrderBy(x => x.LetterGrade).ToListAsync();
            return Ok(lstGrade);
        }

        [HttpGet]
        [Route("GetGradeConversion/{pLetter}")]
        public async Task<IActionResult> Get(string pLetter)
        {
            GradeConversion lstGrade = await _context.GradeConversions.Where(x => x.LetterGrade == pLetter).FirstOrDefaultAsync();
            return Ok(lstGrade);
        }

        [HttpDelete]
        [Route("DeleteGradeConversion/{pLetter}")]
        public async Task<IActionResult> Delete(string pLetter)
        {
            GradeConversion lstGrade = await _context.GradeConversions.Where(x => x.LetterGrade == pLetter).FirstOrDefaultAsync();
            _context.Remove(lstGrade);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeConversion _Letter)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Cvr = await _context.GradeConversions.Where(x => x.LetterGrade == _Letter.LetterGrade).FirstOrDefaultAsync();
                if (_Cvr == null)
                {
                    bExist = false;
                    _Cvr = new GradeConversion();
                }
                else
                    bExist = true;

                _Cvr.LetterGrade = _Letter.LetterGrade;
                _Cvr.GradePoint = _Letter.GradePoint;
                _Cvr.MaxGrade = _Letter.MaxGrade;
                _Cvr.MinGrade = _Letter.MinGrade;
                _Cvr.SchoolId = _Letter.SchoolId;
                if (bExist)
                {
                    _context.GradeConversions.Update(_Cvr);
                }
                else
                    _context.GradeConversions.Add(_Cvr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Letter.LetterGrade);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeConversion _Letter)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Cvr = await _context.GradeConversions.Where(x => x.LetterGrade == _Letter.LetterGrade).FirstOrDefaultAsync();
                if (_Cvr != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }
                _Cvr = new GradeConversion();
                _Cvr.LetterGrade = _Letter.LetterGrade;
                _Cvr.GradePoint = _Letter.GradePoint;
                _Cvr.MaxGrade = _Letter.MaxGrade;
                _Cvr.MinGrade = _Letter.MinGrade;
                _Cvr.SchoolId = _Letter.SchoolId;
                _context.GradeConversions.Add(_Cvr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Letter.LetterGrade);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
