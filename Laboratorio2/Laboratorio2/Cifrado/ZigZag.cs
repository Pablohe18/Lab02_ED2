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
            var M = default(int);
            var Filas = default(int);
            var Columnas = default(int);

            switch (iClave)
            {
                case 1:

                    break;
                case 2:
                    M = CalcularM(file.Length);
                    break;
                default:
                    M = CalcularM(file.Length);
                    Filas = iClave - 2;
                    Columnas = 2 * (M - 1);
                    break;
            }
            

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