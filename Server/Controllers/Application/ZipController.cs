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

namespace SWARM.Server.Controllers.Zp
{
    public class ZipController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public ZipController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetZipCode")]
        public async Task<IActionResult> Get()
        {
            List<Zipcode> lstZip = await _context.Zipcodes.OrderBy(x => x.Zip).ToListAsync();
            return Ok(lstZip);
        }

        [HttpGet]
        [Route("GetZipCode/{pZip}")]
        public async Task<IActionResult> Get(string pZip)
        {
            Zipcode lstZip = await _context.Zipcodes.Where(x => x.Zip == pZip).FirstOrDefaultAsync();
            return Ok(lstZip);
        }

        [HttpDelete]
        [Route("DeleteZipCode/{pZip}")]
        public async Task<IActionResult> Delete(string pZip)
        {
            Zipcode lstZip = await _context.Zipcodes.Where(x => x.Zip == pZip).FirstOrDefaultAsync();
            _context.Remove(lstZip);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Zipcode _ZipCode)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _zip = await _context.Zipcodes.Where(x => x.Zip == _ZipCode.Zip).FirstOrDefaultAsync();
                if (_zip == null)
                {
                    bExist = false;
                    _zip = new Zipcode();
                }
                else
                    bExist = true;

                _zip.City = _ZipCode.City;
                _zip.State = _ZipCode.State;
                _zip.Zip = _ZipCode.Zip;

                if (bExist)
                {
                    _context.Zipcodes.Update(_zip);
                }
                else
                    _context.Zipcodes.Add(_zip);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_ZipCode.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Zipcode _ZipCode)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Zip = await _context.Zipcodes.Where(x => x.Zip == _ZipCode.Zip).FirstOrDefaultAsync();
                if (_Zip != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }
                _Zip = new Zipcode();
                _Zip.City = _ZipCode.City;
                _Zip.State = _ZipCode.State;
                _Zip.Zip = _ZipCode.Zip;

                _context.Zipcodes.Add(_Zip);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_ZipCode.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
