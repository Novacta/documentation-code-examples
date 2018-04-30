// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Novacta.Documentation.CodeExamples
{
    /// <summary>
    /// Analyzes code examples by executing exemplified code 
    /// and capturing output and error information.
    /// </summary>
    public class CodeExamplesAnalyzer
    {
        /// <summary>
        /// Gets or sets the default namespace of the 
        /// project which includes the example source code files.
        /// </summary>
        /// <value>
        /// The default namespace of the 
        /// project including the example source code files.
        /// </value>
        public string DefaultNamespace { get; private set; }

        /// <summary>
        /// Gets or sets the root path of the directory tree 
        /// where code example files are stored.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The root is expected to contain the files 
        /// corresponding to types in the default namespace, 
        /// together with additional subdirectories 
        /// reflecting the namespace hierarchy.
        /// </para>
        /// </remarks>
        /// <value>
        /// The root path of the directory tree 
        /// where the code example files are stored.
        /// </value>
        public string CodeBase { get; private set; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CodeExamplesAnalyzer" /> class with the specified
        /// default namespace and code base.
        /// </summary>
        /// <param name="codeBase">The root path of the directory tree 
        /// where code example files are stored.</param>
        /// <param name="defaultNamespace">The default namespace.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="defaultNamespace" /> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The directory specified in parameter <paramref name="codeBase"/> does
        /// not exist or an error occurs when trying to determine 
        /// if the specified path exists.
        /// </exception>
        public CodeExamplesAnalyzer(
            string codeBase,
            string defaultNamespace)
        {
            if (!Directory.Exists(codeBase))
            {
                throw new ArgumentException(
                    "The specified directory does not exist or an error occurred when " +
                    "trying to determine if the specified folder exists. ",
                    nameof(codeBase));
            }
            this.CodeBase = codeBase;
            this.DefaultNamespace = defaultNamespace ??
                throw new ArgumentNullException(nameof(defaultNamespace));
        }

        static string ICodeExampleFullName = "Novacta.Documentation.CodeExamples.ICodeExample";

        /// <summary>
        /// Runs the code examples defined in the calling assembly, 
        /// and writes to files their console outputs together with 
        /// the corresponding source code, if any.
        /// </summary>
        /// <param name="reportExecutionInfo">
        /// <c>true</c> if information about examples' execution must be reported;
        /// otherwise <c>false</c>.
        /// </param>
        /// <returns>
        /// A list of <see cref="CodeExampleInfo"/> instances containing information
        /// about the code examples and the results of their execution.
        /// </returns>
        public List<CodeExampleInfo> Run(bool reportExecutionInfo = true)
        {
            ConsoleColor currentBackground = Console.BackgroundColor;
            ConsoleColor currentForeground = Console.ForegroundColor;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            bool allExamplesSuccessful = true;

            var callingAssembly = Assembly.GetCallingAssembly();

            List<CodeExampleInfo> examples =
               this.GetCodeExamples(callingAssembly);

            int numberOfailedExamples = 0;
            int numberOfNotHavingSourceExamples = 0;

            foreach (var example in examples)
            {
                var exampleFullName = example.Type.FullName;

                if (reportExecutionInfo)
                    Console.WriteLine("{0}:", exampleFullName);

                CodeExamplesAnalyzer.Execute(example);

                if ((example.ExitCode == 0) && example.HasSourceCode)
                {
                    CodeExamplesAnalyzer.CreateExampleSourceAndOutputFile(example);
                }
                else
                {
                    allExamplesSuccessful = false;
                }

                if (example.HasSourceCode)
                {
                    if (reportExecutionInfo)
                    {
                        Console.WriteLine(
                            "   Source code: found.");
                    }
                }
                else
                {
                    numberOfNotHavingSourceExamples++;
                    if (reportExecutionInfo)
                    {
                        ConsoleColor color = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "   Source code: cannot be found.");
                        Console.ForegroundColor = color;
                    }
                }

                if (example.ExitCode == 0)
                {
                    if (reportExecutionInfo)
                    {
                        Console.WriteLine(
                            "   Execution: successful.");
                    }
                }
                else
                {
                    numberOfailedExamples++;
                    if (reportExecutionInfo)
                    {
                        ConsoleColor color = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "   Execution: failed. Exception: {0}", example.Error);
                        Console.ForegroundColor = color;
                    }
                }
            }

            if (!allExamplesSuccessful)
            {
                Environment.ExitCode = -1;
            }

            if (reportExecutionInfo)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Number of examples: {0}.", examples.Count);

                if (numberOfNotHavingSourceExamples > 0)
                {
                    ConsoleColor color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        "   Number of examples with no source code: {0}.",
                        numberOfNotHavingSourceExamples);
                    Console.ForegroundColor = color;
                }
                else
                {
                    Console.WriteLine(
                        "   Number of examples with no source code: 0.");
                }
                if (numberOfailedExamples > 0)
                {
                    ConsoleColor color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        "   Number of failed examples: {0}.",
                        numberOfailedExamples);
                    Console.ForegroundColor = color;
                }
                else
                {
                    Console.WriteLine(
                        "   Number of failed examples: 0.");
                }
            }

            Console.BackgroundColor = currentBackground;
            Console.ForegroundColor = currentForeground;

            return examples;
        }

        #region Collecting info about code examples

        /// <summary>
        /// Gets info about the code examples defined in the calling assembly.
        /// </summary>
        /// <param name="callingAssembly">The calling assembly.</param>
        /// <returns>The collection of examples defined in the calling assembly.</returns>
        private List<CodeExampleInfo> GetCodeExamples(Assembly callingAssembly)
        {
            var exportedTypes = callingAssembly.ExportedTypes;

            List<CodeExampleInfo> examples = new List<CodeExampleInfo>();

            Func<Type, object, bool> iExampleFilter =
                (interfaceType, fullName) =>
                {
                    return String.CompareOrdinal(
                        interfaceType.FullName, fullName.ToString()) == 0;
                };

            TypeFilter typeFilter = new TypeFilter(iExampleFilter);

            foreach (var type in exportedTypes)
            {
                // Check if the exported type derives from ICodeExample
                Type[] interfaces = type.FindInterfaces(
                    typeFilter,
                    ICodeExampleFullName);

                bool isExample = interfaces.Length > 0;

                // if so, use its full name to identify its source code file.
                if (isExample)
                {
                    if (type.FullName.StartsWith(this.DefaultNamespace))
                    {
                        this.GetSourceCodeInfo(
                            type,
                            out string path,
                            out ProgrammingLanguage language);

                        examples.Add(new CodeExampleInfo(type, path, language));
                    }
                }
            }

            return examples;
        }

        private static List<ProgrammingLanguage> SupportedLanguages =
            new List<ProgrammingLanguage>
            {
                new CSharpLanguage()
            };

        /// <summary>
        /// Gets the info about the source code file where the
        /// specified type is defined.
        /// </summary>
        /// <param name="exampleType">The example type.</param>
        /// <param name="path">The source code file, if it can be found;
        /// otherwise <b>null</b>.</param>
        /// <param name="language">The language in which the source code is
        /// written, if the source code file can be found; 
        /// otherwise <b>null</b>.</param>
        /// <remarks>
        /// <para>
        /// This method assumes the each type is defined in
        /// a C# or VB file named from the type name, and that source code
        /// files are stored in
        /// a directory tree, under the folder returned by 
        /// <see cref="CodeBase"/>,
        /// reflecting the project namespaces.
        /// </para>
        /// <para>
        /// It is also assumed that the default namespace
        /// matches the value returned by <see cref="DefaultNamespace"/>.
        /// </para>
        /// </remarks>
        private void GetSourceCodeInfo(
            Type exampleType,
            out string path,
            out ProgrammingLanguage language)
        {
            string codeBase = this.CodeBase;
            string defaultNamespace = this.DefaultNamespace;

            // fullName contains the namespace in which the type is defined.
            var fullName = exampleType.FullName;
            int length = defaultNamespace.Length;
            var nestedNamespaces = fullName.Substring(length + 1);
            var tokens = nestedNamespaces.Split('.');

            string basePath = codeBase;

            // Here if the type is not defined in the default namespace
            for (int i = 0; i < tokens.Length; i++)
            {
                basePath = Path.Combine(basePath, tokens[i]);
            }

            foreach (var supportedLanguage in SupportedLanguages)
            {
                string sourceCodePath = basePath + supportedLanguage.FileExtension;
                if (File.Exists(sourceCodePath))
                {
                    path = sourceCodePath;
                    language = supportedLanguage;
                    return;
                }
            }

            path = null;
            language = null;
        }

        #endregion

        #region Executing code examples and collecting console outputs

        /// <summary>
        /// Executes the specified example.
        /// </summary>
        /// <param name="codeExampleInfo">
        /// The info about the code example to be executed.
        /// </param>
        private static void Execute(
            CodeExampleInfo codeExampleInfo)
        {
            var obj = Activator.CreateInstance(codeExampleInfo.Type);
            StringWriter stringWriter = new StringWriter();
            var defaultOut = Console.Out;
            Console.SetOut(stringWriter);
            try
            {
                codeExampleInfo.Type.GetMethod("Main").Invoke(obj, null);
                codeExampleInfo.ExitCode = 0;
            }
            catch (Exception e)
            {
                codeExampleInfo.Error = e.InnerException.Message;
                codeExampleInfo.ExitCode = -1;
            }

            codeExampleInfo.Output = stringWriter.ToString();
            Console.SetOut(defaultOut);
        }

        /// <summary>
        /// Creates a file containing the source code and 
        /// the console output of the 
        /// specified example.
        /// </summary>
        /// <param name="codeExampleInfo">
        /// The info about the code example under study.
        /// </param>
        /// <remarks>
        /// <para>
        /// The is created in the same directory containing the source 
        /// code file, and its path is obtained by adding <c>".txt"</c> to 
        /// the file path.
        /// </para>
        /// </remarks>
        private static void CreateExampleSourceAndOutputFile(
            CodeExampleInfo codeExampleInfo)
        {
            string path = codeExampleInfo.SourceCodePath;

            List<string> lines = new List<string>();
            string sourceCode = null;
            using (StreamReader reader = new StreamReader(path))
            {
                sourceCode = reader.ReadToEnd();
            }

            sourceCode = codeExampleInfo.Language.RemoveICodeExampleReferences(sourceCode);

            string line;
            StringReader sourceCodeReader = new StringReader(sourceCode);
            while ((line = sourceCodeReader.ReadLine()) != null)
            {
                lines.Add(line);
            }
            string commentSymbol = codeExampleInfo.Language.CommentSymbol;
            StringReader stringReader = new StringReader(codeExampleInfo.Output);

            using (StreamWriter writer =
                new StreamWriter(path + ".txt", false, new UTF8Encoding(false)))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    writer.WriteLine(lines[i]);
                }
                writer.WriteLine();
                writer.WriteLine(commentSymbol +
                    "Executing method Main() produces the following output:");
                writer.WriteLine(commentSymbol);
                while (true)
                {
                    line = stringReader.ReadLine();
                    if (line != null)
                    {
                        writer.WriteLine(commentSymbol + line);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        #endregion
    }
}
