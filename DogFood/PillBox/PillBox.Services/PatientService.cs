using PillBox.DAL;
using PillBox.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Services
{
    public interface IPatientService
    {
        bool AddPatient(string firstName, string email);
        ICollection<Patient> GetAllUsers();
    }

    public class PatientService : IPatientService
    {
        IRepository _repo;

        public PatientService(IRepository repo)
        {
            _repo = repo;
        }

        public bool AddPatient(string firstName, string email)
        {
            try
            {
                Patient entity = new Patient()
                {
                    FirstName = firstName,
                    Email = email
                };

                _repo.AddEntity(entity);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public ICollection<Patient> GetAllUsers()
        {
            return _repo.GetList<Patient>().ToList();
        }
    }
}
