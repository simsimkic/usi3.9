using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;

namespace ZdravoCorp.Nurse;

public partial class NurseAdmissionWindow : Window
{
    public Patient Patient;
    public appointmentRepository appointmentData;
    public AnamnesisRepository AnamnesisData;
    public PatientRepository PatientData;
    NurseWindow parent;
    public NurseAdmissionWindow(NurseWindow parent, appointmentRepository appointmentData, 
        Patient patient, PatientRepository patientData, AnamnesisRepository anamnesisData)
    {
        this.appointmentData = appointmentData;
        this.Patient = patient;
        this.parent = parent;
        this.PatientData = patientData;
        this.AnamnesisData = anamnesisData;
        InitializeComponent();
        populate();
    }
    
    public void populate()
    {
        List.Items.Clear();
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        if (!appointmentData.allAppointments.ContainsKey(today)) {
            return;
        }

        List<Appointment> todayAppointments = appointmentData.allAppointments[today];
        foreach (Appointment appointment in todayAppointments) {
            if (appointment.patientUser == this.Patient.Username)
            {
                List.Items.Add(appointment);
            }
        }
    }
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        parent.Visibility = Visibility.Visible;
        parent.Show();
    }

    private void Admit_OnClick(object sender, RoutedEventArgs e)
    {
        int index = List.SelectedIndex;
        if (index == -1) return;
        
        // check if start time <= 15 min from now
        TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);

        Appointment appointment = (Appointment)List.Items[index];
        if (appointment.status != "online")
        {
            MessageBox.Show("You cannot admit already admitted.");
            return;
        }

        if (now.IsBetween(appointment.timeStart.AddMinutes(-15), appointment.timeEnd))
        {
            NurseAdmissionAnamnesis nurseAdmissionAnamnesis = new NurseAdmissionAnamnesis(this, appointment,
                PatientData, AnamnesisData, appointmentData);
            this.Visibility = Visibility.Hidden;
            nurseAdmissionAnamnesis.Show();
        }
        else
        {
            MessageBox.Show("You cannot admit now.");
        }
    }
}