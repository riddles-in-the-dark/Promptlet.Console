namespace Bodie.Promplet.Console.Models
{
    public record PromptParts
    {
        public PromptParts(string? header, string? body, string? footer, Dictionary<string, string> replacementPairs)
        {
            Header = header;
            Body = body;
            Footer = footer;
            ReplacementPairs = replacementPairs;
        }

        public string? Body { get; set; }
        public string? Header { get; set; }
        public string? Footer { get; set; }
        public Dictionary<string, string> ReplacementPairs { get; set; }
    }
}
