using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace microcritic.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Review > Reviews { get; set; }
        
    }
}
