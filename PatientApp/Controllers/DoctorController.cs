using ZdravoCorp.Model;
using ZdravoCorp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Controllers
{
    public class DoctorController
    {
        private DoctorService _doctorService;

        public DoctorController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public List<Doctor> GetAll()
        {
            return _doctorService.GetAllDoctors();
        }

        public Doctor Save(Doctor doctor)
        {
            return _doctorService.Save(doctor);
        }

        public Doctor GetByUsername(string username)
        {
            return _doctorService.GetByUsername(username);
        }

       
    }
}
