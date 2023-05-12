using ZdravoCorp.Helpers;
using ZdravoCorp.Model.Enums;
using ZdravoCorp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Model
{
    public class PatientAppointment : ISerializable
    {
        public int Id { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }

        public DateTime Start { get; set; }
        public int Duration { get; set; }   //in minutes

        public Anamnesis Anamnesis { get; set; }


        public PatientAppointment()
        {
        }
        public PatientAppointment(int id, Doctor doctor, Patient patient, DateTime start, int duration)
        {
            Id = id;
            Doctor = doctor;
            Patient = patient;
            Start = start;
            Duration = duration;
            Anamnesis = new Anamnesis();
        }

        public PatientAppointment(int id, Doctor doctor, Patient patient, DateTime start, int duration, Anamnesis anamnesis)
        {
            Id = id;
            Doctor = doctor;
            Patient = patient;
            Start = start;
            Duration = duration;
            Anamnesis = anamnesis;
        }

        public string[] ToCSV()
        {
            string[] csvValues = new string[6];
            if (Id != null) csvValues[0] = Id.ToString();
            if (Doctor != null && Doctor.Id != null) csvValues[1] = Doctor.Id.ToString();
            if (Patient != null && Patient.Id != null) csvValues[2] = Patient.Id.ToString();
            if (Start != null) csvValues[3] = DateTimeHelper.DateTimeToString(Start);
            if (Duration != null) csvValues[4] = Duration.ToString();
            if (Anamnesis != null && Anamnesis.Id != null) csvValues[5] = Anamnesis.Id.ToString();
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Doctor = new Doctor() { Id = Convert.ToInt32(values[1]) };
            Patient = new Patient() { Id = Convert.ToInt32(values[2]) };
            Start = DateTimeHelper.StringToDateTime(values[3]);
            Duration = Convert.ToInt32(values[4]);
            if (!string.IsNullOrEmpty(values[5]))
            {
                Anamnesis = new Anamnesis() { Id = Convert.ToInt32(values[5]) };
            }
        }
    }
}