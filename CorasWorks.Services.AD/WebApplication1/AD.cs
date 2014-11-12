using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.DirectoryServices;
using System.Security.Principal;
using System.Security.Permissions;
using System.Security.Cryptography;
using System.Text;
using CorasWorks.Services.AD.Models;
using System.Text.RegularExpressions;


namespace CorasWorks.Services.AD
{
    public class AD : IDisposable
    {
        string _adMgr = "extaccmngr";
        string _adMgrPass = "Cr3@t3Users!";
        PasswordGenerator _mypg = new PasswordGenerator();
        string _adname = "LDAP://dc=external,dc=corasworks,dc=net";
        DirectoryEntry _entry;
        DirectorySearcher _search;

        public AD()
        {
            _mypg.Minimum = 8;
            _mypg.Maximum = 8;
            _entry = new DirectoryEntry(_adname, _adMgr, _adMgrPass);
            _search = new DirectorySearcher(_entry);
        }

        internal bool GetUser(ADUser user)
        { 
                GetUserFilter(user);
                if (_search.FindAll().Count > 0)
                    return true;
                return false;
          
        }

        private string ExtractUsername(string username)
        {
            string[] parts = username.Split('@');
            if (parts.Length == 2)
            {
                return parts[0];
            }

            parts = username.Split('\\');
            if (parts.Length == 2)
            {
                return parts[1];
            }

            return username;
        }

        private void GetUserFilter(ADUser user)
        {
            _search.Filter = ("(anr=" + (user.UserName.IndexOf("\\") >= 0 ? user.UserName.Split('\\').GetValue(1).ToString() : user.UserName) + ")");
      
            //_search.Filter = ExtractUsername(user.UserName);
             
        }

        internal bool ChangePassword(ADUser user)
        {

            GetUserFilter(user);
            _search.PropertiesToLoad.Add("distinguishedName");

            if (_search.FindAll().Count > 0)
            {
                string strHoldPath = _search.FindOne().Path.ToString();

                using (DirectoryEntry de1 = new DirectoryEntry(strHoldPath, _adMgr, _adMgrPass, AuthenticationTypes.Secure))
                {
                    de1.Invoke("SetPassword", new object[] { user.Password });
                    de1.CommitChanges();
                    de1.RefreshCache();
                    return true;
                }
            }
            return false;

        }

        internal string ResetPassword(ADUser user)
        {
            GetUserFilter(user);
            string strNewPassword = _mypg.Generate();
            strNewPassword = strNewPassword.Replace(" ", "9");

            if (_search.FindAll().Count > 0)
            {
                string strHoldPath = _search.FindOne().Path.ToString();
                using (DirectoryEntry de1 = new DirectoryEntry(strHoldPath, _adMgr, _adMgrPass))
                {
                    user.Email = "<Email>" + de1.Properties["mail"].Value.ToString() + "</Email>";
                    de1.Invoke("setPassword", new object[] { strNewPassword });
                    de1.CommitChanges();
                    de1.RefreshCache();
                }
            }
            return strNewPassword;

        }

        internal string GetUserName(ADUser user)
        {
            _search.Filter = "(mail=" + user.Email + ")";

            if (_search.FindAll().Count > 0)
            {
                string strHoldPath = _search.FindOne().Path.ToString();
                _entry.Dispose();
                _search.Dispose();
                using (DirectoryEntry de1 = new DirectoryEntry(strHoldPath, _adMgr, _adMgrPass))
                {
                    return "ext\\\\" + de1.Properties["samaccountname"].Value.ToString();
                }
            }
            return string.Empty;

        }

        internal void AddUser(ADUser user)
        {

            string ADname = "LDAP://ou=Users,ou=Community,dc=external,dc=corasworks,dc=net";
            using (DirectoryEntry de = new DirectoryEntry(ADname, _adMgr, _adMgrPass))
            {
                using (DirectoryEntry newUser = de.Children.Add("CN=" + user.UserName.ToLower(), "user"))
                {
                    newUser.Properties["samAccountName"].Value = user.UserName.ToLower();
                    newUser.Properties["mail"].Add(user.Email);
                    newUser.Properties["SN"].Add(user.FirstName);
                    newUser.Properties["givenName"].Add(user.LastName);
                    newUser.Properties["displayName"].Add(user.FirstName + " " + user.LastName);
                    newUser.Properties["company"].Add(user.Company);
                    newUser.CommitChanges();
                    newUser.Invoke("SetPassword", new object[] { user.Password });
                    newUser.CommitChanges();

                    using (DirectoryEntry EnableEntry = new DirectoryEntry(newUser.Path, _adMgr, _adMgrPass))
                    {

                        int val = (int)EnableEntry.Properties["userAccountControl"].Value;
                        EnableEntry.Properties["userAccountControl"].Value = val & ~0x2;
                        EnableEntry.CommitChanges();

                        using (DirectoryEntry grEntry = new DirectoryEntry("LDAP://cn=community_MEMBERS,ou=Groups,ou=Community,dc=external,dc=corasworks,dc=net", _adMgr, _adMgrPass))
                        {
                            object[] user_path = new object[] { newUser.Path };
                            grEntry.Invoke("Add", user_path);
                            grEntry.CommitChanges();
                        }
                    }
                }
            }

        }

        public void Dispose()
        {
            _search.Dispose();
            _entry.Dispose();
        }
    }
}