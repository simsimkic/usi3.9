using System.IO.Compression;
using System.Windows;
using ZdravoCorp.Nurse;

namespace ZdravoCorp;

public partial class NurseWindow : Window
{
    private PatientRepository patientData;
    private appointmentRepository appointmentData;
    private AnamnesisRepository AnamnesisRepository;
    private UserRepository userRepository;
    public LoginWindow loginWindow;
    public NurseWindow(PatientRepository patientData, appointmentRepository appointmentData, LoginWindow loginWindow,
        AnamnesisRepository anamnesisRepository, UserRepository userRepository)
    {
        this.loginWindow = loginWindow;
        this.patientData = patientData;
        this.appointmentData = appointmentData;
        this.AnamnesisRepository = anamnesisRepository;
        this.userRepository = userRepository;
        InitializeComponent();
        populate();
    }

    public void populate()
    {
        List.Items.Clear();
        foreach (Patient patient in patientData.Patients)
        {
            List.Items.Add(patient);
        }
    }

    private void AddPatientButton_OnClick(object sender, RoutedEventArgs e)
    {
        NurseAddPatientWindow addPatientWindow = new NurseAddPatientWindow(this, patientData, userRepository);
        this.Visibility = Visibility.Hidden;
        addPatientWindow.Show();
    }

    private void DeletePatientButton_OnClick(object sender, RoutedEventArgs e)
    {
        int index = List.SelectedIndex;
        if (index == -1) return;
        patientData.RemovePatient(patientData.Patients[index].FirstName);
        populate();
    }

    private void UpdatePatientButton_OnClick(object sender, RoutedEventArgs e)
    {
        int index = List.SelectedIndex;
        if (index == -1) return;
        NurseUpdatePatientWindow updatePatientWindow = new NurseUpdatePatientWindow(this,patientData, patientData.Patients[index]);
        this.Visibility = Visibility.Hidden;
        updatePatientWindow.Show();
    }

    private void LogOutButton_OnClick(object sender, RoutedEventArgs e)
    {
        loginWindow.Visibility = Visibility.Visible;
        loginWindow.Show();
        Close();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        loginWindow.Visibility = Visibility.Visible;
        loginWindow.Show();
    }

    private void AdmitPatientButton_OnClick(object sender, RoutedEventArgs e)
    {
        int index = List.SelectedIndex;
        if (index == -1) return;
        NurseAdmissionWindow nurseAdmissionWindow = new NurseAdmissionWindow(this, appointmentData, patientData.Patients[index], patientData, this.AnamnesisRepository);
        this.Visibility = Visibility.Hidden;
        nurseAdmissionWindow.Show();
    }

    private void UrgentButton_OnClick(object sender, RoutedEventArgs e)
    {
        int index = List.SelectedIndex;
        if (index == -1) return;
        Patient patient = (Patient) List.Items[index];
        NurseUrgentWindow nurseUrgentWindow = new NurseUrgentWindow(this, patient, userRepository, appointmentData);
        this.Visibility = Visibility.Hidden;
        nurseUrgentWindow.Show();
    }
}