using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CsEvaluator.Data.ViewModels
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

        public double StaticEvaluationResult { get; set; } = -1;

        public double FunctionalEvaluationResult { get; set; } = -1;

        public double EvaluationResult
        {
            get
            {
                return (StaticEvaluationResult + FunctionalEvaluationResult) / 2;
            }
        }

        public IEnumerable<StudentPreviewModel> Students { get; set; }

        public IEnumerable<HomeworkDescriptionPreviewModel> HomeworkDescriptions { get; set; }

        public string GetHomeworkDirectory()
        {
            return $"student_{StudentID}_homework_{HomeworkDescriptionID}";
        }

    }
}
