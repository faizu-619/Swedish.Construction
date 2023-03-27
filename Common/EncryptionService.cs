using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EncryptionService
    {
        private readonly ObjectCache cache;
        private readonly IPrincipal user;
        private CacheItemPolicy cachePolicy;

        public EncryptionService(IPrincipal user)
        {
            cache = MemoryCache.Default;
            this.user = user;

            cachePolicy = new CacheItemPolicy()
            {
                AbsoluteExpiration =
                new DateTimeOffset(
                    DateTime.UtcNow.AddSeconds(3600))
            };
        }

        public CryptoStream EncryptStream(Stream responseStream)
        {
            Aes aes = GetEncryptionAlgorithm();
            ToBase64Transform base64Transform = new ToBase64Transform();
            CryptoStream base64EncodedStream = new CryptoStream(responseStream, base64Transform, CryptoStreamMode.Write);
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            CryptoStream cryptoStream = new CryptoStream(base64EncodedStream, encryptor, CryptoStreamMode.Write);
            return cryptoStream;
        }
        static byte[] Encrypt(string plainText)
        {
            byte[] encrypted;
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            return encrypted;
        }
        public Stream DecryptStream(Stream cipherStream)
        {
            Aes aes = GetEncryptionAlgorithm();
            FromBase64Transform base64Transform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces);
            CryptoStream base64DecodedStream = new CryptoStream(cipherStream, base64Transform, CryptoStreamMode.Read);
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            CryptoStream decryptedStream = new CryptoStream(base64DecodedStream, decryptor, CryptoStreamMode.Read);
            return decryptedStream;
        }
        public string DecryptString(string cipherText)
        {
            Aes aes = GetEncryptionAlgorithm();
            byte[] buffer = Convert.FromBase64String(cipherText);
            MemoryStream memoryStream = new MemoryStream(buffer);
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            StreamReader streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }
        private Aes GetEncryptionAlgorithm()
        {
            Aes aes = Aes.Create();
            AesManaged keys = GetKeys();
            aes.Key = keys.Key;
            aes.IV = keys.IV;
            return aes;
        }

        public AesManaged CreateKeys()
        {
            AesManaged aes = new AesManaged();
            aes.GenerateKey();
            aes.GenerateIV();
            cache.Add(user?.Identity?.Name, aes, cachePolicy);
            return aes;
        }

        private AesManaged GetKeys()
        {
            if (cache.Contains(user?.Identity?.Name))
            {
                return cache.Get(user?.Identity?.Name) as AesManaged;
            }
            throw new AccessViolationException("Key was expired.");
        }
    }
}
