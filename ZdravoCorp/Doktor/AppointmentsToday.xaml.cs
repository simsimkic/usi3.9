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

namespace ZdravoCorp.Doktor
{
    public partial class appointmentsToday : Window
    {
        //fields
        private ObservableCollection<Appointment> _suitableAppointments;
        public ObservableCollection<Appointment> suitableAppointments
        {
            get { return _suitableAppointments; }
            set { _suitableAppointments = value; }
        }
        Doctor loggedDoc;
        public appointmentsToday(Doctor LoggedDoc)
        {
            this.loggedDoc = LoggedDoc;
            DataContext = this;
            _suitableAppointments = new ObservableCollection<Appointment>();
            loadTodaysAppointments();
            InitializeComponent();
        }


        //helper functions
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
        public bool doTimesOverlap(TimeOnly begin, TimeOnly end, TimeOnly appointmentBegin, TimeOnly appointmentEnd)
        {
            if (begin.IsBetween(appointmentBegin, appointmentEnd) || end.IsBetween(appointmentBegin, appointmentEnd) ||
                           (begin <= appointmentBegin && end >= appointmentEnd))   //if they overlap
            {
                return true;
            }
            return false;
        }
        public void loadTodaysAppointments()
        {
            suitableAppointments.Clear();
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            try
            {
                foreach (Appointment a in loggedDoc.appointmentRepo.allAppointments[today])
                {
                    if (a.doctorUser == loggedDoc.userName && a.status == "admitted")
                    {
                        _suitableAppointments.Add(a);
                    }
                }

            }
            catch (System.Collections.Generic.KeyNotFoundException) { }//return empty list but dont end program
        }
        public bool isPatientTooEarly(TimeOnly now, Appointment selectedAppointment)
        {
            if(now < selectedAppointment.timeStart.AddMinutes(-15))
            {
                return true;
            }
            return false;

        }
        public bool checkTimeForExamStart()
        {
            TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);
            Appointment selectedItem = (Appointment)listOfAppointments.SelectedItem;
            if (!isDoctorFree(selectedItem.date, now, now.AddMinutes(1))){  //if current moment overlaps with some appointment doctor is on that appointment
                MessageBox.Show("Doctor not available yet. Wait for your time!");  
                return false;
            }
            if (isPatientTooEarly(now, selectedItem))       //if doctor is now available check if patient came more than 15 minutes before the exam
            {
                MessageBox.Show("It is not time for examination yet. You are to early.");
                return false;
            }
            return true;
        }



        //GUI functionality
        private void openMedCard_Click(object sender, RoutedEventArgs e)
        {
            Appointment selectedItem = (Appointment)listOfAppointments.SelectedItem;
            string patientUser = selectedItem.patientUser;
            medicalBackground win = new medicalBackground(loggedDoc, loggedDoc.patientRepo.returnPatient(patientUser).Username);
            win.Show();
        }
        private void listOfAppointments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Appointment selectedObject = listOfAppointments.SelectedItem as Appointment;
            changeAppointment win = new changeAppointment(selectedObject, this.loggedDoc);
            win.WindowClosed += changeAppointment_WindowClosed;
            win.Show();
        }
        private void changeAppointment_WindowClosed(object sender, EventArgs e)
        {
            loadTodaysAppointments();
        }
        private void examinationWindow_WindowClosed(object sender, EventArgs e)
        {
            loadTodaysAppointments();
        }
        private void startTreatment_Click(object sender, RoutedEventArgs e)
        {
            if(listOfAppointments.SelectedIndex == -1)
            {
                MessageBox.Show("You didnt select anything in the list!");
                return;
            }
           
            if (checkTimeForExamStart())
            {
                Appointment selectedAppointment = (Appointment)listOfAppointments.SelectedItem;
                examinationWindow win = new examinationWindow(loggedDoc.appointmentRepo.getAppointment(selectedAppointment), loggedDoc);
                win.WindowClosed += examinationWindow_WindowClosed;
                win.Show();
            }
        }
    }
    
}
