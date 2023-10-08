using Bodie.Promplet.Console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bodie.Promplet.Console.Parsing
{
    internal static class ParsingUtils
    {
        private const string CodeSnippetKey = "[code snippet]";
        private const string CodeSnippetPlaceHolderKey = "[aquafartbubble]";
        private const int MaxVariableLength = 120;

        public static string AssemblePrompt(PromptParts promptParts)
        {
            var bufferBuilder = new StringBuilder();
            bufferBuilder.AppendLine(promptParts.Header);
            bufferBuilder.AppendLine(promptParts.Body);
            bufferBuilder.AppendLine(CodeSnippetPlaceHolderKey);
            bufferBuilder.AppendLine(promptParts.Footer);

            var multiSnippet = bufferBuilder.ToString().IndexOf(CodeSnippetKey, StringComparison.Ordinal) < bufferBuilder.ToString().LastIndexOf(CodeSnippetKey, StringComparison.Ordinal);

            if (multiSnippet)
            {
                bufferBuilder.Replace(CodeSnippetKey, "");
                bufferBuilder.Replace(CodeSnippetPlaceHolderKey, promptParts.ReplacementPairs[CodeSnippetKey]);
            }
            else
            {
                bufferBuilder.Replace(CodeSnippetPlaceHolderKey, "");
            }

            foreach (var replacement in promptParts.ReplacementPairs)
            {
                bufferBuilder.Replace(replacement.Key, replacement.Value);
            }
            return bufferBuilder.ToString();
        }

        public static string AsseembleRawPromptFromParts(PromptParts promptParts)
        {
            var bufferBuilder = new StringBuilder();
            bufferBuilder.AppendLine(promptParts.Header);
            bufferBuilder.AppendLine(promptParts.Body);
            bufferBuilder.AppendLine(promptParts.Footer);
            return bufferBuilder.ToString();
        }
        public static Dictionary<string, Tuple<int, int>> ParseElementsFromParts(PromptParts promptParts, char startDelim, char endDelim)
        {
            return ExtractVariableNamesAndOrdinalAndCoordinates(AsseembleRawPromptFromParts(promptParts), startDelim, endDelim);
        }

        public static Dictionary<string, string> ExtractVariableDictionary(PromptParts promptParts, char startDelim, char endDelim)
        {
            return ExtractVariableDictionary(AsseembleRawPromptFromParts(promptParts), startDelim, endDelim);
        }

        public static Dictionary<string, Tuple<int, int>> ExtractVariableNamesAndOrdinalAndCoordinates(string input, char startDelim, char endDelim)
        {
            var result = new Dictionary<string, Tuple<int, int>>();
            var inputArray = input.ToCharArray();
            var valueBuilder = new StringBuilder(MaxVariableLength);
            var foundCounter = 0;

            for (int pos = 0; pos <= inputArray.Length - 1; pos++)
            {
                if (inputArray[pos] == startDelim)
                {
                    valueBuilder.Clear();

                    for (int walk = pos; walk <= pos + MaxVariableLength; walk++)
                    {
                        valueBuilder.Append(inputArray[walk]);

                        if (inputArray[walk] == endDelim)
                        {
                            foundCounter += 1;
                            string? valueHolding = valueBuilder.ToString();
                            valueBuilder.Clear();
                            var coordinateTuple = new Tuple<int, int>(pos, walk);
                            result.Add($"{valueHolding}[{foundCounter}]", coordinateTuple);
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public static Dictionary<string, string> ExtractVariableDictionary(string input, char startDelim, char endDelim)
        {
            var result = new Dictionary<string, string>();
            var inputArray = input.ToCharArray();
            var valueBuilder = new StringBuilder(MaxVariableLength);
            for (int pos = 0; pos <= inputArray.Length - 1; pos++)
            {
                if (inputArray[pos] == startDelim)
                {
                    valueBuilder.Clear();

                    for (int walk = pos; walk <= pos + MaxVariableLength && walk < inputArray.Length - 1; walk++)
                    {
                        valueBuilder.Append(inputArray[walk]);

                        if (inputArray[walk] == endDelim)
                        {
                            result.TryAdd(valueBuilder.ToString(), "");
                            valueBuilder.Clear();
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}
