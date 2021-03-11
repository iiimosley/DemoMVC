using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DemoMVC;
using DemoMVC.Controllers;
using Autofac.Extras.Moq;
using DemoMVC.Repositories;
using Moq;
using DemoMVC.ViewModels;
using DemoMVC.Models;
using Bogus;

namespace DemoMVC.Tests.Controllers
{
    [TestClass]
    public class ProviderControllerTest
    {
        ProviderController _ctrlr;
        Mock<IProviderRepository> _prvRepo;
        AutoMock _moq;

        string _randomSearch; 

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
        public void ShouldGiveMessageOfNoProvidersFound()
        {
            GivenRepoToReturnNoProviderResults();
            WhenSearchingForProviders();
            ThenNoResultsMessageShouldRender();
        }

        #region Given
        private void GivenRepoToReturnNoProviderResults()
        {
            var psvm = new ProviderSearchViewModel() { Providers = new List<Provider>() };

            _prvRepo = _moq.Mock<IProviderRepository>();
            _prvRepo.Setup(x => x.GetProviderSearchResults(It.IsAny<string>(), It.IsAny<int>())).Returns(psvm);
        }
        #endregion


        #region When
        private void WhenSearchingForProviders()
        {
            var f = new Faker();
            _randomSearch = f.Address.City();

            _ctrlr = new ProviderController(_prvRepo.Object);
            _ctrlr.Search(_randomSearch);
        }
        #endregion


        #region Then
        private void ThenNoResultsMessageShouldRender()
        {
            Assert.AreEqual($"Sorry, no results for \'{_randomSearch}\'", _ctrlr.ViewBag.ErrorMessage);
        }
        #endregion
    }
}
