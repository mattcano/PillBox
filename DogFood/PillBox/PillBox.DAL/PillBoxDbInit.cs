using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PillBox.Core.Enums;
using PillBox.Core.Helpers;
using PillBox.DAL.Entities;
using PillBox.Model;
using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace PillBox.DAL
{
    public class PillBoxDbInit :
        DropCreateDatabaseIfModelChanges<PillBoxDbContext>
    {
        protected override void Seed(PillBoxDbContext context)
        {

            UserManager<PillBoxUser> userMgr = new UserManager<PillBoxUser>(new UserStore<PillBoxUser>(context));
            RoleManager<PillBoxRole> roleMgr = new RoleManager<PillBoxRole>(new RoleStore<PillBoxRole>(context));

            string roleName = "Admin";
            string userName = "Admin";
            string password = "password";
            string email = "damola.omotosho@gmail.com";
            string phoneNumber = "3014373223";

            if (!roleMgr.RoleExists(roleName))
            {
                roleMgr.Create(new PillBoxRole(roleName));
            }

            PillBoxUser user = userMgr.FindByName(userName);
            if (user == null)
            {
                userMgr.Create(
                    new PillBoxUser { 
                        UserName = userName, 
                        Email = email, 
                        PhoneNumber = phoneNumber,
                        FirstName = "Admin",
                        LastName = "password",
                        Gender = "M",
                        AgeGroup = "18-25",
                    }, password);
                user = userMgr.FindByName(userName);
            }

            if (!userMgr.IsInRole(user.Id, roleName))
            {
                userMgr.AddToRole(user.Id, roleName);
            }

            //Medicines
            //var medicineAleve = new Medicine
            //{
            //    Name = "Aleve"
            //};

            //var medicineVitaminD = new Medicine
            //{
            //    Name = "Vitamin D"
            //};

            //var medicineFishOil = new Medicine
            //{
            //    Name = "Fish Oil"
            //};

            //var medicineMultiVitamin = new Medicine
            //{
            //    Name = "Multi Vitamin"
            //};

            //var medicines = new[] { medicineAleve, medicineVitaminD, medicineFishOil, medicineMultiVitamin };

            //Patients
            //var patientMichelle = new PillboxUser
            //{
            //    FirstName = "Michelle",
            //    Email = "mmjiang@stanford.edu",
            //    PhoneNumber = "3609096636",
            //    AutoSendPhone = true,
            //    AutoSendSMS = true,
            //    IsInTrial = true,
            //    Medicines = medicinesMichelle
            //};

            //var patientDamola = new PillboxUser
            //{
            //    FirstName = "Damola",
            //    Email = "damola.omotosho@gmail.com",
            //    PhoneNumber = "3014373223",
            //    AutoSendPhone = true,
            //    AutoSendSMS = true,
            //    IsInTrial = true,
            //    Medicines = medicinesDamola
            //};

            //var patientMatt = new PillboxUser
            //{
            //    FirstName = "Matt",
            //    Email = "mcano11@stanford.edu",
            //    PhoneNumber = "3107130421",
            //    AutoSendPhone = true,
            //    AutoSendSMS = true,
            //    IsInTrial = true,
            //    Medicines = medicinesMatt
            //};

            //var patients = new[]
            //{
            //    patientMichelle,
            //    patientDamola,
            //    patientMatt
            //};


            //List<IEntityBase> list = new List<IEntityBase>();
            //list.AddRange(patients);
            //list.AddRange(medicines);


            //foreach (var entity in list)
            //{
            //    Type entityType = entity.GetType();
            //    Add(context, entity, entityType);
            //}

            base.Seed(context);

        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        public void Add<T>(DbContext context, T entity, Type entityType) where T : class
        {
            try
            {
                ((DbContext)context).Set(entityType).Add(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("An error occured during the Add Entity.\r\n{0}", ex.Message));
            }
        }
    }
}
