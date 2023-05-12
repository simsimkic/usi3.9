using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class DoctorWindow : Window
    {
        //fields
        public Doctor doc;
        LoginWindow loginWindow;
        private ObservableCollection<Patient> _searchedPatients;
        public ObservableCollection<Patient> searchedPatients
        {
            get { return _searchedPatients; }
            set { _searchedPatients = value; }
        }
        

        //constructor
        public DoctorWindow(User user, LoginWindow loginWindow)
        {
            doc = (Doctor)user;
            DataContext = this;
            this.searchedPatients = new ObservableCollection<Patient>();
            this.Show();
            this.loginWindow = loginWindow;
            InitializeComponent();
        }


        //main functions
        public void searchPatients()
        {
            searchedPatients.Clear();

            string searchedName = txtInput.Text;
            if(searchedName == "")
            {
                return;
            }
            foreach(Patient p in doc.patientRepo.Patients)
            {
                if (p.FirstName.Contains(searchedName))
                {
                    searchedPatients.Add(p);
                }
            }
        }
        public bool hadAppointmentBefore(Patient p)
        {
            DateTime dateTime = DateTime.Today;
            DateOnly today = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
            foreach (KeyValuePair<DateOnly, List<Appointment>> pair in doc.appointmentRepo.allAppointments)
            {
                foreach(Appointment a in pair.Value)
                {
                    if(a.doctorUser == this.doc.userName && a.patientUser == p.Username && a.date <= today && a.status == "finished")
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //function to determine if doctor can open medCard or not


        //gui functionality
        private void appointmentsTommorowBtn_Click(object sender, RoutedEventArgs e)
        {
            appointmentsTomorrow win = new appointmentsTomorrow(doc);
            win.Show();
         }
        private void appointmentsIn3DaysBtn_Click(object sender, RoutedEventArgs e)
        {   
            appointmentsIn3Days win = new appointmentsIn3Days(doc);
            win.Show();
        }
        private void todaysAppointmentsBtn_Click(object sender, RoutedEventArgs e)
        {
            appointmentsToday win = new appointmentsToday(doc);    
            win.Show();
        }
        private void newAppointmentBtn_Click(object sender, RoutedEventArgs e)
        {
            newAppointment win = new newAppointment(doc);
            win.Show();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loginWindow.Show();
            loginWindow.Visibility = Visibility.Visible;
        }
        public void handlePlaceholder()
        {
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                placeholder.Visibility = Visibility.Visible;
                return;
            }
            placeholder.Visibility = Visibility.Hidden;
        }
        private void txtInput_TextChanged(object sender, TextChangedEventArgs e) 
        {
            handlePlaceholder();
            searchPatients();
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Clear();
            txtInput.Focus();
        }
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Patient selectedItem = (Patient)searchResults.SelectedItem;
            if (hadAppointmentBefore(selectedItem))
            {
                medicalBackground background = new medicalBackground(doc, doc.patientRepo.returnPatient(selectedItem.Username).Username);
                background.Show();
            }
            else
            {
                MessageBox.Show("You dont have access rights for this patient");
            }
        }
        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            doc.patientRepo.WritePatients();
            doc.appointmentRepo.writeAppointments();
            this.Close();
        }

       
    }
}
