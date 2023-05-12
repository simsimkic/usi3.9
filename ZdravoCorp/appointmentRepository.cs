using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class appointmentRepository
    {
        public Dictionary<DateOnly, List<Appointment>> allAppointments { get; set; }
        string filePath = "../../../appointments.csv";

        public appointmentRepository()
        {
            allAppointments = new Dictionary<DateOnly, List<Appointment>>();
            loadAllAppoinntments();
        }
        void loadAllAppoinntments()
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                Appointment a = makeAppointment(line);

                if (!hasKey(a))
                {
                   addAppointment(a);
                }
                else
                {
                    addAppointment(a);
                }
            }
        }

        public Appointment getAppointment(Appointment takenFromGUI)
        {
            foreach(Appointment a in allAppointments[takenFromGUI.date])
            {
                if (a.Equals(takenFromGUI))
                {
                    return a;
                }
            }
            return null;
        }           

        public Appointment makeAppointment(string line)
        {
            string[] data = line.Split(',');
            Appointment appointment = new Appointment(data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7]);
            return appointment;
        }

        public bool hasKey(Appointment appointment)
        {
            if (allAppointments.ContainsKey(appointment.date)){
                return true;
            }
            return false;
        }
    
        public void addAppointment(Appointment a)
        {
            if(!hasKey(a))
            {
                List<Appointment> list = new List<Appointment>();
                list.Add(a);
                allAppointments.Add(a.date, list);
            }
            else
            {
                allAppointments[a.date].Add(a);
            }
            writeAppointments();
        }
   
        public void removeApointment(Appointment appointment)
        {
            List<Appointment > list = this.allAppointments[appointment.date];
            foreach(Appointment a in list)
            {
                if (a.Equals(appointment))
                {
                    list.Remove(a);
                    return;
                }
            }
        }

        public string encodeToCSV()
        {
            StringBuilder res = new StringBuilder();
            foreach(KeyValuePair<DateOnly, List<Appointment>> pair in allAppointments)
            {
                foreach(Appointment a in pair.Value) 
                {
                    res.Append(a.encodeToCSV() + "\n");
                }
            }
            string fileContent = res.ToString().Trim();
            return fileContent;
        }
        public void writeAppointments()
        {
            string content = encodeToCSV();
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(content);
            }
        }
    }
}
