using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class User : IdentityUser
    {
        public Cart Cart { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}