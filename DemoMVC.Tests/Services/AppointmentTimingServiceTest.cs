using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using DemoMVC.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoMVC.Tests.Services
{
    [TestClass]
    public class AppointmentTimingServiceTest
    {
        string _time;
        int _timeValue;
        DateTime _date;
        DateTime _result;
        AppointmentTimingService _ats;
        List<SelectListItem> _timeSelection;

        [TestInitialize]
        public void TestSetup()
        {
            _ats = new AppointmentTimingService();
        }

        [TestMethod]
        public void ShouldReturnListofSelectableTimes()
        {
            GivenTimeSelectionServiceInstantiated();
            WhenGettingStaticTimeSelections();
            ThenListofTimeSelectionShouldBeReturned();
        }
        
        [TestMethod]
        public void ShouldReturnValidDateTime()
        {
            GivenSelectedDateAndTime();
            WhenCallingParseDateTime();
            ThenSingleDateTimeShouldBeConcatenatedFromDateAndTime();
        }

        [TestMethod]
        public void ShouldReturnValidIntegerForHourMinute()
        {
            GivenSelected24HourMinuteValue();
            WhenGettingValidationHourMinuteInParsedInt();
            ThenTimeValueShouldBeMatchingInteger();
        }

        #region Given
        private void GivenTimeSelectionServiceInstantiated()
        {
            Assert.IsNotNull(_ats);
        }

        private void GivenSelectedDateAndTime()
        {
            _date = new DateTime(2012, 12, 21);
            _time = "17:00";
        }
        private void GivenSelected24HourMinuteValue()
        {
            _time = "17:00";
        }
        #endregion

        #region When
        private void WhenGettingStaticTimeSelections()
        {
            _timeSelection = AppointmentTimingService.GetTimeSelections();
        }

        private void WhenCallingParseDateTime()
        {
            _result = _ats.ParseDateTime(_date, _time);
        }

        private void WhenGettingValidationHourMinuteInParsedInt()
        {
            _timeValue = _ats.GetValidationHourMinute(_time);
        }
        #endregion

        #region Then
        private void ThenListofTimeSelectionShouldBeReturned()
        {
            _timeSelection.ForEach(s => 
            {
                StringAssert.Matches(s.Text, new Regex("(1[0-2]|[1-9]):([0-5][0-9]) [AP]M$"));
                StringAssert.Matches(s.Value, new Regex(""));
            });
        }

        private void ThenSingleDateTimeShouldBeConcatenatedFromDateAndTime()
        {
            Assert.AreEqual(new DateTime(2012, 12, 21, 17, 0, 0), _result);
        }

        private void ThenTimeValueShouldBeMatchingInteger()
        {
            Assert.AreEqual(1700, _timeValue);
        }
        #endregion
    }
}
