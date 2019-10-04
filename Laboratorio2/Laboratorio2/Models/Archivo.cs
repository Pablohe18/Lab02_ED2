using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratorio2.Models
{
    public class Archivo
    {
        public DateTime fechaCifrado;
        public string nombreArchivo;
        public long tamanioArchivo;
        public long tamanioArchivoNuevo;

        public Archivo()
        {
            fechaCifrado = new DateTime();
            nombreArchivo = default(string);
            tamanioArchivo = default(long);
            tamanioArchivoNuevo = default(long);

        }

        public int CompareTo(Archivo other)
        {
            return this.fechaCifrado.CompareTo(other.fechaCifrado);
        }
    }
}