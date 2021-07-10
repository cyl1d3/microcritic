using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcritic.Shared.ViewModels
{
    public class Game
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Titel wird benötigt")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Entwickler wird benötigt")]
        public Developer Developer { get; set; }

        [Required(ErrorMessage = "Beschreibung wird benötigt")]
        public string Description { get; set; }

        public decimal? Score { get; set; }

        public int? ReviewCount { get; set; }
    }
}
