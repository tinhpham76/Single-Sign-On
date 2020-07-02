using Microsoft.AspNetCore.Identity;
using SSO.Backend.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string AdminRoleName = "Admin";
        private readonly string UserRoleName = "Member";

        public DbInitializer(ApplicationDbContext context,
          UserManager<User> userManager,
          RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //Seed data if data on database is null
        public async Task Seed()
        {
            #region Role

            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = AdminRoleName,
                    Name = AdminRoleName,
                    NormalizedName = AdminRoleName.ToUpper(),
                });
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = UserRoleName,
                    Name = UserRoleName,
                    NormalizedName = UserRoleName.ToUpper(),
                });
            }

            #endregion

            #region User

            if (!_userManager.Users.Any())
            {
                var result1 = await _userManager.CreateAsync(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    FirstName = "Admin",
                    LastName = "",
                    Email = "admin@admin.com",
                    LockoutEnabled = false
                }, "Admin@123");
                if (result1.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("admin");
                    await _userManager.AddToRoleAsync(user, AdminRoleName);
                }
                var result2 = await _userManager.CreateAsync(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "xdxg",
                    FirstName = "Tịnh",
                    LastName = "Phạm Văn",
                    Email = "tinh_pham@outlook.com",
                    LockoutEnabled = false
                }, "!kAa36qDc");
                if (result2.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("xdxg");
                    await _userManager.AddToRoleAsync(user, UserRoleName);
                }
            }

            #endregion
            await _context.SaveChangesAsync();
        }
    }
}