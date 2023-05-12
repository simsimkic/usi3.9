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
    public partial class medicalBackground : Window
    {
        //fileds
        private ObservableCollection<Appointment> _suitableAppointments;
        public ObservableCollection<Appointment> suitableAppointments
        {
            get { return _suitableAppointments; }
            set { _suitableAppointments = value; }
        }
        private ObservableCollection<string> _diseases;
        public ObservableCollection<string> diseases
        {
            get { return _diseases; }
            set { _diseases = value; }
        }
        private ObservableCollection<string> _allergies;
        public ObservableCollection<string> allergies
        {
            get { return _allergies; }
            set { _allergies = value; }
        }
        Patient selectedPatient;
        Doctor loggedDoc;


        //constructors
        public medicalBackground(Doctor LoggedDoc, string patientUser)
        {
            DataContext = this;
            loggedDoc = LoggedDoc;
            selectedPatient = loggedDoc.patientRepo.returnPatient(patientUser);
            _diseases = new ObservableCollection<string>();
            _allergies = new ObservableCollection<string>();
            _suitableAppointments = new ObservableCollection<Appointment>();
            InitializeComponent();
            PatInfoCB.Checked += PatInfoCB_CheckedChanged;
            PatInfoCB.Unchecked += PatInfoCB_CheckedChanged;
            loadComponents();
        }



        //helper functions
        public void loadComponents()
        {
            patLastName.Text = selectedPatient.LastName;
            patName.Text = this.selectedPatient.FirstName;
            patSex.Text = selectedPatient.MedicalRecord.Gender;
            patWeight.Text = selectedPatient.MedicalRecord.Weight.ToString();
            patHeight.Text = selectedPatient.MedicalRecord?.Height.ToString();
            patAge.Text = selectedPatient.MedicalRecord.Age.ToString();

            foreach (string d in this.selectedPatient.MedicalRecord.PastConditions)
            {
                _diseases.Add(d);
            }
            foreach(string a in this.selectedPatient.MedicalRecord.Allergies)
            {
                _allergies.Add(a);
            }
        }
        public DateOnly getDate()
        {
            DateTime dateTime = (DateTime)dateOfApp.SelectedDate;
            DateOnly date = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
            return date;
        }
        public bool alreadyAdded(string toAdd, ObservableCollection<string> collection) //for adding diseases and allergies
        {
            foreach(string s in collection)
            {
                if(s == toAdd)
                {
                    return true;
                }
            }
            return false;
        }
        public bool isDiseaseSelected()
        {
            if(patDiseasesList.SelectedIndex == -1)
            {
                return false;
            }
            return true;
        }
        public bool isAllergySelected()
        {
            if(patAllergiesList.SelectedIndex == -1)
            {
                return false;
            }
            return true;
        }
        public bool floatValidation(string toCheck)
        {
            try
            {
                float number = float.Parse(toCheck);
                if (number < 0) { return false; }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool numberValidation(string toCheck)
        {
            try 
            {
                int number = int.Parse(toCheck);
                if(number < 0) { return false; }
                return true;
            }
            catch
            {
                return false;   
            }
        }
        public bool sexValidation(string toCheck)
        {
            if(toCheck != "male" && toCheck != "female")
            {
                return false;
            }
            return true;
        }
        public bool dataValidation()
        {
            if (!sexValidation(patSex.Text)) { MessageBox.Show("Bad sex entered"); return false; }
            if(!numberValidation(patAge.Text)) { MessageBox.Show("Bad age entered"); return false; };
            if (!numberValidation(patHeight.Text)) { MessageBox.Show("Bad height entered"); return false; };
            if (!floatValidation(patWeight.Text)) { MessageBox.Show("Bad weight entered"); return false; };
            return true;
        }
        public List<string> makeListFromGUI(ObservableCollection<string> collection)
        {
            List<string> newList = new List<string>();
            foreach(string s in collection)
            {
                newList.Add(s);
            }
            return newList;
        }
       

        //main functions
        public void addDisease()
        {
            string toAdd = newDiseaseTB.Text;
            if(alreadyAdded(toAdd, diseases))
            {
                MessageBox.Show("Patient already has this disease");
                return;
            }
            if(toAdd.Length == 0) 
            {
                MessageBox.Show("You didnt give disease any name");
                return;
            }
            diseases.Add(toAdd);
        }
        public void deleteDisease()
        {
            if (isDiseaseSelected())
            {
                string toDelete = patDiseasesList.SelectedValue.ToString();
                diseases.Remove(toDelete);
                return;
            }
            MessageBox.Show("You didnt select any disease!");
        }
        public void addAllergy()
        {
            string toAdd = newAllergyTB.Text;
            if(alreadyAdded(toAdd, allergies))
            {
                MessageBox.Show("Patient already has this allergy");
                return;
            }
            if (toAdd.Length == 0)
            {
                MessageBox.Show("You didnt give allergy any name");
                return;
            }
            allergies.Add(toAdd);
        }
        public void deleteAllergy()
        {
            if (isAllergySelected())
            {
                string toDelete = patAllergiesList.SelectedValue.ToString();
                allergies.Remove(toDelete);
                return;
            }
            MessageBox.Show("You didnt select any allergy");
        }
        public bool changeMedicalRecord()
        {
            if(!dataValidation()) { return false;}
            selectedPatient.MedicalRecord.Age = int.Parse(patAge.Text);
            selectedPatient.MedicalRecord.Gender = patSex.Text;
            selectedPatient.MedicalRecord.Height = int.Parse(patHeight.Text);
            selectedPatient.MedicalRecord.Weight = float.Parse(patWeight.Text);
            selectedPatient.MedicalRecord.Allergies = makeListFromGUI(allergies);
            selectedPatient.MedicalRecord.PastConditions = makeListFromGUI(diseases);
            return true;
        }


        //gui functionality
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {   
            _suitableAppointments.Clear();
            DateOnly selectedDate = getDate();
            try
            {
                List<Appointment> appointmentsOfTheDay = loggedDoc.appointmentRepo.allAppointments[selectedDate];
                foreach (Appointment a in appointmentsOfTheDay)
                {
                    if (a.patientUser == this.selectedPatient.Username)
                    {
                        _suitableAppointments.Add(a);
                    }
                }
            }
            catch (System.Collections.Generic.KeyNotFoundException) { }//return empty list but dont end program
           
        }
        private void PatInfoCB_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (PatInfoCB.IsChecked == true)
            {
                patAge.IsEnabled = true;
                patSex.IsEnabled = true;
                patHeight.IsEnabled = true;
                patWeight.IsEnabled = true;
                patDiseasesList.IsEnabled = true;
                patAllergiesList.IsEnabled = true;
                newDiseaseTB.IsEnabled = true;
                newAllergyTB.IsEnabled = true;
                addDiseaseBtn.IsEnabled = true;
                addAllergyBtn.IsEnabled = true;
                deleteAllergyBtn.IsEnabled = true;
                deleteDiseaseBtn.IsEnabled = true;
                SaveChangesBtn.IsEnabled = true;
            }
            else
            {
                patAge.IsEnabled = false;
                patSex.IsEnabled = false;
                patHeight.IsEnabled = false;
                patWeight.IsEnabled = false;
                patDiseasesList.IsEnabled = false;
                patAllergiesList.IsEnabled = false;
                newDiseaseTB.IsEnabled = false;
                newAllergyTB.IsEnabled = false;
                addDiseaseBtn.IsEnabled = false;
                addAllergyBtn.IsEnabled = false;
                deleteAllergyBtn.IsEnabled = false;
                deleteDiseaseBtn.IsEnabled = false;
                SaveChangesBtn.IsEnabled = false;
            }
        }
        private void AddDiseaseBtn_Click(object sender, RoutedEventArgs e)
        {
            addDisease();
            newDiseaseTB.Text = string.Empty;
        }
        private void DeleteDiseaseBtn_Click(object sender, RoutedEventArgs e)
        {         
            deleteDisease();
        }
        private void AddAllergyBtn_Click(object sender, RoutedEventArgs e)
        {      
            addAllergy();
            newAllergyTB.Text = string.Empty;
        }
        private void DeleteAllergyBtn_Click(object sender, RoutedEventArgs e)
        {        
            deleteAllergy();

        }
        private void SaveChangesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (changeMedicalRecord())
            {
                this.Close();
            }
        }

    }

}
