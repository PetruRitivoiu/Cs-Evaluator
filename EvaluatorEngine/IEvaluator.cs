using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvaluatorEngine
{
    interface IEvaluator
    {
        Evaluation Evaluate(string shortFileName, string shortValidationFileName);
    }
}
