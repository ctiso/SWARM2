using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWARM.Shared.DTO
{
    public class GradeTwDTO
    {
        public string GradeTypeCode { get; set; }
        public int SectionId { get; set; }
        public int NumberPerSection { get; set; }
        public int PercentOfFinalGrade { get; set; }
        public int DropLowest { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SchoolId { get; set; }
    }
}
