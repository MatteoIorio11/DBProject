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
    public class InfermieriController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        // GET: Infermieri
        public ActionResult Index()
        {
            return View(db.infermieres.ToList());
        }

        // GET: Infermieri/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            infermiere infermiere = db.infermieres.Find(id);
            if (infermiere == null)
            {
                return HttpNotFound();
            }
            return View(infermiere);
        }

        // GET: Infermieri/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Infermieri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdInfermiere,Nome,Cognome,CodiceFiscale,DataNascita,Genere,NumeroDiTelefono")] infermiere infermiere)
        {
            if (!this.Check(infermiere))
            {
                db.infermieres.Add(infermiere);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Infermiere aggiunto con successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Infermiere aggiunto con successo";
            return RedirectToAction("Index");
        }

        private bool Check(infermiere infermiere)
        {
            return db.chirurgoes.Any(ch => ch.CodiceFiscale == infermiere.CodiceFiscale) ||
                   db.pazientes.Any(pa => pa.CodiceFiscale == infermiere.CodiceFiscale) ||
                   db.infermieres.Any(inf => inf.CodiceFiscale == infermiere.CodiceFiscale) ||
                   db.medicos.Any(med => med.CodiceFiscale == infermiere.CodiceFiscale);
        }

        // GET: Infermieri/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            infermiere infermiere = db.infermieres.Find(id);
            if (infermiere == null)
            {
                return HttpNotFound();
            }
            return View(infermiere);
        }

        // POST: Infermieri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdInfermiere,Nome,Cognome,CodiceFiscale,DataNascita,Genere,NumeroDiTelefono")] infermiere infermiere)
        {
            if (ModelState.IsValid)
            {
                db.Entry(infermiere).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(infermiere);
        }

        // GET: Infermieri/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            infermiere infermiere = db.infermieres.Find(id);
            if (infermiere == null)
            {
                return HttpNotFound();
            }
            return View(infermiere);
        }

        // POST: Infermieri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            infermiere infermiere = db.infermieres.Where(inf => inf.IdInfermiere == id).First(); ;
            if (!this.CheckOperazioni(infermiere))
            {
                db.infermieres.Remove(infermiere);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Infermiere eliminato con successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Infermiere non eliminato ";
            return RedirectToAction("Index");
        }

        private bool CheckOperazioni(infermiere infermiere)
        {
            return db.visitas.Any(vis => vis.infermieres.Any(infer=> infer.IdInfermiere == infermiere.IdInfermiere))||
                db.interventoes.Any(inter => inter.infermieres.Any(inf => inf.IdInfermiere == infermiere.IdInfermiere));
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
