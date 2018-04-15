using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CsEvaluator.Data.ViewModels
{
    public class HomeworkDescriptionViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        [Required]
        public string FullDescription { get; set; }

        [Required]
        [Display(Name = "Reflection File")]
        public IFormFile ReflectionFile { get; set; }

        [Required]
        [Display(Name = "Unit tests file")]
        public IFormFile UnitTestFile { get; set; }
    }
}
