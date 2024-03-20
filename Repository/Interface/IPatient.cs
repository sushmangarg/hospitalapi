using HospitalManagement_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Repository.Interface
{
   public interface IPatient
    {
        public PatientModel PatientLogin(PatientModel patients);
        public int InsertUpdatePatient(PatientModel objPatientModel);
        public int InsertUpdatePatientImagePath(PatientModel objPatientModel);
        int UpdatePatientPassword(PatientModel patientModel);
        public List<PatientModel> GetPatient(string strSearch);
        public List<PatientModel> GetPatientFamily(int intParentId);
        List<PatientModel> GetPatientAndFamily(int intParentId);
        public List<PatientModel> GetPatientsByMobileNumber(String strMobileNumber);
        public PatientModel GetPatientById(int intId);
        public int DeletePatientByid(PatientModel objPatientModel);
        public int InsertUpdatePatientAppointements(PatientAppointmentModel objPatientAppointmentModel);
        int InsertReschedulePatientAppointementsAdmin(PatientAppointmentModel objpatientAppointmentModel);
        int InsertReschedulePatientAppointements(PatientAppointmentModel objpatientAppointmentModel); 
        int CancelPatientAppointements(PatientAppointmentModel objpatientAppointmentModel);
        int CompletePatientAppointements(PatientAppointmentModel objpatientAppointmentModel);
        public List<AppointementsModel> GetAllAppointements();
        public List<PatientAppointmentModel> GetPatientAppointmentsByintDoctorId(int intDoctorId, int intStatus);
        public List<PatientAppointmentModel> GetPatientAppointmentsByintPatientId(int intPatientId,int intStatus);
        List<PatientModel> GetPatientByDocIdSlotId(int intSlotId);
        List<PatientModel> GetAllPatientByDocIdSlotId(int intSlotId);
        int InsertUpdatePatientHealthCheckUp(PatientModel objPatientModel);
        List<PatientModel> GetPatientHealthCheckUpByPatientId(int intPatientId);
        int DeletePatientHealthCheckByPatientId(int intBodyMeasurementId);
        PatientModel GetPatientHealthCheckUpByPatientHealthId(int intPatientHealthId);

        List<PatientAppointmentModel> GetPatientAppointmentsHistoryByintPatientAppointmentId(int intPatientAppointmentId);
    }
}
