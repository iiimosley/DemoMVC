using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using DemoMVC.DataAccess;
using DemoMVC.Models;
using DemoMVC.Repositories;
using DemoMVC.Services;
using DemoMVC.ViewModels;

namespace DemoMVC.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private int _currentProfileID;
        private IAppointmentRepository _apptRepo;
        private IAppointmentTimingService _apptTiming;
        private IProfileService _profileService;

        public AppointmentController(IAppointmentRepository apptRepo, IAppointmentTimingService apptTiming, IProfileService profileService)
        {
            _apptRepo = apptRepo;
            _apptTiming = apptTiming;
            _profileService = profileService;
        }

        public ActionResult Index()
        {
            _currentProfileID = _profileService.GetCurrentProfileID();
            if (_currentProfileID > 0)
                return View();
            else
                return RedirectToAction("Login", "Account");
        }

        public ActionResult List([Bind(Include="Previous,Upcoming")] string results)
        {
            _currentProfileID = _profileService.GetCurrentProfileID();
            List<AppointmentListViewModel> model;

            ViewBag.DeterminerState = results;
            model = _apptRepo.GetAppointmentsByDeterminer(_currentProfileID, results);
            return View(model);
        }

        public ActionResult Details(Guid? id)
        {
            Appointment appt;

            if (id == null || id == Guid.Empty)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            else 
                appt = _apptRepo.GetAppointmentByID(id.Value);


            if (appt == null)
                return HttpNotFound();
            else
                return View(appt);
        }

        public ActionResult Editor(AppointmentEditorViewModel aevm) => View(aevm);

        public ActionResult Create(Guid prvid, int prfid) => View("Editor", _apptRepo.InitializeNewAppointmentEditor(prvid, prfid));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppointmentEditorViewModel aevm)
        {
            ValidateAppointmentEditor(aevm);

            if (ModelState.IsValid)
                _apptRepo.InsertAppointment(aevm);
            else
                return View(aevm);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Appointment appt = _apptRepo.GetAppointmentByID(id.Value);
            if (appt == null)
                return HttpNotFound();
            else
                return View("Editor", _apptRepo.InitializeAppointmentEditor(appt));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AppointmentEditorViewModel aevm)
        {
            ValidateAppointmentEditor(aevm);

            if (ModelState.IsValid)
                _apptRepo.UpdateAppointment(aevm);
            else
                return View(aevm);

            return RedirectToAction("Index");
        }


        public ActionResult Delete(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Appointment appt = _apptRepo.GetAppointmentByID(id.Value);
            if (appt == null)
                return HttpNotFound();
            else
                return View(_apptRepo.InitializeAppointmentEditor(appt));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            _apptRepo.DeleteAppointmentByID(id);
            return RedirectToAction("Index");
        }

        public ActionResult PreviousPage()
        {
            return Redirect(Request.UrlReferrer.ToString());
        }
        
        private void ValidateAppointmentEditor(AppointmentEditorViewModel aevm)
        {
            DateTime apptDate;

            foreach (var v in ModelState.Values) { v.Errors.Clear(); }

            if (!DateTime.TryParse(aevm.AppointmentDate, out apptDate))
                ModelState.AddModelError("Appointment Date", "Please enter valid appointment date.");
            else
                if (_apptTiming.ParseDateTime(apptDate, aevm.AppointmentStart).AddHours(3) < DateTime.Now)
                ModelState.AddModelError("Appointment Schedule", "Please set appointment for atleast 3 hours out from the current time.");
            if (_apptTiming.GetValidationHourMinute(aevm.AppointmentStart) >= _apptTiming.GetValidationHourMinute(aevm.AppointmentEnd))
                ModelState.AddModelError("Appointment Time", "Appointment End Time must be atleast 30 min greater than Start Time");
        }
    }
}
