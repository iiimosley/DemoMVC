using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoMVC.Models
{
    public class Provider
    {
        public Provider()
        {
            ID = Guid.NewGuid();
        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}