using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class AdminHomeViewModel
    {
        public CreatePatientModel CreatePatientModel { get; set; }
        public ICollection<PillBoxUser> Users { get; set; }

    }
}