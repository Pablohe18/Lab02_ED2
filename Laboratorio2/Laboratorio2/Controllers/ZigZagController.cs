using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio2.Models;
using Laboratorio2.DBContext;
using System.Net;
using System.IO;
using Laboratorio2.Cifrado;

namespace Laboratorio2.Controllers
{
    public class ZigZagController : Controller
    {
        DefaultConnection db = DefaultConnection.getInstance;
        ZigZag cifradoZigzag = new ZigZag();


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
                if (keyzigzag.Clave > 0)
                {
                    db.keyzigzag = keyzigzag.Clave;
                    return RedirectToAction("UploadFile");
                }
                ViewBag.Message = "Debe ingresar un numero mayor a 0.";
            }

            return View(keyzigzag);
        }


        //SUBIR DE ARCHIVO 
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }
        //SUBIR DE ARCHIVO 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase File)
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

                cifradoZigzag = new ZigZag(db.keyzigzag);
                cifradoZigzag.Cifrar(db.ObtenerRuta());


                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
