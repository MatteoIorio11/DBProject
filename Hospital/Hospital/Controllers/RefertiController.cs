using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hospital.Models;

namespace Hospital.Controllers
{
    public class RefertiController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        // GET: Referti
        public ActionResult Index()
        {
            return View(db.refertoes.ToList());
        }

        // GET: Referti/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            referto referto = db.refertoes.Find(id);
            if (referto == null)
            {
                return HttpNotFound();
            }
            return View(referto);
        }

        // GET: Referti/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Referti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdReferto,Descrizione")] referto referto)
        {
            if (ModelState.IsValid)
            {
                db.refertoes.Add(referto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(referto);
        }

        // GET: Referti/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            referto referto = db.refertoes.Find(id);
            if (referto == null)
            {
                return HttpNotFound();
            }
            return View(referto);
        }

        // POST: Referti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdReferto,Descrizione")] referto referto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(referto).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Referto aggiunto con successo";
                return RedirectToAction("Index");
            }
            return View(referto);
        }

        // GET: Referti/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            referto referto = db.refertoes.Find(id);
            if (referto == null)
            {
                return HttpNotFound();
            }
            return View(referto);
        }

        // POST: Referti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            referto referto = db.refertoes.Find(id);
            if (!this.CheckReferto(referto))
            {
                db.refertoes.Remove(referto);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Referto eliminato successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Referto non eliminato";

            return RedirectToAction("Index");
        }

        private bool CheckReferto(referto referto)
        {
            return db.interventoes.Any(inter => inter.IdReferto == referto.IdReferto) ||
                db.visitas.Any(visit => visit.IdReferto == referto.IdReferto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
