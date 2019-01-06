// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Novacta.Documentation.CodeExamples.Tests.Tools;
using System.Runtime.InteropServices;

namespace Novacta.Documentation.CodeExamples.Tests
{
    [DeploymentItem(
    @"Advanced\CodeExample4.cs",
    @"Advanced")]
    [DeploymentItem(
    @"Advanced\CodeExample5.cs",
    @"Advanced")]
    [DeploymentItem(
    @"Advanced\CodeExample6.cs",
    @"Advanced")]
    [DeploymentItem(
    @"Advanced\CodeExample7.cs",
    @"Advanced")]
    [DeploymentItem(@"CodeExample0.cs")]
    [DeploymentItem(@"CodeExample1.cs")]
    [DeploymentItem(@"CodeExample2.cs")]
    [DeploymentItem(@"CodeExample3.cs")]
    [TestClass]
    public class CodeExampleAnalyzerTests
    {
        void RemoveExampleOutputs() {
            string codeBase = Environment.CurrentDirectory;
            foreach (var file in Directory.EnumerateFiles(
                codeBase,
                "*.cs.txt",
                SearchOption.AllDirectories))
            {
                File.Delete(file);
            }
        }

        [TestInitialize()]
        public void Initialize()
        {
            RemoveExampleOutputs();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            RemoveExampleOutputs();
        }

        [TestMethod]
        public void CtorTest()
        {
            // Wrong code base
            {
                string expectedPartialMessage =
                    RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ?
                        "The specified directory does not exist or an error occurred when " +
                            "trying to determine if the specified folder exists. "
                    :
                        "The specified directory does not exist or an error occurred when " +
                        "trying to determine if the specified folder exists.";

                ArgumentExceptionAssert.IsThrown(
                    () =>
                    {
                        string codeBase = Path.Combine(Environment.CurrentDirectory,
                            "1a793a0c-1178-42e3-a6bc-c23ca6d7289d");
                        string defaultNamespace = "SampleClassLibrary.CodeExamples";
                        var analyzer = new CodeExamplesAnalyzer(
                            codeBase,
                            defaultNamespace);
                    },
                    expectedType: typeof(ArgumentException),
                    expectedPartialMessage:
                        "The specified directory does not exist or an error occurred when " +
                        "trying to determine if the specified folder exists. ",
                    expectedParameterName: "codeBase");
            }
            // Null default namespace
            {
                ArgumentExceptionAssert.IsThrown(
                    () =>
                    {
                        string codeBase = Environment.CurrentDirectory;
                        string defaultNamespace = null;
                        var analyzer = new CodeExamplesAnalyzer(
                            codeBase,
                            defaultNamespace);
                    },
                    expectedType: typeof(ArgumentNullException),
                    expectedPartialMessage: ArgumentExceptionAssert.NullPartialMessage,
                    expectedParameterName: "defaultNamespace");
            }
            // Mainstream use case
            {
                string codeBase = Environment.CurrentDirectory;
                string defaultNamespace = "SampleClassLibrary.CodeExamples";
                var analyzer = new CodeExamplesAnalyzer(
                    codeBase,
                    defaultNamespace);
                Assert.AreEqual(expected: codeBase, actual: analyzer.CodeBase);
                Assert.AreEqual(expected: defaultNamespace, actual: analyzer.DefaultNamespace);
            }
        }

        [TestMethod]
        public void RunWithoutReportInfoTest()
        {
            string codeBase = Environment.CurrentDirectory;
            string defaultNamespace = "Tests.CodeExamples";
            var analyzer = new CodeExamplesAnalyzer(
                codeBase,
                defaultNamespace);

            // Without report info.

            StringWriter consoleWriter = new StringWriter();
            var defaultOut = Console.Out;
            Console.SetOut(consoleWriter);

            var examples = analyzer.Run(false);
            Assert.AreEqual(String.Empty, consoleWriter.ToString());
            Console.SetOut(defaultOut);

            CodeExampleTester.TestCodeExample0(examples);
            CodeExampleTester.TestCodeExample1(examples);
            CodeExampleTester.TestCodeExample2(examples);
            CodeExampleTester.TestCodeExample3(examples);
            CodeExampleTester.TestCodeExample4(examples);
            CodeExampleTester.TestCodeExample5(examples);
            CodeExampleTester.TestCodeExample6(examples);
            CodeExampleTester.TestCodeExample7(examples);
        }

        [TestMethod]
        public void RunWithReportInfoTest()
        {
            string codeBase = Environment.CurrentDirectory;
            string defaultNamespace = "Tests.CodeExamples";
            var analyzer = new CodeExamplesAnalyzer(
                codeBase,
                defaultNamespace);

            // With report info.

            var consoleWriter = new StringWriter();
            var defaultOut = Console.Out;
            Console.SetOut(consoleWriter);

            var examples = analyzer.Run(true);

            StringWriter expectedWriter = new StringWriter();

            expectedWriter.WriteLine("Tests.CodeExamples.CodeExample6:");
            expectedWriter.WriteLine("   Source code: cannot be found.");
            expectedWriter.WriteLine("   Execution: successful.");

            expectedWriter.WriteLine("Tests.CodeExamples.CodeExample7:");
            expectedWriter.WriteLine("   Source code: cannot be found.");
            expectedWriter.WriteLine("   Execution: failed. Exception: CodeExample7:error");

            expectedWriter.WriteLine("Tests.CodeExamples.CodeExample0:");
            expectedWriter.WriteLine("   Source code: found.");
            expectedWriter.WriteLine("   Execution: failed. Exception: CodeExample0:error");

            expectedWriter.WriteLine("Tests.CodeExamples.CodeExample1:");
            expectedWriter.WriteLine("   Source code: found.");
            expectedWriter.WriteLine("   Execution: successful.");

            expectedWriter.WriteLine("Tests.CodeExamples.Advanced.CodeExample4:");
            expectedWriter.WriteLine("   Source code: found.");
            expectedWriter.WriteLine("   Execution: failed. Exception: CodeExample4:error");

            expectedWriter.WriteLine("Tests.CodeExamples.Advanced.CodeExample5:");
            expectedWriter.WriteLine("   Source code: found.");
            expectedWriter.WriteLine("   Execution: successful.");

            expectedWriter.WriteLine("Tests.CodeExamples.Advanced.CodeExample2:");
            expectedWriter.WriteLine("   Source code: cannot be found.");
            expectedWriter.WriteLine("   Execution: successful.");

            expectedWriter.WriteLine("Tests.CodeExamples.Advanced.CodeExample3:");
            expectedWriter.WriteLine("   Source code: cannot be found.");
            expectedWriter.WriteLine("   Execution: failed. Exception: CodeExample3:error");
            expectedWriter.WriteLine();
            expectedWriter.WriteLine("Number of examples: 8.");
            expectedWriter.WriteLine("   Number of examples with no source code: 4.");
            expectedWriter.WriteLine("   Number of failed examples: 4.");

            var expected = expectedWriter.ToString();
            var actual = consoleWriter.ToString();
            Assert.AreEqual(expected, actual);
            Console.SetOut(defaultOut);


            CodeExampleTester.TestCodeExample0(examples);
            CodeExampleTester.TestCodeExample1(examples);
            CodeExampleTester.TestCodeExample2(examples);
            CodeExampleTester.TestCodeExample3(examples);
            CodeExampleTester.TestCodeExample4(examples);
            CodeExampleTester.TestCodeExample5(examples);
            CodeExampleTester.TestCodeExample6(examples);
            CodeExampleTester.TestCodeExample7(examples);
        }

        [TestMethod]
        public void RunWithReportInfoNoIssuesTest()
        {
            string codeBase = Environment.CurrentDirectory;
            string defaultNamespace = "NoIssues.Tests.CodeExamples";
            var analyzer = new CodeExamplesAnalyzer(
                codeBase,
                defaultNamespace);

            // With report info.

            var consoleWriter = new StringWriter();
            var defaultOut = Console.Out;
            Console.SetOut(consoleWriter);

            var examples = analyzer.Run(true);

            StringWriter expectedWriter = new StringWriter();

            expectedWriter.WriteLine("NoIssues.Tests.CodeExamples.CodeExample8:");
            expectedWriter.WriteLine("   Source code: found.");
            expectedWriter.WriteLine("   Execution: successful.");

            expectedWriter.WriteLine("NoIssues.Tests.CodeExamples.Advanced.CodeExample9:");
            expectedWriter.WriteLine("   Source code: found.");
            expectedWriter.WriteLine("   Execution: successful.");

            expectedWriter.WriteLine();
            expectedWriter.WriteLine("Number of examples: 2.");
            expectedWriter.WriteLine("   Number of examples with no source code: 0.");
            expectedWriter.WriteLine("   Number of failed examples: 0.");

            var expected = expectedWriter.ToString();
            var actual = consoleWriter.ToString();
            Assert.AreEqual(expected, actual);
            Console.SetOut(defaultOut);

            CodeExampleTester.TestCodeExample8(examples);
            CodeExampleTester.TestCodeExample9(examples);
        }
    }
}