using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Models
{
    public class Request
    {
        public int RequestID { get; set; }
        [Required]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$")]
        public string Requester { get; set; }
        [Required]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$")]
        public string Owner { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        [RegularExpression(@"[^/}{)(><=][^\.\.][^./]+$")]
        public string BookTitle { get; set; }

        [Required]
        public string Duration { get; set; }
        public bool Accepted { get; set; }
    }
}