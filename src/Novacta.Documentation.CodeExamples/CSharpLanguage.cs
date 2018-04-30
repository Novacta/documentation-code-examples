// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
using System;
using System.Text.RegularExpressions;

namespace Novacta.Documentation.CodeExamples
{
    /// <summary>
    /// Represents the C# programming language.
    /// </summary>
    /// <seealso cref="Novacta.Documentation.CodeExamples.ProgrammingLanguage" />
    public class CSharpLanguage : ProgrammingLanguage
    {
        static Regex ICodeExampleRegex = new Regex(
            @":[\t\r\n\s.\w]*ICodeExample");
        static Regex UsingRegex = new Regex(
            @"using[\t\r\n\s]*Novacta.Documentation.CodeExamples[\t\r\n\s]*;[\t\r\n\s]*");
        static Regex MainRegex = new Regex(
            @"public[\t\r\n\s]*void[\t\r\n\s]*Main[\t\r\n\s]*[(][\t\r\n\s]*[)]");

        static readonly string MainPrototype = "public void Main()";
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="CSharpLanguage"/> class.
        /// </summary>
        public CSharpLanguage()
        {
            this.CommentSymbol = "// ";
            this.FileExtension = ".cs";
        }

        /// <inheritdoc/>
        public override string RemoveICodeExampleReferences(string code)
        {
            code = ICodeExampleRegex.Replace(code, " ");
            code = UsingRegex.Replace(code, String.Empty);
            code = MainRegex.Replace(code, MainPrototype);

            return code;
        }
    }
}
