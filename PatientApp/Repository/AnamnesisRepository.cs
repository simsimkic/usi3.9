using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Repository
{
    public class AnamnesisRepository
    {
        private const string FilePath = "../../Resources/Data/anamnesis.csv";

        private readonly Serializer<Anamnesis> _serializer;

        private List<Anamnesis> _anamnesis;

        public AnamnesisRepository()
        {
            _serializer = new Serializer<Anamnesis>();
            Load();
        }

        public void Load()
        {
            _anamnesis = _serializer.FromCSV(FilePath);
        }

        public void Save()
        {
            _serializer.ToCSV(FilePath, _anamnesis);
        }

        public Anamnesis Save(Anamnesis anamnesis)
        {
            anamnesis.PatientAppointment.Anamnesis = anamnesis;
            _anamnesis.Add(anamnesis);
            Save();
            return anamnesis;
        }

        public List<Anamnesis> GetAll()
        {
            return _anamnesis.ToList();
        }

        public List<Anamnesis> GetByPatient(Patient patient)
        {
            return _anamnesis.Where(a => a.PatientAppointment.Patient.Id == patient.Id).ToList();
        }


        public Anamnesis GetAnamnesisById(int id)
        {
            return _anamnesis.FirstOrDefault(a => a.Id ==id);
        }
    }
}
