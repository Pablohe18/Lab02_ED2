using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Laboratorio2.DBContext;
using Laboratorio2.Cifrado;
using Laboratorio2.Models;
using System.IO;
using System.Text;

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
        //    cifradoRSA.generate_key(29, 23, true);
        //    int cifrado = cifradoRSA.cipher(Convert.ToInt32('N'));
        //    cifradoRSA.generate_key(29, 23, false);
        //    int descifrado = cifradoRSA.decipher(cifrado);
            return View(db.ObtenerLista());
        }

        ////////
        //METODOS PARA LA GENERACION Y OBTENCION DE LLAVES
        ////////

        //GET: SDES/GetKey
        [HttpGet]
        public ActionResult GenerateKeys()
        {
            return View();
        }
        //POST: SDES/GetKey
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateKeys([Bind(Include = "ValorP,ValorQ")]KeyRSA keyRSA)
        {
            if (ModelState.IsValid)
            {
                if (keyRSA.ValorP > 0 && keyRSA.ValorQ > 0)
                {
                    if ((keyRSA.ValorP * keyRSA.ValorQ) > 255)
                    {
                        cifradoRSA.generate_key(keyRSA.ValorP, keyRSA.ValorQ, true);
                        db.publicKeyRSA[0] = Convert.ToInt32(cifradoRSA.keys[0].ToString());
                        db.publicKeyRSA[1] = Convert.ToInt32(cifradoRSA.keys[1].ToString());
                        cifradoRSA.generate_key(keyRSA.ValorP, keyRSA.ValorQ, false);
                        db.privateKeyRSA[0] = Convert.ToInt32(cifradoRSA.keys[0].ToString());
                        db.privateKeyRSA[1] = Convert.ToInt32(cifradoRSA.keys[1].ToString());
                        ////////
                        var ruta = Server.MapPath("~/DownloadedFiles/") + "private.key";
                        using (StreamWriter outputFile = new StreamWriter(ruta))
                        {

                            outputFile.Write(db.privateKeyRSA[0].ToString()+","+db.privateKeyRSA[1].ToString());

                        }

                        ////////
                        ruta = Server.MapPath("~/DownloadedFiles/") + "public.key";
                        using (StreamWriter outputFile = new StreamWriter(ruta))
                        {

                            outputFile.Write(db.publicKeyRSA[0].ToString() + "," + db.publicKeyRSA[1].ToString());

                        }

                        return RedirectToAction("DownloadKeys");
                    }
                    ViewBag.Message1 = "Debe ingresar dos numeros que al multiplicarse den como resultado un numero mayor a 255";
                }
                ViewBag.Message = "Debe ingresar numeros positivos";
            }
            return View(keyRSA);
        }

        //METODO PARA DESCARGAR LAS LLAVES
        public ActionResult DownloadKeys()
        {
            return View();
        }

        //DESCARGAR LA LLAVE PUBLICA
        public FileResult DownloadPublicKey()
        {
            var ruta = Server.MapPath("~/DownloadedFiles/") + "public.key";
            return File(ruta, "text/plain", "public.key");
        }
        //DESCARGAR LA LLAVE PRIVADA
        public FileResult DownloadPrivateKey()
        {
            var ruta = Server.MapPath("~/DownloadedFiles/") + "private.key";
            return File(ruta, "text/plain", "private.key");
        }

        ////////
        //METODOS PARA CIFRAR RSA
        ////////

        //GET: SDES/EncryptFile
        [HttpGet]
        public ActionResult ReadPublicKey()
        {
            return View();
        }

        //SUBIR DE ARCHIVO PARA CIFRAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadPublicKey(HttpPostedFileBase File)
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

                string builder = default(string);

                StreamReader reader = new StreamReader(fileInfo.FullName);
                while ((builder = reader.ReadLine()) != null)
                {
                    db.publicKeyRSA[0] = Convert.ToInt64(builder.Split(',')[0]);
                    db.publicKeyRSA[1] = Convert.ToInt64(builder.Split(',')[1]);
                }
                reader.Close();

                return RedirectToAction("EncryptFile");
            }

            return View();
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
                db.AsignarRuta(fileInfo);
                StringBuilder builder = new StringBuilder();

                cifradoRSA.keys[0] = db.publicKeyRSA[0];
                cifradoRSA.keys[1] = db.publicKeyRSA[1];

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
                                builder.Append(((char)cifradoRSA.cipher((int)item)).ToString());
                            }
                        }

                    }

                }

                ////////
                var ruta = Server.MapPath("~/DownloadedFiles/") + db.ObtenerRuta().Name.Split('.')[0] + ".rsacif";
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

        ////////
        //METODOS PARA CIFRAR RSA
        ////////

        //GET: SDES/EncryptFile
        [HttpGet]
        public ActionResult ReadPrivateKey()
        {
            return View();
        }

        //SUBIR DE ARCHIVO PARA CIFRAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadPrivateKey(HttpPostedFileBase File)
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

                string builder = default(string);

                StreamReader reader = new StreamReader(fileInfo.FullName);
                while ((builder = reader.ReadLine()) != null)
                {
                    db.privateKeyRSA[0] = Convert.ToInt64(builder.Split(',')[0]);
                    db.privateKeyRSA[1] = Convert.ToInt64(builder.Split(',')[1]);
                }
                reader.Close();

                return RedirectToAction("DecryptFile");
            }

            return View();
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

                db.AsignarRuta(fileInfo);

                StringBuilder builder = new StringBuilder();

                cifradoRSA.keys[0] = db.privateKeyRSA[0];
                cifradoRSA.keys[1] = db.privateKeyRSA[1];

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
                                builder.Append(((char)cifradoRSA.decipher((int)item)).ToString());
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

