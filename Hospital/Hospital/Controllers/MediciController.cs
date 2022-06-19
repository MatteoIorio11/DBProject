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
    public class MediciController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        // GET: Medici
        public ActionResult Index()
        {
            return View(db.medicos.ToList());
        }

        // GET: Medici/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medico medico = db.medicos.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }

        // GET: Medici/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medici/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdMedico,Nome,Cognome,CodiceFiscale,DataNascita,Genere,NumeroVisiteEffettuate,NumeroDiTelefono")] medico medico)
        {
            if (ModelState.IsValid)
            {
                db.medicos.Add(medico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medico);
        }

        // GET: Medici/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medico medico = db.medicos.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }

        // POST: Medici/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMedico,Nome,Cognome,CodiceFiscale,DataNascita,Genere,NumeroVisiteEffettuate,NumeroDiTelefono")] medico medico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medico);
        }

        // GET: Medici/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medico medico = db.medicos.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }

        // POST: Medici/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            medico medico = db.medicos.Find(id);
            db.medicos.Remove(medico);
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

        [HttpGet, ActionName("ListVisite")]
        public ActionResult ListVisite()
        {
            return View(db.medicos.Select(med => med).ToList());
        }
    }
}
