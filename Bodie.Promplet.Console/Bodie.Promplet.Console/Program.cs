

using Bodie.Promplet.Console.Models;
using Bodie.Promplet.Console.Parsing;
using System.Text;

var loadedTemplates = new JsonFileParserForType<Template>(Environment.CurrentDirectory.ToString() + @"\DataFiles\templates.json").ParseFile();

var selectedTemplateId = 0;
var codesnippet = "some test code";
var header = "a random header";
var footer = "more random footer";

if (loadedTemplates == null || loadedTemplates.Count()==0)
{
    Console.ForegroundColor = ConsoleColor.DarkRed;

    Console.WriteLine("Sorry, could not load templates.");
}
else
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\tAvailable Templates:");
    foreach (var tmp in loadedTemplates)
    {
        Console.WriteLine($"\t\t({tmp.TemplateId}){tmp.TemplateName}");
    }
   
    Console.WriteLine("");
    Console.Write("\tEnter the Id of the Template you want to use:\t");
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
        if (Int32.TryParse(Console.ReadLine(), out var selectedId))
        {
            selectedTemplateId = selectedId;
        }
    Console.WriteLine("");
    var template = loadedTemplates.Where(template => template.TemplateId.Equals(selectedTemplateId)).FirstOrDefault();

        Console.ForegroundColor = ConsoleColor.DarkYellow;

        Console.Write("\tAdd a header or press enter to continue:\t");

        Console.ForegroundColor = ConsoleColor.Yellow;

        header = (Console.ReadLine());

        Console.ForegroundColor = ConsoleColor.DarkYellow;

        Console.Write("\tAdd a footer or press enter to continue:\t");

        Console.ForegroundColor = ConsoleColor.Yellow;

        footer = (Console.ReadLine());

        var replaceDictionary = new Dictionary<string, string>();

        var body = new StringBuilder();

            foreach (var item in template.TemplateAssets.OrderBy(t => t.AssetOrder))
            {
                body.AppendLine(item.AssetContent);
            }

            var bufferBuilder = new StringBuilder();
            bufferBuilder.AppendLine(header);
            bufferBuilder.AppendLine(body.ToString());
            bufferBuilder.AppendLine(footer);

            var parsedVariables = ParsingUtils.ExtractVariableDictionary(bufferBuilder.ToString(), '[', ']');

            if (parsedVariables.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("");
                Console.WriteLine("\t * * * * * * * * * * * * * * * * * *");
                Console.WriteLine("\tVariables were found in the selected Template, you will be prompted for the values.");
            }
            foreach (var variable in parsedVariables)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"\tEnter a value for {variable.Key}:\t");
                Console.ForegroundColor = ConsoleColor.White;
                var val = (Console.ReadLine());
                replaceDictionary.Add(variable.Key, val);
            }

            var promptParts = new PromptParts(header, body.ToString(), footer, replaceDictionary);

            var raw = ParsingUtils.AsseembleRawPromptFromParts(promptParts);

            Console.WriteLine("Generated Prompt:");

            var x = ParsingUtils.AssemblePrompt(promptParts);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(x);
}


Console.ReadLine();




