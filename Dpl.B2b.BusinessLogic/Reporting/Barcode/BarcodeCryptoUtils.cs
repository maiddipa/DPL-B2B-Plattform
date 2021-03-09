using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Dpl.B2b.BusinessLogic.Reporting.Barcode
{
    /**
     * Some simple crypto utils
     */
    public class BarcodeCryptoUtils
    {
        public static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(data);
            }
        }

        public static string EncryptWithRijndael(string input, string key)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(input);
            byte[] array = new byte[16];

            RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(array);
            Rijndael rijndael = Rijndael.Create();
            PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(key, array);
            rijndael.Key = passwordDeriveBytes.GetBytes(32);
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(array, 0, 16);
            memoryStream.Write(rijndael.IV, 0, 16);
            ICryptoTransform transform = rijndael.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] array2 = new byte[memoryStream.Length];
            memoryStream.Seek(0L, SeekOrigin.Begin);
            memoryStream.Read(array2, 0, (int) memoryStream.Length);
            return Convert.ToBase64String(array2);
        }

        public static string DecryptWithRijndael(string str, string key)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            byte[] array = Convert.FromBase64String(str);
            byte[] array2 = new byte[16];
            Array.Copy(array, array2, 16);
            Rijndael rijndael = Rijndael.Create();
            PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(key, array2);
            rijndael.Key = passwordDeriveBytes.GetBytes(32);
            byte[] array3 = new byte[16];
            Array.Copy(array, 16, array3, 0, 16);
            rijndael.IV = array3;
            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform transform = rijndael.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(array, 32, array.Length - 32);
            cryptoStream.FlushFinalBlock();
            byte[] array4 = new byte[memoryStream.Length];
            memoryStream.Seek(0L, SeekOrigin.Begin);
            memoryStream.Read(array4, 0, (int)memoryStream.Length);
            return Encoding.Unicode.GetString(array4);
        }
    }
}