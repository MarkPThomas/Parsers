using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPT.Parsers.Model
{
    /// <summary>
    /// Parses text into a list of text items, and child block hierarchies.
    /// Text items are based upon provided delimiters.
    /// Blocks are based on and opening/closing tags.
    /// </summary>
    public interface IBlockParser
    {
        /// <summary>
        /// Logs any relevant information from an operation, such as errors that don't throw exceptions.
        /// </summary>
        string Log { get; }

        /// <summary>
        /// Returns a list of strings as items, split by delimiters that lie outside of block indicators.
        /// Delimiters that lie within block indicators are ignored.
        /// Child block tags are left in the string items.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        List<string> Parse(string text);

        /// <summary>
        /// Validates that there are an equal number of opening and closing tags.
        /// If not, more detail is logged to the class log.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        bool ValidateBalancedTags(string text);

        /// <summary>
        /// True: Entire text is bounded by an outermost opening/closing tag set. 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        bool BoundedByTags(string text);

        /// <summary>
        /// Counts the number of blocks in the text, based on the occurrence of balanced tag closures.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        int CountBlocks(string text);

        /// <summary>
        /// Number of times a delimiter occurs in the string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        int CountOccurrence(string text, string delimiter);
    }
}
