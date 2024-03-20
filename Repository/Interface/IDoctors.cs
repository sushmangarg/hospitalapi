using HospitalManagement.Repository.DataClass.Doctors;
using HospitalManagement_Models.Doctors;
using HospitalManagement_Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Repository.Interface
{
    public interface IDoctors 
    {
        #region Doctors
     
        public int InsertUpdateDoctors(DoctorsModel objDoctorsModel);
        public int InsertUpdateDoctorsImagePath(DoctorsModel objDoctorsModel);
        public List<DoctorsModel> GetDoctors(DoctorsModel objDoctors);
        public DoctorsModel GetDoctorsById(int intId);
        public int DeleteDoctorsByid(DoctorsModel objDoctorsModel);
        int UpdateDocotrPassword(DoctorsModel doctorsModel);
        List<DoctorsModel> GetDoctorsWithSpeciality(DoctorsModel objDoctor);
        #endregion

        #region Doctors Availability
        public int InsertUpdateDoctorsAvailability(DoctorsAvailabilityModel objDoctorsAvailabilityModel);

        public int InsertMultipleDoctorsAvailabilityByDate(DoctorsAvailabilityModel objDoctorsAvailabilityModel);
        
        public int InsertUpdateDoctorsAvailability_Multiple(DoctorsAvailabilityModel objDoctorsAvailabilityModel);
        public List<DoctorsModel> GetDoctorsAvailability(DoctorsAvailabilityModel objDoctorsAvailabilityModel);
        public List<DoctorsModel> GetDoctorsAvailabilityByDateandSpecaility(DoctorsAvailabilityModel objDoctorsAvailabilityModel);
        public AllDoctorsAppointementsModel GetAllDoctorsAvailabilitysByDate(DoctorsAvailabilityModel objDoctorsAvailabilityModel);
        public AllDoctorsAppointementsModel GetAllDoctorsAvailabilitysByDateAndPatientCounts(DoctorsAvailabilityModel objDoctorsAvailabilityModel);
        public List<DoctorsAppointementsModel> GetDoctorsAppointementsBySlotId(int intDoctorId,int intStatus);
        public List<DoctorsAppointementsModel> GetDoctorsAppointementsByDate(int intDoctorId,int intTypeId);
        public int DeleteDoctorsAvailabilityByid(DoctorsAvailabilityModel objDoctorsAvailabilityModel);
        #endregion

        #region Doctors Availability History
        public int InsertUpdateDoctorsAvailabilityHistory(DoctorsAvailabilityHistoryModel objDoctorsAvailabilityHistoryModel);
        public List<DoctorsAvailabilityHistoryModel> GetDoctorsAvailabilityHistory();
        public DoctorsAvailabilityHistoryModel GetDoctorsAvailabilityHistoryById(int intId);
        public int DeleteDoctorsAvailabilityHistoryByid(DoctorsAvailabilityHistoryModel objDoctorsAvailabilityHistoryModel);

        #endregion

        #region DoctorsAvailabilitySlots
        public int InsertUpdateDoctorsAvailabilitySlots(DoctorsAvailabilitySlotsModel objDoctorsAvailabilitySlotsModel);
        public List<DoctorsAvailabilitySlotsModel> GetDoctorsAvailabilitySlots();
        public List<DoctorsAvailabilitySlotsModel> GetDoctorsAvailabilitySlotsByDoctorId(int intDoctorId);
        List<DoctorsAvailabilitySlotsModel> GetDoctorsAvailabilitySlotsByIdAndDate(int intDoctorId, string strDate);
        public int DeleteDoctorsAvailabilitySlotsByid(DoctorsAvailabilitySlotsModel objDoctorsAvailabilitySlotsModel);
        DoctorsModel GetDoctorsAvailabilitysByDateAndDocId(DoctorsAvailabilityModel objDoctorsAvailabilityModel);        
        #endregion

        #region DoctorSpecialty
        public int InsertUpdateDoctorSpecialty(DoctorSpecialtyModel objDoctorSpecialtyModel);
        public List<DoctorSpecialtyModel> GetDoctorSpecialty();
        public List<DoctorsModel> GetDoctorSpecialtyById(int intSpecialityId);
        public int DeleteDoctorSpecialtyByid(DoctorSpecialtyModel objDoctorSpecialtyModel);


        #endregion

        #region DoctorsAvailabilitySlotsHistory
        public List<DoctorsAvailabilitySlotsHistoryModel> GetDoctorsAvailabilitySlotsHistory();
        #endregion


        #region Appointments
        //SlotsModel GetDoctorsSlotsAvailabilitysByDateAndDocId(DoctorsAvailabilityModel objDoctorsAvailabilityModel);
        #endregion
        public DoctorsModel DoctorsLogin(DoctorsModel doctors);
    }
}
