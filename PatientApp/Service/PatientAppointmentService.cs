using ZdravoCorp.Model;
using ZdravoCorp.Repository;
using ZdravoCorp.View;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZdravoCorp.Service
{
    public class PatientAppointmentService
    {
        private PatientAppointmentRepository _patientAppointmentRepository;
        private DoctorRepository _doctorRepository;
        public PatientAppointmentService(PatientAppointmentRepository patientAppointmentRepository, DoctorRepository doctorRepository)
        {
            _patientAppointmentRepository = patientAppointmentRepository;
            _doctorRepository = doctorRepository;
        }

        public List<PatientAppointment> GetAll()
        {
            return _patientAppointmentRepository.GetAll();
        }

        public PatientAppointment Save(PatientAppointment patientAppointment)
        {
            return _patientAppointmentRepository.Save(patientAppointment);
        }


        public bool DatesIntertwine(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return start1 < end2 && end1 > start2;

        }

        public bool IsIntervalFree(DateTime start, DateTime end, List<PatientAppointment> patientAppointments)
        {
            foreach (PatientAppointment patientAppointment in patientAppointments)
            {
                if (DatesIntertwine(start, end, patientAppointment.Start, patientAppointment.Start.AddMinutes(patientAppointment.Duration)))
                {
                    return false;
                }
            }
            return true;
        }

        public DateTime? FindFreeAppointment(DateTime start, DateTime end, List<PatientAppointment> doctorsAppointments)
        {
            DateTime iter = start;
            while (iter.AddMinutes(15) <= end)
            {
                if (IsIntervalFree(iter, iter.AddMinutes(15), doctorsAppointments))
                {
                    return iter;
                }
                iter = iter.AddMinutes(15);
            }
            return null;
        }

        public PatientAppointment FindAppointmentDoctorPriority(AppointmentRequest appointmentRequest)
        {
            List<PatientAppointment> doctorsAppointments = _patientAppointmentRepository.GetAppointmentsForDoctor(appointmentRequest.Doctor.Id);

            Patient patient = new Patient() { Id = View.SignIn.LoggedUser.Id }; 


            DateTime start = appointmentRequest.WantedStart;
            DateTime end = appointmentRequest.WantedEnd;
            PatientAppointment patientAppointment = null;


            while (start.Date <= appointmentRequest.LatestDate.Date)
            {
                DateTime? foundDate = FindFreeAppointment(start, end, doctorsAppointments);
                if (foundDate != null)
                {
                    patientAppointment = new PatientAppointment(-1, appointmentRequest.Doctor, patient, (DateTime)foundDate, 15);
                    patientAppointment = _patientAppointmentRepository.Save(patientAppointment);
                    break;
                }
                start = start.AddDays(1);
                end = end.AddDays(1);
            }


            return patientAppointment;
        }

        public PatientAppointment FindAppointmentDatePriority(AppointmentRequest appointmentRequest)
        {
            Patient patient = new Patient() { Id = View.SignIn.LoggedUser.Id }; 

            DateTime start = appointmentRequest.WantedStart;
            DateTime end = appointmentRequest.WantedEnd;
            List<Doctor> availableDoctors;

            PatientAppointment patientAppointment = null;

            List<PatientAppointment> doctorAppointments = _patientAppointmentRepository.GetAppointmentsForDoctor(appointmentRequest.Doctor.Id);


            DateTime? foundDate = FindFreeAppointment(start, end, doctorAppointments);
            if (foundDate != null)
            {
                patientAppointment = new PatientAppointment(-1, appointmentRequest.Doctor, patient, (DateTime)foundDate, 15);
                patientAppointment = _patientAppointmentRepository.Save(patientAppointment);
                return patientAppointment;


            }

            availableDoctors = _doctorRepository.GetAllDoctors();
            foreach (var doctor in availableDoctors)
            {
                doctorAppointments = _patientAppointmentRepository.GetAppointmentsForDoctor(doctor.Id);


                foundDate = FindFreeAppointment(start, end, doctorAppointments);
                if (foundDate != null)
                {
                    patientAppointment = new PatientAppointment(-1, doctor, patient, (DateTime)foundDate, 15);
                    patientAppointment = _patientAppointmentRepository.Save(patientAppointment);
                    return patientAppointment;

                }
            }

            return null;
        }


        public PatientAppointment GetAppointmentForRequest(AppointmentRequest appointmentRequest)
        {
            if (appointmentRequest.Priority == Model.Enums.Priority.Doctor)
            {
                return FindAppointmentDoctorPriority(appointmentRequest);
            }
            else
            {
                return FindAppointmentDatePriority(appointmentRequest);
            }
        }



        public List<PatientAppointment> FindCLoseOneDoctorPrior(AppointmentRequest appointmentRequest)
        {
            List<PatientAppointment> doctorsAppointments = _patientAppointmentRepository.GetAppointmentsForDoctor(appointmentRequest.Doctor.Id);

            Patient patient = new Patient() { Id = View.SignIn.LoggedUser.Id }; 


            DateTime start = appointmentRequest.WantedStart.AddHours(-2);
            DateTime end = appointmentRequest.WantedEnd.AddHours(2);
            PatientAppointment patientAppointment = null;
            List<PatientAppointment> validAppointments = new List<PatientAppointment>();


            while (validAppointments.Count < 3)
            {
                DateTime? foundDate = FindFreeAppointment(start, end, doctorsAppointments);
                if (foundDate != null)
                {
                    patientAppointment = new PatientAppointment(-1, appointmentRequest.Doctor, patient, (DateTime)foundDate, 15);
                    validAppointments.Add(patientAppointment);
                }
                start = start.AddDays(1);
                end = end.AddDays(1);
            }


            return validAppointments;

        }

        public List<PatientAppointment> FindCLoseOneTimePrior(AppointmentRequest appointmentRequest)
        {

            Patient patient = new Patient() { Id = View.SignIn.LoggedUser.Id };


            DateTime start = appointmentRequest.WantedStart.AddHours(-2);
            DateTime end = appointmentRequest.WantedEnd.AddHours(2);
            PatientAppointment patientAppointment = null;
            List<PatientAppointment> validAppointments = new List<PatientAppointment>();
            List<Doctor> availableDoctors = _doctorRepository.GetAllDoctors();

            while (validAppointments.Count < 3)
            {
                foreach (Doctor doctor in availableDoctors)
                {
                    List<PatientAppointment> doctorsAppointments = _patientAppointmentRepository.GetAppointmentsForDoctor(doctor.Id);

                    DateTime? foundDate = FindFreeAppointment(start, end, doctorsAppointments);
                    if (foundDate != null)
                    {
                        patientAppointment = new PatientAppointment(-1, doctor, patient, (DateTime)foundDate, 15);
                        validAppointments.Add(patientAppointment);
                    }
                }
                start = start.AddDays(1);
                end = end.AddDays(1);
            }


            return validAppointments.Take(3).ToList();

        }


        public List<PatientAppointment> FindCloseOnes(AppointmentRequest appointmentRequest)
        {
            if(appointmentRequest.Priority == Model.Enums.Priority.Doctor)
            {
                return FindCLoseOneDoctorPrior(appointmentRequest);
            }
            else
            {
                return FindCLoseOneTimePrior(appointmentRequest);
            }
        }




        public List<PatientAppointment> GetPreviousPatientAppointments(int userId)
        {
            return _patientAppointmentRepository.GetAll()
                .Where(a => a.Patient.Id == userId && a.Start < DateTime.Now)
                .ToList();
        }

        public List<PatientAppointment> GetPatientAppointments(int userId)
        {
            return _patientAppointmentRepository.GetAll()
                .Where(a => a.Patient.Id == userId)
                .ToList();
        }

        public List<PatientAppointment> GetSearchedAppointments(string searchParam, int patientId)
        {
            return GetPreviousPatientAppointments(patientId).Where(ap =>( ap.Anamnesis == null)? false:(ap.Anamnesis.Record.ToLower().Contains(searchParam.ToLower()))).ToList();

        }
}
}


