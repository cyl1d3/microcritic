using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcritic.Shared.ViewModels
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Developer { get; set; }

        public decimal Score { get; set; }

        public string Description { get; set; }
    }
}
