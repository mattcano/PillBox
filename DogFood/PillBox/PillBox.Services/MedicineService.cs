using PillBox.DAL;
using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Services
{
    public interface IMedicineService
    {
        bool AddMedicine(string name);
        bool DeleteUserMedicines(string id);
    }

    public class MedicineService : IMedicineService
    {
        IRepository _repo;
        IUnitOfWork _uow;

        public MedicineService(IUnitOfWork uow, IRepository repo)
        {
            _uow = uow;
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

        public bool DeleteUserMedicines(string id)
        {
            try
            {
                var userMedicines = _repo.GetList<Medicine>(m => m.UserId == id);

                //_uow.
                foreach(var med in userMedicines)
                {
                    _repo.DeleteEntity(med);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
