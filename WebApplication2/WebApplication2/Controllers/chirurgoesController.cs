using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class chirurgoesController : Controller
    {
        private hospitaldbEntities db = new hospitaldbEntities();

        // GET: chirurgoes
        public ActionResult Index()
        {
            return View(db.chirurgoes.ToList());
        }

        // GET: chirurgoes/Details/5
        public ActionResult Details(string id)
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

        // GET: chirurgoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: chirurgoes/Create
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

        // GET: chirurgoes/Edit/5
        public ActionResult Edit(string id)
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

        // POST: chirurgoes/Edit/5
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

        // GET: chirurgoes/Delete/5
        public ActionResult Delete(string id)
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

        // POST: chirurgoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
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
