using ZdravoCorp.Model;
using ZdravoCorp.Service;
using System.Collections.Generic;

namespace ZdravoCorp.Controllers
{
    public class PatientAppointmentController
    {
        private PatientAppointmentService _patientAppointmentService;
        public PatientAppointmentController(PatientAppointmentService patientAppointmentService)
        {
            _patientAppointmentService = patientAppointmentService;
        }

        public List<PatientAppointment> GetAll()
        {
            return _patientAppointmentService.GetAll();
        }


        public PatientAppointment Save(PatientAppointment patientAppointment)
        {
            return _patientAppointmentService.Save(patientAppointment);
        }

        public PatientAppointment GetAppointmentForRequest(AppointmentRequest appointmentRequest)
        {
            return _patientAppointmentService.GetAppointmentForRequest(appointmentRequest);
        }

        public List<PatientAppointment> FindCloseOnes(AppointmentRequest appointmentRequest)
        {
            return _patientAppointmentService.FindCloseOnes(appointmentRequest);
        }

        public List<PatientAppointment> GetPreviousPatientAppointments(int userId)
        {
            return _patientAppointmentService.GetPreviousPatientAppointments(userId);
        }
        public List<PatientAppointment> GetPatientAppointments(int userId)
        {
            return _patientAppointmentService.GetPatientAppointments(userId);
        }

        public List<PatientAppointment> GetSearchedAppointments(string searchParam, int patientId)
        {
            return _patientAppointmentService.GetSearchedAppointments(searchParam, patientId); 

        }
}
}
