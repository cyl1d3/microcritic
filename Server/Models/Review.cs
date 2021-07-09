﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using microcritic.Shared.Enums;

namespace microcritic.Server.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public Rating Rating { get; set; }

        [Required]
        public Game Game { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Text { get; set; }

    }
}
