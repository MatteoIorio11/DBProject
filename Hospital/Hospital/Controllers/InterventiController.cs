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
        public ActionResult Create(intervento intervento)
        {
            if (ModelState["chirurgoes"] != null && ModelState["IdPaziente"] != null)
            {
                var id_chirurghi = ModelState["chirurgoes"].Value.AttemptedValue.Split(',');
                if (ModelState["infermieres"] != null)
                {
                    string[] id_infermieri = ModelState["infermieres"].Value.AttemptedValue.Split(',');
                    var infermieri = this.AddInfermieri(id_infermieri);
                    if (!this.CheckInfermieri(infermieri, intervento))
                    {
                        TempData["FailMessage"] = "Intervento non aggiunto ";
                        return RedirectToAction("Index");
                    }
                    infermieri.ForEach(inf => intervento.infermieres.Add(inf));
                }
                var chirurghi = this.AddChirurghi(id_chirurghi);
                if (ModelState["tipologias"] != null)
                {
                    string[] id_tipologie = ModelState["tipologias"].Value.AttemptedValue.Split(',');
                    var tipologie = this.AddTipologie(id_tipologie);
                    if(!this.CheckTipologia(tipologie, chirurghi))
                    {
                        TempData["FailMessage"] = "Intervento non aggiunto ";
                        return RedirectToAction("Index");
                    }
                    tipologie.ForEach(tipo => intervento.tipologias.Add(tipo));
                }
                var paziente = db.pazientes.FirstOrDefault(paz => paz.IdPaziente == intervento.IdPaziente);
                /*CONTROLLO SE IL CHIRURGO NON E' GIA IMPEGNATO IN UN ALTRO INTERVENTO*/
                if(!this.CheckChirurgo(chirurghi, intervento) || 
                    !this.CheckPazienti(paziente,intervento))
                {
                    TempData["FailMessage"] = "Intervento non aggiunto ";
                    return RedirectToAction("Index");
                }
                if (paziente != null)
                {
                    paziente.NumeroInterventiEffettuati += 1;
                    db.Entry(paziente).State = EntityState.Modified;
                }
                chirurghi.ForEach(ch => intervento.chirurgoes.Add(ch));

                db.interventoes.Add(intervento);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Intervento aggiunto con successo";
                return RedirectToAction("Index");
            }

            TempData["FailMessage"] = "Intervento non aggiunto ";

            return RedirectToAction("Index");
        }

        private bool CheckTipologia(List<tipologia> tipologie, List<chirurgo> chirurgo)
        {
            // Tutte le tipologie dell intervento che si devono creare devono essere contenute nei vari chirurghi
            return tipologie.All(tipo => chirurgo.All(ch => ch.tipologias.Contains(tipo)));
        }

        public List<chirurgo> AddChirurghi(string[] ids)
        {
            List<chirurgo> output = new List<chirurgo>();
            foreach(var id in ids)
            {
                output.Add(this.db.chirurgoes.First(ch => ch.IdChirurgo.ToString().Equals(id)));
            }
            return output;
        }
        public List<infermiere> AddInfermieri(string[] ids)
        {
            List<infermiere> output = new List<infermiere>();
            foreach (var id in ids)
            {
                output.Add(this.db.infermieres.First(inf => inf.IdInfermiere.ToString().Equals(id)));
            }
            return output;
        }

        public List<tipologia> AddTipologie(string[] ids)
        {
            List<tipologia> output = new List<tipologia>();
            foreach (var id in ids)
            {
                output.Add(this.db.tipologias.First(tipo => tipo.IdTipologia.ToString().Equals(id)));
            }
            return output;
        }

        private bool CheckInfermieri(List<infermiere> infermieri, intervento intervento)
        {
            foreach (infermiere i in infermieri)
            {
                var interventi_infermieri = db.interventoes.Where(inter => inter.infermieres.Any(infe => infe.IdInfermiere == i.IdInfermiere)).ToList();
                var interventiInfermiere =
                    from inf in interventi_infermieri
                    join inter in db.interventoes on inf.IdIntervento equals inter.IdIntervento
                    where inter.Giorno == intervento.Giorno &&
                    ((inter.OraInizio <= intervento.OraInizio &&
                    inter.OraFine >= intervento.OraFine) ||
                    (inter.OraInizio > intervento.OraInizio &&
                    inter.OraInizio < intervento.OraFine) ||
                     (inter.OraFine > intervento.OraInizio &&
                    inter.OraFine < intervento.OraFine))
                    select inter.IdIntervento;
                if (interventiInfermiere.Count() > 0)
                {
                    return false;
                }
                var visite_infermieri = db.visitas.Where(vis => vis.infermieres.Any(infe => infe.IdInfermiere == i.IdInfermiere)).ToList();
                var visiteInfermiere =
                    from inf in visite_infermieri
                    join vis in db.visitas on inf.IdVisita equals vis.IdVisita
                    where vis.Giorno == intervento.Giorno &&
                    ((vis.OraInizio <= intervento.OraInizio &&
                    vis.OraFine >= intervento.OraFine) ||
                    (vis.OraInizio > intervento.OraInizio &&
                    vis.OraInizio < intervento.OraFine) ||
                     (vis.OraFine > intervento.OraInizio &&
                    vis.OraFine < intervento.OraFine))
                    select vis.IdVisita;
                if (interventiInfermiere.Count() > 0)
                {
                    return false;
                }
            }
            return true;
        }
        private bool CheckPazienti(paziente p, intervento intervento)
        {
            var interventiPaziente =
            from  inter in db.interventoes
            where inter.IdPaziente == p.IdPaziente && 
            inter.Giorno == intervento.Giorno &&
            ((inter.OraInizio <= intervento.OraInizio &&
            inter.OraFine >= intervento.OraFine) ||
            (inter.OraInizio > intervento.OraInizio &&
            inter.OraInizio < intervento.OraFine) ||
                (inter.OraFine > intervento.OraInizio &&
            inter.OraFine < intervento.OraFine))
            select inter.IdIntervento;
            if (interventiPaziente.Count() > 0)
            {
                return false;
            }

            var visitePaziente =
                from vis in db.visitas
                where vis.IdPaziente == p.IdPaziente &&
                vis.Giorno == intervento.Giorno &&
                ((vis.OraInizio <= intervento.OraInizio &&
                vis.OraFine >= intervento.OraFine) ||
                (vis.OraInizio > intervento.OraInizio &&
                vis.OraInizio < intervento.OraFine) ||
                    (vis.OraFine > intervento.OraInizio &&
                vis.OraFine < intervento.OraFine))
                select vis.IdVisita;
            if (visitePaziente.Count() > 0)
            {
                return false;
            }

            return true;
        }

        private bool CheckChirurgo(List<chirurgo> chirurghi, intervento intervento)
        {
            foreach(chirurgo c in chirurghi)
            {
                var interventi_chirurgo = db.interventoes.Where(inter => inter.chirurgoes.Any(ch => ch.IdChirurgo == c.IdChirurgo)).ToList();
                var interventiChirurgo =
                    from chi in interventi_chirurgo
                    join inter in db.interventoes on chi.IdIntervento equals inter.IdIntervento
                    where inter.Giorno == intervento.Giorno &&
                    ((inter.OraInizio <= intervento.OraInizio &&
                    inter.OraFine >= intervento.OraFine) ||
                    (inter.OraInizio > intervento.OraInizio &&
                    inter.OraInizio < intervento.OraFine) ||
                     (inter.OraFine > intervento.OraInizio &&
                    inter.OraFine < intervento.OraFine))
                    select inter.IdIntervento;
                if (interventiChirurgo.Count() > 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool Check(string[] ids)
        {
            return ids.Length > 0;
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
        public ActionResult Edit([Bind(Include = "IdIntervento,IdReferto,IdPaziente")] intervento intervento)
        {
            var inter = db.interventoes.Where(inte => inte.IdIntervento == intervento.IdIntervento).FirstOrDefault();
            if (ModelState["IdReferto"] != null && inter != null)
            {
                var id = ModelState["IdReferto"].Value.AttemptedValue;
                referto referto = db.refertoes.First(refer => refer.IdReferto.ToString().Equals(id));
                if (this.CheckReferto(referto))
                {
                    TempData["FailMessage"] = "Intervento non modificato ";
                    return RedirectToAction("Index");
                }
                inter = db.interventoes.First(inte => inte.IdIntervento == intervento.IdIntervento);
                inter.referto = referto;
                db.Entry(inter).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Intervento modificato con successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Intervento non modificato ";
            return RedirectToAction("Index");
        }
        private bool CheckReferto(referto referto)
        {
            return db.visitas.Any(vis => vis.IdReferto == referto.IdReferto) ||
                db.interventoes.Any(inter => inter.IdReferto == referto.IdReferto);
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
