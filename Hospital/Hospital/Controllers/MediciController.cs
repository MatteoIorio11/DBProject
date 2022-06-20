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
    public class MediciController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        // GET: Medici
        public ActionResult Index()
        {
            return View(db.medicos.ToList());
        }

        // GET: Medici/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medico medico = db.medicos.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }

        // GET: Medici/Create
        public ActionResult Create()
        {
            ViewBag.tipologie = new MultiSelectList(db.tipologias, "IdTipologia", "IdTipologia");
            return View();
        }

        // POST: Medici/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdMedico,Nome,Cognome,CodiceFiscale,DataNascita,Genere,NumeroVisiteEffettuate,NumeroDiTelefono,tipologias")] medico medico)
        {
            if (!this.Check(medico))
            {
                if (ModelState["tipologias"] != null)
                {
                    var id_tipologie = ModelState["tipologias"].Value.AttemptedValue.Split(',');
                    //AGGIUNGERE LA TIPOLOGIA
                    var tipologie = this.AddTipologie(id_tipologie);
                    tipologie.ForEach(tipo => medico.tipologias.Add(tipo));
                }
                medico.NumeroVisiteEffettuate = 0;
                db.medicos.Add(medico);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Medico aggiunto con successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Medico non aggiunto successo";
            return RedirectToAction("Index");
        }

        private List<tipologia> AddTipologie(string[] ids)
        {
            List<tipologia> output = new List<tipologia>();
            foreach(var id in ids)
            {
                output.Add(db.tipologias.Where(tipo => tipo.IdTipologia.ToString().Equals(id)).First());
            }
            return output;
        }

        private bool Check(medico medico)
        {
            return db.chirurgoes.Any(ch => ch.CodiceFiscale == medico.CodiceFiscale) ||
                   db.pazientes.Any(pa => pa.CodiceFiscale == medico.CodiceFiscale) ||
                   db.infermieres.Any(inf => inf.CodiceFiscale == medico.CodiceFiscale) ||
                   db.medicos.Any(med => med.CodiceFiscale == medico.CodiceFiscale);
        }

        // GET: Medici/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medico medico = db.medicos.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }

        // POST: Medici/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMedico,Nome,Cognome,CodiceFiscale,DataNascita,Genere,NumeroVisiteEffettuate,NumeroDiTelefono")] medico medico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medico);
        }

        // GET: Medici/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medico medico = db.medicos.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }

        // POST: Medici/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            medico medico = db.medicos.Find(id);
            if(!this.CheckVisite(medico) && !this.CheckTipologie(medico))
            {
                db.medicos.Remove(medico);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Medico eliminato successo";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Medico non eliminato";

            return RedirectToAction("Index");
        }

        private bool CheckTipologie(medico medico)
        {
            return db.tipologias.Any(tipo => tipo.medicos.Any(medi => medi.IdMedico == medico.IdMedico));
        }

        private bool CheckVisite(medico medico)
        {
            return db.visitas.Any(vist => vist.medicos.Any(medi => medi.IdMedico == medico.IdMedico));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet, ActionName("ListVisite")]
        public ActionResult ListVisite()
        {
            return View(db.medicos.Select(med => med).ToList());
        }
    }
}
