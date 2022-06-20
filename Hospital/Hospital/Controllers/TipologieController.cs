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
                TempData["SuccessMessage"] = "Tipologia aggiunta con successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Tipologia non aggiunta";

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
                TempData["SuccessMessage"] = "Tipologia eliminata con successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Tipologia non eliminata";
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
            if (this.CheckTipologia(tipologia))
            {
                db.tipologias.Remove(tipologia);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Tipologia aggiunto con successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Medico non eliminato";
            return RedirectToAction("Index");
        }

        public bool CheckTipologia(tipologia tipologia)
        {
            return db.chirurgoes.Any(ch => ch.tipologias.Any(tip => tip.IdTipologia == tipologia.IdTipologia)) ||
                db.medicos.Any(med => med.tipologias.Any(tip => tip.IdTipologia == tipologia.IdTipologia)) ||
                db.interventoes.Any(inter => inter.tipologias.Any(tip => tip.IdTipologia == tipologia.IdTipologia)) ||
                db.visitas.Any(vist => vist.tipologias.Any(tip => tip.IdTipologia == tipologia.IdTipologia));
        }

        [HttpGet]
        public ActionResult TopDieciTipologie()
        {
            //TOP 10 TIPOLOGIE DI INTERVENTI E VISITE FATTE
            var tipologiaInterventi =
                from tipoInter in db.interventoes.SelectMany(inter => inter.tipologias).ToList()
                select tipoInter.IdTipologia;
            var tipologiaVisite =
                 from tipoVisita in db.visitas.SelectMany(vist=> vist.tipologias).ToList()
                 select tipoVisita.IdTipologia;
            var tipologieTotali = tipologiaInterventi.Concat(tipologiaVisite);

            var classifica =
                from tipo in tipologieTotali
                group tipo by tipo into GruppoTipologia
                select GruppoTipologia;
                
            var classificaFinale = classifica.Select(element => new Tuple<int, int>(element.Key, element.Count()))
                .ToList()
                    .OrderByDescending(tupl => tupl.Item2)
                    .Take(10);
            ViewBag.classifica = classificaFinale;
            return View();
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
