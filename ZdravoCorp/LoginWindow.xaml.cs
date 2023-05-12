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
using ZdravoCorp;
using ZdravoCorp.Doktor;
using ZdravoCorp.Notifications;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            LoginBtn.Click += LoginBtn_Click;
        }

        
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            loginValidation();
        }
        private void loginValidation()
        {
            String userName = UserNameTextBox.Text;
            String password = PasswordTextBox.Password;

            User loggedInUser = new User(userName, password);

            if (((App)App.Current).Login(loggedInUser))
            {
                openCorrespondingRoleWindow();
                this.Visibility = Visibility.Hidden;
                
                // show notifications first
                var notifications = ((App)App.Current).NotificationRepository.GetAllNotificationsByUsername(userName);
                if (notifications.Count > 0)
                {
                    NotificationsWindow notificationsWindow = new NotificationsWindow(null, notifications);
                    notificationsWindow.Show();
                }
            }
            else
            {
                MessageBox.Show("Greska pri logovanju");
            }

        }
        private void openCorrespondingRoleWindow()
        {
            switch (((App)App.Current).LoggedInUser.Role)
            {
                case Role.NURSE:
                    openNurseWindow();
                    break;
                case Role.MANAGER:
                    openManagerWindow();
                    break;
                case Role.DOCTOR:
                    openDoctorWindow();
                    break;
            }
        }
        private void openNurseWindow()
        {
            NurseWindow nurseWindow = new NurseWindow(((App)App.Current).patientRepository, 
                            ((App)App.Current).AppointmentRepository, 
                            this,
                            ((App)App.Current).AnamnesisRepository, 
                            ((App)App.Current).UserRepository);
            nurseWindow.Show();
            nurseWindow.Visibility = Visibility.Visible;
        }
        private void openManagerWindow()
        {
            ManagerWindow managerWindow = new ManagerWindow(this);
            managerWindow.Show();
            managerWindow.Visibility = Visibility.Visible;
        }
        private void openDoctorWindow()
        {
            DoctorWindow doctorWindow = new DoctorWindow(((App)App.Current).LoggedInUser, this);
            doctorWindow.Show();
            doctorWindow.Visibility = Visibility.Visible;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}
