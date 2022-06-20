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
    public class VisiteController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        // GET: Visite
        public ActionResult Index()
        {
            var visitas = db.visitas.Include(v => v.paziente).Include(v => v.referto);
            return View(visitas.ToList());
        }

        // GET: Visite/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            visita visita = db.visitas.Find(id);
            if (visita == null)
            {
                return HttpNotFound();
            }
            return View(visita);
        }

        // GET: Visite/Create
        public ActionResult Create()
        {
            ViewBag.medici = new MultiSelectList(db.medicos, "IdMedico", "IdMedico");
            ViewBag.tipologie = new MultiSelectList(db.tipologias, "IdTipologia", "IdTipologia");
            ViewBag.infermieri = new MultiSelectList(db.infermieres, "IdInfermiere", "IdInfermiere");
            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "IdPaziente");
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "IdReferto");
            return View();
        }

        // POST: Visite/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( visita visita)
        {

            if (ModelState["medicos"] != null && ModelState["infermieres"] != null && ModelState["IdPaziente"] != null)
            {
                string[] id_medici = ModelState["medicos"].Value.AttemptedValue.Split(',');
                string[] id_infermieri = ModelState["infermieres"].Value.AttemptedValue.Split(',');
                string[] id_tipologie = ModelState["tipologias"].Value.AttemptedValue.Split(',');
                string id_paziente = ModelState["IdPaziente"].Value.AttemptedValue;
                var medici = this.AddMedici(id_medici);
                var infermieri = this.AddInfermieri(id_infermieri);
                var tipologie = this.AddTipologie(id_tipologie);
                var paziente = db.pazientes.First(paz => paz.IdPaziente.ToString().Equals(id_paziente));
                if (!this.CheckMedico(medici, visita) ||
                        !this.CheckInfermieri(infermieri, visita) ||
                        !this.CheckPazienti(paziente, visita))
                {
                    return RedirectToAction("Index");
                }
                medici.ForEach(med => visita.medicos.Add(med));
                infermieri.ForEach(infer => visita.infermieres.Add(infer));
                tipologie.ForEach(tipo => visita.tipologias.Add(tipo));
                visita.paziente = paziente;
                db.visitas.Add(visita);
                this.UpdateVisiteMedici(medici);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Visite aggiunto con successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Visite non aggiunta";
            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "IdPaziente", visita.IdPaziente);
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "IdReferto", visita.IdReferto);
            return View(visita);
        }

        public void UpdateVisiteMedici(List<medico> medici)
        {
            medici.ForEach(med => med.NumeroVisiteEffettuate += 1);
            medici.ForEach(med => db.Entry(med).State = EntityState.Modified);
        }

        public bool Check()
        {
            return true;
        }


        private bool CheckInfermieri(List<infermiere> infermieri, visita visita)
        {
            foreach (infermiere i in infermieri)
            {
                var interventi_infermieri = db.interventoes.Where(inter => inter.infermieres.Any(infe => infe.IdInfermiere == i.IdInfermiere)).ToList();
                var interventiInfermiere =
                    from inf in interventi_infermieri
                    join inter in db.interventoes on inf.IdIntervento equals inter.IdIntervento
                    where inter.Giorno == visita.Giorno &&
                    ((inter.OraInizio < visita.OraInizio &&
                    inter.OraFine > visita.OraFine) ||
                    (inter.OraInizio > visita.OraInizio &&
                    inter.OraInizio < visita.OraFine) ||
                     (inter.OraFine > visita.OraInizio &&
                    inter.OraFine < visita.OraFine))
                    select inter.IdIntervento;
                if (interventiInfermiere.Count() > 0)
                {
                    return false;
                }
                var visite_infermieri = db.visitas.Where(vis => vis.infermieres.Any(infe => infe.IdInfermiere == i.IdInfermiere)).ToList();
                var visiteInfermiere =
                    from inf in visite_infermieri
                    join vis in db.visitas on inf.IdVisita equals vis.IdVisita
                    where vis.Giorno == visita.Giorno &&
                    ((vis.OraInizio < visita.OraInizio &&
                    vis.OraFine > visita.OraFine) ||
                    (vis.OraInizio > visita.OraInizio &&
                    vis.OraInizio < visita.OraFine) ||
                     (vis.OraFine > visita.OraInizio &&
                    vis.OraFine < visita.OraFine))
                    select vis.IdVisita;
                if (interventiInfermiere.Count() > 0)
                {
                    return false;
                }
            }
            return true;
        }
        private bool CheckPazienti(paziente p, visita visita)
        {
            var visitePaziente =
            from vis in db.visitas
            where vis.IdPaziente == p.IdPaziente &&
            vis.Giorno == visita.Giorno &&
            ((vis.OraInizio < visita.OraInizio &&
            vis.OraFine > visita.OraFine) ||
            (vis.OraInizio > visita.OraInizio &&
            vis.OraInizio < visita.OraFine) ||
                (vis.OraFine > visita.OraInizio &&
            vis.OraFine < visita.OraFine))
            select vis.IdVisita;
            if (visitePaziente.Count() > 0)
            {
                return false;
            }

            var interventiPaziente =
            from inter in db.interventoes
            where inter.IdPaziente == p.IdPaziente &&
            inter.Giorno == visita.Giorno &&
            ((inter.OraInizio < visita.OraInizio &&
            inter.OraFine > visita.OraFine) ||
            (inter.OraInizio > visita.OraInizio &&
            inter.OraInizio < visita.OraFine) ||
                (inter.OraFine > visita.OraInizio &&
            inter.OraFine < visita.OraFine))
            select inter.IdIntervento;
            if (interventiPaziente.Count() > 0)
            {
                return false;
            }

            return true;
        }

        private bool CheckMedico(List<medico> medici, visita visita)
        {
            foreach (medico m in medici)
            {
                var visite_medici = db.visitas.Where(vis => vis.medicos.Any(vs => vs.IdMedico == vs.IdMedico)).ToList();
                var visiteMedici =
                    from med in visite_medici
                    join vis in db.visitas on med.IdVisita equals vis.IdVisita
                    where vis.Giorno == visita.Giorno &&
                    ((vis.OraInizio < visita.OraInizio &&
                    vis.OraFine > visita.OraFine) ||
                    (vis.OraInizio > visita.OraInizio &&
                    vis.OraInizio < visita.OraFine) ||
                     (vis.OraFine > visita.OraInizio &&
                    vis.OraFine < visita.OraFine))
                    select vis.IdVisita;
                if (visiteMedici.Count() > 0)
                {
                    return false;
                }
            }
            return true;
        }
        public List<medico> AddMedici(string[] ids)
        {
            List<medico> output = new List<medico>();
            foreach (var id in ids)
            {
                output.Add(this.db.medicos.First(med => med.IdMedico.ToString().Equals(id)));
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

        // GET: Visite/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            visita visita = db.visitas.Find(id);
            if (visita == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "IdPaziente", visita.IdPaziente);
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "IdReferto", visita.IdReferto);
            return View(visita);
        }

        // POST: Visite/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdVisita,IdReferto")] visita visita)
        {
            visita vis = db.visitas.Where(vs => vs.IdVisita == visita.IdVisita).FirstOrDefault();
            if (!this.CheckVisita(visita) && ModelState["IdReferto"] != null && vis != null )
            {
                var id_referto = ModelState["IdReferto"].Value.AttemptedValue;
                referto referto = db.refertoes.First(refe => refe.IdReferto.ToString().Equals(id_referto));
                if (this.CheckReferto(referto))
                {
                    TempData["FailMessage"] = "Visita non modificato";
                    return RedirectToAction("Index");
                }
                vis.referto = referto;
                db.Entry(vis).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Visita modificato  con successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Visita non modificato";

            ViewBag.IdPaziente = new SelectList(db.pazientes, "IdPaziente", "IdPaziente", visita.IdPaziente);
            ViewBag.IdReferto = new SelectList(db.refertoes, "IdReferto", "IdReferto", visita.IdReferto);
            return RedirectToAction("Index");
        }

        private bool CheckReferto(referto referto)
        {
            return db.visitas.Any(vis => vis.IdReferto == referto.IdReferto) ||
                db.interventoes.Any(inter => inter.IdReferto == referto.IdReferto);
        }

        private bool CheckVisita(visita visita)
        {
            return visita.IdReferto == null;
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
