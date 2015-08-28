using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GlobalTrackService.Security
{
    //public class User
    //{
    //    [BsonId]
    //    public ObjectId UserId { get; set; }
    //    public string Name { get; set; }
    //    public string NameLowerSpace { get; set; }
    //    public string Password { get; set;}

    //}
    public class User
    {
        [BsonId]
        public ObjectId UserId { get; set; }

        public string Name { get; set; }

        public string NameLowerSpace { get; set; }


        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

     
        public string OrganizationName { get; set; }

   
        public bool IsOrganization { get; set; }

        public string Email { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Phone3 { get; set; }

        public string Address { get; set; }


        public bool EnableSubscribersEmailNotifications { get; set; }


        public string SmtpUserName { get; set; }


        public string SmtpPassword { get; set; }


        public string SmtpServerUrl { get; set; }

 
        public int SmtpPort { get; set; }

  
        public bool SmtpHttps { get; set; }

    }

}