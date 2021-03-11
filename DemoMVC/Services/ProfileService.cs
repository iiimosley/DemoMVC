using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DemoMVC.Services
{
    public interface IProfileService
    {
        int GetCurrentProfileID();
    }
    public class ProfileService : IProfileService
    {
        public int GetCurrentProfileID() => (int?)Membership.GetUser().ProviderUserKey ?? 0;
    }
}