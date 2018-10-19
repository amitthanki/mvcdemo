using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ENRLReconSystem.Common
{
   
    public class EncryptDecrypt
    {
        private const string className = "EncryptDecrypt";

        // Author: Pradeep Patil
        // Create date: 11/03/2017
        // Method Description: Encrypt Password
        // Method Name: Encrypt
        /// <summary>
        /// Encrypt password
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="useHashing"></param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, bool useHashing, string key)
        {
            MD5CryptoServiceProvider hashmd5 = null;
            TripleDESCryptoServiceProvider tdes = null;
            ICryptoTransform cTransform = null;

            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
                //System.Windows.Forms.MessageBox.Show(key);
                //If hashing use get hashcode regards to your key
                if (useHashing)
                {
                    hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    //Always release the resources and flush data
                    // of the Cryptographic service provide. Best Practice

                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                cTransform = tdes.CreateEncryptor();
                byte[] resultArray =
                  cTransform.TransformFinalBlock(toEncryptArray, 0,
                  toEncryptArray.Length);

                tdes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (hashmd5 != null)
                    hashmd5.Dispose();

                if (tdes != null)
                    tdes.Dispose();

                if (cTransform != null)
                    cTransform.Dispose();
            }
        }

        // Author: Pradeep Patil
        // Create date: 11/03/2017
        // Method Description: Decrypt Password
        // Method Name: Decrypt
        /// <summary>
        /// Decrypt password
        /// </summary>
        /// <param name="cipherString"></param>
        /// <param name="useHashing"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherString, bool useHashing, string key)
        {
            MD5CryptoServiceProvider hashmd5 = null;
            TripleDESCryptoServiceProvider tdes = null;
            ICryptoTransform cTransform = null;

            try
            {
                byte[] keyArray;
                //get the byte code of the string

                byte[] toEncryptArray = Convert.FromBase64String(cipherString);
                if (useHashing)
                {
                    //if hashing was used get the hash code with regards to your key
                    hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    //release any resource held by the MD5CryptoServiceProvider

                    hashmd5.Clear();
                }
                else
                {
                    //if hashing was not implemented get the byte code of the key
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
                }

                tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes. 
                //We choose ECB(Electronic code Book)

                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)
                tdes.Padding = PaddingMode.PKCS7;

                cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     toEncryptArray, 0, toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor                
                tdes.Clear();
                //return the Clear decrypted TEXT
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (hashmd5 != null)
                    hashmd5.Dispose();

                if (tdes != null)
                    tdes.Dispose();

                if (cTransform != null)
                    cTransform.Dispose();
            }

        }
    }
}