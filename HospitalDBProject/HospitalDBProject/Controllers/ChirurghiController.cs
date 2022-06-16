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
    public class ChirurghiController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        // GET: Chirurghi
        public ActionResult Index()
        {
            return View(db.chirurgoes.ToList());
        }

        // GET: Chirurghi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chirurgo chirurgo = db.chirurgoes.Find(id);
            if (chirurgo == null)
            {
                return HttpNotFound();
            }
            return View(chirurgo);
        }

        // GET: Chirurghi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chirurghi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdChirurgo,Nome,Cognome,CodiceFiscale,DataNascita,Genere,InterventiEffettuati")] chirurgo chirurgo)
        {
            if (ModelState.IsValid)
            {
                db.chirurgoes.Add(chirurgo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chirurgo);
        }

        // GET: Chirurghi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chirurgo chirurgo = db.chirurgoes.Find(id);
            if (chirurgo == null)
            {
                return HttpNotFound();
            }
            return View(chirurgo);
        }

        // POST: Chirurghi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdChirurgo,Nome,Cognome,CodiceFiscale,DataNascita,Genere,InterventiEffettuati")] chirurgo chirurgo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chirurgo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chirurgo);
        }

        // GET: Chirurghi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chirurgo chirurgo = db.chirurgoes.Find(id);
            if (chirurgo == null)
            {
                return HttpNotFound();
            }
            return View(chirurgo);
        }

        // POST: Chirurghi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            chirurgo chirurgo = db.chirurgoes.Find(id);
            db.chirurgoes.Remove(chirurgo);
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
