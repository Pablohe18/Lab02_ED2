using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio2.DBContext;
using Laboratorio2.Models;
using Laboratorio2.Cifrado;
using System.Net;
using System.IO;

namespace Laboratorio2.Controllers
{
    public class SDESController : Controller
    {
        DefaultConnection db = DefaultConnection.getInstance;
        SDes cifradoSDes = new SDes();
        // GET: SDES
        public ActionResult Index()
        {
            return View(db.ObtenerLista());
        }

        //GET: SDES/GetKey
        [HttpGet]
        public ActionResult GetKey()
        {
            return View();
        }
        //POST: SDES/GetKey
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetKey([Bind(Include ="Key")]Keysdes keysdes)
        {
            if (ModelState.IsValid)
            {
                if (keysdes.Key > 0 && keysdes.Key < 1024) 
                {
                    db.keysdes = keysdes.Key;
                    return RedirectToAction("EncryptFile");
                }
                ViewBag.Message = "Debe ingresar un numero mayor a 0 y menor a 1024";
            }
            return View(keysdes);
        }

        //GET: SDES/EncryptFile
        [HttpGet]
        public ActionResult EncryptFile()
        {
            return View();
        }

        //SUBIR DE ARCHIVO PARA CIFRAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EncryptFile(HttpPostedFileBase File)
        {
            string filePath = string.Empty;
            if (File != null)
            {
                string path = Server.MapPath("~/UploadedFiles/");
                filePath = path + Path.GetFileName(File.FileName);
                string extension = Path.GetExtension(File.FileName);
                File.SaveAs(filePath);
                ViewBag.Message = "Archivo Cargado";

                FileInfo fileInfo = new FileInfo(filePath);

                string nombre_original = fileInfo.Name;
                long tamanio_original = fileInfo.Length;

                db.AsignarRuta(fileInfo);

                cifradoSDes = new SDes(1);
                cifradoSDes.GenerarKeys(db.keysdes);

                ////////
                //var ruta = Server.MapPath("~/DownloadedFiles/") + db.ObtenerRuta().Name.Split('.')[0] + ".cif";
                //using (StreamWriter outputFile = new StreamWriter(ruta))
                //{
                //    foreach (char caracter in texto)
                //    {
                //        outputFile.Write(caracter.ToString());
                //    }
                //}

                //db.AsignarRuta(new FileInfo(ruta));

                //return RedirectToAction("DownloadFile");
            }

            return View();
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
