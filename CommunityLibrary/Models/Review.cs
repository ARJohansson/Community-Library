using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        [Required]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$")]
        public string Reviewer { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        [RegularExpression(@"[^/}{)(><=][^\.\.][^./]+$")]
        public string BookTitle { get; set; }

        [RegularExpression(@"[^/}{)(><=][^\.\.][^./]+.")]
        public string Text { get; set; }
        [Required]
        [Range(1, 5)]
        public int BookRating { get; set; }
        [Range(1, 5)]
        public int ReviewRating { get; set; }
    }
}