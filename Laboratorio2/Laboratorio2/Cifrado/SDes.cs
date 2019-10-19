using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratorio2.Cifrado
{
    public class SDes
    {
        private int[] P10;
        private int[] P8;
        private int[] P4;
        private int[] EP;
        private int[] IP;
        private int[] Swap;
        private int[] IP1;
        private string[,] S0;
        private string[,] S1;
        private int[] Key1;
        private int[] Key2;
        private int[] Key;

        public SDes()
        {
            P10 = new int[10];
            P8 = new int[8];
            P4 = new int[4];
            EP = new int[8];
            IP = new int[8];
            Swap = new int[8];
            IP1 = new int[8];
            S0 = new string[4, 4];
            S1 = new string[4, 4];
            Key1 = new int[8];
            Key2 = new int[8];
            Key = new int[10];
        }

        public void Cifrado(int key)
        {
            
        }

        public void Descrifrado()
        {

        }

        public void GenerarKeys(int key)
        {
            Key = GenerarKeyInicial(key);
        }

        private int[] GenerarKeyInicial(int keyInicial)
        {
            var binario = Convert.ToString(keyInicial, 2);
            int[] aux = new int[10];
            int contador = 9;

            for (int i = binario.Length-1; i >= 0; i--)
            {
                aux[contador] = int.Parse(binario.Substring(i, 1));
                contador--;
            }

            return aux;
        }
    }
}