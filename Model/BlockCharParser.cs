using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPT.Parsers.Model
{
    public class BlockCharParser : IBlockParser
    {
        private bool _useDelimiter = false;

        public char Delimiter { get; private set; }
        public char OpenTag { get; private set; }
        public char CloseTag { get; private set; }

        public string Log { get; private set; }

        public BlockCharParser(char openTag, char closeTag)
        {
            Initialize(openTag, closeTag);
        }

        public BlockCharParser(char openTag, char closeTag, char delimiter)
        {
            Delimiter = delimiter;
            _useDelimiter = true;

            Initialize(openTag, closeTag);
        }

        /// <summary>
        /// Returns a list of strings as items, split by delimiters that lie outside of block indicators.
        /// Delimiters that lie within block indicators are ignored.
        /// Child block tags are left in the string items.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<string> Parse(string text)
        {
            // Clean text
            text = text.Trim();
            if (BoundedByTags(text))
            {
                text = text.Remove(0, 1);
                text = text.Remove(text.Length - 1, 1);
            }

            string currentText = "";
            List<string> currentTextItems = new List<string>();
            int bracketCount = 0;
            foreach (char c in text)
            {
                // Get bracket balance
                if (c == OpenTag)
                {
                    bracketCount++;
                }
                else if (c == CloseTag)
                {
                    bracketCount--;
                }

                // Separate string and skip delimiter only if outside of closed brackets
                if (DelimitString(c, bracketCount, currentText))
                {
                    currentTextItems.Add(currentText);
                    currentText = "";
                }
                else
                {
                    currentText += c;
                }
            }
            currentTextItems.Add(currentText);

            return currentTextItems;
        }

        /// <summary>
        /// Validates that there are an equal number of opening and closing tags.
        /// If not, more detail is logged to the class log.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool ValidateBalancedTags(string text)
        {
            int openTagNum = CountOccurrence(text, OpenTag.ToString());
            int closeTagNum = CountOccurrence(text, CloseTag.ToString());
            if (openTagNum != closeTagNum)
            {
                Log = string.Format("Not all tags in the string are closed. \nOpen: {0} Closed: {1} \n{2}", openTagNum, closeTagNum, text);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// True: Entire text is bounded by an outermost opening/closing tag set. 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool BoundedByTags(string text)
        {
            if (text.First() == OpenTag &&
                text.Last() == CloseTag)
            {
                // Bounded by tags if first closure does not occur until the end
                int bracketCount = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    // Get bracket balance
                    if (c == OpenTag)
                    {
                        bracketCount++;
                    }
                    else if (c == CloseTag)
                    {
                        bracketCount--;
                        if (bracketCount == 0)
                        {
                            return (i == text.Length - 1);
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Counts the number of blocks in the text, based on the occurrence of balanced tag closures.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int CountBlocks(string text)
        {
            int bracketCount = 0;
            int blockCount = 0;
            foreach (char c in text)
            {
                // Get bracket balance
                if (c == OpenTag)
                {
                    bracketCount++;
                }
                else if (c == CloseTag)
                {
                    bracketCount--;
                    if (bracketCount == 0)
                    {
                        blockCount++;
                    }
                }
            }

            return blockCount;
        }

        /// <summary>
        /// Number of times a delimiter occurs in the string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public int CountOccurrence(string text, string delimiter)
        {
            return text.Length - text.Replace(delimiter, "").Length;
        }


        private bool DelimitString(char c, int bracketCount, string currentText)
        {
            return (_useDelimiter &&
                    (c == Delimiter) &&
                    (bracketCount == 0) &&
                    !string.IsNullOrEmpty(currentText));
        }

        private void Initialize(char openTag, char closeTag)
        {
            OpenTag = openTag;
            CloseTag = closeTag;
        }
    }
}
