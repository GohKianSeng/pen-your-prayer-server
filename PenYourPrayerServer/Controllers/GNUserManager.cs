using PenYourPrayer.Models.UserAccount;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PenYourPrayer
{
    public class GNUserManager : UserManager<PenYourPrayerUser>
    {

        public GNUserManager(IUserStore<PenYourPrayerUser> store)
            : base(store)
        {
            
        }

        public override async Task<PenYourPrayerUser> FindAsync(string userName, string password)
        {
            /* Performs some logic here that returns true */
            PenYourPrayerUser user = await base.FindAsync(userName, password);
            //set Username as the email/social medial id
            //can change id to non primary key and can set to default 0 and reassign new primary key
            //http://stackoverflow.com/questions/20529401/how-to-customize-authentication-to-my-own-set-of-tables-in-asp-net-web-api-2
            return user;
        }

        public override Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            return base.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        public override Task<IdentityResult> CreateAsync(PenYourPrayerUser user, string password)
        {
             
 	         return base.CreateAsync(user, password);
        }

    }

}