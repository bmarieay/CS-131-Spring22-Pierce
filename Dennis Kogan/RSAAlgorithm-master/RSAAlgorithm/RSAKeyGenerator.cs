using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace RSAAlgorithm
{
    public class RSAKeyGenerator
    {
        MillerRabin mb;

        BigInteger n;
        BigInteger d;
        BigInteger e;
        BigInteger p;
        BigInteger q;

        int keySizeBits;// bits
        public RSAKeyGenerator(int keysizebits = 1024)
        {
            mb = new MillerRabin();
            keySizeBits = keysizebits;
            ComputeNumbers();
        }

        // extended euclidean algorithm
        public static BigInteger Egcd(BigInteger left, BigInteger right, out BigInteger leftFactor, out BigInteger rightFactor)
        {
            leftFactor = 0;
            rightFactor = 1;
            BigInteger u = 1;
            BigInteger v = 0;
            BigInteger gcd = 0;

            while (left != 0)
            {
                BigInteger q = right / left;
                BigInteger r = right % left;

                BigInteger m = leftFactor - u * q;
                BigInteger n = rightFactor - v * q;

                right = left;
                left = r;
                leftFactor = u;
                rightFactor = v;
                u = m;
                v = n;

                gcd = right;
            }

            return gcd;
        }

        public static BigInteger ModInverse(BigInteger value, BigInteger modulo)
        {
            BigInteger x, y;

            if (1 != Egcd(value, modulo, out x, out y))
                throw new ArgumentException("Invalid modulo", "modulo");

            if (x < 0)
                x += modulo;

            return x % modulo;
        }

        private void ComputeNumbers()
        {
            p = mb.GenerateLargePrimeNumber(keySizeBits);

            do
            {
                q = mb.GenerateLargePrimeNumber(keySizeBits);
            } while (q.Equals(p));

            n = BigInteger.Multiply(p, q);


            // compute e which is relatively prime with (p - 1) * (q - 1)

            var temp = BigInteger.Multiply(p - 1, q - 1);

            while (true)
            {
                e = mb.GenerateLargePrimeNumber(1024);
                if (BigInteger.GreatestCommonDivisor(e, temp) == 1)
                    break;
            }


            // calculate the mod inverse of e with respect to (p - 1) * (q - 1), it will be d
            d = ModInverse(e, temp);

        }

        private (BigInteger n, BigInteger e) GetPublicKeyTuple()
        {
            return (this.n, this.e);
        }

        private (BigInteger n, BigInteger d) GetPrivateKeyTuple()
        {
            return (this.n, this.d);
        }


        public string GetPublicKey()
        {
            var res = GetPublicKeyTuple();

            return $"{res.n}{res.e}";
        }

        public string GetPrivateKey()
        {
            var res = GetPrivateKeyTuple();

            return $"{res.n}{res.d}";
        }

        // n, e
        public void SavePublicKey(string filename)
        {
            var pubKey = GetPublicKeyTuple();
            string output = $"{keySizeBits},{pubKey.n},{pubKey.e}";

            File.WriteAllText(filename, output);
        }

        // n, d
        public void SavePrivateKey(string filename)
        {
            var priKey = GetPrivateKeyTuple();
            string output = $"{keySizeBits},{priKey.n},{priKey.d}";

            File.WriteAllText(filename, output);
        }

        public void SaveGeneratedKeys(string filename)
        {
            if (File.Exists($"{filename}_publickeyrsa.txt") || File.Exists($"{filename}_privatekeyrsa.txt"))
            {
                throw new Exception("File already exists. This will overwrite the keys.");
            }
            File.WriteAllText($"{filename}_publickeyrsa.txt", GetPublicKey());
            File.WriteAllText($"{filename}_privatekeyrsa.txt", GetPrivateKey());

        }
    }
}