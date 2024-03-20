using HospitalManagement_Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Repository.Interface
{
    public interface ISystemUser
    {
        #region User
        UserModel UserLogin(UserModel Users);
        #endregion


        #region SystemUser

        SystemUsersModel SystemUserLogin(SystemUsersModel objsystemUsersModel);
        public int InsertUpdateSystemUsers(SystemUsersModel objsystemUsersModel);
        public List<SystemUsersModel> GetSystemUsers();
        public SystemUsersModel GetSystemUsersById(int intId);
        #endregion
    }
}
