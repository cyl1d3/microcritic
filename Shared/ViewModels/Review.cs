using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using microcritic.Shared.Enums;

namespace microcritic.Shared.ViewModels
{
    public class Review
    {
        public Guid Id { get; set; }

        public Guid Game { get; set; }

        public string UserName { get; set; }

        [Required]
        public Rating Rating { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime Date { get; set; }
        public string DateString { get; set; }
    }
}
