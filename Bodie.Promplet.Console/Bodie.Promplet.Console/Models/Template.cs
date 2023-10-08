namespace Bodie.Promplet.Console.Models 
{ 
    public class Template
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public List<Asset> TemplateAssets { get; set; }
    }
}
