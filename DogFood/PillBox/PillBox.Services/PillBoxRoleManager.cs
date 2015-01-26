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
    public class PillBoxRoleManager : RoleManager<PillBoxRole>, IDisposable
    {
        public PillBoxRoleManager(RoleStore<PillBoxRole> store)
            :base(store)
        {

        }

        public static PillBoxRoleManager Create(
                IdentityFactoryOptions<PillBoxRoleManager> options,
                IOwinContext context)
        {
            return new PillBoxRoleManager(new RoleStore<PillBoxRole>(context.Get<PillBoxDbContext>()));
        }
    }
}
