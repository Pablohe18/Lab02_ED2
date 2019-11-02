using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio2.DBContext;
using Laboratorio2.Cifrado;
using Laboratorio2.Models;

namespace Laboratorio2.Controllers
{
    public class AlgoritmoRSAController : Controller
    {
        DefaultConnection db = DefaultConnection.getInstance;
        AlgoritmoRSA cifradoRSA = new AlgoritmoRSA();
        int bufferLength = 100;


        // GET: AlgoritmoRSA
        public ActionResult Index()
        {
            //cifradoRSA.generate_key(17, 23, true);
            //int cifrado = cifradoRSA.cipher(Convert.ToInt32('N'));
            //cifradoRSA.generate_key(17, 23, false);
            //int descifrado = cifradoRSA.decipher(cifrado);
            return View(db.ObtenerLista());
        }

        // GET: AlgoritmoRSA/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AlgoritmoRSA/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AlgoritmoRSA/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AlgoritmoRSA/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AlgoritmoRSA/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AlgoritmoRSA/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AlgoritmoRSA/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
