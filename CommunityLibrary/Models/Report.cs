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
        [Required]
        public string ReportedUserName { get; set; }
        [Required]
        public string Reason { get; set; }
        public string Details { get; set; }
    }
}