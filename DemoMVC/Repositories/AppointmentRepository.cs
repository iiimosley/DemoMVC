using DemoMVC.Common;
using DemoMVC.DataAccess;
using DemoMVC.Models;
using DemoMVC.Services;
using DemoMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoMVC.Repositories
{
    public interface IAppointmentRepository
    {
        List<AppointmentListViewModel> GetAppointmentsByDeterminer(int profileID, string determiner);
        Appointment GetAppointmentByID(Guid apptID);
        void InsertAppointment(AppointmentEditorViewModel aevm);
        void UpdateAppointment(AppointmentEditorViewModel aevm);
        void DeleteAppointmentByID(Guid apptID);
        AppointmentEditorViewModel InitializeAppointmentEditor(Appointment appt);

        AppointmentEditorViewModel InitializeNewAppointmentEditor(Guid providerID, int profileID);

    }

    public class AppointmentRepository : IAppointmentRepository 
    {
        IAppointmentDAL _apptDAL;
        IProviderDAL _providerDAL;
        IAppointmentTimingService _apptTiming;

        public AppointmentRepository(IAppointmentDAL apptDAL,
                                     IAppointmentTimingService apptTiming,
                                     IProviderDAL providerDAL)
        {
            _apptDAL = apptDAL;
            _apptTiming = apptTiming;
            _providerDAL = providerDAL;
        }

        public List<AppointmentListViewModel> GetAppointmentsByDeterminer(int profileID, string determiner)
        {
            List<Appointment> appointments;
            var alvm = new List<AppointmentListViewModel>();

            appointments = _apptDAL.GetAppointmentsByDeterminer(profileID, determiner);

            appointments.ForEach(a =>   
            {
                alvm.Add(new AppointmentListViewModel
                {
                    AppointmentID = a.ID,
                    ProfileID = a.ProfileID,
                    ProviderID = a.ProviderID,
                    ProviderName = a.AppointmentProvider.Name,
                    Date = a.StartDateTime.ToString("d"),
                    StartTime = a.StartDateTime.ToString("h:mm tt"),
                    EndTime = a.EndDateTime.ToString("h:mm tt")
                });
            });

            return alvm;
        }

        public Appointment GetAppointmentByID(Guid apptID) => _apptDAL.GetAppointmentByID(apptID);

        public void InsertAppointment(AppointmentEditorViewModel aevm)
        {
            var appt = MapEditorVMToAppointment(aevm);
            _apptDAL.InsertAppointment(appt);
        }

        public void UpdateAppointment(AppointmentEditorViewModel aevm)
        {
            var appt = MapEditorVMToAppointment(aevm);
            _apptDAL.UpdateAppointment(appt);
        }

        public void DeleteAppointmentByID(Guid apptID)
        {
            var appt = GetAppointmentByID(apptID);
            _apptDAL.DeleteAppointment(appt);
        }

        #region Appointment Editor VM Mapping
        public AppointmentEditorViewModel InitializeNewAppointmentEditor(Guid providerID, int profileID)
            =>  new AppointmentEditorViewModel()
                {
                    EditorState = ObjectState.New,
                    AppointmentID = Guid.NewGuid(),
                    ProviderID = providerID,
                    ProfileID = profileID,
                    ProviderName = _providerDAL.GetProviderByID(providerID).Name
                };

        public AppointmentEditorViewModel InitializeAppointmentEditor(Appointment appt)
            => new AppointmentEditorViewModel()
            {
                EditorState = ObjectState.Modified,
                AppointmentID = appt.ID,
                ProviderID = appt.ProviderID,
                ProfileID = appt.ProfileID,
                AppointmentStart = appt.StartDateTime.ToString("H:mm"),
                AppointmentEnd = appt.EndDateTime.ToString("H:mm"),
                AppointmentDate = appt.StartDateTime.ToString("MM/dd/yyyy"),
                ProviderName = _providerDAL.GetProviderByID(appt.ProviderID).Name
            };

        private Appointment MapEditorVMToAppointment(AppointmentEditorViewModel aevm)
            => new Appointment()
            {
                ID = aevm.AppointmentID,
                ProfileID = aevm.ProfileID,
                ProviderID = aevm.ProviderID,
                StartDateTime = _apptTiming.ParseDateTime(DateTime.Parse(aevm.AppointmentDate), aevm.AppointmentStart),
                EndDateTime = _apptTiming.ParseDateTime(DateTime.Parse(aevm.AppointmentDate), aevm.AppointmentEnd),
            };
        #endregion
    }
}