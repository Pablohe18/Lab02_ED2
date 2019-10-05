using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            cRelleno = (char)1;
        }

        public string Cifrar(FileInfo file)
        {

            var M = default(int);
            var Filas = default(int);
            var Columnas = default(int);
            var texto = default(StringBuilder);

            switch (iClave)
            {
                case 1:
                    texto = LeerArchivo(1, file);
                    break;
                case 2:
                    M = CalcularM(file.Length);
                    texto = LeerArchivo(M, M - 1, file);
                    break;
                default:
                    M = CalcularM(file.Length);
                    Filas = iClave - 2;
                    Columnas = 2 * (M - 1);
                    texto = LeerArchivo(M, Filas, Columnas, file);
                    break;
            }

            return texto.ToString();
        }


        public string Descifrar(FileInfo file)
        {

            var M = default(int);
            var Filas = default(int);
            var Columnas = default(int);
            var texto = default(StringBuilder);

            switch (iClave)
            {
                case 1:
                    texto = LeerArchivoCifrado(1, file);
                    break;
                case 2:
                    M = CalcularM(file.Length);
                    texto = LeerArchivoCifrado(M, M - 1, file);
                    break;
                default:
                    M = CalcularM(file.Length);
                    Filas = iClave - 2;
                    Columnas = 2 * (M - 1);
                    texto = LeerArchivoCifrado(M, Filas, Columnas, file);
                    break;
            }

            return texto.ToString();
        }

        private int CalcularM(long lengthArchivo)
        {
            double M = 0;

            M = ((double)(Convert.ToDouble(lengthArchivo) + 1 + (2 * ((double)iClave - 2)))) / ((double)(2 + (2 * ((double)iClave - 2))));

            return (int)Math.Ceiling(M);
        }

        private int CalcularFaltante(int M, long lengthArchivo)
        {
            int faltante = 0;

            faltante = ((2 + (2 * (iClave - 2))) * M) - (1 + (2 * (iClave - 2))) - (int)lengthArchivo;

            return faltante;
        }


        /// <summary>
        /// LECTURA CUANDO LA CLAVE ES 1
        /// </summary>
        /// <param name="M"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private StringBuilder LeerArchivo(int M, FileInfo info)
        {
            StringBuilder builder = new StringBuilder();

            const int bufferLength = 100;

            var buffer = new byte[bufferLength];
            using (var file = new FileStream(info.FullName, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(bufferLength);
                        foreach (var item in buffer)
                        {
                            builder.Append(((char)item).ToString());
                        }

                        //Console.ReadKey();
                    }

                }

            }

            return builder;
        }


        /// <summary>
        /// LECTURA CUANDO LA CLAVE ES 2
        /// </summary>
        /// <param name="M"></param>
        /// <param name="M_1"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private StringBuilder LeerArchivo(int M, int M_1, FileInfo info)
        {
            StringBuilder builder = new StringBuilder();
            char[] nivelSuperior = new char[M];
            char[] nivelInferior = new char[M - 1];
            int faltante = CalcularFaltante(M, info.Length);

            const int bufferLength = 100;

            var buffer = new byte[bufferLength];
            using (var file = new FileStream(info.FullName, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(bufferLength);
                        foreach (var item in buffer)
                        {
                            builder.Append(((char)item).ToString());
                        }

                        //Console.ReadKey();
                    }

                }

            }

            for(int i=0; i < faltante; i++)
            {
                builder.Append(cRelleno.ToString());
            }

            nivelSuperior[0] = builder.ToString()[0];
            for (int i = 1; i < M ; i++)
            {
                nivelInferior[i - 1] = builder.ToString()[(i * 2) - 1];
                nivelSuperior[i] = builder.ToString()[(i * 2)];
            }

            builder = new StringBuilder();

            foreach (var item in nivelSuperior)
            {
                builder.Append(item.ToString());
            }

            foreach (var item in nivelInferior)
            {
                builder.Append(item.ToString());
            }

            return builder;
        }


        /// <summary>
        /// LECTURA CUANDO LA CLAVE ES MAS DE 2
        /// </summary>
        /// <param name="M"></param>
        /// <param name="filas"></param>
        /// <param name="columnas"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private StringBuilder LeerArchivo(int M, int filas, int columnas, FileInfo info)
        {
            StringBuilder builder = new StringBuilder();
            char[] nivelSuperior = new char[M];
            char[,] nivelesIntermedios = new char[filas, columnas];
            char[] nivelInferior = new char[M - 1];
            int faltante = CalcularFaltante(M, info.Length);

            const int bufferLength = 100;

            var buffer = new byte[bufferLength];
            using (var file = new FileStream(info.FullName, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(bufferLength);
                        foreach (var item in buffer)
                        {
                            builder.Append(((char)item).ToString());
                        }

                        //Console.ReadKey();
                    }

                }

            }

            for (int i = 0; i < faltante; i++)
            {
                builder.Append(cRelleno.ToString());
            }

            nivelSuperior[0] = builder.ToString()[0];
            int cont = 0;
            for (int i = 1; i < M; i++)
            {
                for (int j = 0; j < filas; j++)
                {
                    cont++;
                    nivelesIntermedios[j, ((i - 1) * 2)] = builder.ToString()[cont];
                }

                cont++;
                nivelInferior[i - 1] = builder.ToString()[cont];

                for (int j = filas-1; j > -1; j--)
                {
                    cont++;
                    nivelesIntermedios[j, ((i - 1) * 2) + 1] = builder.ToString()[cont];
                }

                cont++;
                nivelSuperior[i] = builder.ToString()[cont];
            }

            builder = new StringBuilder();

            foreach (var item in nivelSuperior)
            {
                builder.Append(item.ToString());
            }

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    builder.Append(nivelesIntermedios[i,j]);
                }
            }

            foreach (var item in nivelInferior)
            {
                builder.Append(item.ToString());
            }


            return builder;
        }


        /// <summary>
        /// LECTURA CUANDO LA CLAVE ES 1
        /// </summary>
        /// <param name="M"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private StringBuilder LeerArchivoCifrado(int M, FileInfo info)
        {
            StringBuilder builder = new StringBuilder();

            const int bufferLength = 100;

            var buffer = new byte[bufferLength];
            using (var file = new FileStream(info.FullName, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(bufferLength);
                        foreach (var item in buffer)
                        {
                            builder.Append(((char)item).ToString());
                        }

                        //Console.ReadKey();
                    }

                }

            }

            return builder;
        }


        /// <summary>
        /// LECTURA CUANDO LA CLAVE ES 2
        /// </summary>
        /// <param name="M"></param>
        /// <param name="M_1"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private StringBuilder LeerArchivoCifrado(int M, int M_1, FileInfo info)
        {
            StringBuilder builder = new StringBuilder();
            char[] nivelSuperior = new char[M];
            char[] nivelInferior = new char[M - 1];
            int faltante = CalcularFaltante(M, info.Length);

            const int bufferLength = 100;

            var buffer = new byte[bufferLength];
            using (var file = new FileStream(info.FullName, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(bufferLength);
                        foreach (var item in buffer)
                        {
                            builder.Append(((char)item).ToString());
                        }

                        //Console.ReadKey();
                    }

                }

            }

            for (int i = 0; i < faltante; i++)
            {
                builder.Append(cRelleno.ToString());
            }

            int cont = 0;
            for (int i = 0; i < M; i++)
            {
                nivelSuperior[i] = builder.ToString()[cont];
                cont++;
            }
            for (int i = 0; i < M-1; i++)
            {
                nivelInferior[i] = builder.ToString()[cont];
                cont++;
            }

            builder = new StringBuilder();

            builder.Append(nivelSuperior[0]);
            for (int i = 1; i < M; i++)
            {
                builder.Append(nivelInferior[i - 1]);
                builder.Append(nivelSuperior[i]);
            }

            return builder;
        }


        /// <summary>
        /// LECTURA CUANDO LA CLAVE ES MAS DE 2
        /// </summary>
        /// <param name="M"></param>
        /// <param name="filas"></param>
        /// <param name="columnas"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private StringBuilder LeerArchivoCifrado(int M, int filas, int columnas, FileInfo info)
        {
            StringBuilder builder = new StringBuilder();
            char[] nivelSuperior = new char[M];
            char[,] nivelesIntermedios = new char[filas, columnas];
            char[] nivelInferior = new char[M - 1];
            int faltante = CalcularFaltante(M, info.Length);

            const int bufferLength = 100;

            var buffer = new byte[bufferLength];
            using (var file = new FileStream(info.FullName, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(bufferLength);
                        foreach (var item in buffer)
                        {
                            builder.Append(((char)item).ToString());
                        }

                        //Console.ReadKey();
                    }

                }

            }

            for (int i = 0; i < faltante; i++)
            {
                builder.Append(cRelleno.ToString());
            }

            nivelSuperior[0] = builder.ToString()[0];
            int cont = 0;
            for (int i = 1; i < M; i++)
            {
                for (int j = 0; j < filas; j++)
                {
                    cont++;
                    nivelesIntermedios[j, ((i - 1) * 2)] = builder.ToString()[cont];
                }

                cont++;
                nivelInferior[i - 1] = builder.ToString()[cont];

                for (int j = filas - 1; j > -1; j--)
                {
                    cont++;
                    nivelesIntermedios[j, ((i - 1) * 2) + 1] = builder.ToString()[cont];
                }

                cont++;
                nivelSuperior[i] = builder.ToString()[cont];
            }

            builder = new StringBuilder();

            foreach (var item in nivelSuperior)
            {
                builder.Append(item.ToString());
            }

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    builder.Append(nivelesIntermedios[i, j]);
                }
            }

            foreach (var item in nivelInferior)
            {
                builder.Append(item.ToString());
            }


            return builder;
        }
    }
}