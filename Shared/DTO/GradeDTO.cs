using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWARM.Shared.DTO
{
    public class GradeDTO
    {
        public int StudentId { get; set; }
        public int SectionId { get; set; }
        public string GradeTypeCode { get; set; }
        public string GradeCodeOccurence { get; set; }
        public int NumericGrade { get; set; }
        public string? Comments { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SchoolId { get; set; }
    }
}
