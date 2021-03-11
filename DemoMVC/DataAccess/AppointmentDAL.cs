using DemoMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DemoMVC.DataAccess
{
    public interface IAppointmentDAL : IDisposable
    {
        List<Appointment> GetAppointmentsByDeterminer(int profileID, string determiner);
        Appointment GetAppointmentByID(Guid apptID);
        void InsertAppointment(Appointment appt);
        void UpdateAppointment(Appointment appt);
        void DeleteAppointment(Appointment appt);
    }
    public class AppointmentDAL : IAppointmentDAL
    {
        private IRPDContext _db;

        public AppointmentDAL(IRPDContext db)
        {
            _db = db;
        }

        public List<Appointment> GetAppointmentsByDeterminer(int profileID, string determiner)
        {
            List<Appointment> appointments;
            var today = DateTime.Now;

            switch (determiner)
            {
                case "Previous":
                    appointments = _db.Appointment.Where(a => a.StartDateTime < today && a.ProfileID == profileID).ToList();
                    break;
                case "Upcoming":
                    appointments = _db.Appointment.Where(a => a.StartDateTime >= today && a.ProfileID == profileID).ToList();
                    break;
                default:
                    appointments = new List<Appointment>();
                    break;
            };

            if (appointments.Count() > 0)
                appointments.ForEach(a => a.AppointmentProvider = _db.Provider.Find(a.ProviderID));

            return appointments;
        }

        public Appointment GetAppointmentByID(Guid apptID) => _db.Appointment.Find(apptID);

        public void InsertAppointment(Appointment appt)
        {
            _db.Appointment.Add(appt);
            _db.SaveChanges();
        }

        public void UpdateAppointment(Appointment appt)
        {
            _db.Entry(appt).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeleteAppointment(Appointment appt)
        {
            _db.Appointment.Remove(appt);
            _db.SaveChanges();
        }

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}