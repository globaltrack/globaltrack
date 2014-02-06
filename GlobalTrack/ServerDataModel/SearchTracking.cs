using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServerDataModel
{
    public class SearchTracking
    {
        [Display(Name = "SearchTracking_TrackingNumber", ResourceType = typeof(Resources.Resource))]
        [StringLength(24, ErrorMessageResourceName = "SearchTracking_TrackingNumber_Validation", MinimumLength = 24, ErrorMessageResourceType = typeof(Resources.Resource))]
        public string TrackingNumber { get; set; }

        [Display(Name = "SearchTracking_Password", ResourceType = typeof(Resources.Resource))]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}