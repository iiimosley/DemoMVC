using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DemoMVC;
using DemoMVC.Controllers;
using DemoMVC.Models;
using DemoMVC.ViewModels;
using Autofac.Extras.Moq;
using DemoMVC.Repositories;
using DemoMVC.DataAccess;
using Moq;

namespace DemoMVC.Tests.Repositories
{
    [TestClass]
    public class AppointmentRepositoryTest
    {
        AppointmentRepository _apptRepo;
        Appointment _appt1;
        Appointment _appt2;
        List<Appointment> _appts;
        List<AppointmentListViewModel> _alvms;

        [TestMethod]
        public void ShouldReturnMappedAppointmentListViewModel()
        {
            GivenAppointmentsCreated();
            WhenGettingAppointmentsByDeterminer();
            ThenAppointmentsShouldBeMappedToAppointmentListViewModel();
        }

        #region Given
        private void GivenAppointmentsCreated()
        {
            _appt1 = new Appointment()
            {
                ID = Guid.NewGuid(),
                ProfileID = 9998,
                ProviderID = Guid.NewGuid(),
                AppointmentProvider = new Provider { Name = "Test Clinic" },
                StartDateTime = new DateTime(2019, 12, 1, 14, 0, 0),
                EndDateTime = new DateTime(2019, 12, 1, 15, 0, 0)
            };

            _appt2 = new Appointment()
            {
                ID = Guid.NewGuid(),
                ProfileID = 9998,
                ProviderID = Guid.NewGuid(),
                AppointmentProvider = new Provider { Name = "Another Test Clinic" },
                StartDateTime = new DateTime(2019, 12, 2, 10, 0, 0),
                EndDateTime = new DateTime(2019, 12, 2, 11, 0, 0)
            };

            _appts = new List<Appointment>() { _appt1, _appt2 };
        }
        #endregion

        #region When
        private void WhenGettingAppointmentsByDeterminer()
        {
            using (var moq = AutoMock.GetLoose())
            {
                moq.Mock<IAppointmentDAL>().Setup(x => x.GetAppointmentsByDeterminer(9998,"")).Returns(_appts);
                _apptRepo = moq.Create<AppointmentRepository>();

                _alvms = _apptRepo.GetAppointmentsByDeterminer(9998, "");
                moq.Mock<IAppointmentDAL>().Verify(x => x.GetAppointmentsByDeterminer(9998, ""));
            }
        }
        #endregion

        #region Then
        private void ThenAppointmentsShouldBeMappedToAppointmentListViewModel()
        {
            VerifyAppropriatelyMappedApointmentListViewModel(_appts.SingleOrDefault(a => a.ID == _appt1.ID),
                                                             _alvms.SingleOrDefault(a => a.AppointmentID == _appt1.ID));
            VerifyAppropriatelyMappedApointmentListViewModel(_appts.SingleOrDefault(a => a.ID == _appt2.ID),
                                                             _alvms.SingleOrDefault(a => a.AppointmentID == _appt2.ID));
        }

        private void VerifyAppropriatelyMappedApointmentListViewModel(Appointment appt, AppointmentListViewModel alvm)
        {
            Assert.AreEqual(appt.ProfileID, alvm.ProfileID);
            Assert.AreEqual(appt.ProviderID, alvm.ProviderID);
            Assert.AreEqual(appt.AppointmentProvider.Name, alvm.ProviderName);
            Assert.AreEqual(appt.StartDateTime.ToString("d"), alvm.Date);
            Assert.AreEqual(appt.StartDateTime.ToString("h:mm tt"), alvm.StartTime);
            Assert.AreEqual(appt.EndDateTime.ToString("h:mm tt"), alvm.EndTime);
        }
        #endregion
    }
}
