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
            //Medicines
            var medicineAleve = new Medicine
            {
                Name = "Aleve"
            };

            var medicineVitaminD = new Medicine
            {
                Name = "Vitamin D"
            };

            var medicineFishOil = new Medicine
            {
                Name = "Fish Oil"
            };

            var medicineMultiVitamin = new Medicine
            {
                Name = "Multi Vitamin"
            };

            var medicines = new[] { medicineAleve, medicineVitaminD, medicineFishOil, medicineMultiVitamin };

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


            List<IEntityBase> list = new List<IEntityBase>();
            //list.AddRange(patients);
            list.AddRange(medicines);


            foreach (var entity in list)
            {
                Type entityType = entity.GetType();
                Add(context, entity, entityType);
            }

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
