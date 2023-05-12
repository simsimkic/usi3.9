using System.Linq;
using System.Windows;

namespace ZdravoCorp.Nurse;

public partial class NurseAdmissionAnamnesis : Window
{
    public Appointment Appointment;
    public NurseAdmissionWindow parent;
    public PatientRepository patientData;
    public Patient patient;
    public AnamnesisRepository anamnesisRepository;
    public appointmentRepository AppointmentRepository;
    
    public NurseAdmissionAnamnesis(NurseAdmissionWindow parent, Appointment appointment, 
        PatientRepository patientData, AnamnesisRepository anamnesisRepository, appointmentRepository appointmentRepository)
    {
        this.Appointment = appointment;
        this.parent = parent;
        this.patientData = patientData;
        this.anamnesisRepository = anamnesisRepository;
        this.AppointmentRepository = appointmentRepository;
        InitializeComponent();
        
        foreach (Patient patient in patientData.Patients) {
            if (patient.Username == this.Appointment.patientUser) {
                this.patient = patient;
                break;
            }
        }
    }

    private void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        // only symptoms cannot be empty
        if (SymptomsTB.Text.Length == 0)
        {
            MessageBox.Show("Symptoms cannot be empty");
            return;
        }

        // update patient
        patient.MedicalRecord.Allergies = AllergiesTB.Text.Split(',').ToList();
        patient.MedicalRecord.PastConditions = PastConditionsTB.Text.Split(',').ToList();
        patientData.UpdatePatients();
        
        // add new anamnesis
        Anamnesis anamnesis = new Anamnesis(SymptomsTB.Text, patient.Username, this.Appointment.timeStart, this.Appointment.date);
        this.anamnesisRepository.AddAnamnesis(anamnesis);
        
        AppointmentRepository.removeApointment(Appointment);
        Appointment.status = "admitted";
        AppointmentRepository.addAppointment(Appointment);
        
        parent.Visibility = Visibility.Visible;
        parent.Show();
        
        parent.populate();
        Close();
    }
    
    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        parent.Visibility = Visibility.Visible;
        parent.Show();
    }
}