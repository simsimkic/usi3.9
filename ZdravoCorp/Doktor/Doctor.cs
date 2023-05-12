using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;
using System.Xml.Linq;

namespace ZdravoCorp.Doktor
{
    public class Doctor
    {
        public string userName { get; set; } 
        public string password { get; set; }
        public appointmentRepository appointmentRepo; 
        public PatientRepository patientRepo;
        public AnamnesisRepository anamnesisRepo;
             
        void loadAllAppoinntments()
        {
            appointmentRepository repo = new appointmentRepository();
            this.appointmentRepo = repo;
        }

        void loadAllPatients() {//key - patientID, value - patient
            PatientRepository repo = new PatientRepository();
            this.patientRepo = repo;
        }
        void loadAllAnamnesis()
        {
            AnamnesisRepository AnamnesisRepo = new AnamnesisRepository();
            this.anamnesisRepo = AnamnesisRepo;
        }
       
        public Doctor(string UserName, string Password)
        {
            userName = UserName;
            password = Password;
            loadAllAppoinntments();
            loadAllPatients();
            loadAllAnamnesis();
        }

        public override string ToString()       
        {
            string appointmentsStr = "";    
            try
            {
                foreach(KeyValuePair<DateOnly, List<Appointment>> pair in appointmentRepo.allAppointments)
                {
                    foreach(Appointment a in pair.Value)
                    {
                        if(a.doctorUser == userName)
                        {
                            appointmentsStr += a.ToString();
                        }
                    }
                }
                
            } catch(Exception e) { }
            return "Username: " + userName + " Pass: " + password + "\nPregledi:\n" + appointmentsStr;
        }

        public static explicit operator Doctor(User v)
        {
            return new Doctor(v.Username, v.Password);
        }

        
    }
}
