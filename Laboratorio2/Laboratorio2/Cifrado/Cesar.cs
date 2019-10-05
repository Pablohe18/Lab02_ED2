using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Laboratorio2.Cifrado
{
    public class Cesar
    {
        public static String CifrarCadena(FileInfo file)
        {
            int valorCaracter, letras;
            string cadena = file.ToString(), cifrado = "";
            letras = cadena.Length;
            char[] ch = new char[letras];
            for (int i = 0; i < letras; i++)
            {
                valorCaracter = (int)cadena[i];
                ch[i] = (char)(valorCaracter + 3 * letras);
                cifrado += ch[i];
            }
            return cifrado;
        }

        public static String DecifrarCadena(FileInfo file)
        {
            int valorCaracter, letras;
            string cadena = file.ToString(), descifrado = "";
            letras = cadena.Length;
            char[] ch = new char[letras];
            for (int i = 0; i < letras; i++)
            {
                valorCaracter = (int)cadena[i];
                ch[i] = (char)(valorCaracter - 3 * letras);
                descifrado += ch[i];
            }
            return descifrado;
        }
    }
}