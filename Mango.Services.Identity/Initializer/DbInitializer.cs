using IdentityModel;
using Mango.Services.Identity.DbContext;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Mango.Services.Identity.Initializer
{
    public class DbInitializer:IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public void Initialize()
        {
            if(_roleManager.FindByNameAsync(SD.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }    
            else
            {
                return;
            }

            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = "sureshnakum856@gmail.com",
                Email= "sureshnakum856@gmail.com",
                EmailConfirmed=true,
                PhoneNumber = "8561956637",
                FirstName = "Suresh",
                LastName = "Nakum"
            };
            _userManager.CreateAsync(applicationUser,"Suresh@123").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(applicationUser,SD.Admin).GetAwaiter().GetResult();
            var temp1 = _userManager.AddClaimsAsync(applicationUser,new Claim[] { 
                             new Claim(JwtClaimTypes.Name,applicationUser.FirstName+" "+applicationUser.LastName),
                             new Claim(JwtClaimTypes.GivenName,applicationUser.FirstName),
                             new Claim(JwtClaimTypes.FamilyName,applicationUser.LastName),
                             new Claim(JwtClaimTypes.Role,SD.Admin),
                             new Claim(JwtClaimTypes.PhoneNumber,applicationUser.PhoneNumber)
            }).Result;

            ApplicationUser customerUser = new ApplicationUser()
            {
                UserName = "customer@gmail.com",
                Email = "customer@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "8561956637",
                FirstName = "Customer",
                LastName = "Nakum"
            };
            _userManager.CreateAsync(customerUser, "Customer@123").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, SD.Customer).GetAwaiter().GetResult();
            var temp2 = _userManager.AddClaimsAsync(customerUser, new Claim[] {
                             new Claim(JwtClaimTypes.Name,customerUser.FirstName+" "+customerUser.LastName),
                             new Claim(JwtClaimTypes.GivenName,customerUser.FirstName),
                             new Claim(JwtClaimTypes.FamilyName,customerUser.LastName),
                             new Claim(JwtClaimTypes.Role,SD.Admin),
                             new Claim(JwtClaimTypes.PhoneNumber,customerUser.PhoneNumber)
            }).Result;
        }
    }
}
