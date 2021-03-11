using DemoMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoMVC.DataAccess
{
    public interface IProviderDAL : IDisposable
    {
        Provider GetProviderByID(Guid ID);
        List<Provider> GetProvidersBySearchString(string search);
    }

    public class ProviderDAL : IProviderDAL
    {
        private IRPDContext _db;


        public ProviderDAL(IRPDContext db)
        {
            _db = db;
        }

        public Provider GetProviderByID(Guid ID) => _db.Provider.Find(ID);

        public List<Provider> GetProvidersBySearchString(string search)
        {
            if (String.IsNullOrEmpty(search))
                return _db.Provider.OrderBy(p => p.Name).ToList();
            else
                return _db.Provider.Where(p => p.Name.ToLower().Contains(search.ToLower())).OrderBy(p => p.Name).ToList();
        }

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}