using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace RSAAlgorithm
{
    public class MillerRabin
    {
        int lowLimit = 100000;
        List<BigInteger> lowPrimes;

        RNGCryptoServiceProvider rng;
        Random rand;
        public MillerRabin()
        {
            lowPrimes = new List<BigInteger>();

            rand = new Random();
            rng = new RNGCryptoServiceProvider();

            for (int i = 2; i <= lowLimit; i++)
            {
                if (IsPrimeSlow(i))
                {
                    lowPrimes.Add(i);
                }

            }

        }

        public bool IsPrimeSlow(int num)
        {
            if (num < 2)
            {
                return false;
            }
            else if (num == 2)
            {
                return true;
            }
            else if (num % 2 == 0)
            {
                return false;
            }
            else
            {
                for (int i = 3; i <= Math.Ceiling(Math.Sqrt(num)); i += 2)
                {
                    if (num % i == 0)
                    {
                        return false;
                    }
                }

                return true;
            }

        }

        public BigInteger GenerateLargePrimeNumber(int bits)
        {
            int bytesize = (int)Math.Ceiling(bits / 8.0);
            byte[] arr = new byte[bytesize];

            rng.GetBytes(arr);

            BigInteger bigInt = BigInteger.Abs(new BigInteger(arr));

            while (!IsPrime(bigInt))
            {
                rng.GetBytes(arr);
                bigInt = BigInteger.Abs(new BigInteger(arr));
            }

            return bigInt;
        }

        public bool IsPrime(BigInteger num)
        {
            if (num < 2)
            {
                return false;
            }
            else if (lowPrimes.Contains(num))
            {
                return true;
            }

            foreach (var prime in lowPrimes)
            {
                if (num % prime == 0)
                {
                    return false;
                }
            }

            return RabinMillerTest(num);
        }

        // returns true if num is a prime number
        private bool RabinMillerTest(BigInteger num)
        {
            BigInteger s = num - 1;
            int t = 0;

            // keep halving s until it is odd, and use t to count how many times we halve s
            while (s % 2 == 0)
            {
                s /= 2;
                t += 1;
            }

            // try to falsify nums primality 5 times
            for (int trial = 0; trial < 5; trial++)
            {
                // generate a number between 2 and num - 1
                BigInteger a = RandomBigInteger(num - 2) + 1;

                BigInteger v = BigInteger.ModPow(a, s, num);

                if (v != 1)
                {
                    int i = 0;
                    while (v != (num - 1))
                    {
                        if (i == t - 1)
                        {
                            return false;
                        }
                        else
                        {
                            i++;

                            v = BigInteger.ModPow(v, 2, num);
                        }

                    }
                }

            }


            return true;
        }

        private BigInteger RandomBigInteger(BigInteger N)
        {
            BigInteger result = 0;
            do
            {
                int length = (int)Math.Ceiling(BigInteger.Log(N, 2));
                int numBytes = (int)Math.Ceiling(length / 8.0);
                byte[] data = new byte[numBytes];
                rand.NextBytes(data);
                result = new BigInteger(data);
            } while (result >= N || result <= 0);
            return result;
        }
    }
}