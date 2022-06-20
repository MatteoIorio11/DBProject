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
                TempData["SuccessMessage"] = "Cura aggiunto con successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Cura non aggiunta";
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
            if (ModelState["refertoes"] != null)
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
            if (!this.CheckCura(cura))
            {
                db.curas.Remove(cura);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Cura eliminata successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Cura non eliminata";

            return RedirectToAction("Index");
        }

        public bool CheckCura(cura cura)
        {
            return db.refertoes.Any(refe => refe.curas.Any(cur => cur.IdCura == cura.IdCura)) ||
                db.medicinas.Any(med => med.curas.Any(cur => cur.IdCura == cura.IdCura));
        }

        [HttpGet]
        public ActionResult AggiungiMedicinale(int id)
        {
            cura cura = db.curas.Find(id);
            ViewBag.medicine = new MultiSelectList(db.medicinas, "IdMedicina", "IdMedicina");
            return View(cura);
        }

        [HttpPost]
        public ActionResult AggiungiMedicinale(cura cura)
        {
            var id_medicinali= ModelState["medicinas"].Value.AttemptedValue.Split(',');
            var medicine = this.AddMedicine(id_medicinali);
            cura cu = db.curas.Where(c => c.IdCura == cura.IdCura).First();
            medicine.ForEach(medi => cu.medicinas.Add(medi));
            db.Entry(cu).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private List<medicina> AddMedicine(string[] medicine)
        {
            var ids = medicine.Skip(1).ToList();
            List<medicina>  output = new List<medicina>();
            foreach(var id in ids)
            {
                output.Add(db.medicinas.Where(med => med.IdMedicina.ToString().Equals(id)).First());
            }
            return output;
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
