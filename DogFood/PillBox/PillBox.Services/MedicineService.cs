using PillBox.DAL;
using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Services
{
    interface IMedicineService
    {
        bool AddMedicine(string name);
    }

    public class MedicineService : IMedicineService
    {
        IRepository _repo;

        public MedicineService(IRepository repo)
        {
            _repo = repo;
        }

        public bool AddMedicine(string name)
        {
            try
            {
                Medicine entity = new Medicine()
                {
                    Name = name
                };
                _repo.AddEntity(entity);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
