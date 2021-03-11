using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DemoMVC;
using DemoMVC.Controllers;
using DemoMVC.DataAccess;
using Autofac.Extras.Moq;
using DemoMVC.Models;
using System.Data.Entity;

namespace DemoMVC.Tests.DataAccess
{
    [TestClass]
    public class AppointmentDALTest
    {
        AppointmentDAL _apptDAL;
        Appointment _appt1;
        Appointment _appt2;
        Appointment _appt3;
        Provider _prv;
        List<Appointment> _apptResults;
        IDbSet<Appointment> _apptSet;
        IDbSet<Provider> _prvSet;
        DateTime _now;

        //[TestInitialize]
        //public void TestSetup()
        //{

        //}

        [TestMethod]
        public void ShouldReturnAllUpcomingAppointments()
        {
            GivenAppointmentsForProviderCreated();
            WhenGettingUpcomingAppointmentsForProfile();
            ThenUpcomingAppointmentsReturned();
        }


        [TestMethod]
        public void ShouldReturnAllPreviousAppointments()
        {
            GivenAppointmentsForProviderCreated();
            WhenGettingPreviousAppointmentsForProfile();
            ThenPreviousAppointmentsReturned();
        }

        #region Given
        private void GivenAppointmentsForProviderCreated()
        {
            _now = DateTime.Now;

            _prv = new Provider()
            {
                Name = "Test Clinic"
            };

            _appt1 = new Appointment()
            {
                ID = Guid.NewGuid(),
                ProfileID = 9998,
                ProviderID = _prv.ID,
                AppointmentProvider = _prv,
                StartDateTime = _now.AddYears(-1),
                EndDateTime = _now.AddYears(-1).AddHours(1)
            };

            _appt2 = new Appointment()
            {
                ID = Guid.NewGuid(),
                ProfileID = 9998,
                ProviderID = _prv.ID,
                AppointmentProvider = _prv,
                StartDateTime = _now.AddMonths(1),
                EndDateTime = _now.AddMonths(1).AddHours(1)
            };

            _appt3 = new Appointment()
            {
                ID = Guid.NewGuid(),
                ProfileID = 9998,
                ProviderID = _prv.ID,
                AppointmentProvider = _prv,
                StartDateTime = _now.AddMonths(2),
                EndDateTime = _now.AddMonths(2).AddHours(1)
            };
            
            _apptSet = new FakeAppointmentSet() { _appt1, _appt2, _appt3 };
            _prvSet = new FakeProviderSet() { _prv };
        }
        # endregion

        #region When
        private void WhenGettingUpcomingAppointmentsForProfile()
        {
            MockGetAppointments(9998, "Upcoming");
        }

        private void WhenGettingPreviousAppointmentsForProfile()
        {
            MockGetAppointments(9998, "Previous");
        }
        #endregion

        #region Then
        private void ThenUpcomingAppointmentsReturned()
        {
            Assert.IsTrue(_apptResults.All(a => a.StartDateTime >= _now));
        }

        private void ThenPreviousAppointmentsReturned()
        {
            Assert.IsTrue(_apptResults.All(a => a.StartDateTime < _now));
        }
        #endregion

        private void MockGetAppointments(int pID, string determiner)
        {
            using (var moq = AutoMock.GetLoose())
            {
                var _fakeContext = moq.Mock<IRPDContext>();
                _fakeContext.Setup(x => x.Appointment).Returns(_apptSet);
                _fakeContext.Setup(x => x.Provider).Returns(_prvSet);
                
                _apptDAL = new AppointmentDAL(_fakeContext.Object);

                _apptResults = _apptDAL.GetAppointmentsByDeterminer(pID, determiner);
            }
        }
    }
}
