using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ENRLReconSystem
{
  
    public class ERSRoleProvider : RoleProvider
    {
        public UIUserLogin CurrentUser
        {
            get
            {
                try
                {
                    UIUserLogin rtnValue = HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
                    return rtnValue;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                try
                {
                    HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] = value;
                }
                catch
                {
                    //"CurrentUser" sometimes its observed that session object is null 
                    //there is a immedeate request which follows the above request with the actual data
                    //Hence the "session object is null" request must be ignored
                    //no logging for this error
                }
            }
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            try
            {
                ERSAuthenticationRoles role;
                if (Enum.TryParse<ERSAuthenticationRoles>(roleName, out role))
                {
                    if (CurrentUser != null)
                    {
                        switch (role)
                        {
                            case ERSAuthenticationRoles.AdminUser:
                                return CurrentUser.IsAdminUser;

                            case ERSAuthenticationRoles.AdmOSTUser:
                                return CurrentUser.IsAdmOSTUser;
                            case ERSAuthenticationRoles.MgrOSTUser:
                                return CurrentUser.IsMgrOSTUser;
                            case ERSAuthenticationRoles.PrcrOSTUser:
                                return CurrentUser.IsPrcrOSTUser;
                            case ERSAuthenticationRoles.VwrOSTUser:
                                return CurrentUser.IsVwrOSTUser;

                            case ERSAuthenticationRoles.AdmEligUser:
                                return CurrentUser.IsAdmEligUser;
                            case ERSAuthenticationRoles.MgrEligUser:
                                return CurrentUser.IsMgrEligUser;
                            case ERSAuthenticationRoles.PrcrEligUser:
                                return CurrentUser.IsPrcrEligUser;
                            case ERSAuthenticationRoles.VwrEligUser:
                                return CurrentUser.IsVwrEligUser;

                            case ERSAuthenticationRoles.AdmRPRUser:
                                return CurrentUser.IsAdmRPRUser;
                            case ERSAuthenticationRoles.MgrRPRUser:
                                return CurrentUser.IsMgrRPRUser;
                            case ERSAuthenticationRoles.PrcrRPRUser:
                                return CurrentUser.IsPrcrRPRUser;
                            case ERSAuthenticationRoles.VwrRPRUser:
                                return CurrentUser.IsVwrRPRUser;

                            case ERSAuthenticationRoles.WebServiceUser:
                                return CurrentUser.IsWebServiceUser;
                            case ERSAuthenticationRoles.MacroServiceUser:
                                return CurrentUser.IsMacroServiceUser;

                            case ERSAuthenticationRoles.User:
                                return true;
                        }
                    }
                    else
                    {
                        if (role == ERSAuthenticationRoles.Unauthenticated)
                            return true;
                    }
                }
            }
            catch
            {

            }
            return false;
        }

        public override string[] GetRolesForUser(string username)
        {
            List<string> roles = new List<string>();
            try
            {
                var currUser = CurrentUser;
                if (currUser == null)
                {
                    //if user already not logged in then try to login
                    if (UserLoggedIn(out currUser) == false)
                    {
                        roles.Add(ERSAuthenticationRoles.Unauthenticated.ToString());
                        return roles.ToArray();
                    }
                }
                roles.Add(ERSAuthenticationRoles.User.ToString());
                if (currUser.IsAdminUser)
                {
                    roles.Add(ERSAuthenticationRoles.AdminUser.ToString());
                }
                if (currUser.IsAdmOSTUser)
                {
                    roles.Add(ERSAuthenticationRoles.AdmOSTUser.ToString());
                }
                if (currUser.IsAdmEligUser)
                {
                    roles.Add(ERSAuthenticationRoles.AdmEligUser.ToString());
                }
                if (currUser.IsAdmRPRUser)
                {
                    roles.Add(ERSAuthenticationRoles.AdmRPRUser.ToString());
                }
                if (currUser.IsMgrOSTUser)
                {
                    roles.Add(ERSAuthenticationRoles.MgrOSTUser.ToString());
                }
                if (currUser.IsMgrEligUser)
                {
                    roles.Add(ERSAuthenticationRoles.MgrEligUser.ToString());
                }
                if (currUser.IsMgrRPRUser)
                {
                    roles.Add(ERSAuthenticationRoles.MgrRPRUser.ToString());
                }
                if (currUser.IsPrcrOSTUser)
                {
                    roles.Add(ERSAuthenticationRoles.PrcrOSTUser.ToString());
                }
                if (currUser.IsPrcrEligUser)
                {
                    roles.Add(ERSAuthenticationRoles.PrcrEligUser.ToString());
                }
                if (currUser.IsPrcrRPRUser)
                {
                    roles.Add(ERSAuthenticationRoles.PrcrRPRUser.ToString());
                }
                if (currUser.IsVwrOSTUser)
                {
                    roles.Add(ERSAuthenticationRoles.VwrOSTUser.ToString());
                }
                if (currUser.IsVwrEligUser)
                {
                    roles.Add(ERSAuthenticationRoles.VwrEligUser.ToString());
                }
                if (currUser.IsVwrRPRUser)
                {
                    roles.Add(ERSAuthenticationRoles.VwrRPRUser.ToString());
                }
                if (currUser.IsWebServiceUser)
                {
                    roles.Add(ERSAuthenticationRoles.WebServiceUser.ToString());
                }
                if (currUser.IsMacroServiceUser)
                {
                    roles.Add(ERSAuthenticationRoles.MacroServiceUser.ToString());
                }
                return roles.ToArray();
            }
            catch
            {
                roles.Add(ERSAuthenticationRoles.Unauthenticated.ToString());
                return roles.ToArray();
            }
        }

        private bool UserLoggedIn(out UIUserLogin loggedInUser)
        {
            loggedInUser = null;
            try
            {
                if (CurrentUser == null)
                {

                    string[] strLoginName = System.Web.HttpContext.Current.User.Identity.Name.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
                    string domain = strLoginName[0];
                    string loginName = strLoginName[1];

                    //Checking user in Database.
                    BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                    ExceptionTypes result = objBLUserAdministration.GetUserAccessPermission(loginName, null, null, null, out loggedInUser);
                    if (result == ExceptionTypes.ZeroRecords)
                    {
                        return false;
                    }
                    if (result != (long)ExceptionTypes.Success)
                    {
                        return false;
                    }
                    else
                    {
                        System.Security.Principal.WindowsIdentity winIdnt = System.Web.HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity;
                        System.Security.Principal.IdentityReferenceCollection grps = winIdnt.Groups;
                        //Admin
                        if (IsUserInADGroup(grps, WebConfigData.AdminSID))
                            loggedInUser.IsAdminUser = true;

                        //OST
                        if (IsUserInADGroup(grps, WebConfigData.AdminOSTSID))
                            loggedInUser.IsAdmOSTUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ManagerOSTSID))
                            loggedInUser.IsMgrOSTUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ProcessorOSTSID))
                            loggedInUser.IsPrcrOSTUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ViewerOSTSID))
                            loggedInUser.IsVwrOSTUser = true;

                        //Eligibility
                        if (IsUserInADGroup(grps, WebConfigData.AdminEligSID))
                            loggedInUser.IsAdmEligUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ManagerEligSID))
                            loggedInUser.IsMgrEligUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ProcessorEligSID))
                            loggedInUser.IsPrcrEligUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ViewerEligSID))
                            loggedInUser.IsVwrEligUser = true;

                        //RPR
                        if (IsUserInADGroup(grps, WebConfigData.AdminRPRSID))
                            loggedInUser.IsAdmRPRUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ManagerRPRSID))
                            loggedInUser.IsMgrRPRUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ProcessorRPRSID))
                            loggedInUser.IsPrcrRPRUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ViewerRPRSID))
                            loggedInUser.IsVwrRPRUser = true;

                        if (loggedInUser.IsAdminUser == false
                           && loggedInUser.IsAdmOSTUser == false
                           && loggedInUser.IsAdmEligUser == false
                           && loggedInUser.IsAdmRPRUser == false
                           && loggedInUser.IsMgrOSTUser == false
                           && loggedInUser.IsMgrEligUser == false
                           && loggedInUser.IsMgrRPRUser == false
                           && loggedInUser.IsPrcrOSTUser == false
                           && loggedInUser.IsPrcrEligUser == false
                           && loggedInUser.IsPrcrRPRUser == false
                           && loggedInUser.IsVwrOSTUser == false
                           && loggedInUser.IsVwrEligUser == false
                           && loggedInUser.IsVwrRPRUser == false
                           && loggedInUser.IsWebServiceUser == false
                           && loggedInUser.IsMacroServiceUser == false
                            )
                        {
                            return false;
                        }
                        else
                        {
                            CurrentUser = loggedInUser;
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool IsUserInADGroup(System.Security.Principal.IdentityReferenceCollection grps, string SIDs)
        {
            string[] ADGroupSIDs = SIDs.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            bool isUserInADGroup = (from g in grps
                                    where ADGroupSIDs.Any(s => s == g.Value)
                                    select g).Any();
            return isUserInADGroup;
        }

        #region Not Implemented Methods
        public override string ApplicationName { get; set; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}