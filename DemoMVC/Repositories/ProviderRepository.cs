using DemoMVC.DataAccess;
using DemoMVC.Models;
using DemoMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoMVC.Repositories
{
    public interface IProviderRepository
    {
        Provider GetProviderByID(Guid ID);
        ProviderSearchViewModel GetProviderSearchResults(string search, int page = 1);
    }

    public class ProviderRepository : IProviderRepository
    {
        IProviderDAL _providerDAL;

        public ProviderRepository(IProviderDAL providerDAL)
        {
            _providerDAL = providerDAL;
        }

        public Provider GetProviderByID(Guid ID) => _providerDAL.GetProviderByID(ID);

        public ProviderSearchViewModel GetProviderSearchResults(string search, int page = 1)
        {
            List<Provider> providers;
            int pageSize = 12;
            int pageSkip = pageSize * (page - 1);

            providers = _providerDAL.GetProvidersBySearchString(search);

            var dividend = (int)Math.Ceiling((decimal)providers.Count() / pageSize);

            return new ProviderSearchViewModel
            {
                SearchQuery = search,
                CurrentPage = page,
                TotalPages = Enumerable.Range(1, dividend),
                Providers = providers.Skip(pageSkip).Take(pageSize)
            };
        }
    }
}