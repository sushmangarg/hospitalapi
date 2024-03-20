using HospitalManagement_Models;
using HospitalManagement_Models.Doctors;
using HospitalManagement_Models.Master;
using ISM_AppTrackerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Repository.Interface.Master
{
    public interface IMaster
    {
        #region City
        public int InsertUpdateCity(CityModel objCityModel);
        public List<CityModel> GetCity();
        public CityModel GetCityById(int id);
        public int DeleteCityByid(CityModel objCityModel);

        #endregion

        #region Locations
        public int InsertUpdateLocations(LocationsModel objLocationsModel);
        public List<LocationsModel> GetLocations();
        public LocationsModel GetLocationsById(int intId);
        public int DeleteLocationByid(LocationsModel objLocationsModel);

        #endregion

        #region Slots
        public int InsertUpdateSlots(SlotsModel objSlotsModel);
        public List<SlotsModel> GetSlots();
        public List<SlotsModel> GetAllSlots();
        public List<SlotsModel> GetSlotsById(int intDoctorId);
        public int DeleteSlotsByid(SlotsModel objSlotsModel);
        #endregion

        #region Speciality
        public int InsertUpdateSpeciality(SpecialityModel objSpecialityModel);
        public List<SpecialityModel> GetSpeciality();
        List<SpecialityModel> GetAllSpeciality();
        public SpecialityModel GetSpecialityById(int intId);
        public int DeleteSpecialityById(SpecialityModel objSpecialityModel);
        List<DoctorSpecialityModel> GetActiveDoctorSpeciality(DoctorSpecialityModel objDoctorSpeciality);
        #endregion

        #region country state district
        public List<CountryModel> GetCountry();
        List<CountryModel> GetCountryForMaster();
        int InsertUpdateCountry(CountryModel objCountryModel);
        int DeleteCountryById(CountryModel objCountryModel);
        public List<StateModel> GetStateByCountryId(int intCountryID);
        public List<CityModel> GetCityByStateId(int intStateId);

        List<StateModel> GetStates();
        int DeleteStateyById(StateModel objStateModel);

        int InsertUpdateState(StateModel objStateModel);
        #endregion

        #region Doctor Skills
        public int InsertUpdateDoctorSkills(DoctorSkillsModel objDoctorSkills);
        public List<DoctorSkillsModel> GetDoctorSkillsByDoctorId(DoctorSkillsModel objDoctorSkills);
        public int DeleteDoctorSkillByDoctorId(DoctorSkillsModel objDoctorSkills);

        public DoctorSkillsModel GetDoctorSkillsById(DoctorSkillsModel objDoctorSkills);
        #endregion

        #region Doctor Speciality
        public int InsertUpdateDoctorSpeciality(DoctorSpecialityModel objDoctorSpeciality);
        public List<DoctorSpecialityModel> GetDoctorSpecialityByDoctorId(DoctorSpecialityModel objDoctorSpeciality);
        int GetDoctorSpecialityId(DoctorSpecialityModel objDoctorSpeciality);
        public int DeleteDoctorSpecialityByDoctorId(DoctorSpecialityModel objDoctorSpeciality);

        DoctorSpecialityModel GetDoctorSpecialityById(DoctorSpecialityModel objDoctorSpecialityModel);
        #endregion

        #region Doctor Education
        public int InsertUpdateDoctorEducation(DoctorEducationModel objDoctorEducation);
        public List<DoctorEducationModel> GetDoctorEducationByDoctorId(DoctorEducationModel objDoctorEducation);
        public int DeleteDoctorEducationByDoctorId(DoctorEducationModel objDoctorEducation);
        public DoctorEducationModel GetDoctorEducationById(DoctorEducationModel objDoctorEducation);
        
        #endregion

        #region Doctor Experience
        public int InsertUpdateDoctorExperience(DoctorExperienceModel objDoctorExperience);
        public List<DoctorExperienceModel> GetDoctorExperienceByDoctorId(DoctorExperienceModel objDoctorExperience);
        public int DeleteDoctorExperienceByDoctorId(DoctorExperienceModel objDoctorExperience);
        public DoctorExperienceModel GetDoctorExperienceById(DoctorExperienceModel objDoctorExp);
        #endregion

        #region Doctor Slot count
        public int InsertUpdateDoctorSlotsCount(DoctorSlots objDoctorSlots);
        public DoctorSlots GetDoctorSlotsCountsByDoctorIdSpecialtyId(DoctorSlots objDoctorSlots);
        public List<DoctorSlots> GetDoctorSlotsCountsByDoctorId(DoctorSlots objDoctorSlots);
        #endregion

        #region report
        List<ReportPatientCount> GetPatientCountByGenderMonthWise();
        ReportPatientCount GetPatientCountYearWise();
        List<PatientAppointmentModel> GetRecentPatientAppointments(int intStatus);
        List<ReportPatientCount> GetPatientCountByDepartment();
        List<PatientAppointmentModel> GetRecentPatient();
        List<PatientAppointmentModel> GetRecentPatientAppointmentsAll();
        List<DoctorsModel> GetDoctorSpecialtyGraph();
        List<int> GetPatientAgeGraph();
        #endregion
        #region
        ReportPatientCount GetPatientCountYearDoctorWise(int intDoctorId);
        List<ReportPatientCount> GetPatientCountByGenderMonthDoctorWise(int intDoctorId);
        PatientModel GetPatientHealthCheckUp_ByPatientId(int intPatentId);

        List<PatientAppointmentModel> GetRecentPatientAppointments_ByDoctor(int intDoctorId);

        List<PatientAppointmentModel> GetRecentPatient_ByDoctor(int intDoctorId);
        #endregion
        #region BodyMeasurement
        List<BodyMeasurementModel> GetBodyMeasurement();
        #endregion

        #region Relation

        public int InsertUpdateRelation(RelationModel objRelationModel);
        List<RelationModel> GetRelation();
        RelationModel GetRelationById(int intRelationId);
        List<RelationModel> GetRelationForAll();
        int DeleteRelationById(RelationModel objRelationModel);

        #endregion
        #region
        public List<EmployeePlannerModel> GetEmployeePlannerByEmployeeId(int intYear, int intMonth, int intEmpId);
        #endregion
        #region Notification
        List<NotificationModel> GetNotification(int intUserId);
        #endregion

        List<PatientAppointmentModel> GetUnattendedPatientAppointments(int intStatusId, string ddtStartTime, string ddtEndTime, int intDoctorId);
    }
}
