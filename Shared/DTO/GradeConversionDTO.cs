using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWARM.Shared.DTO
{
    public class GradeConversionDTO
    {
        public string LetterGrade { get; set; }
        public int GradePoint { get; set; }
        public decimal MaxGrade { get; set; }
        public decimal MinGrade { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SchoolId { get; set; }
    }
}
