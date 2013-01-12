using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AntiForgeryTest.Models
{
    public class SpoofModel
    {
        [Required]
        public string AntiForgeryKey {get; set;}
        [Required]
        public string AntiForgeryCookieValue { get; set; }
    }
}