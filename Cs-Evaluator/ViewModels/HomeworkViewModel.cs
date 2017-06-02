using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using CsEvaluator.Entities;

namespace CsEvaluator.ViewModels
{
    public class HomeworkViewModel
    {
        [Required]
        [Display(Name = "Source Code")]
        public IFormFile CsProject { get; set; }

        [Required]
        public int StudentID { get; set; }

        [Required]
        public int HomeworkDescriptionID { get; set; }

        public IEnumerable<StudentPreviewModel> Students { get; set; }

        public IEnumerable<HomeworkDescriptionPreviewModel> HomeworkDescriptions { get; set; }

    }
}
