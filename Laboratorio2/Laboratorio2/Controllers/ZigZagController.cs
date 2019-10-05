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


        //SUBIR DE ARCHIVO PARA CIFRAR
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }
        //SUBIR DE ARCHIVO PARA CIFRAR
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
                var texto = cifradoZigzag.Cifrar(db.ObtenerRuta());

                ////////
                var ruta = Server.MapPath("~/DownloadedFiles/") + db.ObtenerRuta().Name.Split('.')[0] + ".cif";
                using (StreamWriter outputFile = new StreamWriter(ruta))
                {
                    foreach (char caracter in texto)
                    {
                        outputFile.Write(caracter.ToString());
                    }
                }

                db.AsignarRuta(new FileInfo(ruta));

                return RedirectToAction("DownloadFile");
            }

            return View();
        }



        // GET: ZigZag/Create
        public ActionResult GetKeyDecipher()
        {
            return View();
        }

        // POST: ZigZag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetKeyDecipher([Bind(Include = "Clave")]Keyzigzag keyzigzag)
        {
            if (ModelState.IsValid)
            {
                if (keyzigzag.Clave > 0)
                {
                    db.keyzigzag = keyzigzag.Clave;
                    return RedirectToAction("UploadFileDecipher");
                }
                ViewBag.Message = "Debe ingresar un numero mayor a 0.";
            }

            return View(keyzigzag);
        }

        //SUBIR DE ARCHIVO PARA DESCIFRAR
        [HttpGet]
        public ActionResult UploadFileDecipher()
        {
            return View();
        }
        //SUBIR DE ARCHIVO PARA DECIFRAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFileDecipher(HttpPostedFileBase File)
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
                var texto = cifradoZigzag.Cifrar(db.ObtenerRuta());

                ////////
                var ruta = Server.MapPath("~/DownloadedFiles/") + db.ObtenerRuta().Name.Split('.')[0] + ".txt";
                using (StreamWriter outputFile = new StreamWriter(ruta))
                {
                    foreach (char caracter in texto)
                    {
                        outputFile.Write(caracter.ToString());
                    }
                }

                db.AsignarRuta(new FileInfo(ruta));

                return RedirectToAction("DownloadFile");
            }

            return View();
        }


        //METODO PARA DESCARGAR EL ARCHIVO
        public ActionResult DownloadFile()
        {
            return View();
        }

        //DESCARGAR EL ARCHIVO
        public FileResult Download()
        {

            var ruta = Server.MapPath("~/DownloadedFiles/") + db.ObtenerRuta().Name;
            return File(ruta, "text/plain", db.ObtenerRuta().Name);
        }
    }
}
