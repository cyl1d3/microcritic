using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace microcritic.Server.Models
{
    public enum Rating
    {
        Bad = 0,
        Poor = 1,
        Disappointing = 2,
        Average = 3,
        Great = 4,
        Perfect = 5
    }

    public class Review
    {
        [Key]
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public Rating Rating { get; set; }
        public string Text { get; set; }

    }
}
