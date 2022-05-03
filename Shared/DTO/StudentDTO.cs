﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWARM.Shared.DTO
{
    public class StudentDTO
    {
        public int StudentId { get; set; }
        public string? Salutation { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SchoolId { get; set; }
        public string? StreetAddress { get; set; }
        public string Zip { get; set; }
        public string? Phone { get; set; }
        public string? Employer { get; set; }
        public DateTime RegistrationDate { get; set; }



    }
}
