using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.DAL.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }
}
