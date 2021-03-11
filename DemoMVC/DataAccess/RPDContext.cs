using DemoMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DemoMVC.DataAccess
{
    //public interface IDBContext
    //{
    //    DbEntityEntry Entry(Object entity);
    //    IEnumerable<DbEntityValidationResult> GetValidationErrors();
    //    Int32 SaveChanges();
    //    DbSet Set(Type entityType);
    //    IDbSet<TEntity> Set<TEntity>() where TEntity : class;
    //}

    public interface IRPDContext : IDisposable
    {
        IDbSet<Provider> Provider { get; set; }
        IDbSet<Profile> Profile { get; set; }
        IDbSet<Appointment> Appointment { get; set; }
        Int32 SaveChanges();
        DbEntityEntry Entry(object entity);
    }

    public class RPDContext : DbContext, IRPDContext 
    {
        public RPDContext() : base("name=DefaultConnection") { }

        public IDbSet<Provider> Provider { get; set; }
        public IDbSet<Profile> Profile { get; set; }
        public IDbSet<Appointment> Appointment { get; set; }
    }
}

