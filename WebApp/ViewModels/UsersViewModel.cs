using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApp.ViewModels
{
    public class UsersViewModel
    {
        public IEnumerable<IdentityUser> Users { get; set; }
    }
}
