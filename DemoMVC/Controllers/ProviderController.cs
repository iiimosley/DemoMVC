using DemoMVC.DataAccess;
using DemoMVC.Filters;
using DemoMVC.Models;
using DemoMVC.Repositories;
using DemoMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoMVC.Controllers
{
    public class ProviderController : Controller
    {
        IProviderRepository providerRepo;

        public ProviderController(IProviderRepository _providerRepo)
        {
            providerRepo = _providerRepo;
        }

        public ActionResult NewSearch() => View("Search");
        
        public ActionResult Search(string search, int page = 1)
        {
            var model = providerRepo.GetProviderSearchResults(search, page);

            if (model.Providers.Count() == 0)
                ViewBag.ErrorMessage = $"Sorry, no results for \'{search}\'";

            return View(model);
        }
        
        public ActionResult Details(Guid id) => View(providerRepo.GetProviderByID(id));

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
