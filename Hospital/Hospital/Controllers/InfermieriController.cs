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
            if (ModelState.IsValid)
            {
                db.infermieres.Add(infermiere);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(infermiere);
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
            infermiere infermiere = db.infermieres.Find(id);
            db.infermieres.Remove(infermiere);
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
