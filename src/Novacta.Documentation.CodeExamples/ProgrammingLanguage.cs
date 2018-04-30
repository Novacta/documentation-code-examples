// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
namespace Novacta.Documentation.CodeExamples
{
    /// <summary>
    /// Represents a programming language.
    /// </summary>
    public abstract class ProgrammingLanguage
    {
        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>The file extension.</value>
        public string FileExtension { get; protected set; }

        /// <summary>
        /// Gets or sets the comment symbol.
        /// </summary>
        /// <value>The comment symbol.</value>
        public string CommentSymbol { get; protected set; }

        /// <summary>
        /// Removes <see cref="ICodeExample"/> references from the 
        /// specified source code.
        /// </summary>
        /// <param name="code">The code of the example where 
        /// the references must be removed.</param>
        /// <returns>A string representing the source of the 
        /// code example with the references to 
        /// <see cref="ICodeExample"/> removed.</returns>
        public abstract string RemoveICodeExampleReferences(string code);
    }
}
