//using InitialProject.Model;
using ZdravoCorp.Serializer;
using ZdravoCorp.Model;
using System.Collections.Generic;
using System.Linq;
namespace ZdravoCorp.Repository
{
    public class PatientAppointmentRepository
    {
        private const string FilePath = "../../Resources/Data/patientAppointments.csv";

        private readonly Serializer<PatientAppointment> _serializer;

        private List<PatientAppointment> _patientAppointments;
        private DoctorRepository _doctorRepository;
        private AnamnesisRepository _anamnesisRepository;

        public PatientAppointmentRepository(DoctorRepository doctorRepository, AnamnesisRepository anamnesisRepository)
        {
            _doctorRepository = doctorRepository;
            _anamnesisRepository = anamnesisRepository;
            _serializer = new Serializer<PatientAppointment>();
            load();
        }

        public List<PatientAppointment> GetAll()
        {
            return _patientAppointments.ToList();
        }

        public void load()
        {
            _patientAppointments = _serializer.FromCSV(FilePath);
            bind();
        }

        public void save()
        {
            _serializer.ToCSV(FilePath, _patientAppointments);
        }

        private void bind()
        {
            foreach (PatientAppointment appointment in _patientAppointments)
            {
                if (appointment.Doctor != null)
                {
                    Doctor d = _doctorRepository.GetById(appointment.Doctor.Id);
                    appointment.Doctor = d;
                }

                if (appointment.Anamnesis != null)
                {
                    Anamnesis a = _anamnesisRepository.GetAnamnesisById(appointment.Anamnesis.Id);
                    appointment.Anamnesis = a;
                }
            }
        }

        public PatientAppointment Save(PatientAppointment patientAppointment)
        {
            patientAppointment.Id = NextId();
            _patientAppointments.Add(patientAppointment);
            save();
            return patientAppointment;
        }

        public int NextId()
        {

            if (_patientAppointments.Count < 1)
            {
                return 1;
            }
            return _patientAppointments.Max(c => c.Id) + 1;
        }




       


        public List<PatientAppointment> GetAppointmentsForDoctor(int doctorId)
        {
            List<PatientAppointment> appointments = new List<PatientAppointment>();
            foreach (PatientAppointment appointment in _patientAppointments)
            {
                if (appointment.Doctor.Id == doctorId)
                {
                    appointments.Add(appointment);
                }
            }
            return appointments;
        }

      


    }
}

