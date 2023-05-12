using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Service;
using ZdravoCorp.Model;

namespace ZdravoCorp.Controllers
{
    public class PatientController
    {
        private PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        public Patient GetByUsername(string username)
        {
            return _patientService.GetByUsername(username);
        }

        public List<Patient> GetAllPatients()
        {
            return _patientService.GetAllPatients();
        }


        
    }
}
