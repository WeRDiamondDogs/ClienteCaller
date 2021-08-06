using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CallerPR.Common
{
    public class Configuracion
    {
        public static IConfiguration configuration = null;
        private static string passwordcodify = "CRYPT4X3S0DNP";
        private static string charscodify = "MEEBAKSOFT";

        public static string entorno = "QAS";

        public static async Task<bool> NotifyAsync(string to, string title, string body)
        {
            try
            {
                // Get the server key from FCM console
                var serverKey = string.Format("key={0}", "AAAASgLokiU:APA91bHncXcQvxySvXkCQkE5fcI_jsOlwjxHmcypHDnNUIlzqVXRsAPiF3WH-sZZ49ahrAr2J-Q5GmzxF5vh6wRXNNnSUu2J5A9R2wt8OVDcUsJK8FFuaMtVKbn0WUYfazBxpsHK34sw");

                // Get the sender id from FCM console
                var senderId = string.Format("id={0}", "317876376101");

                var data = new
                {
                    to, // Recipient device token
                    //notification = new { title, body },
                    data = new { title, body }
                };

                // Using Newtonsoft.Json
                var jsonBody = JsonConvert.SerializeObject(data);

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                    httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                    httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        var result = await httpClient.SendAsync(httpRequest);

                        if (result.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            // Use result.StatusCode to handle failure
                            // Your custom error handler here
                            //_logger.LogError($"Error sending notification. Status Code: {result.StatusCode}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //_logger.LogError($"Exception thrown in Notify Service: {ex}");
            }

            return false;
        }

        /// <summary>
        /// Encriptar cadena de texto.
        /// </summary>
        /// <typeparam name="T">RijndaelManaged | TripleDESCryptoServiceProvider </typepara>m
        /// <param name="value"> Valor a encriptar</param>
        /// <returns>Texto encriptado</returns>
        public static string Encrypt<T>(string value)
          where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(passwordcodify, Encoding.Unicode.GetBytes(charscodify));

            SymmetricAlgorithm algorithm = new T();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateEncryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream())
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        writer.Write(value);
                    }
                }

                return Convert.ToBase64String(buffer.ToArray());
            }
        }

        /// <summary>
        /// Desencriptar cadena de texto.
        /// </summary>
        /// <typeparam name="T">RijndaelManaged | TripleDESCryptoServiceProvider </typeparam>
        /// <param name="text">Texto encriptado</param>
        /// <returns>Texto desencriptado</returns>
        public static string Decrypt<T>(string text)
           where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(passwordcodify, Encoding.Unicode.GetBytes(charscodify));

            SymmetricAlgorithm algorithm = new T();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream(Convert.FromBase64String(text)))
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Unicode))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        /*-------------------------------------------------------*/
        // EJEMPLO DE USO:
        //  String encrypted = CRYPT.Encrypt<TripleDESCryptoServiceProvider>("valor_encriptar");
        //  String decrypted = CRYPT.Encrypt<RijndaelManaged>("texto_desencriptar");
        /*-------------------------------------------------------*/

    }

}