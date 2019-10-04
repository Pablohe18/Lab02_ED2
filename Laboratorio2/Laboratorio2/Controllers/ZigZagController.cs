using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio2.Models;
using Laboratorio2.DBContext;
using System.Net;

namespace Laboratorio2.Controllers
{
    public class ZigZagController : Controller
    {
        DefaultConnection db = DefaultConnection.getInstance;

        // GET: ZigZag
        public ActionResult Index()
        {
            return View(db.ObtenerLista());
        }

        // GET: ZigZag/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ZigZag/Create
        public ActionResult GetKey()
        {
            return View();
        }

        // POST: ZigZag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetKey([Bind(Include ="Clave")]Keyzigzag keyzigzag)
        {
            if (ModelState.IsValid)
            {
                db.keyzigzag = keyzigzag.Clave;
                return RedirectToAction("Index");
            }

            return View(keyzigzag);
        }


        // GET: ZigZag/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ZigZag/Edit/5
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

        // GET: ZigZag/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ZigZag/Delete/5
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
