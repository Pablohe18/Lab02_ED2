using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Laboratorio2.DBContext
{
    public class DefaultConnection
    {
        private static volatile DefaultConnection Instance;
        private static object syncRoot = new Object();

        public static List<Models.Archivo> archivos = new List<Models.Archivo>();
        public int keyzigzag { get; set; }

        private static FileInfo fileInfo = default(FileInfo);

        //public List<Models.Archivo> historialCompresiones = new List<Models.Archivo>();
        public int IdActual { get; set; }

        public DefaultConnection()
        {
            IdActual = 0;
        }

        public List<Models.Archivo> ObtenerLista()
        {
            return archivos;
        }
        public void AsignarRuta(FileInfo file)
        {
            fileInfo = file;
        }

        public FileInfo ObtenerRuta()
        {
            return fileInfo;
        }

        public static DefaultConnection getInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (syncRoot)
                    {
                        if (Instance == null)
                        {
                            Instance = new DefaultConnection();
                        }
                    }
                }
                return Instance;
            }
        }
    }
}