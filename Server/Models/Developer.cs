﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace microcritic.Server.Models
{
    public class Developer
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Game> Games { get; set; }
    }
}