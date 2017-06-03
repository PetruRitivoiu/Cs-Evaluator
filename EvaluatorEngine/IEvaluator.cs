using System;
using System.Collections.Generic;
using System.Text;

namespace EvaluatorEngine
{
    interface IEvaluator
    {
        Evaluation Evaluate(string[] args);
    }
}
