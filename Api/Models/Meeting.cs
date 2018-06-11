using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Meeting
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public DateTimeOffset? EventDateTime { get; set; }

        [Required]
        public string Description { get; set; }

        public int? AttendeeLimit { get; set; }

        [Required]
        public string Location { get; set; }        
    }
}