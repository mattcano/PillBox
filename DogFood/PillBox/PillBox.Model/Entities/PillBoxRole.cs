using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillBox.Model.Entities
{
    public class PillBoxRole: IdentityRole
    {
        public PillBoxRole():base() { }

        public PillBoxRole(string name) : base(name) { }
    }
}
