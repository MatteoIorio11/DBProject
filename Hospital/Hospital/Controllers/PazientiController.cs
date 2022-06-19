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
    public class PazientiController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        // GET: Pazienti
        public ActionResult Index()
        {
            return View(db.pazientes.ToList());
        }

        // GET: Pazienti/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paziente paziente = db.pazientes.Find(id);
            if (paziente == null)
            {
                return HttpNotFound();
            }
            return View(paziente);
        }

        // GET: Pazienti/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pazienti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPaziente,Nome,Cognome,CodiceFiscale,DataNascita,Genere,NumeroInterventiEffettuati,NumeroDiTelefono")] paziente paziente)
        {
            
            if (ModelState.IsValid)
            {
                paziente.NumeroInterventiEffettuati = 0;
                db.pazientes.Add(paziente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paziente);
        }

        // GET: Pazienti/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paziente paziente = db.pazientes.Find(id);
            if (paziente == null)
            {
                return HttpNotFound();
            }
            return View(paziente);
        }

        // POST: Pazienti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPaziente,Nome,Cognome,CodiceFiscale,DataNascita,Genere,NumeroInterventiEffettuati,NumeroDiTelefono")] paziente paziente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paziente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paziente);
        }

        // GET: Pazienti/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paziente paziente = db.pazientes.Find(id);
            if (paziente == null)
            {
                return HttpNotFound();
            }
            return View(paziente);
        }

        // POST: Pazienti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            paziente paziente = db.pazientes.Find(id);
            db.pazientes.Remove(paziente);
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

        public ActionResult VisualizzaReferti(int id)
        {
            List<referto> referti = new List<referto>();
            var InterventiPaziente =
                from inter in db.interventoes
                where inter.IdPaziente == id
                select inter.IdReferto;
            var VisitePaziente =
                from vis in db.visitas
                where vis.IdPaziente == id
                select vis.IdReferto;
            var RefertiTotali = InterventiPaziente.Union(VisitePaziente);
            referti = RefertiTotali.Where(val => val.HasValue)
                .SelectMany(refe => db.refertoes.Where(re => re.IdReferto == refe.Value)).ToList();
            return View(referti);
        }

        public ActionResult VisualizzaCure(int id)
        {
            var InterventiPaziente =
                from inter in db.interventoes
                where inter.IdPaziente == id
                select inter.IdReferto;
            var VisitePaziente =
                from vis in db.visitas
                where vis.IdPaziente == id
                select vis.IdReferto;
            var RefertiTotali = InterventiPaziente.Union(VisitePaziente);
            var referti = RefertiTotali.Where(val => val.HasValue)
                .SelectMany(refe => db.refertoes.Where(re => re.IdReferto == refe.Value)).ToList();
            List<cura> cure = referti.SelectMany(refer => db.curas
                .Where(cu => cu.refertoes.Any(refe => refe.IdReferto == refer.IdReferto))
                .Select(cu => cu))
                .ToList();
            return View(cure);
        }

        [HttpGet]
        public ActionResult ListInterventi()
        {
            return View(db.pazientes.Select(paz => paz).ToList());
        }
    }
}
