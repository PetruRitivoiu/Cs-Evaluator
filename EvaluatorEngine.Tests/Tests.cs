﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace EvaluatorEngine.Tests
{
    class Tests
    {
        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            Console.WriteLine("Hello World!");

            PAWEvaluator p = new PAWEvaluator();
            p.Evaluate("DemoProiectCS", "asdf.txt");

            watch.Stop();

            Console.WriteLine($"Tests.Main total time elapsed: {watch.ElapsedMilliseconds}");

            Console.ReadLine();
        }

    }
}