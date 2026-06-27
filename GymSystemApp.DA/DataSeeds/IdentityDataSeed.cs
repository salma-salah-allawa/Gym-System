using GymSystemApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.DAL.DataSeeds
{
    public static class IdentityDataSeed
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager,ILogger logger,CancellationToken ct = default)
        {
            try
            {
                bool HasRoles = roleManager.Roles.Any();
                bool HasUsers = userManager.Users.Any();
                if (HasRoles && HasUsers) return;

                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new IdentityRole{Name="SuperAdmin"},
                        new IdentityRole{Name="Admin"}
                    };
                    foreach (var roleName in Roles.Select(r=>r.Name))
                    {
                        if(!await roleManager.RoleExistsAsync(roleName))
                        {
                            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                            if (!roleResult.Succeeded)
                            {
                                logger.LogError("Feild to create roles.");
                            }
                        }
                    }
                    logger.LogInformation("Roles created and seeded successfully.");
                }

                if (!HasUsers)
                {
                    var MainUsers = new ApplicationUser()
                    {
                        FirstName = "Salma",
                        LastName ="Salah",
                        UserName="SalmaS",
                        Email="salma.salah182001@gmail.com",
                        PhoneNumber="01025109119"
                    };
                   var userResult = await userManager.CreateAsync(MainUsers,"P@ssw0rd");
                    await userManager.AddToRoleAsync(MainUsers, "SuperAdmin");
                    if (!userResult.Succeeded)
                    {
                        logger.LogError("Failed to create user.");
                        return;
                    }
                    logger.LogInformation("User created and seeded successfully.");
                }
                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Field to seed identity data.");
                throw;
            }
        }
    }
}
