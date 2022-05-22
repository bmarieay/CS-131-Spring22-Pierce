using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Linq;
using System.Text;

namespace RSAAlgorithm
{
    class Program
    {
        static int byteSize = 256; // 256 bits

        static void Main(string[] args)
        {

            //for (int i = 0; i < 2000; i++)
            //{
            //    var factors = PrimeFactorization.GetFactors(i);
            //    Console.Write($"Num: {i} prime factorization is: ");

            //    foreach(var factor in factors)
            //    {
            //        Console.Write($"{factor},");
            //    }

            //    Console.WriteLine();
            //}

            //var factors = PrimeFactorization.GetFactors(int.MaxValue / 10);

            //foreach (var item in factors)
            //{
            //    Console.WriteLine(item);
            //}

            RSAKeyGenerator rsa = new RSAKeyGenerator();

            var pub = rsa.GetPublicKey();
            var priv = rsa.GetPrivateKey();

            string pubkeyfilename = @"C:\Users\denni\source\repos\RSAAlgorithm\RSAAlgorithm\testpubkey.txt";
            string privkeyfilename = @"C:\Users\denni\source\repos\RSAAlgorithm\RSAAlgorithm\testprivkey.txt";
            rsa.SavePublicKey(pubkeyfilename);
            rsa.SavePrivateKey(privkeyfilename);
            Console.WriteLine($"Public Key:{Environment.NewLine}{pub}");
            Console.WriteLine();
            Console.WriteLine($"Private Key:{Environment.NewLine}{priv}");


            string filename = @"C:\Users\denni\source\repos\RSAAlgorithm\RSAAlgorithm\encrypted_file.txt";
            string mode = "encrypt";
            int blockSizeInBytes = 128;  // 128 bytes, we gotta make sure that the blocksize is <= than the length of the key

            if (mode == "encrypt")
            {
                var message = "what is for lunch?";

                Console.WriteLine($"encrypting and saving to ${filename}");

                if (!File.Exists(pubkeyfilename))
                {
                    throw new FileNotFoundException("public key file not found");
                }

                EncryptAndWriteToFile(filename, pubkeyfilename, message, blockSizeInBytes);


                Console.WriteLine("..done");

                mode = "decrypt";
            }



            if (mode == "decrypt")
            {
                string decryptedText = ReadFromFileAndDecrypt(filename, privkeyfilename);

                Console.WriteLine("Decrypted text");
                Console.WriteLine(decryptedText);

            }
        }

        private static string ReadFromFileAndDecrypt(string encryptedFilename, string privkeyfilename)
        {

            var keyData = File.ReadAllText(privkeyfilename).Split(',');
            var keyInfo = new
            {
                Keysize = int.Parse(keyData[0]),
                n = BigInteger.Parse(keyData[1]),
                d = BigInteger.Parse(keyData[2])
            };

            var encryptedData = File.ReadAllText(encryptedFilename).Split('_');
            var encryptedInfo = new
            {
                MessageLength = int.Parse(encryptedData[0]),
                Blocksize = int.Parse(encryptedData[1]),
                EncryptedMessage = encryptedData[2]
            };

            if (keyInfo.Keysize < encryptedInfo.Blocksize * 8)
            {
                throw new Exception("Keysize and blocksize mismatch.");
            }

            //convert the encrypted message into BigInteger values
            var encryptedBlocks = new List<BigInteger>();

            var tempblocks = encryptedInfo.EncryptedMessage.Split(',');

            foreach (var block in tempblocks)
            {
                encryptedBlocks.Add(BigInteger.Parse(block));
            }


            // decrypt the BigInteger values

            return DecryptMessage(encryptedBlocks, encryptedInfo.MessageLength, keyInfo.n, keyInfo.d, encryptedInfo.Blocksize);
        }

        private static string DecryptMessage(List<BigInteger> encryptedBlocks, int messageLength, BigInteger n, BigInteger d, int blocksize)
        {

            var decryptedBlocks = new List<BigInteger>();

            foreach (var block in encryptedBlocks)
            {
                decryptedBlocks.Add(BigInteger.ModPow(block, d, n));
            }

            string decryptedMessage = GetTextFromBlocks(decryptedBlocks, messageLength, blocksize);

            return decryptedMessage;
        }

        private static string GetTextFromBlocks(List<BigInteger> decryptedBlocks, int messageLength, int blocksize)
        {
            StringBuilder message = new StringBuilder();

            for (int j = 0; j < decryptedBlocks.Count; j++)
            {
                var block = decryptedBlocks[j];

                var blockMessage = new StringBuilder();
                for (int i = blocksize - 1; i >= 0; i--)
                {
                    if (message.Length + i < messageLength)
                    {
                        var asciiValue = BigInteger.Divide(block, BigInteger.Pow(byteSize, i));

                        block = block % BigInteger.Pow(byteSize, i);
                        blockMessage.Insert(0, (char)asciiValue);
                    }
                }
                message.Append(blockMessage);
            }

            return message.ToString();
        }

        // read the public key file and extract the size, n and e from it
        // encrypt the message and save it to file
        private static void EncryptAndWriteToFile(string filename, string pubkeyfilename, string message, int blocksize)
        {
            var data = File.ReadAllText(pubkeyfilename).Split(',');

            var tempobj = new
            {
                Keysize = int.Parse(data[0]),
                n = BigInteger.Parse(data[1]),
                e = BigInteger.Parse(data[2])
            };

            var encryptedBlocks = EncryptMessage(message, tempobj.n, tempobj.e, blocksize);

            StringBuilder encryptedContent = new StringBuilder();

            encryptedContent.AppendJoin(',', encryptedBlocks);

            string output = $"{message.Length}_{blocksize}_{encryptedContent.ToString()}";

            File.WriteAllText(filename, output);

            }

        // Converts the message into a list of block encoded BigIntegers
        private static List<BigInteger> EncryptMessage(string message, BigInteger n, BigInteger e, int blockSizeInBytes)
        {

            var encryptedBlocks = new List<BigInteger>();

            var blocks = GetBlocksFromText(message, blockSizeInBytes);


            foreach (var block in blocks)
            {
                var encryptedBlock = BigInteger.ModPow(block, e, n);
                encryptedBlocks.Add(encryptedBlock);
            }


            return encryptedBlocks;

        }

        // convert the letters of the message into their ascii values
        // for the entire message we need to encode the entire blocksize into its corresponding BigInteger
        // the formula for that is the ascii value * (byteSize raised to i mod blocksize)
        private static List<BigInteger> GetBlocksFromText(string message, int blockSizeInBytes)
        {
            var bytes = new List<byte>();

            foreach (var letter in message)
            {
                bytes.Add((byte)letter);
            }

            var blocks = new List<BigInteger>();

            for (int blockstart = 0; blockstart < bytes.Count; blockstart += blockSizeInBytes)
            {
                BigInteger blockint = 0;

                for (int i = blockstart; i < Math.Min(blockstart + blockSizeInBytes, bytes.Count); i++)
                {
                    var val = (BigInteger)(bytes[i] * (Math.Pow(byteSize, i % blockSizeInBytes)));
                    blockint += val;
                }

                blocks.Add(blockint);
            }

            return blocks;
        }
    }
}