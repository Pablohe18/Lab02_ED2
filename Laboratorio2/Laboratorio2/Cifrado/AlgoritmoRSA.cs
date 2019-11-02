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

        public void generate_key(int p, int q, bool inout)
        {
            long n = Convert.ToInt64((p * q).ToString());
            keys[0] = n;
            int phi = (p - 1) * (q - 1);
            int e = chooseE(phi);
            if (inout)
            {
                keys[1] = Convert.ToInt64((e).ToString());
            }
            else
            {
                long x = Convert.ToInt64((e).ToString());
                long d = modInverse(x, Convert.ToInt64((phi).ToString()));
                keys[1] = d;
            }

        }
        public int cipher(int c)
        {
            int result = 0;
            long partial = Convert.ToInt64((c).ToString());
            //result = partial.modPow(keys[1], keys[0]).intValue();
            result = Convert.ToInt32(modPow(partial, keys[1], keys[0]).ToString());
            return result;
        }
        
        bool coprimos(int a, int b)
        {
            bool verify = false;
            if (mcd(a, b) == 1)
            {
                verify = true;
            }
            return verify;
        }
        int chooseE(int phi)
        {
            int e = 0;
            bool found = false;
            int counter = 2;
            while (!found && counter < phi)
            {
                bool isIt = coprimos(counter, phi);
                if (isIt)
                {
                    bool verify = isPrime(counter);
                    if (verify && counter > 10)
                    {
                        e = counter;
                        found = true;
                    }
                }
                counter++;

            }
            return e;
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