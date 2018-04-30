// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace Novacta.Documentation.CodeExamples.Tests.Tools
{
    static class CodeExampleInfoAssert
    {
        public static void HasExitCode(
            CodeExampleInfo codeExampleInfo,
            int expected)
        {
            Assert.AreEqual(expected: expected, actual: codeExampleInfo.ExitCode);
        }

        public static void HasNotExitCode(
            CodeExampleInfo codeExampleInfo,
            int notExpected)
        {
            Assert.AreNotEqual(notExpected: notExpected, actual: codeExampleInfo.ExitCode);
        }

        public static void IsNotSuccessful(
            CodeExampleInfo codeExampleInfo)
        {
            Assert.IsNotNull(codeExampleInfo.Error);
            HasNotExitCode(codeExampleInfo, notExpected: 0);
        }

        public static void IsSuccessful(
    CodeExampleInfo codeExampleInfo)
        {
            Assert.IsNull(codeExampleInfo.Error);
            HasExitCode(codeExampleInfo, expected: 0);
        }

        public static void HasError(
            CodeExampleInfo codeExampleInfo,
            string expected)
        {
            Assert.IsNotNull(codeExampleInfo.Error);
            Assert.AreNotEqual(notExpected: 0, actual: codeExampleInfo.ExitCode);
            Assert.AreEqual(expected: expected, actual: codeExampleInfo.Error);
        }

        public static void HasOutput(
            CodeExampleInfo codeExampleInfo,
            string expected)
        {
            Assert.AreEqual(expected: expected, actual: codeExampleInfo.Output);
        }

        public static void HasOutputFile(
            CodeExampleInfo codeExampleInfo,
            string expected)
        {
            HasExitCode(codeExampleInfo, 0);
            Assert.IsTrue(codeExampleInfo.HasSourceCode);
            var actualPath = codeExampleInfo.SourceCodePath + ".txt";
            Assert.IsTrue(File.Exists(actualPath));

            StringReader expectedReader = new StringReader(expected);

            using (StreamReader reader = new StreamReader(
                actualPath, new UTF8Encoding(false)))
            {
                string expectedLine;
                while ((expectedLine = expectedReader.ReadLine()) != null)
                {
                    var actualLine = reader.ReadLine();
                    Assert.AreEqual(expectedLine, actualLine);
                }
            }
        }

        public static void HasNotOutputFile(
            CodeExampleInfo codeExampleInfo)
        {
            var actualPath = codeExampleInfo.SourceCodePath + ".txt";
            Assert.IsTrue(!File.Exists(actualPath));
        }
    }
}
