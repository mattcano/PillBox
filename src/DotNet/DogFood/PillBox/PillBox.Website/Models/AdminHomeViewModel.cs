using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PillBox.Website.Models
{
    public class AdminHomeViewModel
    {
        public AdminHomeViewModel()
        {
            CreateMedicineModel = new CreateMedicineModel();
            CreatePatientModel = new CreatePatientModel();
        }

        public CreatePatientModel CreatePatientModel { get; set; }
        public CreateMedicineModel CreateMedicineModel { get; set; }
        public ICollection<PillBoxUser> Users { get; set; }

    }
}