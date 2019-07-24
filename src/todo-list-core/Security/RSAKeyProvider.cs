using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TodoListAPI.Interfaces;

namespace TodoListAPI.Security
{
    public class RSAKeyProvider : IRSAKeyProvider
    {
        public RSAKeyProvider()
        {
            _rsaKeyPath = AppDomain.CurrentDomain.BaseDirectory + @"RsaKeys\RsaUserKey.txt";
        }

        #region Implementation of IRSAKeyProvider

        public async Task<string> GetKeysAsync()
        {
            string keys = await ReadKeysAsync();

            if (!string.IsNullOrEmpty(keys))
            {
                return keys;
            }

            string newKeys = CreateKeys();

            await SaveKeysAsync(newKeys);
                
            return newKeys;
        }

        #endregion
        
        private static string CreateKeys()
        {
            var rsaProvider = new RSACryptoServiceProvider(dwKeySize: 2048);
            rsaProvider.ExportParameters(true);
            return rsaProvider.ToXmlString(includePrivateParameters: true);
        }

        private async Task SaveKeysAsync(string key)
        {
            using (var streamWriter = new StreamWriter(_rsaKeyPath))
            {
                await streamWriter.WriteLineAsync(key);
            }
        }

        private async Task<string> ReadKeysAsync()
        {
            using (var streamReader = new StreamReader(_rsaKeyPath))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        private readonly string _rsaKeyPath;
    }
}
