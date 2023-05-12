using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Doktor;

namespace ZdravoCorp.Nurse;

public partial class NurseUrgentWindow : Window
{
    public NurseWindow parent;
    public Patient patient;
    public UserRepository UserRepository;
    public appointmentRepository AppointmentRepository;
        
    public NurseUrgentWindow(NurseWindow parent, Patient patient, UserRepository userRepository, appointmentRepository appointmentRepository)
    {
        this.parent = parent;
        this.patient = patient;
        this.UserRepository = userRepository;
        this.AppointmentRepository = appointmentRepository;
        InitializeComponent();
    }

    private void NurseUrgentWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        this.parent.Visibility = Visibility.Visible;
        parent.Show();
    }

    private void ScheduleAppointmentButton_OnClick(object sender, RoutedEventArgs e)
    {
        // find all doctors with the specific specialization
        var doctors = findAllDoctors();
        if (doctors.Count == 0) {
            MessageBox.Show("Doctor with the given specialization does not exist.");
            return;
        }
        else {
        }
    }

    private List<Doctor> findAllDoctors()
    {
        var specialization = (String)((ComboBoxItem)SpecializationCombo.SelectedValue).Content;
        var doctors = new List<Doctor>();
        foreach (User user in UserRepository._users) {
            if (user.Username.Equals(specialization))
            {
                doctors.Add((Doctor) user);
            }
        }
        return doctors;
    }

    private List<Appointment> findAllAppointemntsForADoctor(string doctorUsername)
    {
        var res = new List<Appointment>();
        foreach (Appointment appointment in AppointmentRepository.allAppointments[DateOnly.FromDateTime(DateTime.Now)])
        {
            if (appointment.doctorUser == doctorUsername) res.Add(appointment);
        }
        
        // sort appointments
        res.Sort((ap1, ap2) => ap1.timeStart.CompareTo(ap2.timeStart));
        
        return res;
    }
}