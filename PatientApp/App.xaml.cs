using ZdravoCorp.Controllers;
using ZdravoCorp.Repository;
using ZdravoCorp.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public  DoctorController DoctorController { get; set; }
        public  PatientController PatientController { get; set; }
        public  PatientAppointmentController PatientAppointmentController { get; set; }
        public App()
        {
            DoctorRepository doctorRepository = new DoctorRepository();
            AnamnesisRepository anamnesisRepository = new AnamnesisRepository();
            PatientRepository patientRepository = new PatientRepository();
            PatientAppointmentRepository patientAppointmentRepository = new PatientAppointmentRepository(doctorRepository, anamnesisRepository);

            DoctorService doctorService = new DoctorService(doctorRepository);
            PatientService  patientService = new PatientService(patientRepository);
            PatientAppointmentService patientAppointmentService = new PatientAppointmentService(patientAppointmentRepository, doctorRepository);

            DoctorController = new DoctorController(doctorService);
            PatientController = new PatientController(patientService);
            PatientAppointmentController = new PatientAppointmentController(patientAppointmentService);

        }
    }
}
