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
    public class MedicineController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        // GET: Medicine
        public ActionResult Index()
        {
            return View(db.medicinas.ToList());
        }

        // GET: Medicine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medicina medicina = db.medicinas.Find(id);
            if (medicina == null)
            {
                return HttpNotFound();
            }
            return View(medicina);
        }

        // GET: Medicine/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medicine/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdMedicina,Nome,Marca,Descrizione")] medicina medicina)
        {
            if (ModelState.IsValid)
            {
                db.medicinas.Add(medicina);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Medico aggiunto con successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Medico non aggiunto ";
            return View(medicina);
        }

        // GET: Medicine/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medicina medicina = db.medicinas.Find(id);
            if (medicina == null)
            {
                return HttpNotFound();
            }
            return View(medicina);
        }

        // POST: Medicine/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMedicina,Nome,Marca,Descrizione")] medicina medicina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicina);
        }

        // GET: Medicine/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medicina medicina = db.medicinas.Find(id);
            if (medicina == null)
            {
                return HttpNotFound();
            }
            return View(medicina);
        }

        // POST: Medicine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            medicina medicina = db.medicinas.Find(id);
            if (!this.CheckMedicina(medicina))
            {
                db.medicinas.Remove(medicina);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Medico modificato con successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Medico non modificato ";

            return RedirectToAction("Index");
        }

        public bool CheckMedicina(medicina medicina)
        {
            return db.curas.Any(cur => cur.medicinas.Any(med => med.IdMedicina == medicina.IdMedicina));
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
