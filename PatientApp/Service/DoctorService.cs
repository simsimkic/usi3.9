using ZdravoCorp.Model;
using ZdravoCorp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ZdravoCorp.Service
{
    public class DoctorService
    {
        private DoctorRepository _doctorRepository;

        public DoctorService(DoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public Doctor Save(Doctor doctor)
        {
            return _doctorRepository.Save(doctor);
        }

        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepository.GetAllDoctors();
        }

        public Doctor GetByUsername(string username)
        {
            return _doctorRepository.GetByUsername(username);
        }

      
    }
}


