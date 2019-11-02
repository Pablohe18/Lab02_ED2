using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratorio2.Cifrado
{
    public class AlgoritmoRSA
    {
        public long[] keys;

        public AlgoritmoRSA()
        {
            keys = new long[2];
        }
        

        public bool isPrime(int n)
        {
            int counter = 0;
            for (int i = 1; i <= n; i++)
            {
                if (n % i == 0)
                {
                    counter++;
                }
            }
            if (counter == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int mcd(int a, int b)
        {
            if (b == 0)
            {
                return a;
            }
            else
            {
                return mcd(b, a % b);
            }


        }

        long modInverse(long a, long n)
        {
            long i = n, v = 0, d = 1;
            while (a > 0)
            {
                long t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }

        long modPow(long b, long X, long M)
        {
            long B = b;
            long P = 1;

            while (X != 0)
            {
                if ((X & 1) == 1)
                {
                    P = (P * B) % M;
                }
                B = (B * B) % M;
                X >>= 1;
            }

            return (long)P;
        }
    }
}