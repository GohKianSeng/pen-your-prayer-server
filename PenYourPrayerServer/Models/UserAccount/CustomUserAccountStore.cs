using PenYourPrayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PenYourPrayer.Models.UserAccount
{
    public class CustomUserAccountStore<TUser>
         : IUserStore<TUser>, IUserPasswordStore<TUser>, IDisposable where TUser : CustomUserAccount
    {
        public CustomUserAccountStore()
            : this((DbContext)new CustomUserAccountDBContext<TUser>())
        {
            //this.DisposeContext = true;
        }

        public CustomUserAccountStore(DbContext context)
        {

        }


        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            throw new NotImplementedException("AddLoginAsync");
        }

        public Task<TUser> FindAsync(UserLoginInfo login) 
        {
            throw new NotImplementedException("FindAsync");
        }

        public Task<System.Collections.Generic.IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            throw new NotImplementedException("GetLoginsAsync");
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            throw new NotImplementedException("RemoveLoginAsync");
        }

        public Task CreateAsync(TUser user)
        {
            string sdfsdf = "";
            sdfsdf = "sss";

            return Task.FromResult<int>(0);
        }

        public Task ChangePasswordAsync(String userId, string currentPassword, string newPassword)
        {
            return Task.FromResult<int>(0);
        }

        public Task DeleteAsync(TUser user)
        {
            throw new NotImplementedException("deleteasync");
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException("findbyidasync");
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            return Task.FromResult<TUser>(null);                
        }

        public Task UpdateAsync(TUser user)
        {
            throw new NotImplementedException("updatesync");
        }

        public void Dispose()
        {
            //context.Dispose();
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult("sdf");
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(true);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            String sdf = "";
            sdf = "sdf";
            //throw new NotImplementedException();
            return Task.FromResult<int>(0);
        }

    }
}