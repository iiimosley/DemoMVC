using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DemoMVC;
using DemoMVC.Controllers;
using DemoMVC.Models;
using DemoMVC.DataAccess;
using System.Data.Entity;
using Autofac.Extras.Moq;

namespace DemoMVC.Tests.DataAccess
{
    [TestClass]
    public class ProviderDALTest
    {

        ProviderDAL _prvDAL;
        Provider _prv1;
        Provider _prv2;
        Provider _prv3;
        Provider _prv4;
        List<Provider> _prvResults;
        IDbSet<Provider> _prvSet;
        
        [TestMethod]
        public void ShouldReturnAllProviders()
        {
            GivenProviders();
            WhenSearchingForProvidersWithoutSearhText();
            ThenAllProvidersReturned();
        }

        [TestMethod]
        public void ShouldReturnProvidersMatchingSearchText()
        {
            GivenProviders();
            WhenSearchingForProvidersWithSearhText();
            ThenAllProvidersMatchingSubstringReturned();
        }

        #region Given
        private void GivenProviders()
        {
            _prv1 = new Provider()
            {
                Name = "Test Clinic"
            };

            _prv2 = new Provider()
            {
                Name = "Another Test Clinic"
            };

            _prv3 = new Provider()
            {
                Name = "Real Clinic"
            };

            _prv4 = new Provider()
            {
                Name = "Another Real Clinic"
            };

            _prvSet = new FakeProviderSet() { _prv1, _prv2, _prv3, _prv4 };
        }
        #endregion

        #region When
        private void WhenSearchingForProvidersWithoutSearhText()
        {
            MockGetProviders("");
        }

        private void WhenSearchingForProvidersWithSearhText()
        {
            MockGetProviders("test");
        }
        #endregion

        #region Then
        private void ThenAllProvidersReturned()
        {
            Assert.AreEqual(4, _prvResults.Count);
        }

        private void ThenAllProvidersMatchingSubstringReturned()
        {
            Assert.AreEqual(2, _prvResults.Count);
            Assert.IsTrue(_prvResults.All(p => p.Name.ToLower().Contains("test")));
        }
        #endregion

        private void MockGetProviders(string search)
        {
            using (var moq = AutoMock.GetLoose())
            {
                var _fakeContext = moq.Mock<IRPDContext>();
                _fakeContext.Setup(x => x.Provider).Returns(_prvSet);

                _prvDAL = new ProviderDAL(_fakeContext.Object);

                _prvResults = _prvDAL.GetProvidersBySearchString(search);
            }
        }
    }
}
