using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio2.DBContext;
using Laboratorio2.Models;

namespace Laboratorio2.Controllers
{
    public class SpiralController : Controller
    {
        DefaultConnection db = DefaultConnection.getInstance;
        // GET: Spiral
        public ActionResult Index()
        {
            return View();
        }

        //SUBIR DE ARCHIVO 
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }
        string filePath = string.Empty;
        string namefile = string.Empty;
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
                namefile = Path.GetFileName(File.FileName);
                string extension = Path.GetExtension(File.FileName);
                File.SaveAs(filePath);
                ViewBag.Message = "Archivo Cargado";

                FileInfo fileInfo = new FileInfo(filePath);

                string nombre_original = fileInfo.Name;
                long tamanio_original = fileInfo.Length;

                db.AsignarRuta(fileInfo);
                //db.GuardarArchivo(fileInfo);
                //DownloadFile(fileInfo);


                return RedirectToAction("Index");
            }

            return View();
        }

        public void Cifrado(Espiral espiral)
        {
            var bufferLength = 750;
            var path = Path.Combine(Server.MapPath("~/Archivo"), filePath);
            var byteBuffer = new byte[320000000];
            List<string> TEXTO = new List<string>();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    byteBuffer = reader.ReadBytes(bufferLength);

                    foreach (var item in byteBuffer)
                    {
                        TEXTO.Add(Convert.ToString(item));
                    }
                }
            }
            if ((espiral.TamañoF * espiral.TamañoC) < TEXTO.Count)
            {
                decimal division = TEXTO.Count / espiral.TamañoF;
                espiral.TamañoF = (int)Math.Ceiling(division);
            }

            string[,] MATRIX = new string[espiral.TamañoF, espiral.TamañoC];
            int textoescrito = 0;
            for (int i = 0; i < espiral.TamañoC; i++)
            {
                for (int j = 0; j < espiral.TamañoF; j++)
                {
                    MATRIX[j, i] = textoescrito < TEXTO.Count ? TEXTO[textoescrito] : "11";
                    textoescrito++;
                }
            }

            int[] FIN = { espiral.TamañoF - 1, espiral.TamañoC - 1 };
            int x = 0; int y = 0;
            List<string> TXTCIPHER = new List<string>();

            switch (espiral.Recorrido)
            {
                case "horizontal":

                    while ((espiral.TamañoF * espiral.TamañoC) > TXTCIPHER.Count)
                    {
                        for (int i = x; i <= FIN[0]; i++)
                        {
                            TXTCIPHER.Add(MATRIX[i, y]);
                        }
                        y++;
                        if ((espiral.TamañoF * espiral.TamañoC) == TXTCIPHER.Count) { break; }
                        for (int i = y; i <= FIN[1]; i++)
                        {
                            TXTCIPHER.Add(MATRIX[FIN[0], i]);
                        }
                        if ((espiral.TamañoF * espiral.TamañoC) == TXTCIPHER.Count) { break; }
                        for (int i = FIN[0] - 1; i >= x; i--)
                        {
                            TXTCIPHER.Add(MATRIX[i, FIN[1]]);
                        }
                        if ((espiral.TamañoF * espiral.TamañoC) == TXTCIPHER.Count) { break; }
                        for (int i = FIN[1] - 1; i >= y; i--)
                        {
                            TXTCIPHER.Add(MATRIX[x, i]);
                        }
                        x++;
                        FIN[0]--; FIN[1]--;
                    }
                    break;

                case "vertical":

                    while ((espiral.TamañoF * espiral.TamañoC) > TXTCIPHER.Count)
                    {
                        for (int i = y; i <= FIN[1]; i++)
                        {
                            TXTCIPHER.Add(MATRIX[x, i]);
                        }
                        x++;
                        if ((espiral.TamañoF * espiral.TamañoC) == TXTCIPHER.Count) { break; }
                        for (int i = x; i <= FIN[0]; i++)
                        {
                            TXTCIPHER.Add(MATRIX[x, FIN[1]]);
                        }
                        FIN[1]--;
                        if ((espiral.TamañoF * espiral.TamañoC) == TXTCIPHER.Count) { break; }
                        for (int i = FIN[1]; i >= y; i--)
                        {
                            TXTCIPHER.Add(MATRIX[FIN[0], i]);
                        }
                        FIN[0]--;
                        if ((espiral.TamañoF * espiral.TamañoC) == TXTCIPHER.Count) { break; }
                        for (int i = FIN[0]; i >= x; i--)
                        {
                            TXTCIPHER.Add(MATRIX[i, y]);
                        }
                        y++;
                    }
                    break;
             
            }
            using (var writeStream1 = new FileStream(Server.MapPath("~/Archivo") + "/" + namefile + ".cif", FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream1))
                {
                    foreach (var item in TXTCIPHER)
                    {
                        writer.Write(Convert.ToByte(item));
                    }
                }
            }


        }





        public void Decifrado(Espiral espiral)
        {
            var bufferLength = 750;
            var path = Path.Combine(Server.MapPath("~/Archivo"), namefile);

            var byteBuffer = new byte[320000000];
            List<string> text_archivocifrado = new List<string>();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    byteBuffer = reader.ReadBytes(bufferLength);

                    foreach (var item in byteBuffer)
                    {
                        text_archivocifrado.Add(Convert.ToString(item));
                    }
                }
            }
            if ((espiral.TamañoF * espiral.TamañoC) < text_archivocifrado.Count)
            {
                decimal division = text_archivocifrado.Count / espiral.TamañoF;
                espiral.TamañoC = (int)Math.Ceiling(division);
            }

            string[,] matriz = new string[espiral.TamañoC, espiral.TamañoF];
            int x = 0; int y = 0;
            int[] limites = { espiral.TamañoC - 1, espiral.TamañoF - 1 };

            switch (espiral.Recorrido)
            {
                case "horizontal":
                    while (text_archivocifrado.Count > 0)
                    {
                        for (int i = y; i <= limites[1]; i++)
                        {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[x, i] = text_archivocifrado.First();
                            text_archivocifrado.Remove(text_archivocifrado.First());
                        }
                        x++;


                        for (int i = x; i <= limites[0]; i++)
                        {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[x, limites[1]] = text_archivocifrado.First();
                            text_archivocifrado.Remove(text_archivocifrado.First());
                        }
                        limites[1]--;


                        for (int i = limites[1]; i >= y; i--)
                        {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[limites[0], i] = text_archivocifrado.First();
                            text_archivocifrado.Remove(text_archivocifrado.First());
                        }
                        limites[0]--;


                        for (int i = limites[0]; i >= x; i--)
                        {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[i, y] = text_archivocifrado.First();
                            text_archivocifrado.Remove(text_archivocifrado.First());
                        }
                        y++;
                    }
                    break;

                case "vertical":

                    while (text_archivocifrado.Count > 0)
                    {
                        for (int i = x; i <= limites[0]; i++)
                        {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[i, y] = text_archivocifrado.First();
                            text_archivocifrado.Remove(text_archivocifrado.First());
                        }
                        y++;


                        for (int i = y; i <= limites[1]; i++)
                        {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[limites[0], i] = text_archivocifrado.First();
                            text_archivocifrado.Remove(text_archivocifrado.First());
                        }
                        for (int i = limites[0] - 1; i >= x; i--)
                        {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[i, limites[1]] = text_archivocifrado.First();
                            text_archivocifrado.Remove(text_archivocifrado.First());
                        }

                        for (int i = limites[1] - 1; i >= y; i--)
                        {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[x, i] = text_archivocifrado.First();
                            text_archivocifrado.Remove(text_archivocifrado.First());
                        }
                        x++;
                        limites[0]--; limites[1]--;
                    }
                    break;
            }
            List<string> texto = new List<string>();
            for (int i = 0; i < espiral.TamañoC; i++)
            {
                for (int j = 0; j < espiral.TamañoF; j++)
                {
                    if (matriz[i, j] == null || matriz[i, j] == "" || matriz[i, j] == "11")
                    {
                    }
                    else
                    {
                        texto.Add(matriz[i, j]);
                    }

                }
            }


            using (var writeStream1 = new FileStream(Server.MapPath("~/Archivo") + "/" + namefile + ".descif", FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream1))
                {
                    foreach (var item in texto)
                    {
                        writer.Write(Convert.ToByte(item));
                    }
                }
            }
        }
}