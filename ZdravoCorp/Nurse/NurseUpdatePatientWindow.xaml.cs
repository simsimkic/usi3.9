using System;
using System.Linq;
using System.Threading;
using System.Windows;

namespace ZdravoCorp;

public partial class NurseUpdatePatientWindow : Window
{
    private NurseWindow parent;
    private PatientRepository patientRepository;
    private Patient patient;
    private string originalUsername;
    
    public NurseUpdatePatientWindow(NurseWindow parent, PatientRepository patientRepository, Patient patient)
    {
        this.parent = parent;
        this.patientRepository = patientRepository;
        this.patient = patient;
        InitializeComponent();
        
        // TODO: actual username
        originalUsername = (string)patient.FirstName.Clone();
        
        // populate gui with already existing data
        UsernameTB.Text = patient.Username;
        PasswordTB.Text = patient.Password;
        FirstNameTB.Text = patient.FirstName;
        LastNameTB.Text = patient.LastName;
        BlockedCB.IsChecked = patient.Blocked;
        HeightTB.Text = patient.MedicalRecord.Height.ToString();
        WeightTB.Text = patient.MedicalRecord.Weight.ToString();
        AllergiesTB.Text = string.Join(",", patient.MedicalRecord.Allergies);
        PastConditionsTB.Text = string.Join(",", patient.MedicalRecord.PastConditions);
    }

    private void UpdateButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!ParsePatient()) return;
        patientRepository.UpdatePatients();
        parent.populate();
        parent.Visibility = Visibility.Visible;
        Close();
    }
    
    private bool ParsePatient()
    {
        if (!validateFields()) return false;
        patient.Username = UsernameTB.Text;
        patient.Password = PasswordTB.Text;
        patient.FirstName = FirstNameTB.Text;
        patient.LastName = LastNameTB.Text;
        patient.Blocked = (BlockedCB.IsChecked == true ) ? true : false;
        patient.MedicalRecord.Height = int.Parse(HeightTB.Text);
        patient.MedicalRecord.Weight = float.Parse(WeightTB.Text);
        patient.MedicalRecord.Allergies = AllergiesTB.Text.Split(',').ToList();
        patient.MedicalRecord.PastConditions = PastConditionsTB.Text.Split(',').ToList();
        return true;
    }

    private bool validateFields()
    {
        if (UsernameTB.Text.Length == 0 || PasswordTB.Text.Length == 0 ||FirstNameTB.Text.Length == 0 || LastNameTB.Text.Length == 0 || 
            WeightTB.Text.Length == 0 || HeightTB.Text.Length == 0)
        {
            MessageBox.Show("Make sure to fill all the fields");
            return false;
        }

        if (!originalUsername.Equals(UsernameTB.Text))
        {
            if (patientRepository.UsernameExists(UsernameTB.Text))
            {
                MessageBox.Show("Username is already in use");
                return false;
            }
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