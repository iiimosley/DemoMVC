using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DemoMVC;
using DemoMVC.Models;
using DemoMVC.ViewModels;
using Autofac.Extras.Moq;
using DemoMVC.Repositories;
using DemoMVC.DataAccess;
using Bogus;

namespace DemoMVC.Tests.Repositories
{
    [TestClass]
    public class ProviderRepositoryTest
    {
        List<Provider> _providers;
        ProviderRepository _prvRepo;
        ProviderSearchViewModel _psvm;

        [TestMethod]
        public void ShouldReturnFullPageOfProviders()
        {
            GivenTwentyProviders();
            WhenSearchingForProvidersOnFirstPage();
            ThenFullPageOfProvidersReturned();
        }



        [TestMethod]
        public void ShouldReturnPartiallyFilledPageOfProviders()
        {
            GivenTwentyProviders();
            WhenSearchingForProvidersOnSecond();
            ThenPartialSetOfProvidersReturned();
        }
        

        #region Given
        private void GivenTwentyProviders()
        {
            _providers = new List<Provider>();

            var providerFaker = new Faker<Provider>()
            .RuleFor(p => p.Address, f => f.Address.StreetAddress())
            .RuleFor(p => p.City, f => f.Address.City())
            .RuleFor(p => p.State, f => f.Address.StateAbbr())
            .RuleFor(p => p.Name, (f, p) => $"{p.City} {f.PickRandom("Clinic", "Hospital", "Family Practice")}");
            
            _providers.AddRange(providerFaker.Generate(20));
        }
        #endregion

        #region When

        private void WhenSearchingForProvidersOnFirstPage()
        {
            MockGetProvidersSearchResults(1);
        }

        private void WhenSearchingForProvidersOnSecond()
        {
            MockGetProvidersSearchResults(2);
        }
        #endregion

        #region Then
        private void ThenFullPageOfProvidersReturned()
        {
            Assert.AreEqual(12, _psvm.Providers.Count());
        }

        private void ThenPartialSetOfProvidersReturned()
        {
            Assert.AreEqual(8, _psvm.Providers.Count());
        }
        #endregion

        private void MockGetProvidersSearchResults(int pg)
        {
            using (var moq = AutoMock.GetLoose())
            {
                moq.Mock<IProviderDAL>().Setup(x => x.GetProvidersBySearchString("")).Returns(_providers);
                _prvRepo = moq.Create<ProviderRepository>();

                _psvm = _prvRepo.GetProviderSearchResults("", pg);
                moq.Mock<IProviderDAL>().Verify(x => x.GetProvidersBySearchString(""));
            }
        }
    }
}
