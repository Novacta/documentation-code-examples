using System;

namespace SampleClassLibrary
{
    /// <summary>
    /// Provides a method to operate on integers.
    /// </summary>
    public static class IntegerOperation
    {
        /// <summary>
        /// Applies the specified function to the given operand.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <param name="operand">The operand.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func"/> is <b>null</b>.</exception>
        /// <example>
        /// <para>
        /// In the following example, an integer is squared 
        /// executing the <see cref="Operate(Func{int, int}, int)"/> method.
        /// In addition, input validation is also checked.
        /// </para>
        /// <para>
        /// <code 
        /// language="cs" 
        /// source="..\SampleClassLibrary.CodeExamples\IntegerOperationExample.cs.txt"/>
        /// </para>
        /// </example>
        public static int Operate(Func<int, int> func, int operand)
        {
            if (func==null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return func(operand);
        }
    }
}
