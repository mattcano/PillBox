using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class RoleEditModel
    {
        public PillBoxRole Role { get; set; }
        public IEnumerable<PillBoxUser> Members { get; set; }
        public IEnumerable<PillBoxUser> NonMembers { get; set; }
    }
}