using System.ComponentModel.DataAnnotations;

namespace ServerDataModel
{
    public class TrackingCustomerInformation
    {

        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }
        [Display(Name = "Mobile phone")]
        public string MobilePhone { get; set; }
        [Display(Name = "Email address")]
        public string Email { get; set; }
        
    }
}