using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OrdinarioJuntandoTodo.Controller
{
    public class AESCryptography
    {
        // Clave de encriptación (debe ser de 16 bytes para AES-128 o 32 bytes para AES-256)
        private static readonly string encryptionKey = "YourKey123456789"; // 16 caracteres para AES-128

        // Método para encriptar
        public string Encrypt(string plainText)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);  // Convertir el texto en bytes
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);  // Convertir la clave en bytes

            // Verificación del tamaño de la clave
            if (keyBytes.Length != 16 && keyBytes.Length != 32)
            {
                throw new ArgumentException("La clave debe tener 16 o 32 caracteres.");
            }

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Generar IV aleatorio de 16 bytes
                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();

                        byte[] encryptedBytes = ms.ToArray();

                        // Combinar el IV con los datos encriptados y devolver como Base64
                        byte[] combinedBytes = new byte[iv.Length + encryptedBytes.Length];
                        Array.Copy(iv, 0, combinedBytes, 0, iv.Length);  // Colocar el IV al inicio
                        Array.Copy(encryptedBytes, 0, combinedBytes, iv.Length, encryptedBytes.Length);  // Luego, los datos encriptados

                        return Convert.ToBase64String(combinedBytes);  // Devuelve la cadena encriptada en Base64
                    }
                }
            }
        }

        // Método para desencriptar
        public string Decrypt(string encryptedText)
        {
            byte[] combinedBytes = Convert.FromBase64String(encryptedText);  // Convertir la cadena Base64 en bytes
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);  // Convertir la clave en bytes

            // Verificación del tamaño de la clave
            if (keyBytes.Length != 16 && keyBytes.Length != 32)
            {
                throw new ArgumentException("La clave debe tener 16 o 32 caracteres.");
            }

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Extraer el IV de los primeros 16 bytes
                byte[] iv = new byte[16];
                Array.Copy(combinedBytes, 0, iv, 0, iv.Length);  // Copiar el IV desde la cadena encriptada
                aes.IV = iv;

                // Extraer los datos encriptados (después del IV)
                byte[] encryptedBytes = new byte[combinedBytes.Length - iv.Length];
                Array.Copy(combinedBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);  // Extraer los datos encriptados

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);  // Escribir los datos encriptados en el stream
                        cryptoStream.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());  // Devolver la cadena desencriptada
                    }
                }
            }
        }
    }
}
