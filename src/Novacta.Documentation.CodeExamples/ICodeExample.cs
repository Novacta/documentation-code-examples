// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.
namespace Novacta.Documentation.CodeExamples
{
    /// <summary>
    /// Represents a code example.
    /// </summary>
    public interface ICodeExample
    {
        /// <summary>
        /// The method encapsulating the code to be exemplified.
        /// </summary>
        void Main();
    }
}
