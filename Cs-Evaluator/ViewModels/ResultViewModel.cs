﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.ViewModels
{
    public class ResultViewModel
    {
        public string SubjectName { get; set; }

        public string HomeworkName { get; set; }

        public string HomeworkDescription { get; set; }

        public string StudentName { get; set; }

        public double EvaluationResult { get; set; }

        public string Errors { get; set; }
    }
}
