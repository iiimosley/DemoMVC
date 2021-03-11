using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoMVC.Models
{
    public class Appointment
    {
        [Key]
        public Guid ID { get; set; }
        public int ProfileID { get; set; }
        public Guid ProviderID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public Provider AppointmentProvider { get; set; }
        public Profile AppointmentProfile { get; set; }
    }
}
