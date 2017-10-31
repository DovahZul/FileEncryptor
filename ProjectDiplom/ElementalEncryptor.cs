using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace ProjectDiplom
{
    class ElementalEncryptor
    {

        //public static string getHashSha256(string text)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(text);
        //    SHA256Managed hashstring = new SHA256Managed();
        //    byte[] hash = hashstring.ComputeHash(bytes);
        //    string hashString = string.Empty;
        //    foreach (byte x in hash)
        //    {
        //        hashString += String.Format("{0:x2}", x);
        //    }
        //    return hashString;
        //}
        public static Int16 Get16BitHash2(string s)
        {
            using (var md5Hasher = MD5.Create())
            {
                var data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(s));
                return  BitConverter.ToInt16(data, 0);
            }
        }
        public static byte[] getHashSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            return hash;
        }
        public static byte[] encryptStream(byte[] plain, byte[] Key, byte[] IV)
        {
            byte[] encrypted; ;
            using (MemoryStream mstream = new MemoryStream())
            {
                using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mstream,
                        aesProvider.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plain, 0, plain.Length);
                    }
                    encrypted = mstream.ToArray();
                }
            }
            return encrypted;
        }
        public static byte[] decryptStream(byte[] encrypted, byte[] Key, byte[] IV)
        {
          
                byte[] plain;
                int count;


                using (MemoryStream mStream = new MemoryStream(encrypted))
                {
                    using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                    {
                        //aesProvider.Mode = CipherMode.CBC;
                        using (CryptoStream cryptoStream = new CryptoStream(mStream,
                         aesProvider.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                        {
                            plain = new byte[encrypted.Length];
                            //aesProvider.Padding = PaddingMode.None;
                            count = cryptoStream.Read(plain, 0, plain.Length);

                        }
                    }


                    //????
                    byte[] returnval = new byte[count];
                    Array.Copy(plain, returnval, count);
                    return returnval;
                }
            }
        
        public static void EncryptFile(string input, string output)
        {
            byte[] bytes = File.ReadAllBytes(input);
            byte temp;
            Array.Reverse(bytes, 0, bytes.Length);
            File.WriteAllBytes(output, bytes);
        }
        public static void DecryptFile(string input, string output)
        {
            byte[] bytes = File.ReadAllBytes(input);
            byte temp;
 
            Array.Reverse(bytes, 0, bytes.Length);
  
            File.WriteAllBytes(output, bytes);

        }
    }
}
