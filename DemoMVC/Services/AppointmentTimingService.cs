using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DemoMVC.Services
{
    public interface IAppointmentTimingService
    {
        DateTime ParseDateTime(DateTime date, string time);
        int GetValidationHourMinute(string hm);
    }

    public class AppointmentTimingService : IAppointmentTimingService
    {
        public static List<SelectListItem> GetTimeSelections()
        {
            var selections = new List<SelectListItem>();

            for (var time = new DateTime(2000, 1, 1, 8, 0, 0);
                 time <= new DateTime(2000, 1, 1, 17, 30, 0);
                 time = time.AddMinutes(30))
            {
                selections.Add(new SelectListItem() { Value = time.ToString("H:mm"), Text = time.ToString("h:mm tt") });
            }

            return selections;
        }

        public DateTime ParseDateTime(DateTime date, string time)
        {
            int[] hm = time.Split(':').Select(Int32.Parse).ToArray();
            return new DateTime(date.Year, date.Month, date.Day, hm[0], hm[1], 0);
        }

        public int GetValidationHourMinute(string hm) => Int32.Parse(string.Join("", hm.Split(':')));
    }
}