using GymManagement.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.DataSessding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedIdentityDataAsync(UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            , ILogger logger,

            CancellationToken ct = default)
        {

            try
            {
                bool hasUser = await userManager.Users.AnyAsync(ct);
                bool hasrole = await roleManager.Roles.AnyAsync(ct);
                if (hasrole && hasUser)

                {
                    logger.LogInformation("Data Already Seeded");
                    return;
                }


                var roles = new List<IdentityRole>()
            {

                new IdentityRole(){Name="SuperAdmin" },
                new IdentityRole(){Name="Admin" }

            };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name!))
                    {
                        var roleResult = await roleManager.CreateAsync(role);
                        if (!roleResult.Succeeded)
                        {
                            logger.LogError($"Failed To Create Role  {role.Name} : {string.Join(";", roleResult.Errors.Select(x => x.Description))} ");

                        }

                    }


                }



                if (!hasUser)
                {

                    var mainAdmin = new ApplicationUser()
                    {
                        FirstName = "Youssef",
                        LastName = "Said",
                        Email = "youssef@gmail.com",
                        PhoneNumber = "01207733737",
                        UserName = "YoussefSaid"


                    };

                    await userManager.CreateAsync(mainAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(mainAdmin, "SuperAdmin");

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Sara",
                        LastName = "Said",
                        Email = "Sara@gmail.com",
                        PhoneNumber = "01107733737",
                        UserName = "SaraSaid"


                    };

                    await userManager.CreateAsync(Admin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(Admin, "Admin");

                    logger.LogInformation("Identity Data Seeded");

                }


                return;

            }
            catch (Exception ex )
            {
                logger.LogError(ex,"Ideninty Seeding Fialed");
                return;
            
            }


            }


    }
}
