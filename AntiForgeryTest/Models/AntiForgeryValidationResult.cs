using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AntiForgeryTest.Models
{
    public class AntiForgeryValidationResult
    {
        public bool ValidationAttempted { get; set; }
        public bool ValidationResult { get; set; }

    }
}
