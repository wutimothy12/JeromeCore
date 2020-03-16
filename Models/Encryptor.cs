﻿using Microsoft.AspNetCore.DataProtection;
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

using System.Text;
using System.Security.Cryptography;

namespace JeromeCore.Models
{
    public class Encryptor
    {
        IDataProtector _protector;

        // the 'provider' parameter is provided by DI
        public Encryptor(IDataProtectionProvider provider)
        {
            
            _protector = provider.CreateProtector("Encryptor");
        }

        public string Encrypt(string clearText)
        {
            string EncryptionKey = "xxx";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Dispose();
                        //cs.
                        //cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "xxx";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Dispose();
                        //cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        /*
        public string Encrypt(string text)
        {
            string input = text;

            // protect string
            string protectedString = _protector.Protect(input);

            return protectedString;
        }

        public string Decrypt(string text)
        {
            string input = text;

            // protect string
            string unProtectedString = _protector.Unprotect(input);
            return unProtectedString;
        }
        */
    }
}