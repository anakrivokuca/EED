﻿using EED.DAL;
using EED.Domain;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.Web.Security;

namespace EED.Service.Membership_Provider
{
    public interface IMembershipProvider
    {
        string ProviderName { get; }
        IRepository<User> Repository { get; set; }
        MachineKeySection MachineKey { get;  set; }
        
        void Initialize(string name, NameValueCollection config);
        MachineKeySection GetMachineKeySection();
        IEnumerable<User> GetAllUsers();
        User GetUser(string username);
        string GetUserNameByEmail(string email);
        User CreateUser(User user, out MembershipCreateStatus status);
        void UpdateUser(User user);
        bool DeleteUser(string username, bool deleteAllRelatedData);
        bool ValidateUser(string username, string password);
        IEnumerable<User> FilterUsers(IEnumerable<User> users, string searchText);
        void OnValidatePassword(object sender, ValidatePasswordEventArgs e);
    }
}
