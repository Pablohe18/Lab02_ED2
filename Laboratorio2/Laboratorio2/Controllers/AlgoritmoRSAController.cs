using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
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
        //    cifradoRSA.generate_key(29, 23, true);
        //    int cifrado = cifradoRSA.cipher(Convert.ToInt32('N'));
        //    cifradoRSA.generate_key(29, 23, false);
        //    int descifrado = cifradoRSA.decipher(cifrado);
            return View(db.ObtenerLista());
        }


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
                        return RedirectToAction("DecryptFile");
                    }
                    ViewBag.Message1 = "Debe ingresar dos numeros que al multiplicarse den como resultado un numero mayor a 255";
                }
                ViewBag.Message = "Debe ingresar numeros positivos";
            }
            return View(keyRSA);
        }


    }
}
