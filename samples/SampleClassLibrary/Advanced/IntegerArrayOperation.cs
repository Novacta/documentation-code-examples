using System;

namespace SampleClassLibrary.Advanced
{
    /// <summary>
    /// Provides a method to operate on arrays of integers.
    /// </summary>
    public static class IntegerArrayOperation
    {
        /// <summary>
        /// Applies the specified function to the given array of operands.
        /// </summary>
        /// <param name="func">The function to evaluate at each operand.</param>
        /// <param name="operands">The array of operands.</param>
        /// <returns>The results of the operations.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func"/> is <b>null</b>.<br/>
        /// -or-<br/>
        /// <paramref name="operands"/> is <b>null</b>.
        /// </exception>
        /// <example>
        /// <para>
        /// In the following example, integers in a given array are squared 
        /// executing the <see cref="Operate(Func{int, int}, int[])"/> method.
        /// In addition, input validation is also checked.
        /// </para>
        /// <para>
        /// <code 
        /// language="cs" 
        /// source="..\SampleClassLibrary.CodeExamples\Advanced\IntegerArrayOperationExample.cs.txt"/>
        /// </para>
        /// </example>
        public static int[] Operate(Func<int, int> func, int[] operands)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (operands == null)
            {
                throw new ArgumentNullException(nameof(operands));
            }

            int[] result = new int[operands.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = IntegerOperation.Operate(func, operands[i]);
            }
            return result;
        }
    }

}
