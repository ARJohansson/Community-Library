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
        public AppUser Reviewer { get; set; }
        [Required]
        public string BookTitle { get; set; }
        public string Text { get; set; }
        [Required]
        [Range(1, 5)]
        public int BookRating { get; set; }
        [Range(1, 5)]
        public int ReviewRating { get; set; }
    }
}