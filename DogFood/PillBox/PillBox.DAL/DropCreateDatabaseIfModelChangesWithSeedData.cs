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
    public class DropCreateDatabaseIfModelChangesWithSeedData :
        DropCreateDatabaseIfModelChanges<PillBoxContext>
    {
        protected override void Seed(PillBoxContext context)
        {
            //Remind Times
            var remindTimeEarlyMorning = new RemindTime
            {
                RemindValue = StringHelper.GetEnumString(
                RemindTimeEnum.EARLY_MORNING.ToString())            
            };

            var remindTimeMorning = new RemindTime
            {
                RemindValue = StringHelper.GetEnumString(
                RemindTimeEnum.MORNING.ToString())            
            };

            var remindTimeEarlyAfternoon = new RemindTime
            {
                RemindValue = StringHelper.GetEnumString(
                RemindTimeEnum.EARLY_AFTERNOON.ToString())
            };

            var remindTimeAfternoon = new RemindTime
            {
                RemindValue = StringHelper.GetEnumString(
                RemindTimeEnum.AFTERNOON.ToString())
            };

            var remindTimeEvening = new RemindTime
            {
                RemindValue = StringHelper.GetEnumString(
                RemindTimeEnum.EVENING.ToString())
            };

            var remindTimeNight = new RemindTime
            {
                RemindValue = StringHelper.GetEnumString(
                RemindTimeEnum.NIGHT.ToString())
            };

            var remindTimes = new[]
                {
                    remindTimeEarlyMorning,
                    remindTimeMorning,
                    remindTimeEarlyAfternoon,
                    remindTimeAfternoon,
                    remindTimeEvening,
                    remindTimeNight
                };

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

            //Michelles Medicines
            var medicinesMichelle = new List<UserMedicineMap>()
            {
                new UserMedicineMap
                { 
                    Medicine = medicineAleve,
                    IsWithFood = true,
                    NumberOfPills = 2,
                    RemindTime = remindTimeAfternoon
                }

            };

            //Matts Medicines
            var medicinesMatt = new List<UserMedicineMap>()
            {
                new UserMedicineMap
                {
                    Medicine = medicineVitaminD,
                    IsWithFood = true,
                    NumberOfPills = 1,
                    RemindTime = remindTimeMorning
                },
                new UserMedicineMap
                {
                    Medicine = medicineFishOil,
                    IsWithFood = true,
                    NumberOfPills = 1,
                    RemindTime = remindTimeMorning
                }
            };

            //Damolas Medicines
            var medicinesDamola = new List<UserMedicineMap>()
            {
                new UserMedicineMap
                {
                    Medicine = medicineVitaminD,
                    IsWithFood = true,
                    NumberOfPills = 1,
                    RemindTime = remindTimeMorning
                },
                new UserMedicineMap
                {
                    Medicine = medicineFishOil,
                    IsWithFood = true,
                    NumberOfPills = 2,
                    RemindTime = remindTimeMorning
                },
                new UserMedicineMap
                {
                    Medicine = medicineMultiVitamin,
                    IsWithFood = true,
                    NumberOfPills = 1,
                    RemindTime = remindTimeMorning
                }
            };


            //Patients
            var patientMichelle = new Patient
            {
                FirstName = "Michelle",
                Email = "mmjiang@stanford.edu",
                AutoSendPhone = true,
                AutoSendSMS = true,
                IsInTrial = true,
                UserMedicineMaps = medicinesMichelle
            };

            var patientDamola = new Patient
            {
                FirstName = "Damola",
                Email = "damola.omotosho@gmail.com",
                AutoSendPhone = true,
                AutoSendSMS = true,
                IsInTrial = true,
                UserMedicineMaps = medicinesDamola
            };

            var patientMatt = new Patient
            {
                FirstName = "Matt",
                Email = "mcano11@stanford.edu",
                AutoSendPhone = true,
                AutoSendSMS = true,
                IsInTrial = true,
                UserMedicineMaps = medicinesMatt
            };

            var patients = new[]
            {
                patientMichelle,
                patientDamola,
                patientMatt
            };


            List<IEntityBase> list = new List<IEntityBase>();
            list.AddRange(remindTimes);
            list.AddRange(patients);
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
