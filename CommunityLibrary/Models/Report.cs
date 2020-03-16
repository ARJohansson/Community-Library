using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Models
{
    public class Report
    {
        public int ReportID { get; set; }
        public AppUser Reporter { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$")]
        public string ReportedUserName { get; set; }

        [Required]
        public string Reason { get; set; }

        [RegularExpression(@"[^/}{)(><=][^\.\.][^./]+.")]
        public string Details { get; set; }
    }
}