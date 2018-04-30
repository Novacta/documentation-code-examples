using Novacta.Documentation.CodeExamples;
using System;

namespace SampleClassLibrary.CodeExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            string codeBase = @"..\..\..\..\SampleClassLibrary.CodeExamples";
            string defaultNamespace = "SampleClassLibrary.CodeExamples";
            var analyzer = new CodeExamplesAnalyzer(
                codeBase,
                defaultNamespace);
            analyzer.Run();
            Console.ReadKey();
        }
    }
}
