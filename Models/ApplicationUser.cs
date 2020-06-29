using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObservrProgrammingTest.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NewYorkBorough { get; set; }
    }
}
