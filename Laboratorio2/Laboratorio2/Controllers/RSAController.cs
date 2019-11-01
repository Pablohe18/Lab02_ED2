using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laboratorio2.Controllers
{
    public class RSAController : Controller
    {
        // GET: RSA
        public ActionResult Index()
        {
            return View();
        }

        // GET: RSA/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RSA/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RSA/Create
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

        // GET: RSA/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RSA/Edit/5
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

        // GET: RSA/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RSA/Delete/5
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
