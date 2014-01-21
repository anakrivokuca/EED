using EED.Domain;
using EED.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Configuration.Provider;

namespace EED.Service.Membership_Provider
{
    public class CustomMembershipProvider : MembershipProvider, IMembershipProvider
    {
        private IRepository<User> _repository;
        
        private MembershipPasswordFormat _passwordFormat;
        private int _minRequiredNonAlphanumericCharacters;
        private int _minRequiredPasswordLength;
        private string _passwordStrengthRegularExpression;
        private bool _requiresUniqueEmail;
        private MachineKeySection _machineKey;

        #region Initialization

        public override void Initialize(string name, NameValueCollection config)
        {
            config = SetConfigDefaults(config);
            name = SetDefaultName(name);

            base.Initialize(name, config);

            ValidatingPassword += OnValidatePassword;
            SetConfigurationProperties(config);
            CheckEncryptionKey();
            SetUserRepository();
        }

        private string SetDefaultName(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                name = "CustomMembershipProvider";
            }
            return name;
        }

        private void SetUserRepository()
        {
            if (_repository == null)
            {
                _repository = DependencyResolver.Current.GetService<IRepository<User>>();
            }
        }

        private NameValueCollection SetConfigDefaults(NameValueCollection config)
        {
            if (config == null) throw new ArgumentNullException("config");
            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom Membership Provider");
            }
            return config;
        }

        private void SetConfigurationProperties(NameValueCollection config)
        {
            _minRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            _minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            _passwordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], @"(?=(.*\d){1,})"));
            _requiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
            SetPasswordFormat(config["passwordFormat"]);
            _machineKey = GetMachineKeySection();
        }

        public MachineKeySection GetMachineKeySection()
        {
            System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            return cfg.GetSection("system.web/machineKey") as MachineKeySection;
        }

        private bool CheckEncryptionKey()
        {
            if (_machineKey.ValidationKey.Contains("AutoGenerate"))
            {
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                {
                    throw new ProviderException("Hashed or Encrypted passwords are not supported with auto-generated keys.");
                }
            }
            return true;
        }

        private void SetPasswordFormat(string passwordFormat)
        {
            if (passwordFormat == null)
            {
                passwordFormat = "Clear";
            }

            switch (passwordFormat)
            {
                case "Hashed":
                    _passwordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    _passwordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    _passwordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }
        }

        private static string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
            {
                return defaultValue;
            }
            return configValue;
        }

        #endregion

        public string ProviderName
        {
            get { return Name; }
        }

        public MachineKeySection MachineKey
        {
            get { return _machineKey; }
            set { _machineKey = value; }
        }

        public IRepository<User> Repository
        {
            get { return _repository; }
            set { _repository = value; }
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public User CreateUser(User user, out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args =
                new ValidatePasswordEventArgs(user.UserName, user.Password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(user.Email) != string.Empty)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            User userObj = GetUser(user.UserName);

            if (userObj == null)
            {
                user.CreationDate = DateTime.Now;
                user.LastLoginDate = DateTime.Now;
                _repository.Save(user);

                status = MembershipCreateStatus.Success;

                return GetUser(user.UserName);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }

            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            try
            {
                User user = GetUser(username);

                if (user != null)
                {
                    _repository.Delete(user);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Error processing membership data - " + ex.Message);
            }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers()
        {
            IEnumerable<User> users;
            try
            {
                users = _repository.FindAll();
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Error processing membership data - " + ex.Message);
            }
            
            return users;
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string username)
        {
            User user = null;
            try
            {
                user = GetAllUsers().SingleOrDefault
                    (u => u.UserName == username);
                if (user != null)
                {
                    MembershipUser memUser = GetMembershipUserFromUser(user);
                    user.LastLoginDate = memUser.LastLoginDate;
                    user.CreationDate = memUser.CreationDate;
                    user.IsOnline = memUser.IsOnline;
                    user.IsLockedOut = memUser.IsLockedOut;
                }
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Unable to retrieve user data - " + ex.Message);
            }

            return user;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            try
            {
                User user = GetAllUsers().SingleOrDefault
                    (u => u.Email == email);
                if (user != null)
                    return user.UserName;
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Error processing membership data - " + ex.Message);
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return _passwordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { return _requiresUniqueEmail; }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser membershipUser)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            try
            {
                var userToUpdate = GetUser(user.UserName);
                if (userToUpdate != null)
                {
                    userToUpdate.Name = user.Name;
                    userToUpdate.Surname = user.Surname;
                    userToUpdate.Email = user.Email;
                    userToUpdate.State = user.State;
                    userToUpdate.Country = user.Country;
                    userToUpdate.PhoneNumber = user.PhoneNumber;
                    userToUpdate.IsApproved = user.IsApproved;

                    _repository.Save(userToUpdate);
                }
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Error processing membership data - " + ex.Message);
            }
        }

        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;

            try
            {
                User user = GetUser(username);

                if (user != null)
                {
                    string storedPassword = user.Password;
                    bool isApproved = user.IsApproved;

                    if (CheckPassword(password, storedPassword))
                    {
                        if (isApproved)
                        {
                            isValid = true;
                            user.LastLoginDate = DateTime.Now;
                            user.IsOnline = true;
                            _repository.Save(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Error processing membership data - " + ex.Message);
            }
            
            return isValid;
        }

        public void OnValidatePassword(object sender, ValidatePasswordEventArgs e)
        {
            //Enforce our criteria
            var errorMessage = "";
            var passChar = e.Password.ToCharArray();
            
            //Check length
            if (e.Password.Length < _minRequiredPasswordLength)
            {
                errorMessage += "[Minimum length: " + _minRequiredPasswordLength + "]";
                e.Cancel = true;
            }
            
            //Check strength
            if (_passwordStrengthRegularExpression != string.Empty)
            {
                Regex r = new Regex(_passwordStrengthRegularExpression);
                if (!r.IsMatch(e.Password))
                {
                    errorMessage += "[Insufficient Password Strength]";
                    e.Cancel = true;
                }
            }
            
            //Check non-alpha characters
            int iNumNonAlpha = 0;
            Regex rAlpha = new Regex(@"\w");
            foreach (char c in passChar)
            {
                if (!char.IsLetterOrDigit(c)) 
                    iNumNonAlpha++;
            }
            if (iNumNonAlpha < _minRequiredNonAlphanumericCharacters)
            {
                errorMessage += "[Insufficient Non-Alpha Characters]";
                e.Cancel = true;
            }
            e.FailureInformation = new MembershipPasswordException(errorMessage);
        }

        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
                default:
                    break;
            }

            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }

        private string EncodePassword(string password)
        {
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword =
                        Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(_machineKey.ValidationKey);
                    encodedPassword =
                        Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return encodedPassword;
        }

        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password =
                        Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }

        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        private MembershipUser GetMembershipUserFromUser(User u)
        {
            var creationDate = new DateTime();
            if (u.CreationDate != null)
            {
                creationDate = (DateTime)u.CreationDate;
            }
            var lastLoginDate = new DateTime();
            if (u.LastLoginDate != null)
            {
                lastLoginDate = (DateTime)u.LastLoginDate;
            }
            var lastActivityDate = new DateTime();
            var lastPasswordChangedDate = new DateTime();
            var lastLockedOutDate = new DateTime();

            var membershipUser = new MembershipUser(
                this.Name,
                u.UserName,
                u.Id,
                u.Email,
                string.Empty,
                string.Empty,
                u.IsApproved,
                u.IsLockedOut,
                creationDate,
                lastLoginDate,
                lastActivityDate,
                lastPasswordChangedDate,
                lastLockedOutDate
                );
            return membershipUser;
        }

        public IEnumerable<User> FilterUsers(IEnumerable<User> users, string searchText)
        {
            string[] keywords = searchText.Trim().Split(' ');
            foreach (var k in keywords.Where(k => !String.IsNullOrEmpty(k)))
            {
                string keyword = k;
                users = users
                    .Where(u => (String.Equals(u.Name, keyword, StringComparison.CurrentCultureIgnoreCase) ||
                        String.Equals(u.Surname, keyword, StringComparison.CurrentCultureIgnoreCase) ||
                        String.Equals(u.Email, keyword, StringComparison.CurrentCultureIgnoreCase) ||
                        String.Equals(u.State, keyword, StringComparison.CurrentCultureIgnoreCase) ||
                        String.Equals(u.Country, keyword, StringComparison.CurrentCultureIgnoreCase) ||
                        String.Equals(u.UserName, keyword, StringComparison.CurrentCultureIgnoreCase)));
            }
            return users;
        }
    }
}
