using System;
using System.ComponentModel.DataAnnotations;

using microcritic.Shared.Enums;

namespace microcritic.Shared.ViewModels
{
    public class Review
    {
        public Guid Id { get; set; }

        public Guid Game { get; set; }

        public string UserName { get; set; }

        [Required(ErrorMessage = "Bewertung wird benötigt")]
        public Rating Rating { get; set; }

        [Required(ErrorMessage = "Text wird benötigt")]
        public string Text { get; set; }

        public DateTime Date { get; set; }
        public string DateString { get; set; }
    }
}
