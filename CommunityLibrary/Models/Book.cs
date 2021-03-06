﻿using System;
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
        public string ImgLink { get; set; }

        [Required(ErrorMessage= "Your book must have a title.")]
        [StringLength(100, MinimumLength = 2)]
        [RegularExpression(@"([A-Za-z'\s]){2,100}")]
        public string Title { get; set; }

        [RegularExpression(@"(([A-Z.]{1,4})+\s+[A-Za-z]{0,50})|(([A-Z]{1}[a-z]{1,20}[\s]{0,1}){1,4})")]
        public string Author { get; set; }
        [Required]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$")]
        public string Owner { get; set; }
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$")]
        public string Borrower { get; set; }

        [Required]
        [DataType(DataType.Date)]  // causes default validation and sets the HTML5 data type
        [DisplayFormat(DataFormatString = "{0:yyyy}")]  // only for display formatting with an HTML helper, doesn't validate
        public DateTime Written { get; set; }
        public bool Availability { get; set; }

        [Range(1, 5)]
        [RegularExpression(@"[1-5]")]
        public int AverageRating { get; set; }

        public List<Review> Reviews { get { return reviews; } }
        public List<Request> Requests { get { return requests; } }
    }
}