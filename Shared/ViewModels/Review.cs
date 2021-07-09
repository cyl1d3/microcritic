using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using microcritic.Shared.Enums;

namespace microcritic.Shared.ViewModels
{
    public class Review
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public Rating Rating { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }
    }
}
