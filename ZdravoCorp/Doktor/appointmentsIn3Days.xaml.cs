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
    public partial class appointmentsIn3Days : Window
    {
        //fields
        Doctor loggedDoc;
        private ObservableCollection<Appointment> _suitableAppointments;
        public ObservableCollection<Appointment> suitableAppointments
        {
            get { return _suitableAppointments; }
            set { _suitableAppointments = value; }
        }


        //constructor
        public appointmentsIn3Days(Doctor LoggedDoc)
        {
            DataContext = this;
            _suitableAppointments = new ObservableCollection<Appointment>();
            loggedDoc = LoggedDoc;
            InitializeComponent();
        }


        //helper functions
        public void getSeparateAppointmentIntoColl(List<Appointment> list, Doctor loggedDoc)
        {   
            foreach(Appointment a in list)
            {
                if(a.doctorUser == loggedDoc.userName && a.status == "online") {
                    _suitableAppointments.Add(a);
                }
            }
        }
        public DateOnly[] getNextThreeDays()
        {
            DateOnly[] dates = new DateOnly[3];
            for (int i = 1; i != 4; i++)
            {
                DateOnly date = new DateOnly(dateCal.SelectedDate.Value.Year,
                                          dateCal.SelectedDate.Value.Month,
                                          dateCal.SelectedDate.Value.Day);
                dates[i - 1] = date.AddDays(i - 1);
            }
            return dates;
        }
        

        //main functions
        public void selectedDateChanged()
        {
            suitableAppointments.Clear();
            DateOnly[] dates = getNextThreeDays();

            for (int i = 0; i != 3; i++)
            {
                try
                {
                    getSeparateAppointmentIntoColl(loggedDoc.appointmentRepo.allAppointments[dates[i]], loggedDoc);
                }
                catch (System.Collections.Generic.KeyNotFoundException)
                {
                    continue;       //if there are no appointments for selected date
                }
            }

        }


        //gui functionality
        private void dateCal_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDateChanged();
        }
        private void openMedCard_Click(object sender, RoutedEventArgs e)
        {
            Appointment selectedAppointment = (Appointment)listOfAppointments.SelectedItem;
            string SelectedPatUser = selectedAppointment.patientUser;
            medicalBackground win = new medicalBackground(loggedDoc, SelectedPatUser);
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
            selectedDateChanged();
        }
    }
}
