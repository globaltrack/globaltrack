using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServerDataModel
{
    public class SearchTracking
    {
        [DisplayName("Please specify your tracking number")]
        [StringLength(24, ErrorMessage = "The tracking number  must be 24 characters long.", MinimumLength = 24)]
        public string TrackingNumber { get; set; }
    }
}