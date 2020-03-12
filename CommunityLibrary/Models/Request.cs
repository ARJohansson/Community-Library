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
        public string Requester { get; set; }
        [Required]
        public AppUser Owner { get; set; }
        [Required]
        public string BookTitle { get; set; }
        [Required]
        public string Duration { get; set; }
        public bool Accepted { get; set; }
    }
}