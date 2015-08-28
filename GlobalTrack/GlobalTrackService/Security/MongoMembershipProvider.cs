using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GlobalTrackService.Security
{

    public class MongoMembershipProvider
    
    {
         private MongoCollection<User> _usersCollection;


        private MongoDatabase _database;


        public MongoMembershipProvider()
        {
            var connectionString = ConfigurationManager.AppSettings["mongoDbServerConnection"];
            var dbName = ConfigurationManager.AppSettings["mongoDbName"]; 
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(dbName);
            _database = database;
            _usersCollection = database.GetCollection<User>("Users"); 
        }

        public  MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer,
                                                  bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

       

        public bool ValidateUser(string username, string password)
        {
            username = username.ToLower(); 
            var user =
                _usersCollection.AsQueryable().FirstOrDefault(x => string.Equals(x.NameLowerSpace, username));

            return user != null && user.Password == password;

        }

        

        public  MembershipUser GetUser(string username, bool userIsOnline)
        {
            username = username.ToLower(); 
            var user =
                _usersCollection.AsQueryable().FirstOrDefault(x => string.Equals(x.NameLowerSpace, username)); 

            if (user != null)
            {
                var mp = new MembershipUser("MongoMembershipProvider", user.Name, user.UserId, "", "", "", true, false,
                                          DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

                return mp;

            }
            else
            {
                return null; 
            }

        }

        
        public  string CreateUserAndAccount(string userName, string password, bool requireConfirmation, IDictionary<string, object> values)
        {
            try
            {
                //throw new NotImplementedException();
                User u = new User() {Name = userName, Password = password, NameLowerSpace = userName.ToLower()};
                _usersCollection.Save(u);
                return u.UserId.ToString(); 
            }
            catch(Exception e)
            {
                throw new MembershipCreateUserException("The unknown error wnen creating the user: " + e.Message, e); 
            }
        }
        
    }
}