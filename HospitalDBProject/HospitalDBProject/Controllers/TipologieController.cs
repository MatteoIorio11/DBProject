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
    public class TipologieController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        // GET: Tipologie
        public ActionResult Index()
        {
            return View(db.tipologias.ToList());
        }

        // GET: Tipologie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipologia tipologia = db.tipologias.Find(id);
            if (tipologia == null)
            {
                return HttpNotFound();
            }
            return View(tipologia);
        }

        // GET: Tipologie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tipologie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTipologia,Tipo,Descrizione")] tipologia tipologia)
        {
            if (ModelState.IsValid)
            {
                db.tipologias.Add(tipologia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipologia);
        }

        // GET: Tipologie/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipologia tipologia = db.tipologias.Find(id);
            if (tipologia == null)
            {
                return HttpNotFound();
            }
            return View(tipologia);
        }

        // POST: Tipologie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTipologia,Tipo,Descrizione")] tipologia tipologia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipologia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipologia);
        }

        // GET: Tipologie/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipologia tipologia = db.tipologias.Find(id);
            if (tipologia == null)
            {
                return HttpNotFound();
            }
            return View(tipologia);
        }

        // POST: Tipologie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipologia tipologia = db.tipologias.Find(id);
            db.tipologias.Remove(tipologia);
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
