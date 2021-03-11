using DemoMVC.Common;
using DemoMVC.Models;
using DemoMVC.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoMVC.ViewModels
{
    public class AppointmentEditorViewModel
    {
        public List<SelectListItem> TimeSelection = AppointmentTimingService.GetTimeSelections();
        public Guid AppointmentID { get; set; }
        public ObjectState EditorState { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string AppointmentDate { get; set; }
        public string AppointmentStart { get; set; }
        public string AppointmentEnd { get; set; }
        public string ProviderName { get; set; }
        public Guid ProviderID { get; set; }
        public int ProfileID { get; set; }
    }
}