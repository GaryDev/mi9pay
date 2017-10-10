using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Mi9Pay.Web.Helpers
{
    public class AESHelper
    {
        public const string AESVector = "tu89geji340t89u2";

        public static string AESEncrypt(string dataText, string passPhrase, string vector = null)
        {
            if (string.IsNullOrWhiteSpace(vector))
                vector = AESVector;

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(vector);
            byte[] plainBytes = Encoding.UTF8.GetBytes(dataText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(passPhrase);

            byte[] cryptograph = null;
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.Zeros;
                using (ICryptoTransform decryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            cryptograph = memoryStream.ToArray();
                        }
                    }
                }
            }

            return Convert.ToBase64String(cryptograph);
        }

        public static string AESDecrypt(string cipherText, string passPhrase, string vector = null)
        {
            if (string.IsNullOrWhiteSpace(vector))
                vector = AESVector;

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(vector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(passPhrase);

            string plainText = null;
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.Zeros;
                using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                {
                    using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(plainText))
                return plainText.Replace("\0", string.Empty);

            return plainText;
        }

        public static string GetPassPhase(int n)
        {
            char[] arrChar = new char[] { 'a', 'b', 'd', 'c', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 'q', 's', 't', 'u', 'v', 'w', 'z', 'y', 'x', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Q', 'P', 'R', 'T', 'S', 'V', 'U', 'W', 'X', 'Y', 'Z' };
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
    }
}