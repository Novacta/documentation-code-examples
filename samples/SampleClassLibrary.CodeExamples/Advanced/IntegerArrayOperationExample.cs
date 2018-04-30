using System;
using Novacta.Documentation.CodeExamples;
using SampleClassLibrary.Advanced;

namespace SampleClassLibrary.CodeExamples.Advanced
{
    /// <summary>
    /// An example showing how to exploit class <see cref="IntegerArrayOperation"/>.
    /// </summary>
    public class IntegerArrayOperationExample : ICodeExample
    {
        /// <summary>
        /// The method encapsulating the code to be exemplified.
        /// </summary>
        public void Main()
        {
            // Define an operator that squares its operand
            Func<int, int> square = (int operand) => operand * operand;

            // Define an array of operands
            int[] operands = new int[3] { 2, 4, 8 };

            // Operate on it
            int[] results = IntegerArrayOperation.Operate(square, operands);

            // Show results
            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine(
                    "The result of squaring {0} is {1}.",
                    operands[i],
                    results[i]);
            }
           
            // Check that an operator cannot be null
            try
            {
                IntegerArrayOperation.Operate(null, new int[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine("Cannot apply a null function:");
                Console.WriteLine(e.Message);
            }

            // Check that an array of operands cannot be null
            try
            {
                IntegerArrayOperation.Operate(square, null);
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine("Cannot apply a function to a null array:");
                Console.WriteLine(e.Message);
            }

        }
    }
}
