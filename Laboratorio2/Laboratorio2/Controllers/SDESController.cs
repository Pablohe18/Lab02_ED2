using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio2.DBContext;

namespace Laboratorio2.Controllers
{
    public class SDESController : Controller
    {
        DefaultConnection db = DefaultConnection.getInstance;
        // GET: SDES
        public ActionResult Index()
        {
            return View(db.ObtenerLista());
        }

        // GET: SDES/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SDES/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SDES/Create
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

        // GET: SDES/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SDES/Edit/5
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

        // GET: SDES/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SDES/Delete/5
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
