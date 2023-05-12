using ZdravoCorp.Serializer;
using ZdravoCorp.Model;
using System.Collections.Generic;
using System.Linq;

namespace ZdravoCorp.Repository
{
    public class PatientRepository
    {
        private const string FilePath = "../../Resources/Data/patients.csv";

        private readonly Serializer<Patient> _serializer;

        private List<Patient> _patients;

        public PatientRepository()
        {
            _serializer = new Serializer<Patient>();
            load();
        }

        public Patient GetByUsername(string username)
        {
            return _patients.FirstOrDefault(u => u.Username == username);
        }

        public void load()
        {
            _patients = _serializer.FromCSV(FilePath);
        }

        public void save()
        {
            _serializer.ToCSV(FilePath, _patients);
        }


        public Patient Save(Patient patient)
        {
            patient.Id = NextId();
            _patients.Add(patient);
            save();
            return patient;
        }

        public int NextId()
        {
            
            if (_patients.Count < 1)
            {
                return 1;
            }
            return _patients.Max(c => c.Id) + 1;
        }


        public List<Patient> GetAllPatients()
        {
            return _patients.ToList();
        }



    }
}
