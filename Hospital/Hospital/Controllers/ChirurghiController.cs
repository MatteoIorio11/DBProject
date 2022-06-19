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
    public class ChirurghiController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        // GET: Chirurghi
        public ActionResult Index()
        {
            return View(db.chirurgoes.ToList());
        }

        // GET: Chirurghi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chirurgo chirurgo = db.chirurgoes.Find(id);
            if (chirurgo == null)
            {
                return HttpNotFound();
            }
            return View(chirurgo);
        }

        // GET: Chirurghi/Create
        public ActionResult Create()
        {
            ViewBag.tipologie = new SelectList(db.tipologias, "IdTipologia", "IdTipologia");
            return View();
        }

        // POST: Chirurghi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdChirurgo,Nome,Cognome,CodiceFiscale,DataNascita,Genere,NumeroDiTelefono,tipologias")] chirurgo chirurgo)
        {
            string[] id_tipologie = ModelState["tipologias"].Value.AttemptedValue.Split(',');
            if (this.Check())
            {
                var tipologie= this.AddTipologie(id_tipologie);
                tipologie.ForEach(tipo => chirurgo.tipologias.Add(tipo));
                db.chirurgoes.Add(chirurgo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chirurgo);
        }

        private List<tipologia> AddTipologie(string[] ids)
        {
            List<tipologia> output = new List<tipologia>();
            foreach(var id in ids)
            {
                output.Add(db.tipologias.Where(tipol => tipol.IdTipologia.ToString().Equals(id)).First()); 
            }
            return output;
        }

        public bool Check()
        {
            return true;
        }

        // GET: Chirurghi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chirurgo chirurgo = db.chirurgoes.Find(id);
            if (chirurgo == null)
            {
                return HttpNotFound();
            }
            return View(chirurgo);
        }

        // POST: Chirurghi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdChirurgo,Nome,Cognome,CodiceFiscale,DataNascita,Genere,NumeroDiTelefono")] chirurgo chirurgo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chirurgo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chirurgo);
        }

        // GET: Chirurghi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            chirurgo chirurgo = db.chirurgoes.Find(id);
            if (chirurgo == null)
            {
                return HttpNotFound();
            }
            return View(chirurgo);
        }

        // POST: Chirurghi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            chirurgo chirurgo = db.chirurgoes.Find(id);
            db.chirurgoes.Remove(chirurgo);
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
