
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xaml;

namespace ZdravoCorp.Doktor
{
    public partial class newAppointment : Window
    {
        //fileds
        public Doctor loggedDoc;
        public List<int> hours { get; set; }
        public List<int> minutes { get; set; }
        private ObservableCollection<Patient> _suitablePatients;
        public ObservableCollection<Patient> suitablePatients
        {
            get { return _suitablePatients; }
            set { _suitablePatients = value; }
        }
        public RoomRepository roomRepo { get; set; }


        //constructor
        public newAppointment(Doctor LoggedDoc)
        {
            DataContext = this;
            _suitablePatients = new ObservableCollection<Patient>();
            this.ResizeMode = ResizeMode.NoResize;
            minutes = Enumerable.Range(0, 60).ToList();
            hours = Enumerable.Range(0, 24).ToList();
            this.loggedDoc = LoggedDoc;
            this.roomRepo = new RoomRepository();
            InitializeComponent();
        }


        //helper functions
        public DateOnly getDate()
        {
            DateTime dateTime = (DateTime)DateCal.SelectedDate;
            DateOnly date = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
            return date;
        }
        public TimeOnly strToTime(string str)
        {
            str += ":00";
            TimeOnly time = TimeOnly.Parse(str);
            return time;
        }
        public TimeOnly[] getTime()
        {
            TimeOnly beggining = strToTime(hourStartCB.SelectedItem.ToString() + ":" + minuteStartCB.SelectedItem.ToString());
            TimeOnly end = beggining.AddHours(Convert.ToDouble(hourDurationCB.SelectedItem));
            end = end.AddMinutes(Convert.ToDouble(minuteDurationCB.SelectedItem));
            TimeOnly[] times = new TimeOnly[] { beggining, end };
            return times;
        }
        //return time of beggining and time of ending
        public bool doTimesOverlap(TimeOnly begin, TimeOnly end, TimeOnly appointmentBegin, TimeOnly appointmentEnd)
        {
            if (begin.IsBetween(appointmentBegin, appointmentEnd) || end.IsBetween(appointmentBegin, appointmentEnd) ||
                           (begin <= appointmentBegin && end >= appointmentEnd))   //if they overlap
            {
                return true;
            }
            return false;
        }
        //helper function that checks if two time periods overlap
        public bool checkIfPastMidnight(TimeOnly start, TimeOnly end)
        {
            TimeSpan interval1 = start.ToTimeSpan();
            TimeSpan interval2 = end.ToTimeSpan();

            if ((interval1 < interval2 && TimeSpan.Zero >= interval1 && TimeSpan.Zero <= interval2)
                || (interval1 > interval2 && (TimeSpan.Zero >= interval1 || TimeSpan.Zero <= interval2)))
            {
                MessageBox.Show("You cant make appointments that lead into another day!");
                return true;
            }
            return false;                
        }
        //appointment cant be spreaded into two days
        public HashSet<int> findUnavailableRooms(TimeOnly begin, TimeOnly end, DateOnly date)
        {
            HashSet<int> unavailableRooms = new HashSet<int>();
            try
            {
                foreach (Appointment a in loggedDoc.appointmentRepo.allAppointments[date])
                {
                    if (doTimesOverlap(begin, end, a.timeStart, a.timeEnd))
                    {
                        unavailableRooms.Add(a.roomID);
                    }
                }
                return unavailableRooms;
            }
            catch (Exception ex) { return unavailableRooms; }
        }
        public bool isInThePast(DateOnly date)
        {
            if(date < DateOnly.FromDateTime(DateTime.Now))
            {
                MessageBox.Show("You cant make appointments in the past!");
                return true;
            }
            return false;
        }
        public List<Patient> getUnavailablePatients(DateOnly date, TimeOnly begin, TimeOnly end)
        {
            List<Patient> unavailablePatients = new List<Patient>();
            try
            {
                List<Appointment> selectedDateAppointments = loggedDoc.appointmentRepo.allAppointments[date];
                foreach (Appointment a in selectedDateAppointments)
                {
                    if (a.status == "online" && doTimesOverlap(begin, end, a.timeStart, a.timeEnd)) //check every appointment with selected one which is changing from list
                    {
                        unavailablePatients.Add(loggedDoc.patientRepo.returnPatient(a.patientUser));
                    }
                }
                return unavailablePatients;
            }
            catch (System.Collections.Generic.KeyNotFoundException) { return unavailablePatients; }    //no appointments in that day
        }
        public bool isDoctorFree(DateOnly date, TimeOnly begin, TimeOnly end)
        {
            try
            {
                List<Appointment> selectedDayAppointments = loggedDoc.appointmentRepo.allAppointments[date];

                foreach (Appointment appointment in selectedDayAppointments)
                {
                    if (appointment.status == "online" && appointment.doctorUser == this.loggedDoc.userName)
                    {
                        if (doTimesOverlap(begin, end, appointment.timeStart, appointment.timeEnd))
                        {
                            return false;
                        }
                    }
                }
                return true;

            }
            catch (System.Collections.Generic.KeyNotFoundException) { return true; }    //no key in dictionary == no appoinments of the day
        }


        //main functions
        public bool isAvailable(DateOnly date, TimeOnly start, TimeOnly end)
        {
            if (!isDoctorFree(date, start, end))
            {
                MessageBox.Show("Doctor not available at selected time");    //someone is not available
                return false;
            }
            List<Patient> unavailanlePatients = getUnavailablePatients(date, start, end);
            fillSuitablePatients(unavailanlePatients);
            return true;
        }
        public void fillSuitablePatients(List<Patient> unavailablePatients)
        {
            foreach (Patient pat in loggedDoc.patientRepo.Patients)
            {
                bool found = false;
                foreach (Patient p in unavailablePatients)
                {
                    if (pat.Username == p.Username)
                    {
                        found = true; break;
                    }
                }
                if (found == false)
                {
                    _suitablePatients.Add(pat);
                }
            }
        }
        public bool goodExaminationDuration()
        {
            if(hourDurationCB.SelectedIndex == 0 && minuteDurationCB.SelectedIndex == 15)
            {
                return true;
            }
            MessageBox.Show("Appointments must be 15 minute long");
            return false;
        }
        public int findRoom(TimeOnly begin, TimeOnly end, DateOnly date, string appointmentType)
        {
            HashSet<int> unavailableRooms = findUnavailableRooms(begin, end, date);
            foreach(KeyValuePair<int, Room> r in roomRepo.rooms)
            {
                if (!unavailableRooms.Contains(r.Key))
                {
                    if(appointmentType == "check up" && r.Value.type == Room.Type.CHECKUP)
                    {
                        return r.Key;
                    }
                    else if(appointmentType == "operation" && r.Value.type == Room.Type.OPERATION)
                    {
                        return r.Key;
                    }
                }
            }
            MessageBox.Show("No rooms available at selected time");
            return -1;
        }
        public bool makeAppointment()
        {
            DateOnly date = getDate();
            TimeOnly beggining = strToTime(hourStartCB.SelectedItem.ToString() + ":" + minuteStartCB.SelectedItem.ToString());
            TimeOnly end = beggining.AddHours(Convert.ToDouble(hourDurationCB.SelectedItem));
            end = end.AddMinutes(Convert.ToDouble(minuteDurationCB.SelectedItem)); 
            ComboBoxItem selectedItem = (ComboBoxItem)operationTypeCB.SelectedItem;
            string opType = selectedItem.Content.ToString().ToLower();

            if (checkIfPastMidnight(beggining, end))
            {
                return false;
            }
            if(opType == "check up" && !goodExaminationDuration())
            {
                return false;
            }
            int roomID = findRoom(beggining, end, date, opType);
            if(roomID == -1)
            {
                return false;
            }

            try
            {
                Patient selectedPatient = AvailablePatList.SelectedItem as Patient;
                Appointment a = new Appointment(selectedPatient.Username, loggedDoc.userName, date, beggining, end, "online", opType, roomID.ToString());
                loggedDoc.appointmentRepo.addAppointment(a);
                return true;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("You didnt select anything in the list");
                return false;
            }
        }


        //gui functionality
        private void SearchAvailablePatBtn_Click(object sender, RoutedEventArgs e)
        {
            _suitablePatients.Clear();
            DateOnly date = getDate();
            TimeOnly[] times = getTime();
            if(!isAvailable(date, times[0], times[1]))
            {
                ScheduleAppBtn.IsEnabled = false;
                return;
            }
            if (isInThePast(date))
            {
                ScheduleAppBtn.IsEnabled = false;
                return;
            }
            if(this.suitablePatients.Count > 0) { ScheduleAppBtn.IsEnabled = true; }
            else 
            { 
                ScheduleAppBtn.IsEnabled = false; 
                MessageBox.Show("No patients available at selected time"); 
            }
        }
        private void ScheduleAppBtn_Click(object sender, RoutedEventArgs e)
        {
            if (makeAppointment())
            {
                suitablePatients.Clear();
                ScheduleAppBtn.IsEnabled = false;
                this.Close();
                return;
            }
        }
        private void DurationCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ScheduleAppBtn == null) { }
            else { ScheduleAppBtn.IsEnabled = false; }
        }
    }
}

