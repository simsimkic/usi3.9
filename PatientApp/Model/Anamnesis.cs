using ZdravoCorp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Model
{
    public class Anamnesis : ISerializable
    {
        public int Id { get; set; }
        public string Record { get; set; }
        public PatientAppointment PatientAppointment { get; set; }

        public Anamnesis() { }

        public Anamnesis(string record, PatientAppointment patientAppointment)
        {
            Record = record;
            PatientAppointment = patientAppointment;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Record, PatientAppointment.Id.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Record = values[1];
            PatientAppointment = new PatientAppointment() { Id = Convert.ToInt32(values[2]) };
        }
    }

}
