using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Calendar.v3;

namespace G510Display.Source.StringEncrypter
{
  public static class StringCipher
  {
    // This constant is used to determine the keysize of the encryption algorithm in bits.
    // We divide this by 8 within the code below to get the equivalent number of bytes.
    private const int Keysize = 256;

    // This constant determines the number of iterations for the password bytes generation function.
    private const int DerivationIterations = 1000;

    public static byte[] Encrypt(string plainText, byte[] passPhrase)
    {
      // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
      // so that the same Salt and IV values can be used when decrypting.  
      var saltStringBytes = Generate256BitsOfRandomEntropy();
      var ivStringBytes = Generate256BitsOfRandomEntropy();
      var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
      using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
      {
        var keyBytes = password.GetBytes(Keysize / 8);
        using (var symmetricKey = new RijndaelManaged())
        {
          symmetricKey.BlockSize = 256;
          symmetricKey.Mode = CipherMode.CBC;
          symmetricKey.Padding = PaddingMode.PKCS7;
          using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
          {
            using (var memoryStream = new MemoryStream())
            {
              using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
              {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                var cipherTextBytes = saltStringBytes;
                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                return cipherTextBytes;
              }
            }
          }
        }
      }
    }

    public static byte[] Decrypt(string cipherText, string passPhrase)
    {
      // Get the complete stream of bytes that represent:
      // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
      var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
      // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
      var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
      // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
      var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
      // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
      var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

      using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
      {
        var keyBytes = password.GetBytes(Keysize / 8);
        using (var symmetricKey = new RijndaelManaged())
        {
          symmetricKey.BlockSize = 256;
          symmetricKey.Mode = CipherMode.CBC;
          symmetricKey.Padding = PaddingMode.PKCS7;
          using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
          {
            using (var memoryStream = new MemoryStream(cipherTextBytes))
            {
              using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
              {
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return plainTextBytes;
              }
            }
          }
        }
      }
    }

    public static UserCredential AuthorizeGoogle(string cipherText, byte[] passPhrase)
    {
      var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
      return AuthorizeGoogle(cipherTextBytesWithSaltAndIv, passPhrase);
    }

    public static UserCredential AuthorizeGoogle(byte[] cipherTextBytesWithSaltAndIv, byte[] passPhrase)
    {
      UserCredential GoogleCredential;

      var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
      var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
      var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

      using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
      {
        var keyBytes = password.GetBytes(Keysize / 8);
        using (var symmetricKey = new RijndaelManaged())
        {
          symmetricKey.BlockSize = 256;
          symmetricKey.Mode = CipherMode.CBC;
          symmetricKey.Padding = PaddingMode.PKCS7;
          using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
          {
            using (var memoryStream = new MemoryStream(cipherTextBytes))
            {
              using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
              {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/G510Display");
                string[] Scopes = { CalendarService.Scope.CalendarReadonly };

                GoogleCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                  GoogleClientSecrets.Load(cryptoStream).Secrets,
                  Scopes,
                  "user",
                  CancellationToken.None,
                  new FileDataStore(credPath, true)).Result;
              }
            }
          }
        }
      }

      return GoogleCredential;
    }

    public static byte[] Generate256BitsOfRandomEntropy()
    {
      var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
      using (var rngCsp = new RNGCryptoServiceProvider())
      {
        // Fill the array with cryptographically secure random bytes.
        rngCsp.GetBytes(randomBytes);
      }
      return randomBytes;
    }
  }
}
