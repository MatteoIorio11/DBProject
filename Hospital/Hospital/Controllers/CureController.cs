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
    public class CureController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        // GET: Cure
        public ActionResult Index()
        {
            return View(db.curas.ToList());
        }

        // GET: Cure/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cura cura = db.curas.Find(id);
            if (cura == null)
            {
                return HttpNotFound();
            }
            return View(cura);
        }

        // GET: Cure/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cure/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCura,Descrizione")] cura cura)
        {
            if (ModelState.IsValid)
            {
                db.curas.Add(cura);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cura);
        }

        // GET: Cure/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cura cura = db.curas.Find(id);
            if (cura == null)
            {
                return HttpNotFound();
            }
            ViewBag.referti = new SelectList(db.refertoes, "IdReferto", "IdReferto");
            return View(cura);
        }

        // POST: Cure/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCura,refertoes")] cura cura)
        {
            if (this.Check())
            {
                var refer = ModelState["refertoes"].Value.AttemptedValue.Split(',')[1];
                var cu = db.curas.Where(cur => cura.IdCura == cur.IdCura).First();
                var referto = db.refertoes.Where(refe => refe.IdReferto.ToString().Equals(refer)).First();
                cu.refertoes.Add(referto);
                db.Entry(cu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cura);
        }

        public bool Check()
        {
            return true;
        }

        // GET: Cure/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cura cura = db.curas.Find(id);
            if (cura == null)
            {
                return HttpNotFound();
            }
            return View(cura);
        }

        // POST: Cure/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cura cura = db.curas.Find(id);
            db.curas.Remove(cura);
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
