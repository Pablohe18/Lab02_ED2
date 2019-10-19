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
using System.Text;

namespace Laboratorio2.Controllers
{
    public class SDESController : Controller
    {
        DefaultConnection db = DefaultConnection.getInstance;
        SDes cifradoSDes = new SDes();
        int bufferLength = 100;
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

                StringBuilder builder = new StringBuilder();

                var buffer = new char[bufferLength];
                using (var file = new FileStream(db.ObtenerRuta().FullName, FileMode.Open))
                {
                    using (var reader = new BinaryReader(file))
                    {
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            buffer = reader.ReadChars(bufferLength);
                            foreach (var item in buffer)
                            {
                                builder.Append(cifradoSDes.Cifrado((char)item).ToString());
                            }
                        }

                    }

                }

                ////////
                var ruta = Server.MapPath("~/DownloadedFiles/") + db.ObtenerRuta().Name.Split('.')[0] + ".scif";
                using (StreamWriter outputFile = new StreamWriter(ruta))
                {
                    foreach (char caracter in builder.ToString())
                    {
                        outputFile.Write(caracter.ToString());
                    }
                }

                db.AsignarRuta(new FileInfo(ruta));

                return RedirectToAction("DownloadFile");
            }

            return View();
        }

        /// <summary>
        /// ////////////////////////////
        /// </summary>
        /// <returns></returns>

        //GET: SDES/GetKey
        [HttpGet]
        public ActionResult GetKeyDecrypt()
        {
            return View();
        }
        //POST: SDES/GetKey
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetKeyDecrypt([Bind(Include = "Key")]Keysdes keysdes)
        {
            if (ModelState.IsValid)
            {
                if (keysdes.Key > 0 && keysdes.Key < 1024)
                {
                    db.keysdes = keysdes.Key;
                    return RedirectToAction("DecryptFile");
                }
                ViewBag.Message = "Debe ingresar un numero mayor a 0 y menor a 1024";
            }
            return View(keysdes);
        }

        //GET: SDES/DecryptFile
        [HttpGet]
        public ActionResult DecryptFile()
        {
            return View();
        }

        //SUBIR DE ARCHIVO PARA DESCIFRAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DecryptFile(HttpPostedFileBase File)
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

                StringBuilder builder = new StringBuilder();

                var buffer = new char[bufferLength];
                using (var file = new FileStream(db.ObtenerRuta().FullName, FileMode.Open))
                {
                    using (var reader = new BinaryReader(file))
                    {
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            buffer = reader.ReadChars(bufferLength);
                            foreach (var item in buffer)
                            {
                                builder.Append(cifradoSDes.Descrifrado((char)item).ToString());
                            }
                        }

                    }

                }

                ////////
                var ruta = Server.MapPath("~/DownloadedFiles/") + db.ObtenerRuta().Name.Split('.')[0] + ".txt";
                using (StreamWriter outputFile = new StreamWriter(ruta))
                {
                    foreach (char caracter in builder.ToString())
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
