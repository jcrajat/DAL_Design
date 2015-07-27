using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace CM.Tools.Cryptographic
{
    public static class Crypto
    {
        #region HASH

        public static string HashEncriptar(string nDataToEncrypt, string nSalt, int nIteraciones)
        {
            var _strPasswordSalt = nDataToEncrypt + nSalt;
            var _objSha1 = SHA512.Create();
            byte[] _objTemporal = null;

            try
            {
                _objTemporal = System.Text.Encoding.UTF8.GetBytes(_strPasswordSalt);

                for (var i = 0; i < nIteraciones; i++)
                {
                    _objTemporal = _objSha1.ComputeHash(_objTemporal);
                }
            }
            finally
            {
                _objSha1.Clear();
            }

            return Convert.ToBase64String(_objTemporal);
        }

        public static bool HashComparar(string nValorSinEncriptar, string nValorEncriptado, string nSalt, int nIteraciones)
        {
            return HashEncriptar(nValorSinEncriptar, nSalt, nIteraciones) == nValorEncriptado;
        }

        #endregion

        #region RSA

        public static void RSACrearKeys(out string nprivateKey, out string npublicKey)
        {
            RSACryptoServiceProvider.UseMachineKeyStore = true;
            var rsa = new System.Security.Cryptography.RSACryptoServiceProvider();

            npublicKey = rsa.ToXmlString(false);
            nprivateKey = rsa.ToXmlString(true);
        }

        public static byte[] RSAEncryptText(string nDataToEncrypt, string npublicKey)
        {
            var RSA = new RSACryptoServiceProvider();
            byte[] Data = null;

            if (nDataToEncrypt.Length > 0)
            {
                const byte Pasos = 50;

                var Partes = nDataToEncrypt.Length / Pasos + (((nDataToEncrypt.Length % Pasos) > 0) ? 1 : 0);

                Data = new byte[Partes * 128];

                RSA.FromXmlString(npublicKey);

                for (var i = 0; i < nDataToEncrypt.Length; i += Pasos)
                {
                    var hexData = Encoding.Unicode.GetBytes(i + Pasos > nDataToEncrypt.Length ? nDataToEncrypt.Substring(i, nDataToEncrypt.Length - i) : nDataToEncrypt.Substring(i, Pasos));

                    var preData = RSA.Encrypt(hexData, false);
                    preData.CopyTo(Data, (i / Pasos) * 128);
                }
            }

            return Data;
        }

        public static string RSADecryptText(byte[] nDataToDecrypt, string nprivateKey)
        {
            var RSA = new RSACryptoServiceProvider();
            var Data = new StringBuilder("");
            var preData = new byte[128];

            if (nDataToDecrypt != null)
            {
                RSA.FromXmlString(nprivateKey);

                for (var i = 0; i < nDataToDecrypt.Length; i += 128)
                {
                    Array.Copy(nDataToDecrypt, i, preData, 0, 128);

                    Data.Append(Encoding.Unicode.GetString(RSA.Decrypt(preData, false)));
                }
            }

            //return ConvertHexaToString(Data.ToString());
            return Data.ToString();
        }

        public static string RSAKeysToXmlString(RSAParameters nKey, bool nIncludePrivateParameters)
        {
            RSACryptoServiceProvider.UseMachineKeyStore = true;

            var rsa = new System.Security.Cryptography.RSACryptoServiceProvider();

            rsa.ImportParameters(nKey);

            return rsa.ToXmlString(nIncludePrivateParameters);
        }

        public static RSAParameters RSAKeysFromXmlString(string nKey, bool nIncludePrivateParameters)
        {
            RSACryptoServiceProvider.UseMachineKeyStore = true;

            var rsa = new System.Security.Cryptography.RSACryptoServiceProvider();

            rsa.FromXmlString(nKey);

            return rsa.ExportParameters(nIncludePrivateParameters);
        }

        #endregion

        #region DPAPI

        public static byte[] DPAPIEncryptText(string nDataToEncrypt, MemoryProtectionScope nScope, byte Corrimiento)
        {
            // Create the original data to be encrypted (The data length should be a multiple of 16).
            byte[] toEncrypt = DPAPIgetBytesToEncrypt(nDataToEncrypt);

            // Encrypt the data in memory.
            DPAPIEncryptInMemoryData(toEncrypt, nScope);

            for (var i = 0; i < toEncrypt.Length; i++)
            {
                var Resultado = (short)(toEncrypt[i] + Corrimiento);

                toEncrypt[i] = (byte)(Resultado > 255 ? Resultado - 255 : Resultado);
            }

            return toEncrypt;
        }

        public static string DPAPIDecryptText(byte[] nDataToDencrypt, MemoryProtectionScope nScope, byte Corrimiento)
        {
            if (nDataToDencrypt == null) return "";

            try
            {
                var DataDecrypt = new byte[nDataToDencrypt.Length];

                for (var i = 0; i < nDataToDencrypt.Length; i++)
                {
                    var Resultado = (short)(nDataToDencrypt[i] - Corrimiento);

                    DataDecrypt[i] = (byte)(Resultado < 0 ? Resultado + 255 : Resultado);
                }

                // Decrypt the data in memory.
                DPAPIDecryptInMemoryData(DataDecrypt, nScope);
                var Valor = DPAPIgetTextDencrypt(DataDecrypt);
                return Valor;
            }
            catch
            {
                return "";
            }
        }

        private static void DPAPIEncryptInMemoryData(byte[] nBuffer, MemoryProtectionScope nScope)
        {
            if (nBuffer.Length <= 0)
                throw new ArgumentException("nBuffer");

            if (nBuffer == null)
                throw new ArgumentNullException("nBuffer");

            // Encrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Protect(nBuffer, nScope);
        }

        private static void DPAPIDecryptInMemoryData(byte[] nBuffer, MemoryProtectionScope nScope)
        {
            if (nBuffer.Length <= 0)
                throw new ArgumentException("nBuffer");

            if (nBuffer == null)
                throw new ArgumentNullException("nBuffer");

            // Decrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Unprotect(nBuffer, nScope);
        }

        private static byte[] DPAPIgetBytesToEncrypt(string nDataText)
        {
            var Length = nDataText.Length;
            const byte LengthHeader = 10;
            var newDataText = new StringBuilder("");

            var Length16 = (Length + LengthHeader) % 16;
            var Formato = "{0:" + new string('0', LengthHeader) + "}";
            newDataText.Append(String.Format(Formato, Length));
            newDataText.Append(nDataText);

            if (Length16 > 0)
                newDataText.Append(new string(' ', 16 - Length16));

            return Encoding.Unicode.GetBytes(newDataText.ToString());
        }

        private static string DPAPIgetTextDencrypt(byte[] nData)
        {
            const byte LengthHeader = 10;

            var newDataText = Encoding.Unicode.GetString(nData);
            var Length = int.Parse(newDataText.Substring(0, LengthHeader));
            newDataText = newDataText.Substring(LengthHeader, Length);

            return newDataText;
        }

        #endregion

        #region TRIPLEDES

        public struct TRIPLEDESKeys
        {
            public byte[] iv;
            public byte[] key;
        }

        public static TRIPLEDESKeys TRIPLEDESGenerateKeys()
        {
            var objDES = new TripleDESCryptoServiceProvider();
            var Keys = new TRIPLEDESKeys();

            Keys.iv = objDES.IV;
            Keys.key = objDES.Key;

            return Keys;
        }

        public static byte[] TRIPLEDESEncrypt(string Data, string Password, TRIPLEDESKeys Keys)
        {
            return TRIPLEDESEncrypt(Data, Password, Keys.key, Keys.iv);
        }

        public static byte[] TRIPLEDESEncrypt(string Data, string Password, byte[] Key, byte[] IV)
        {
            try
            {
                var pdb = new PasswordDeriveBytes(Password, Key);
                Key = pdb.CryptDeriveKey("TripleDES", "SHA1", 192, IV);
                //Key = pdb.GetBytes(Key.Length);

                // Create a MemoryStream.
                var mStream = new MemoryStream();

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                var cStream = new CryptoStream(mStream,
                    new TripleDESCryptoServiceProvider().CreateEncryptor(Key, IV),
                    CryptoStreamMode.Write);

                // Convert the passed string to a byte array.
                var toEncrypt = new UnicodeEncoding().GetBytes(Data);

                // Write the byte array to the crypto stream and flush it.
                cStream.Write(toEncrypt, 0, toEncrypt.Length);
                cStream.FlushFinalBlock();

                // Get an array of bytes from the 
                // MemoryStream that holds the 
                // encrypted data.
                var ret = mStream.ToArray();

                // Close the streams.
                cStream.Close();
                mStream.Close();

                // Return the encrypted buffer.
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(@"A Cryptographic error occurred: {0}", e.Message);
                return null;
            }

        }

        public static string TRIPLEDESDecrypt(byte[] Data, string Password, TRIPLEDESKeys Keys)
        {
            return TRIPLEDESDecrypt(Data, Password, Keys.key, Keys.iv);
        }
        
        public static string TRIPLEDESDecrypt(byte[] Data, string Password, byte[] Key, byte[] IV)
        {
            try
            {
                var pdb = new PasswordDeriveBytes(Password, Key);
                Key = pdb.CryptDeriveKey("TripleDES", "SHA1", 192, IV);
                //Key = pdb.GetBytes(Key.Length);

                // Create a new MemoryStream using the passed 
                // array of encrypted data.
                var msDecrypt = new MemoryStream(Data);

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                var csDecrypt = new CryptoStream(msDecrypt,
                    new TripleDESCryptoServiceProvider().CreateDecryptor(Key, IV),
                    CryptoStreamMode.Read);

                // Create buffer to hold the decrypted data.
                var fromEncrypt = new byte[Data.Length];

                // Read the decrypted data out of the crypto stream
                // and place it into the temporary buffer.
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                //Convert the buffer into a string and return it.
                return new UnicodeEncoding().GetString(fromEncrypt);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(@"A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }

        public static void TRIPLEDESKeySerialize(TRIPLEDESKeys nObjectConfig, string nPath)
        {
            var FileXmlSerializer = new XmlSerializer(typeof(TRIPLEDESKeys));
            var FileStreamWriter = new StreamWriter(nPath);

            try
            {
                FileXmlSerializer.Serialize(FileStreamWriter, nObjectConfig);                
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo ecribir el archivo de configuración, " + ex.Message, ex);
            }
            finally
            {
                FileStreamWriter.Close();
            }
        }

        public static byte[] TRIPLEDESKeySerialize(TRIPLEDESKeys nObjectConfig)
        {           
            var FileXmlSerializer = new XmlSerializer(typeof(TRIPLEDESKeys));
            var FileMemoryStream = new MemoryStream();

            try
            {
                FileXmlSerializer.Serialize(FileMemoryStream, nObjectConfig);
                return FileMemoryStream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo ecribir el archivo de configuración, " + ex.Message, ex);
            }
            finally
            {
                FileMemoryStream.Close();
            }
        }

        public static TRIPLEDESKeys TRIPLEDESKeyDeserialize(string nPath)
        {
            var FileXmlSerializer = new XmlSerializer(typeof(TRIPLEDESKeys));
            var FileStreamReader = new StreamReader(nPath);

            try
            {
                return (TRIPLEDESKeys)FileXmlSerializer.Deserialize(FileStreamReader);
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo leer el archivo de configuración. " + ex.Message, ex);
            }
            finally
            {
                FileStreamReader.Close();
            }
        }

        public static TRIPLEDESKeys TRIPLEDESKeyDeserialize(byte[] nData)
        {
            var FileXmlSerializer = new XmlSerializer(typeof(TRIPLEDESKeys));
            var FileMemoryStream = new MemoryStream(nData);

            try
            {
                return (TRIPLEDESKeys)FileXmlSerializer.Deserialize(FileMemoryStream);
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo leer el archivo de configuración. " + ex.Message, ex);
            }
            finally
            {
                FileMemoryStream.Close();
            }
        }

        #endregion

        #region Hexa

        public static string ConvertStringToHexa(string nData)
        {
            string hex = "";
            foreach (char c in nData)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }

        public static string ConvertHexaToString(string nData)
        {
            var sb = new StringBuilder();
            for (var i = 0; i <= nData.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(nData.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        #endregion
    }
}
