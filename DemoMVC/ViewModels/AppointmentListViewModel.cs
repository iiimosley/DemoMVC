using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoMVC.ViewModels
{
    public class AppointmentListViewModel
    {
        public Guid AppointmentID { get; set; }
        public int ProfileID { get; set; }
        public Guid ProviderID { get; set; }

        public string ProviderName { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
