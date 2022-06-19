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
    public class InterventiController : Controller
    {
        private List<chirurgo> chirurghi = new List<chirurgo>();
        private HospitalEntities db = new HospitalEntities();

        // GET: Interventi
        public ActionResult Index()
        {
            var interventoes = db.interventoes.Include(i => i.paziente).Include(i => i.referto);
            return View(interventoes.ToList());
        }

        // GET: Interventi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            intervento intervento = db.interventoes.Find(id);
            if (intervento == null)
            {
                return HttpNotFound();
            }
            return View(intervento);
        }

        // GET: Interventi/Create
        public ActionResult Create()
        {
            ViewBag.chirurghi = new MultiSelectList(db.chirurgoes, "IdChirurgo", "IdChirurgo");
            ViewBag.tipologie = new MultiSelectList(db.tipologias, "IdTipologia", "IdTipologia");
            ViewBag.infermieri = new MultiSelectList(db.infermieres, "IdInfermiere", "IdInfermiere");
            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "IdPaziente");
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "IdReferto");
            return View();
        }

        // POST: Interventi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdIntervento,IdReferto,Descrizione,IdPaziente,OraInizio,OraFine,Giorno,chirurgoes,infermieres,tipologias")] intervento intervento)
        {

            if (this.Check())
            {
                var id_chirurghi = ModelState["chirurgoes"].Value.AttemptedValue;
                var id_infermieri = ModelState["infermieres"].Value.AttemptedValue;
                var id_tipologie = ModelState["tipologias"].Value.AttemptedValue;
                chirurgo chirurgo = db.chirurgoes.First(ch => ch.IdChirurgo.ToString().Equals(id_chirurghi));
                infermiere infermiere = db.infermieres.First(ch => ch.IdInfermiere.ToString().Equals(id_infermieri));
                tipologia tipologia = db.tipologias.First(tip => tip.IdTipologia.ToString().Equals(id_tipologie));
                /*CONTROLLO SE IL CHIRURGO NON E' GIA IMPEGNATO IN UN ALTRO INTERVENTO*/
                this.CheckChirurgo(chirurgo);
                var paziente = db.pazientes.FirstOrDefault(paz => paz.IdPaziente == intervento.IdPaziente);
                if (paziente != null)
                {
                    paziente.NumeroInterventiEffettuati += 1;
                    db.Entry(paziente).State = EntityState.Modified;
                }
                intervento.chirurgoes.Add(chirurgo);
                intervento.tipologias.Add(tipologia);
                intervento.infermieres.Add(infermiere);
                db.interventoes.Add(intervento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.chirurghi = new SelectList(db.chirurgoes, "IdChirurgo", intervento.chirurgoes);
            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "Nome", intervento.IdPaziente);
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "IdReferto", intervento.IdReferto);
            return View(intervento);
        }

        private bool CheckChirurgo(chirurgo chirurgo)
        {
            return true;
        }

        private bool Check()
        {
            var a = ModelState.IsValid;
            return true;
        }
        private bool Check(int id_chirurgo, int id_infermiere, int id_tipologia, intervento intervento)
        {
            /*Controllo se i nuovi valori che si vogliono inserie non siano gia presenti*/
            var chirurgo = !intervento.chirurgoes.Any(ch => ch.IdChirurgo == id_chirurgo);
            var infermiere = !intervento.infermieres.Any(inf => inf.IdInfermiere == id_infermiere);
            var tipologia = !intervento.tipologias.Any(tip => tip.IdTipologia == id_tipologia);
            return true;
        }

        // GET: Interventi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            intervento intervento = db.interventoes.Find(id);
            if (intervento == null)
            {
                return HttpNotFound();
            }
            ViewBag.chirurghi = new MultiSelectList(db.chirurgoes, "IdChirurgo", "IdChirurgo");
            ViewBag.tipologie = new MultiSelectList(db.tipologias, "IdTipologia", "IdTipologia");
            ViewBag.infermieri = new MultiSelectList(db.infermieres, "IdInfermiere", "IdInfermiere");
            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "IdPaziente", intervento.IdPaziente);
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "IdReferto", intervento.IdReferto);
            return View(intervento);
        }

        // POST: Interventi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdIntervento,IdReferto,Descrizione,IdPaziente,OraInizio,OraFine,Giorno,chirurgoes,infermieres,tipologias")] intervento intervento)
        {
            if (this.Check())
            {
                var id_chirurghi = ModelState["chirurgoes"].Value.AttemptedValue;
                var id_infermieri = ModelState["infermieres"].Value.AttemptedValue;
                var id_tipologie = ModelState["tipologias"].Value.AttemptedValue;
                chirurgo chirurgo = db.chirurgoes.First(ch => ch.IdChirurgo.ToString().Equals(id_chirurghi));
                infermiere infermiere = db.infermieres.First(ch => ch.IdInfermiere.ToString().Equals(id_infermieri));
                tipologia tipologia = db.tipologias.First(tip => tip.IdTipologia.ToString().Equals(id_tipologie));
                intervento.chirurgoes.Add(chirurgo);
                intervento.infermieres.Add(infermiere);
                intervento.tipologias.Add(tipologia);
                db.Entry(intervento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "Nome", intervento.IdPaziente);
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "IdReferto", intervento.IdReferto);
            return View(intervento);
        }

        // GET: Interventi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            intervento intervento = db.interventoes.Find(id);
            if (intervento == null)
            {
                return HttpNotFound();
            }
            return View(intervento);
        }

        // POST: Interventi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            intervento intervento = db.interventoes.Find(id);
            db.interventoes.Remove(intervento);
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

        [HttpGet]
        public ActionResult CloseIntervento()
        {
            return RedirectToAction("Create");
        }
    }
}
