using System;
using Novacta.Documentation.CodeExamples;

namespace SampleClassLibrary.CodeExamples
{
    /// <summary>
    /// An example showing how to exploit class <see cref="IntegerOperation"/>.
    /// </summary>
    public class IntegerOperationExample : ICodeExample
    {
        /// <summary>
        /// The method encapsulating the code to be exemplified.
        /// </summary>
        public void Main()
        {
            // Define an operator that squares its operand
            Func<int, int> square = (int operand) => operand * operand;

            // Define an operand
            int integer = 2;

            // Operate on it
            Console.WriteLine("Squaring {0}...", integer);
            int result = IntegerOperation.Operate(square, integer);
            Console.WriteLine("...the result is {0}.", result);

            // Check that an operator cannot be null
            try
            {
                IntegerOperation.Operate(null, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine("Cannot apply a null function:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
