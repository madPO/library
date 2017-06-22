using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using NHibernate;

namespace application.Models
{
    public class IdentityStore : IUserStore<Users, int>,
        IUserPasswordStore<Users, int>,
        IUserLockoutStore<Users, int>,
        IUserTwoFactorStore<Users, int>
    {

        private readonly ISession session;

        public IdentityStore(ISession session)
        {
            this.session = session;
        }

        public Task CreateAsync(Users user)
        {
            return Task.Run(() => session.SaveOrUpdate(user));
        }

        public Task DeleteAsync(Users user)
        {
            return Task.Run(() => session.Delete(user));
        }

        public void Dispose()
        {
            
        }

        public Task<Users> FindByIdAsync(int userId)
        {
            return Task.Run(()=>session.Get<Users>(userId));
        }

        public Task<Users> FindByNameAsync(string userName)
        {
            return Task.Run(()=>session.QueryOver<Users>().Where(u=>u.Login == userName).SingleOrDefault());
        }

        public Task<int> GetAccessFailedCountAsync(Users user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(Users user)
        {
            return Task.FromResult(false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(Users user)
        {
            return Task.FromResult(DateTimeOffset.MaxValue);
        }

        public Task<string> GetPasswordHashAsync(Users user)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> GetTwoFactorEnabledAsync(Users user)
        {
            return Task.FromResult(false);
        }

        public Task<bool> HasPasswordAsync(Users user)
        {
            return Task.FromResult(true);
        }

        public Task<int> IncrementAccessFailedCountAsync(Users user)
        {
            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(Users user)
        {
            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync(Users user, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync(Users user, DateTimeOffset lockoutEnd)
        {
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(Users user, string passwordHash)
        {
            return Task.Run(() => user.Password = passwordHash);
        }

        public Task SetTwoFactorEnabledAsync(Users user, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Users user)
        {
            return Task.Run(()=>session.SaveOrUpdate(user));
        }
    }
}