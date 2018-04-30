// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using System;

namespace Novacta.Documentation.CodeExamples
{
    /// <summary>
    /// Provides information about a source code example.
    /// </summary>
    public class CodeExampleInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeExampleInfo"/> class.
        /// </summary>
        /// <param name="type">The type of the example.</param>
        /// <param name="sourceCodePath">The source code path where 
        /// the example type is defined.</param>
        /// <param name="language">The programming language in which the example 
        /// has been coded.</param>
        public CodeExampleInfo(
            Type type,
            string sourceCodePath,
            ProgrammingLanguage language)
        {
            this.Type = type;
            this.SourceCodePath = sourceCodePath;
            this.Language = language;
        }

        /// <summary>
        /// Gets or sets the exit code of 
        /// the example.
        /// </summary>
        /// <value>The exit code of the example.</value>
        /// <remarks>
        /// <para>
        /// This property returns <code>0</code> in case of a 
        /// successfully execution; otherwise <code>-1</code>.
        /// </para>
        /// </remarks>
        public int ExitCode { get; set; }

        /// <summary>
        /// Gets or sets the textual output of 
        /// the example.
        /// </summary>
        /// <value>The textual output of the example, if any;
        /// otherwise the <see cref="String.Empty"/> string.</value>
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets the error output of 
        /// the example.
        /// </summary>
        /// <value>The error output of the example, if any;
        /// otherwise <b>null</b>.</value>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the type of 
        /// the example.
        /// </summary>
        /// <value>The type of the example.</value>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the path of the source code file where 
        /// the example type is defined.
        /// </summary>
        /// <remarks>
        /// <para>
        /// An example type is expected to be defined  
        /// in a source code file named from the class name.
        /// </para>
        /// </remarks>
        /// <value>The path to the example source code file, if any;
        /// otherwise <b>null</b>.</value>
        public string SourceCodePath { get; set; }

        /// <summary>
        /// Gets a value signaling if 
        /// the example has a source code path.
        /// </summary>
        /// <value><c>true</c> if any source code file has been found;
        /// otherwise <c>false</c>.</value>
        public bool HasSourceCode {
            get {
                return !(this.SourceCodePath is null);
            }
        }

        /// <summary>
        /// Gets or sets the programming language in which the example 
        /// has been coded.
        /// </summary>
        /// <value>The programming language in which the example 
        /// has been coded, if any source code file has been found;
        /// otherwise <b>null</b>.</value>
        public ProgrammingLanguage Language { get; set; }
    }
}
