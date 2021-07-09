using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace microcritic.Server.Models
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Developer Developer { get; set; }

        public string Description { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
