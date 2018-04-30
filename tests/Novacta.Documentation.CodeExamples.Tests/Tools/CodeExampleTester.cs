// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Novacta.Documentation.CodeExamples.Tests.Tools
{
    static class CodeExampleTester
    {
        public static CodeExampleInfo Select(
            IEnumerable<CodeExampleInfo> examples,
            string exampleFullName)
        {
            var query =
                from example
                in examples
                where example.Type.FullName == exampleFullName
                select example;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Tests a faulted code example, defined 
        /// in the default namespace, whose source code
        /// can be found.
        /// </summary>
        public static void TestCodeExample0(List<CodeExampleInfo> examples)
        {
            var exampleFullName = "Tests.CodeExamples.CodeExample0";
            var example = Select(examples, exampleFullName);

            Assert.IsNotNull(example);
            CodeExampleInfoAssert.IsNotSuccessful(example);
            Assert.IsTrue(example.HasSourceCode);
            StringWriter writer = new StringWriter();
            writer.Write("CodeExample0:error");
            CodeExampleInfoAssert.HasError(
                example,
                writer.ToString());

            CodeExampleInfoAssert.HasNotOutputFile(example);
        }

        /// <summary>
        /// Tests a successful code example, defined 
        /// in the default namespace, whose source code
        /// can be found.
        /// </summary>
        public static void TestCodeExample1(List<CodeExampleInfo> examples)
        {
            var exampleFullName = "Tests.CodeExamples.CodeExample1";
            var example = Select(examples, exampleFullName);

            Assert.IsNotNull(example);
            CodeExampleInfoAssert.IsSuccessful(example);
            Assert.IsTrue(example.HasSourceCode);
            StringWriter writer = new StringWriter();
            writer.WriteLine("CodeExample1:output");
            CodeExampleInfoAssert.HasOutput(example, writer.ToString());

            var commentSymbol = example.Language.CommentSymbol;

            writer = new StringWriter();
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace Tests.CodeExamples");
            writer.WriteLine("{");
            writer.WriteLine("    public class CodeExample1  ");
            writer.WriteLine("    {");
            writer.WriteLine("        public void Main()");
            writer.WriteLine("        {");
            writer.WriteLine("            Console.WriteLine(\"CodeExample1:output\");");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine(commentSymbol +
                "Executing method Main() produces the following output:");
            writer.WriteLine(commentSymbol);
            writer.WriteLine(commentSymbol + "CodeExample1:output");
            CodeExampleInfoAssert.HasOutputFile(example, writer.ToString());
        }


        /// <summary>
        /// Tests a successful code example, not defined 
        /// in the default namespace, whose source code
        /// cannot be found.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Source code files must be stored in a directory tree under a 
        /// user defined folder, the <see cref="CodeExamplesAnalyzer.CodeBase"/>, 
        /// reflecting the project namespace hierarchy.
        /// </para>
        /// <para>
        /// As a consequence, the source code for <see cref="CodeExample2"/> cannot 
        /// be found since the file is in not in the expected directory.
        /// </para>
        /// </remarks>
        public static void TestCodeExample2(List<CodeExampleInfo> examples)
        {
            var exampleFullName = "Tests.CodeExamples.Advanced.CodeExample2";
            var example = Select(examples, exampleFullName);

            Assert.IsNotNull(example);
            CodeExampleInfoAssert.IsSuccessful(example);
            Assert.IsFalse(example.HasSourceCode);
            StringWriter writer = new StringWriter();
            writer.WriteLine("CodeExample2:output");
            CodeExampleInfoAssert.HasOutput(example, writer.ToString());

            CodeExampleInfoAssert.HasNotOutputFile(example);
        }

        /// <summary>
        /// Tests a faulted code example, not defined 
        /// in the default namespace, whose source code
        /// cannot be found.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Source code files must be stored in a directory tree under a 
        /// user defined folder, the <see cref="CodeExamplesAnalyzer.CodeBase"/>, 
        /// reflecting the project namespace hierarchy.
        /// </para>
        /// <para>
        /// As a consequence, the source code for <see cref="CodeExample3"/> cannot 
        /// be found since the file is in not in the expected directory.
        /// </para>
        /// </remarks>
        public static void TestCodeExample3(List<CodeExampleInfo> examples)
        {
            var exampleFullName = "Tests.CodeExamples.Advanced.CodeExample3";
            var example = Select(examples, exampleFullName);

            Assert.IsNotNull(example);
            CodeExampleInfoAssert.IsNotSuccessful(example);
            Assert.IsFalse(example.HasSourceCode);
            StringWriter writer = new StringWriter();
            writer.Write("CodeExample3:error");
            CodeExampleInfoAssert.HasError(
                example,
                writer.ToString());

            CodeExampleInfoAssert.HasNotOutputFile(example);
        }

        /// <summary>
        /// Tests a faulted code example, not defined 
        /// in the default namespace, whose source code
        /// can be found.
        /// </summary>
        public static void TestCodeExample4(List<CodeExampleInfo> examples)
        {
            var exampleFullName = "Tests.CodeExamples.Advanced.CodeExample4";
            var example = Select(examples, exampleFullName);

            Assert.IsNotNull(example);
            CodeExampleInfoAssert.IsNotSuccessful(example);
            Assert.IsTrue(example.HasSourceCode);
            StringWriter writer = new StringWriter();
            writer.Write("CodeExample4:error");
            CodeExampleInfoAssert.HasError(
                example,
                writer.ToString());

            CodeExampleInfoAssert.HasNotOutputFile(example);
        }


        /// <summary>
        /// Tests a successful code example, not defined 
        /// in the default namespace, whose source code
        /// can be found.
        /// </summary>
        public static void TestCodeExample5(List<CodeExampleInfo> examples)
        {
            var exampleFullName = "Tests.CodeExamples.Advanced.CodeExample5";
            var example = Select(examples, exampleFullName);

            Assert.IsNotNull(example);
            CodeExampleInfoAssert.IsSuccessful(example);
            Assert.IsTrue(example.HasSourceCode);
            StringWriter writer = new StringWriter();
            writer.WriteLine("CodeExample5:output");
            CodeExampleInfoAssert.HasOutput(example, writer.ToString());

            var commentSymbol = example.Language.CommentSymbol;

            writer = new StringWriter();
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace Tests.CodeExamples.Advanced");
            writer.WriteLine("{");
            writer.WriteLine("    public class CodeExample5  ");
            writer.WriteLine("    {");
            writer.WriteLine("        public void Main()");
            writer.WriteLine("        {");
            writer.WriteLine("            Console.WriteLine(\"CodeExample5:output\");");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine(commentSymbol +
                "Executing method Main() produces the following output:");
            writer.WriteLine(commentSymbol);
            writer.WriteLine(commentSymbol + "CodeExample5:output");
            CodeExampleInfoAssert.HasOutputFile(example, writer.ToString());
        }

        /// <summary>
        /// Tests a successful code example, defined 
        /// in the default namespace, whose source code
        /// cannot be found.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Source code files must be stored in a directory tree under a 
        /// user defined folder, the <see cref="CodeExamplesAnalyzer.CodeBase"/>, 
        /// reflecting the project namespace hierarchy.
        /// </para>
        /// <para>
        /// As a consequence, the source code for <see cref="CodeExample6"/> cannot 
        /// be found since the file is in not in the expected directory.
        /// </para>
        /// </remarks>
        public static void TestCodeExample6(List<CodeExampleInfo> examples)
        {
            var exampleFullName = "Tests.CodeExamples.CodeExample6";
            var example = Select(examples, exampleFullName);

            Assert.IsNotNull(example);
            CodeExampleInfoAssert.IsSuccessful(example);
            Assert.IsFalse(example.HasSourceCode);
            StringWriter writer = new StringWriter();
            writer.WriteLine("CodeExample6:output");
            CodeExampleInfoAssert.HasOutput(example, writer.ToString());

            CodeExampleInfoAssert.HasNotOutputFile(example);
        }

        /// <summary>
        /// Tests a faulted code example, defined 
        /// in the default namespace, whose source code
        /// cannot be found.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Source code files must be stored in a directory tree under a 
        /// user defined folder, the <see cref="CodeExamplesAnalyzer.CodeBase"/>, 
        /// reflecting the project namespace hierarchy.
        /// </para>
        /// <para>
        /// As a consequence, the source code for <see cref="CodeExample7"/> cannot 
        /// be found since the file is in not in the expected directory.
        /// </para>
        /// </remarks>
        public static void TestCodeExample7(List<CodeExampleInfo> examples)
        {
            var exampleFullName = "Tests.CodeExamples.CodeExample7";
            var example = Select(examples, exampleFullName);

            Assert.IsNotNull(example);
            CodeExampleInfoAssert.IsNotSuccessful(example);
            Assert.IsFalse(example.HasSourceCode);
            StringWriter writer = new StringWriter();
            writer.Write("CodeExample7:error");
            CodeExampleInfoAssert.HasError(
                example,
                writer.ToString());

            CodeExampleInfoAssert.HasNotOutputFile(example);
        }

        /// <summary>
        /// Tests a successful code example, defined 
        /// in the default namespace, whose source code
        /// can be found and no issues are detected
        /// from the analyzer.
        /// </summary>
        public static void TestCodeExample8(List<CodeExampleInfo> examples)
        {
            var exampleFullName = "NoIssues.Tests.CodeExamples.CodeExample8";
            var example = Select(examples, exampleFullName);

            Assert.IsNotNull(example);
            CodeExampleInfoAssert.IsSuccessful(example);
            Assert.IsTrue(example.HasSourceCode);
            StringWriter writer = new StringWriter();
            writer.WriteLine("CodeExample8:output");
            CodeExampleInfoAssert.HasOutput(example, writer.ToString());

            var commentSymbol = example.Language.CommentSymbol;

            writer = new StringWriter();
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace NoIssues.Tests.CodeExamples");
            writer.WriteLine("{");
            writer.WriteLine("    public class CodeExample8  ");
            writer.WriteLine("    {");
            writer.WriteLine("        public void Main()");
            writer.WriteLine("        {");
            writer.WriteLine("            Console.WriteLine(\"CodeExample8:output\");");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine(commentSymbol +
                "Executing method Main() produces the following output:");
            writer.WriteLine(commentSymbol);
            writer.WriteLine(commentSymbol + "CodeExample8:output");
            CodeExampleInfoAssert.HasOutputFile(example, writer.ToString());
        }

        /// <summary>
        /// Tests a successful code example, not defined 
        /// in the default namespace, whose source code
        /// can be found and no issues are detected
        /// from the analyzer.
        /// </summary>
        public static void TestCodeExample9(List<CodeExampleInfo> examples)
        {
            var exampleFullName = "NoIssues.Tests.CodeExamples.Advanced.CodeExample9";
            var example = Select(examples, exampleFullName);

            Assert.IsNotNull(example);
            CodeExampleInfoAssert.IsSuccessful(example);
            Assert.IsTrue(example.HasSourceCode);
            StringWriter writer = new StringWriter();
            writer.WriteLine("CodeExample9:output");
            CodeExampleInfoAssert.HasOutput(example, writer.ToString());

            var commentSymbol = example.Language.CommentSymbol;

            writer = new StringWriter();
            writer.WriteLine("using System;");
            writer.WriteLine();
            writer.WriteLine("namespace NoIssues.Tests.CodeExamples.Advanced");
            writer.WriteLine("{");
            writer.WriteLine("    public class CodeExample9  ");
            writer.WriteLine("    {");
            writer.WriteLine("        public void Main()");
            writer.WriteLine("        {");
            writer.WriteLine("            Console.WriteLine(\"CodeExample9:output\");");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine(commentSymbol +
                "Executing method Main() produces the following output:");
            writer.WriteLine(commentSymbol);
            writer.WriteLine(commentSymbol + "CodeExample9:output");
            CodeExampleInfoAssert.HasOutputFile(example, writer.ToString());
        }
    }
}
