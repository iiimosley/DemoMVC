using DemoMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoMVC.ViewModels
{
    public class ProviderSearchViewModel
    {
        public string SearchQuery { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<int> TotalPages { get; set; }

        public IEnumerable<Provider> Providers { get; set; }
    }
}
