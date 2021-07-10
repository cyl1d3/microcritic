using CsvHelper.Configuration.Attributes;

namespace microcritic.Shared.ViewModels
{
    public class CsvGame
    {
        [Name("Name")]
        public string Name { get; set; }

        [Name("Hersteller")]
        public string Developer { get; set; }

        [Name("Beschreibung")]
        public string Description { get; set; }
    }
}
