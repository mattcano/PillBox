using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using PillBox.DAL.Entities;
using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillBox.Services
{
    public class PillBoxUserManager : UserManager<PillBoxUser>
    {
        public PillBoxUserManager(IUserStore<PillBoxUser> store)
            :base(store)
        {

        }

        public static PillBoxUserManager Create(
            IdentityFactoryOptions<PillBoxUserManager> options,
            IOwinContext context)
        {
            PillBoxDbContext db = context.Get<PillBoxDbContext>();
            PillBoxUserManager manager = new PillBoxUserManager(new UserStore<PillBoxUser>(db));
            
            return manager;
        }
    }
}
