using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoMVC.Validators
{
    public class AppointmentValidator
    {

        //public void ValidateAppointmentEditor(AppointmentEditorViewModel aevm, out DateTime apptDate)
        //{
        //    foreach (var v in ModelState.Values) { v.Errors.Clear(); }

        //    if (!DateTime.TryParse(aevm.AppointmentDate, out apptDate))
        //        ModelState.AddModelError("Appointment Date", "Please enter valid appointment date.");
        //    else
        //        if (_timeSelectionService.ParseDateTime(apptDate, aevm.AppointmentStart).AddHours(3) < DateTime.Now)
        //        ModelState.AddModelError("Appointment Schedule", "Please set appointment for atleast 3 hours out from the current time.");
        //    if (_timeSelectionService.GetValidationHourMinute(aevm.AppointmentStart) >= _timeSelectionService.GetValidationHourMinute(aevm.AppointmentEnd))
        //        ModelState.AddModelError("Appointment Time", "Appointment End Time must be atleast 30 min greater than Start Time");
        //}
    }
}