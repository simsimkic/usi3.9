using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace ZdravoCorp.Doktor
{
    public partial class appointmentsTomorrow : Window
    {
        //fields
        private ObservableCollection<Appointment> _suitableAppointments;
        public ObservableCollection<Appointment> suitableAppointments
        {
            get { return _suitableAppointments; }
            set { _suitableAppointments = value; }
        }
        public Doctor loggedDoc;
        public appointmentsTomorrow(Doctor LoggedDoc)
        {
            DataContext = this;
            loggedDoc = LoggedDoc;
            _suitableAppointments = new ObservableCollection<Appointment>();
            InitializeComponent();
            loadObservableColl();
        }


        //helper functions
        public void loadObservableColl()
        {
            _suitableAppointments.Clear();
            DateOnly tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            try
            {
                List<Appointment> tomorrowsAppointments = loggedDoc.appointmentRepo.allAppointments[tomorrow];
                foreach (Appointment a in tomorrowsAppointments)
                {
                    if (a.doctorUser == loggedDoc.userName && a.status == "online")
                    {
                        _suitableAppointments.Add(a);
                    }
                }
            }
            catch (System.Collections.Generic.KeyNotFoundException) { }

            //also sets if the button is enabled or disabled depending on list count
            if (suitableAppointments.Count == 0) { openMedCard.IsEnabled = false; }
        }


        //gui functionality
        private void listOfAppointments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Appointment selectedObject = listOfAppointments.SelectedItem as Appointment;
            changeAppointment win = new changeAppointment(selectedObject, this.loggedDoc);
            win.WindowClosed += changeAppointment_WindowClosed;
            win.Show();
        }
        private void changeAppointment_WindowClosed(object sender, EventArgs e)
        {
            loadObservableColl();
        }
        private void openMedCard_Click(object sender, RoutedEventArgs e)
        {
            Appointment selectedItem = (Appointment)listOfAppointments.SelectedItem;
            string selectedPatientUser = selectedItem.patientUser;
            medicalBackground win = new medicalBackground(loggedDoc, selectedPatientUser);
            win.Show();
        }
       
    }
}
