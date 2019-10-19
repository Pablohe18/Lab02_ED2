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
        }

        public SDes(int uno)
        {
            P10 = new int[] { 8, 5, 3, 7, 9, 2, 6, 0, 1, 4 };
            P8 = new int[] { 7, 9, 3, 5, 8, 2, 1, 6 };
            P4 = new int[] { 0, 3, 2, 1 };
            EP = new int[] { 0, 1, 3, 2, 3, 2, 1, 0 };
            IP = new int[] { 6, 3, 5, 7, 2, 0, 1, 4 };
            Swap = new int[8];
            IP1 = new int[8];
            S0 = new string[4, 4];
            S1 = new string[4, 4];
            Key1 = new int[8];
            Key2 = new int[8];
        }

        public void Cifrado(char num_caracter)
        {
            var caracter = GenerarCaracterInicial((int)num_caracter);
            var caracterIP = ObtenerIP(caracter);
            var caracter_p1 = new int[] { caracterIP[0], caracterIP[1], caracterIP[2], caracterIP[3] };
            var caracter_p2 = new int[] { caracterIP[4], caracterIP[5], caracterIP[6], caracterIP[7] };
            var caracter2EP = ObtenerEP(caracter_p2);
            var caracterXORK1 = ObtenerXORKey(caracter2EP, Key1);
        }

        private int[] ObtenerIP(int[] caracter_inicial)
        {
            int[] aux = new int[8];
            for (int i = 0; i < 8; i++)
            {
                aux[i] = caracter_inicial[IP[i]];
            }

            return aux;
        }

        private int[] ObtenerEP(int[] caracter_IP)
        {
            int[] aux = new int[8];
            for (int i = 0; i < 8; i++)
            {
                aux[i] = caracter_IP[EP[i]];
            }

            return aux;
        }

        private int[] ObtenerXORKey(int[] caracterEP,int[] key)
        {
            int[] aux = new int[8];
            for (int i = 0; i < 8; i++)
            {
                if (caracterEP[i] == key[i])
                {
                    aux[i] = 0;
                }
                else
                {
                    aux[i] = 1;
                }
            }

            return aux;
        }

        private int[] GenerarCaracterInicial(int caracterInicial)
        {
            var binario = Convert.ToString(caracterInicial, 2);
            int[] aux = new int[8];
            int contador = 7;

            for (int i = binario.Length - 1; i >= 0; i--)
            {
                aux[contador] = int.Parse(binario.Substring(i, 1));
                contador--;
            }

            return aux;
        }
        /// <summary>
        /// //////////////////////////////////////////////////////
        /// </summary>
        public void Descrifrado()
        {

        }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="num"></param>
        public void GenerarKeys(int num)
        {
            var key = GenerarKeyInicial(num);
            var keyP10 = ObtenerP10(key);
            var KeyLS1 = ObtenerLS1(keyP10);
            Key1 = ObtenerP8(KeyLS1);
            var KeyLS2 = ObtenerLS2(KeyLS1);
            Key2 = ObtenerP8(KeyLS2);
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

        private int[] ObtenerP10(int[] key_inicial)
        {
            int[] aux = new int[10];
            for (int i = 0; i < 10; i++)
            {
                aux[i] = key_inicial[P10[i]];
            }

            return aux;
        }

        private int[] ObtenerLS1(int[] key_p10)
        {
            int[] aux = new int[10];
            int n1 = key_p10[0];
            int n2 = key_p10[5];
            for (int i = 0; i < 9; i++)
            {
                aux[i] = key_p10[i+1];
            }

            aux[4] = n1;
            aux[9] = n2;

            return aux;
        }

        private int[] ObtenerP8(int[] key_LS1)
        {
            int[] aux = new int[8];
            for (int i = 0; i < 8; i++)
            {
                aux[i] = key_LS1[P8[i]];
            }

            return aux;
        }

        private int[] ObtenerLS2(int[] key_LS1)
        {
            int[] aux = new int[10];
            int n1 = key_LS1[0];
            int n2 = key_LS1[1];
            int n3 = key_LS1[5];
            int n4 = key_LS1[6];
            for (int i = 0; i < 8; i++)
            {
                aux[i] = key_LS1[i + 2];
            }

            aux[3] = n1;
            aux[4] = n2;

            aux[8] = n3;
            aux[9] = n4;

            return aux;
        }
    }
}