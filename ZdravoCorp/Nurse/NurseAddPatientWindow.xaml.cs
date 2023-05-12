using System;
using System.Linq;
using System.Windows;

namespace ZdravoCorp;

public partial class NurseAddPatientWindow : Window
{
    private PatientRepository patientData;
    private UserRepository userRepository;
    private NurseWindow parent;
    public NurseAddPatientWindow(NurseWindow parent, PatientRepository patientRepository, UserRepository userRepository)
    {
        patientData = patientRepository;
        this.userRepository = userRepository;
        this.parent = parent;
        InitializeComponent();
    }

    private void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        Patient temp = ParsePatient();
        if (temp == null) return;
        patientData.AddPatient(temp);
        parent.populate();
        parent.Visibility = Visibility.Visible;
        Close();
    }

    private Patient ParsePatient()
    {
        if (!validateFields()) return null;
        Patient patient = new Patient();
        patient.Username = UsernameTB.Text;
        patient.Password = PasswordTB.Text;
        patient.FirstName = FirstNameTB.Text;
        patient.LastName = LastNameTB.Text;
        patient.Blocked = (BlockedCB.IsChecked == true ) ? true : false;
        patient.MedicalRecord.Height = int.Parse(HeightTB.Text);
        patient.MedicalRecord.Weight = float.Parse(WeightTB.Text);
        patient.MedicalRecord.Allergies = AllergiesTB.Text.Split(',').ToList();
        patient.MedicalRecord.PastConditions = PastConditionsTB.Text.Split(',').ToList();

        User newPatient = (User)patient;
        userRepository.AddUser(newPatient);
        
        return patient;
    }

    private bool validateFields()
    {
        if (UsernameTB.Text.Length == 0 || PasswordTB.Text.Length == 0 || FirstNameTB.Text.Length == 0 || LastNameTB.Text.Length == 0 || 
            WeightTB.Text.Length == 0 || HeightTB.Text.Length == 0)
        {
            MessageBox.Show("Make sure to fill all the fields");
            return false;
        }
        
        if (patientData.UsernameExists(UsernameTB.Text))
        {
            MessageBox.Show("Username is already in use");
            return false;
        }

        try
        {
            int.Parse(HeightTB.Text);
        }
        catch (Exception e)
        {
            MessageBox.Show("Height not entered correctly");
            return false;
        }

        try
        {
            float.Parse(WeightTB.Text);
        }
        catch (Exception e)
        {
            MessageBox.Show("Weight not entered correctly");
            return false;
        }

        return true;
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        parent.Visibility = Visibility.Visible;
        Close();
    }
}