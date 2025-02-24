using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entites.Identity;

namespace Talbat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Omnia Osama",
                    Email = "anaomnia@gmail.com",
                    UserName = "omniaosama",
                    PhoneNumber = "01124974351"
                };
                await userManager.CreateAsync(User, "P@ssw0rd");
            }
        }
    }
}
