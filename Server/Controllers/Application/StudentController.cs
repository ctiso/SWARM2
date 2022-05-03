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

namespace SWARM.Server.Controllers.Stu
{
    public class StudentController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public StudentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetStudents")]
        public async Task<IActionResult> Get()
        {
            List<Student> lstStudents = await _context.Students.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lstStudents);
        }

        [HttpGet]
        [Route("GetStudents/{pStudentId}")]
        public async Task<IActionResult> Get(int pStudentId)
        {
            Student itmStudentId = await _context.Students.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();
            return Ok(itmStudentId);
        }

        [HttpDelete]
        [Route("DeleteStudents/{pStudentId}")]
        public async Task<IActionResult> Delete(int pStudentId)
        {
            Student itmStudentId = await _context.Students.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();
            _context.Remove(itmStudentId);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Student _Student)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Stu = await _context.Students.Where(x => x.StudentId == _Student.StudentId).FirstOrDefaultAsync();
                if (_Stu == null)
                {
                    bExist = false;
                    _Stu = new Student();
                }
                else
                    bExist = true;

                _Stu.StudentId = _Student.StudentId;
                _Stu.Salutation = _Student.Salutation;
                _Stu.FirstName = _Student.FirstName;
                _Stu.LastName = _Student.LastName;
                _Stu.StreetAddress = _Student.StreetAddress;
                _Stu.Zip = _Student.Zip;
                _Stu.Phone = _Student.Phone;
                _Stu.Employer = _Student.Employer;
                _Stu.RegistrationDate = _Student.RegistrationDate;


                _Stu.SchoolId = _Student.SchoolId;
                if (bExist)
                {
                    _context.Students.Update(_Stu);
                }
                else
                    _context.Students.Add(_Stu);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Student.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student _Student)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Stu = await _context.Students.Where(x => x.StudentId == _Student.StudentId).FirstOrDefaultAsync();
                if (_Stu != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }
                _Stu = new Student();
                _Stu.StudentId = _Student.StudentId;
                _Stu.Salutation = _Student.Salutation;
                _Stu.FirstName = _Student.FirstName;
                _Stu.LastName = _Student.LastName;
                _Stu.StreetAddress = _Student.StreetAddress;
                _Stu.Zip = _Student.Zip;
                _Stu.Phone = _Student.Phone;
                _Stu.Employer = _Student.Employer;
                _Stu.RegistrationDate = _Student.RegistrationDate;


                _Stu.SchoolId = _Student.SchoolId;

                _context.Students.Add(_Stu);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Student.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
