using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PenYourPrayer.Models.UserAccount;

namespace PenYourPrayer.Models.UserAccount
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class PenYourPrayerUser : CustomUserAccount
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<PenYourPrayerUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : CustomUserAccountDBContext<PenYourPrayerUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}