using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HospitalDBProject.Models;

namespace HospitalDBProject.Controllers
{
    public class InterventiController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        // GET: Interventi
        public ActionResult Index()
        {
            var interventoes = db.interventoes.Include(i => i.paziente).Include(i => i.referto);
            return View(interventoes.ToList());
        }

        // GET: Interventi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            intervento intervento = db.interventoes.Find(id);
            if (intervento == null)
            {
                return HttpNotFound();
            }
            return View(intervento);
        }

        // GET: Interventi/Create
        public ActionResult Create()
        {
            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "Nome");
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "Descrizione");
            return View();
        }

        // POST: Interventi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdIntervento,IdReferto,Giorno,OraInizio,OraFine,Descrizione,IdPaziente")] intervento intervento)
        {
            if (ModelState.IsValid)
            {
                db.interventoes.Add(intervento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "Nome", intervento.IdPaziente);
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "Descrizione", intervento.IdReferto);
            return View(intervento);
        }

        // GET: Interventi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            intervento intervento = db.interventoes.Find(id);
            if (intervento == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "Nome", intervento.IdPaziente);
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "Descrizione", intervento.IdReferto);
            return View(intervento);
        }

        // POST: Interventi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdIntervento,IdReferto,Giorno,OraInizio,OraFine,Descrizione,IdPaziente")] intervento intervento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(intervento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "Nome", intervento.IdPaziente);
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "Descrizione", intervento.IdReferto);
            return View(intervento);
        }

        // GET: Interventi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            intervento intervento = db.interventoes.Find(id);
            if (intervento == null)
            {
                return HttpNotFound();
            }
            return View(intervento);
        }

        // POST: Interventi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            intervento intervento = db.interventoes.Find(id);
            db.interventoes.Remove(intervento);
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
