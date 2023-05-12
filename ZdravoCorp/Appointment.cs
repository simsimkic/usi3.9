using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Doktor;

namespace ZdravoCorp
{
    public class Appointment  //model for loading 'pregledi.txt'
    {

        public string doctorUser { get; set; }
        public string patientUser { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public DateOnly date { get; set; }
        public TimeOnly timeStart { get; set; }
        public TimeOnly timeEnd { get; set; }
        public int roomID { get; set; }



        public Appointment(string PatientUser, string DoctorUser, string Date, string TimeStart, string TimeEnd, string Status, string Type, string RoomID)
        {
            doctorUser = DoctorUser;
            patientUser = PatientUser;

            string[] strings = Date.Split('/');
            int[] DateInfo = Array.ConvertAll(strings, s => int.Parse(s));
            date = new DateOnly(DateInfo[0], DateInfo[1], DateInfo[2]);

            string[] strings2 = TimeStart.Split(":");
            int[] TimeInfo = Array.ConvertAll(strings2, s => int.Parse(s));
            timeStart = new TimeOnly(TimeInfo[0], TimeInfo[1]);

            string[] strings3 = TimeEnd.Split(":");
            int[] TimeInfo2 = Array.ConvertAll(strings3, s => int.Parse(s));
            timeEnd = new TimeOnly(TimeInfo2[0], TimeInfo2[1]);

            status = Status;
            type = Type;
            this.roomID = int.Parse(RoomID);
        }
        //constructor that reads from file
        public Appointment(string PatientUser, string DoctorUser, DateOnly Date, TimeOnly TimeStart, TimeOnly TimeEnd, string Status, string Type, string RoomID)
        {
            doctorUser = DoctorUser;
            patientUser = PatientUser;
            this.date = Date;
            this.timeStart = TimeStart;
            this.timeEnd = TimeEnd;
            status = Status;
            type = Type;
            roomID = int.Parse(RoomID);
        }
        //constructor for making inside App
   
        

        public override string ToString()
        {
            return "patient: " + patientUser + " doc: " + doctorUser
                + " Date: " + date + " TimeS: " + timeStart + "TimeE" + timeEnd;
        }
        
         public string encodeToCSV()
        {
            StringBuilder res = new StringBuilder();
            res.Append(patientUser);
            res.Append(',');
            res.Append(doctorUser); 
            res.Append(",");
            res.Append(date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));
            res.Append(",");
            res.Append(timeStart.ToString("HH:mm"));
            res.Append(",");
            res.Append(timeEnd.ToString("HH:mm"));
            res.Append(",");
            res.Append(status);
            res.Append(",");
            res.Append(type);
            res.Append(",");
            res.Append(roomID.ToString());
            return res.ToString();
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Appointment))
            {
                return false;
            }

            Appointment other = (Appointment)obj;
            return this.doctorUser == other.doctorUser && this.patientUser == other.patientUser && 
                this.date == other.date && this.timeStart == other.timeStart && this.timeEnd == other.timeEnd && 
                this.status == other.status;
        }
    }

}
