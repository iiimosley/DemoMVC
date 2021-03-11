using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DemoMVC;
using DemoMVC.Controllers;
using DemoMVC.Repositories;
using DemoMVC.Services;
using DemoMVC.ViewModels;
using Autofac.Extras.Moq;
using Moq;

namespace DemoMVC.Tests.Controllers
{
    [TestClass]
    public class AppointmentControllerTest
    {
        AppointmentController _ctrlr;
        List<AppointmentListViewModel> _alvmList;
        AppointmentEditorViewModel _aevm;
        ActionResult _actionResult;
        ViewResult _viewResult;
        AutoMock _moq;

        string _errorKey;
        string _errorMsg;

        [TestInitialize]
        public void TestSetup()
        {
            _moq = AutoMock.GetLoose();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _moq.Dispose();
        }

        [TestMethod]
        public void ShouldRedirectToAccountLoginIfNotLoggedIn()
        {
            //0 = not user logged in
            GivenControllerwithMockDeps(0);
            WhenAttemptingToAccessAppointmentsIndex();
            ThenActionResultIsRedirectToAccountLogin();
        }
        
        [TestMethod]
        public void ShouldRedirectToIndexIfLoggedIn()
        {
            GivenControllerwithMockDeps();
            WhenAttemptingToAccessAppointmentsIndex();
            ThenActionResultIsIndexViewResult();
        }
        
        [TestMethod]
        public void ShouldReturnUpcomingAppointments()
        {
            GivenAppointmentListVM();
            GivenControllerwithMockDeps();
            WhenGettingUpcomingAppointments();
            ThenViewResultShouldBeForUpcomingAppointments();
        }

        [TestMethod]
        public void ShouldThrowModelStateErrorOnDateValidity()
        {
            GivenExpectedErrors("Appointment Date", "Please enter valid appointment date.");
            GivenAppointmentEditorVMWithBadDateString();
            GivenControllerwithMockDeps();
            WhenAttemptingToCreateAppointment();
            ThenModelStateErrorSetForAppointmentDate();
        }

        [TestMethod]
        public void ShouldThrowModelStateErrorOnApptStartTime()
        {
            GivenExpectedErrors("Appointment Schedule", "Please set appointment for atleast 3 hours out from the current time.");
            GivenAppointmentEditorVMWithEarlyStartTime();
            GivenControllerwithMockDeps();
            WhenAttemptingToCreateAppointment();
            ThenModelStateErrorSetForAppointmentDate();
        }

        [TestMethod]
        public void ShouldThrowModelStateErrorOnApptTiming()
        {
            GivenExpectedErrors("Appointment Time", "Appointment End Time must be atleast 30 min greater than Start Time");
            GivenAppointmentEditorVMWithEndTimeSameAsStartTime();
            GivenControllerwithMockDeps();
            WhenAttemptingToCreateAppointment();
            ThenModelStateErrorSetForAppointmentDate();
        }

        #region Given
        private void GivenExpectedErrors(string key, string message)
        {
            _errorKey = key;
            _errorMsg = message;
        }

        private void GivenAppointmentListVM()
        {
            _alvmList = new List<AppointmentListViewModel>() { new AppointmentListViewModel() };
        }

        private void GivenAppointmentEditorVMWithBadDateString()
        {
            _aevm = new AppointmentEditorViewModel()
            {
                AppointmentDate = "blaaaaaaah"
            };
        }

        private void GivenAppointmentEditorVMWithEarlyStartTime()
        {
            var now = DateTime.Now;

            _aevm = new AppointmentEditorViewModel()
            {
                AppointmentDate = now.ToString("MM/dd/yyyy"),
                AppointmentStart = now.AddHours(1).ToString("H:mm")
            };
        }

        private void GivenAppointmentEditorVMWithEndTimeSameAsStartTime()
        {
            var tmrw = DateTime.Now.AddDays(1);

            _aevm = new AppointmentEditorViewModel()
            {
                AppointmentDate = tmrw.ToString("MM/dd/yyyy"),
                AppointmentStart = tmrw.ToString("H:mm"),
                AppointmentEnd = tmrw.ToString("H:mm")
            };
        }

        private void GivenControllerwithMockDeps(int uid = 999999)
        {
            var _apptRepo = _moq.Mock<IAppointmentRepository>();
            var _apptTimingSrv = _moq.Mock<IAppointmentTimingService>();
            var _profileSrv = _moq.Mock<IProfileService>();

            _profileSrv.Setup(x => x.GetCurrentProfileID()).Returns(uid);
            _apptRepo.Setup(x => x.GetAppointmentsByDeterminer(It.IsAny<int>(), It.IsAny<string>())).Returns(_alvmList);

            _ctrlr = new AppointmentController(_apptRepo.Object, _apptTimingSrv.Object, _profileSrv.Object);
        }
        #endregion

        #region When
        private void WhenGettingUpcomingAppointments()
        {
            _viewResult = _ctrlr.List("Upcoming") as ViewResult;
        }
        
        private void WhenAttemptingToAccessAppointmentsIndex()
        {
            _actionResult = _ctrlr.Index();
        }

        private void WhenAttemptingToCreateAppointment()
        {
            _actionResult = _ctrlr.Create(_aevm);
        }
        #endregion

        #region Then
        private void ThenViewResultShouldBeForUpcomingAppointments()
        {
            Assert.IsNotNull(_viewResult);
            Assert.AreEqual("Upcoming", _viewResult.ViewBag.DeterminerState);
        }
        
        private void ThenActionResultIsRedirectToAccountLogin()
        {
            Assert.IsTrue(_actionResult.GetType() == typeof(RedirectToRouteResult));
            var assertResult = (RedirectToRouteResult)_actionResult;
            Assert.AreEqual("Login", assertResult.RouteValues["action"]);
            Assert.AreEqual("Account", assertResult.RouteValues["controller"]);
        }

        private void ThenActionResultIsIndexViewResult()
        {
            Assert.IsTrue(_actionResult.GetType() == typeof(ViewResult));
        }

        private void ThenModelStateErrorSetForAppointmentDate()
        {
            Assert.IsTrue(_actionResult.GetType() == typeof(ViewResult));
            Assert.IsFalse(_ctrlr.ModelState.IsValid);
            Assert.IsTrue(_ctrlr.ModelState[_errorKey].Errors.Count == 1);
            Assert.AreEqual(_errorMsg, _ctrlr.ModelState[_errorKey].Errors[0].ErrorMessage);
        }
        #endregion
    }
}
