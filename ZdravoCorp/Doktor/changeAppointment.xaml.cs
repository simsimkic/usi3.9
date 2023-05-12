using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;

namespace ZdravoCorp.Doktor
{
    public partial class changeAppointment : Window
    {
        //fields
        Appointment selectedAppointment;
        RoomRepository roomRepo;
        public Doctor loggedDoc;
        public List<int> minutes { get; set; }
        public List<int> hours { get; set; }


        //constructor
        public changeAppointment(Appointment SelectedAppointment, Doctor LoggedDoc)
        {
            DataContext = this;
            this.loggedDoc = LoggedDoc;
            this.selectedAppointment = loggedDoc.appointmentRepo.getAppointment(SelectedAppointment);
            this.ResizeMode = ResizeMode.NoResize;
            minutes = Enumerable.Range(0, 60).ToList();
            hours = Enumerable.Range(0, 24).ToList();
            roomRepo = new RoomRepository();
            InitializeComponent();
            loadExistingAppointment(selectedAppointment);
        }


        //helper functions
        public int[] getTimeAttributesAsInts(Appointment SelectedAppointment)
        {
            TimeSpan diff = SelectedAppointment.timeEnd - SelectedAppointment.timeStart;
            int hoursDuration = diff.Hours;
            int minutesDuration = diff.Minutes;

            int hoursStart = SelectedAppointment.timeStart.Hour;
            int minutesStart = SelectedAppointment.timeStart.Minute;

            int[] listTime = { hoursDuration, minutesDuration, hoursStart, minutesStart };
            return listTime;
        }
        public bool doTimesOverlap(TimeOnly begin, TimeOnly end, TimeOnly appointmentBegin, TimeOnly appointmentEnd)
        {
            if (begin.IsBetween(appointmentBegin, appointmentEnd) || end.IsBetween(appointmentBegin, appointmentEnd) ||
                           (begin <= appointmentBegin && end >= appointmentEnd))   //if they overlap
            {
                return true;
            }
            return false;
        }
        public bool isDoctorFree(DateOnly date, TimeOnly begin, TimeOnly end)
        {
            try
            {
                List<Appointment> selectedDayAppointments = loggedDoc.appointmentRepo.allAppointments[date];

                foreach (Appointment appointment in selectedDayAppointments)
                {
                    if (appointment.status == "online" && appointment.doctorUser == loggedDoc.userName)
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
        //function that check if doctor already has scheduled appointment in that time
        public bool isPatientFree(DateOnly date, TimeOnly begin, TimeOnly end)
        {
            try
            {
                List<Appointment> selectedDateAppointments = loggedDoc.appointmentRepo.allAppointments[date];
                foreach (Appointment a in selectedDateAppointments)
                {
                    if (a.patientUser == this.selectedAppointment.patientUser && a.status == "online") //check every appointment with selected one which is changing from list
                    {
                        if (doTimesOverlap(begin, end, a.timeStart, a.timeEnd))
                        {
                            MessageBox.Show("Patient not available");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (System.Collections.Generic.KeyNotFoundException) { return true; }    //no appointments in that day

        }
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
        public TimeOnly[] getTimeAttributesFromInts()
        {
            TimeOnly start = new TimeOnly((int)hourStartCB.SelectedItem, (int)minuteStartCB.SelectedItem, 0);
            int endHour = start.Hour + (int)hourDurationCB.SelectedItem;
            int endMinutes = start.Minute + (int)minuteDurationCB.SelectedItem;
            if (endMinutes >= 60)
            {
                endHour += endMinutes / 60;
                endMinutes = endMinutes % 60;
            }
            TimeOnly end = new TimeOnly(endHour, endMinutes, 0);

            TimeOnly[] list = { start, end };
            return list;
        }
        public bool goodExaminationDuration()
        {
            if (hourDurationCB.SelectedIndex == 0 && minuteDurationCB.SelectedIndex == 15)
            {
                return true;
            }
            MessageBox.Show("Appointments must be 15 minute long");
            return false;
        }
        public HashSet<int> findUnavailableRooms(TimeOnly begin, TimeOnly end, DateOnly date)
        {
            HashSet<int> unavailableRooms = new HashSet<int>();
            foreach (Appointment a in loggedDoc.appointmentRepo.allAppointments[date])
            {
                if (doTimesOverlap(begin, end, a.timeStart, a.timeEnd))
                {
                    unavailableRooms.Add(a.roomID);
                }
            }
            return unavailableRooms;
        }
        public bool isInThePast(DateOnly date)
        {
            if (date < DateOnly.FromDateTime(DateTime.Now))
            {
                MessageBox.Show("You cant make appointments in the past!");
                return true;
            }
            return false;
        }


        //main functions
        public void loadExistingAppointment(Appointment SelectedAppointment)
        {
            DateTime dateTime = DateTime.SpecifyKind(new DateTime(SelectedAppointment.date.Year, SelectedAppointment.date.Month, SelectedAppointment.date.Day), DateTimeKind.Local);
            DateCal.SelectedDate = dateTime;

            int[] listTime = getTimeAttributesAsInts(SelectedAppointment);
            hourDurationCB.SelectedItem = listTime[0];
            minuteDurationCB.SelectedItem = listTime[1];
            hourStartCB.SelectedItem = listTime[2];
            minuteStartCB.SelectedItem = listTime[3];

            if (selectedAppointment.type == "operation")
            {operationTypeCB.SelectedItem = 0;}
            else 
            {operationTypeCB.SelectedIndex = 1;}
        }
        public bool isAvailable(DateOnly date, TimeOnly start, TimeOnly end)
        {
            selectedAppointment.status = "changing";    //temporary so it doesnt count on checking availability
            if (!isDoctorFree(date, start, end))
            {
                MessageBox.Show("Doctor not available");    //someone is not available
                return false;
            }
            if (!isPatientFree(date, start, end))
            {
                MessageBox.Show("Patient not available");    //someone is not available
                return false;
            }
            return true;
        }
        public int findRoom(TimeOnly begin, TimeOnly end, DateOnly date, string appointmentType)
        {
            HashSet<int> unavailableRooms = findUnavailableRooms(begin, end, date);
            foreach (KeyValuePair<int, Room> r in roomRepo.rooms)
            {
                if (!unavailableRooms.Contains(r.Key))
                {
                    if (appointmentType == "check up" && r.Value.type == Room.Type.CHECKUP)
                    {
                        return r.Key;
                    }
                    else if (appointmentType == "operation" && r.Value.type == Room.Type.OPERATION)
                    {
                        return r.Key;
                    }
                }
            }
            MessageBox.Show("No rooms available at selected time");
            return -1;
        }


        //GUI functionality
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.selectedAppointment.status = "canceled";
            CloseWindow();
        }
        private void ChangeAppBtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DateCal.SelectedDate.Value.Date;
            DateOnly date = DateOnly.FromDateTime(dateTime.Date);
            TimeOnly[] list = getTimeAttributesFromInts();
            string newType = operationTypeCB.Text.ToLower();


            if (isInThePast(date)) { return; }
            
            if (list[0] == list[1]) 
            { MessageBox.Show("Bad duration of appointment selected"); return; }
            
            if (checkIfPastMidnight(list[0], list[1])){ return; }
            
            if(newType == "check up" && !goodExaminationDuration()) { return; }

            int newRoomID = findRoom(list[0], list[1], date, newType);
            if (newRoomID == -1) { return; }


            if (isAvailable(date, list[0], list[1]))
            {  
                Appointment a = new Appointment(this.selectedAppointment.patientUser, this.selectedAppointment.doctorUser,
                    date, list[0], list[1], "online", newType, newRoomID.ToString());   
                loggedDoc.appointmentRepo.removeApointment(selectedAppointment);
                loggedDoc.appointmentRepo.addAppointment(a);
                CloseWindow();
            }
            else
            {
                this.selectedAppointment.status = "online"; //return status from changing to online
            }

        }
        public delegate void WindowClosedEventHandler(object sender, EventArgs e);
        // Define a delegate for the window closed event
        public event WindowClosedEventHandler WindowClosed;
        // Define the window closed event using the delegate
        private void OnWindowClosed()
        {
            if (WindowClosed != null)
            {
                WindowClosed(this, EventArgs.Empty);
            }
        }
        private void CloseWindow()
        {
            // Close the window here
            this.Close();
            // Raise the window closed event
            OnWindowClosed();
        }

    }
}


