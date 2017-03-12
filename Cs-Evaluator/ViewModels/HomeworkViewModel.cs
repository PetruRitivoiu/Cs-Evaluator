using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Cs_Evaluator.ViewModels
{
    public class HomeworkViewModel
    {
        [Required]
        public string Surname { get; set; }

        [Required]
        public string Forename { get; set; }

        [Required]
        public SubjectViewModel Subject { get; set; }

        [Required]
        [Display(Name = "Source Code")]
        public IFormFile CsProject { get; set; }

    }
}
