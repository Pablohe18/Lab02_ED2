using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Laboratorio2.DBContext;

namespace Laboratorio2.Cifrado
{
    public class ZigZag
    {
        private int iClave;
        private char cRelleno;

        public ZigZag()
        {
            iClave = default(int);
            cRelleno = (char)1;
        }

        public ZigZag(int clave)
        {
            iClave = clave;
        }

        public void Cifrar(FileInfo file)
        {
            switch (iClave)
            {
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
            var M = CalcularM(file.Length);
            var Filas = iClave - 2;
            var Columnas = 2 * (M - 1);

            //if(iClave)

            char[] nivelSuperior = new char[M];
            char[,] nivelesIntermedios = new char[Filas, Columnas];
            char[] nivelInferior = new char[M - 1];

        }

        private int CalcularM(long lengthArchivo)
        {
            double M = 0;

            M = ((double)(Convert.ToDouble(lengthArchivo) + 1 + (2 * ((double)iClave - 2)))) / ((double)(2 + (2 * ((double)iClave - 2))));

            return (int)Math.Ceiling(M);
        }
    }
}