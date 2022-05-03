using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWARM.Shared.DTO
{
    public class SectionDTO
    {
        public int CourseNo { get; set; }
        public int SectionId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public string? Location { get; set; }
        public int InstructorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SchoolId { get; set; }
        public int? Capacity { get; set; }
    }
}
