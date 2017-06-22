﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace application.Models
{
    public class SignInManager : SignInManager<Users, int>
    {
        public SignInManager(UserManager<Users, int> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager) { }

        public void SignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}