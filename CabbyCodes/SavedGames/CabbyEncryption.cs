using System;
using System.Security.Cryptography;
using System.Text;

namespace CabbyCodes.SavedGames
{
    /// <summary>
    /// Self-contained re-implementation of Hollow Knight's save encryption (AES/Rijndael in ECB
    /// mode with PKCS7 padding, base64-encoded). This mirrors the game's own <c>Encryption</c>
    /// helper byte-for-byte so the mod can read and write the same encrypted save format.
    ///
    /// The mod deliberately does NOT call the game's internal <c>Encryption</c> class: that type
    /// is not reliably resolvable at runtime across game updates / BepInEx variants, and a failure
    /// to resolve it throws a <see cref="TypeLoadException"/> that aborts the entire load before it
    /// can start. Owning this small algorithm here makes custom save load/save version-independent.
    /// </summary>
    public static class CabbyEncryption
    {
        /// <summary>
        /// Hollow Knight's fixed save-file encryption key.
        /// </summary>
        private const string EncryptionKey = "UKu52ePUBwetZ9wNX88o54dnfKRu0T1l";

        /// <summary>
        /// Encrypts a plaintext string into the base64 form used inside Hollow Knight save files.
        /// </summary>
        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(EncryptionKey);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            using (var rijndael = new RijndaelManaged
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            using (ICryptoTransform transform = rijndael.CreateEncryptor())
            {
                byte[] result = transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(result, 0, result.Length);
            }
        }

        /// <summary>
        /// Decrypts a base64 string from a Hollow Knight save file back into plaintext.
        /// </summary>
        public static string Decrypt(string toDecrypt)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(EncryptionKey);
            byte[] toDecryptArray = Convert.FromBase64String(toDecrypt);

            using (var rijndael = new RijndaelManaged
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            using (ICryptoTransform transform = rijndael.CreateDecryptor())
            {
                byte[] result = transform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                return Encoding.UTF8.GetString(result);
            }
        }
    }
}
