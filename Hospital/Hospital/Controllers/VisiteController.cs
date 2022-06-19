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
    public class VisiteController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        // GET: Visite
        public ActionResult Index()
        {
            var visitas = db.visitas.Include(v => v.paziente).Include(v => v.referto);
            return View(visitas.ToList());
        }

        // GET: Visite/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            visita visita = db.visitas.Find(id);
            if (visita == null)
            {
                return HttpNotFound();
            }
            return View(visita);
        }

        // GET: Visite/Create
        public ActionResult Create()
        {
            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "IdPaziente");
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "IdReferto");
            return View();
        }

        // POST: Visite/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdVisita,IdReferto,Descrizione,IdPaziente,OraInizio,OraFine,Giorno")] visita visita)
        {
            if (ModelState.IsValid)
            {
                db.visitas.Add(visita);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "IdPaziente", visita.IdPaziente);
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "IdReferto", visita.IdReferto);
            return View(visita);
        }

        // GET: Visite/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            visita visita = db.visitas.Find(id);
            if (visita == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "IdPaziente", visita.IdPaziente);
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "IdReferto", visita.IdReferto);
            return View(visita);
        }

        // POST: Visite/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdVisita,IdReferto,Descrizione,IdPaziente,OraInizio,OraFine,Giorno")] visita visita)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visita).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "IdPaziente", visita.IdPaziente);
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "IdReferto", visita.IdReferto);
            return View(visita);
        }

        // GET: Visite/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            visita visita = db.visitas.Find(id);
            if (visita == null)
            {
                return HttpNotFound();
            }
            return View(visita);
        }

        // POST: Visite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            visita visita = db.visitas.Find(id);
            db.visitas.Remove(visita);
            db.SaveChanges();
            return RedirectToAction("Index");
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
