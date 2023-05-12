using ZdravoCorp.Model;
using ZdravoCorp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Service
{
    public class PatientService
    {
        private readonly PatientRepository _patientRepository;

        public PatientService(PatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public Patient GetByUsername(string username)
        {
            return _patientRepository.GetByUsername(username);
        }

        public List<Patient> GetAllPatients()
        {
            return _patientRepository.GetAllPatients();
        }

        

    }
}
