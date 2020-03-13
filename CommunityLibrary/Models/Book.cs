using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Models
{
    public class Book
    {
        private List<Review> reviews = new List<Review>();
        private List<Request> requests = new List<Request>();

        public int BookID { get; set; }

        [Required(ErrorMessage= "Your book must have a title.")]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; }
        public string Author { get; set; }
        [Required]
        public string Owner { get; set; }
        public string Borrower { get; set; }

        [Required]
        [DataType(DataType.Date)]  // causes default validation and sets the HTML5 data type
        [DisplayFormat(DataFormatString = "{0:yyyy}")]  // only for display formatting with an HTML helper, doesn't validate
        public DateTime Written { get; set; }
        public bool Availability { get; set; }

        [Range(1, 5)]
        public int AverageRating { get; set; }

        public List<Review> Reviews { get { return reviews; } }
        public List<Request> Requests { get { return requests; } }
    }
}