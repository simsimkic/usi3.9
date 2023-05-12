//using InitialProject.Serializer;
using ZdravoCorp.Serializer;
using ZdravoCorp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Contexts;

namespace ZdravoCorp.Repository
{
    public class DoctorRepository
    {
        private const string FilePath = "../../Resources/Data/doctors.csv";

        private readonly Serializer<Doctor> _serializer;

        private List<Doctor> _doctors;

        public DoctorRepository()
        {
            _serializer = new Serializer<Doctor>();
            load();
        }

        public Doctor GetByUsername(string username)
        {
            return _doctors.FirstOrDefault(u => u.Username == username);
        }

        public Doctor GetById(int id)
        {
            Doctor doctor = _doctors.FirstOrDefault(d => d.Id == id);
            if (doctor == null)
            {
                return null;
            }
            return doctor;
        }

        public void load()
        {
            _doctors = _serializer.FromCSV(FilePath );
        }

        public void save()
        {
            _serializer.ToCSV(FilePath, _doctors);
        }


        public Doctor Save(Doctor doctor)
        {
            doctor.Id = NextId();
            _doctors.Add(doctor);
            save();
            return doctor;
        }

        public int NextId()
        {

            if (_doctors.Count < 1)
            {
                return 1;
            }
            return _doctors.Max(c => c.Id) + 1;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _doctors.ToList();
        }






    }
}
