using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MPT.Parsers.Model;

namespace MPT.Parsers.Tests
{
    public class ParserTests
    {
        public static void Run()
        {
            IBlockParser parser = new BlockCharParser('(', ')', ' ');
            string input;

            Console.WriteLine("Test 1: Invalid Input");
            input = "(var( op var) operator (var op (op var))";
            Run(input, parser);

            Console.WriteLine();
            Console.WriteLine("Test 2: Valid Input");
            input = "(var op var) operator (var op (op var)) where var = val, var = val";
            Run(input, parser);
        }

        public static void Run(string input, IBlockParser parser)
        {
            if (parser.ValidateBalancedTags(input))
            {
                Block expression = new Block(input, parser);
                Console.WriteLine(expression);
            }
            else
            {
                Console.WriteLine(parser.Log);
            }
        }
    }
}
