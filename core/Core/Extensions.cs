﻿//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Cartheur.Animals.Utilities;

namespace Cartheur.Animals.Core
{
    /// <summary>
    /// The class containing the extensions for the application.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets a randomized enum for bias.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Enum GetRandomEnumValue(this Type t)
        {
            return Enum.GetValues(t)          // get values from Type provided
                .OfType<Enum>()               // casts to Enum
                .OrderBy(e => Guid.NewGuid()) // mess with order of results
                .FirstOrDefault();            // take first item in result
        }
        /// <summary>
        /// Retrieve the static type of type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Of<T>()
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("Must use Enum type");

            Array enumValues = Enum.GetValues(typeof(T));
            return (T)enumValues.GetValue(StaticRandom.Next(enumValues.Length));
        }
        /// <summary>
        /// Determines if the specified value has occurred previously.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="lastValue">The previous value.</param>
        /// <returns></returns>
        public static bool HasAppearedBefore(this string value, string lastValue)
        {
            return value.Contains(lastValue);
        }
        /// <summary>
        /// Determines if the specified value has occurred previously.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="lastValue">The previous value.</param>
        /// <returns></returns>
        public static bool HasAppearedBefore(this int value, int lastValue)
        {
            return value.Equals(lastValue);
        }
        /// <summary>
        /// Determines whether the specified value is between a minimum-maximum range (inclusive).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns>If the value is between a minimum and maximum threshold.</returns>
        public static bool IsBetween(this int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
        /// <summary>
        /// Returns the properties of the given object as XElements. Properties with null values are still returned, but as empty elements. Underscores in property names are replaces with hyphens.
        /// </summary>
        public static IEnumerable<XElement> AsXElements(this object source)
        {
            foreach (PropertyInfo prop in source.GetType().GetProperties())
            {
                object value = prop.GetValue(source, null);
                yield return new XElement(prop.Name.Replace("_", "-"), value);
            }
        }
        /// <summary>
        /// Returns the properties of the given object as XElements.Properties with null values are returned as empty attributes. Underscores in property names are replaces with hyphens.
        /// </summary>
        public static IEnumerable<XAttribute> AsXAttributes(this object source)
        {
            foreach (PropertyInfo prop in source.GetType().GetProperties())
            {
                object value = prop.GetValue(source, null);
                yield return new XAttribute(prop.Name.Replace("_", "-"), value ?? "");
            }
        }
        // /// <summary>
        // /// Encrypts the file.
        // /// </summary>
        // /// <param name="inputFile">The input file.</param>
        // /// <param name="outputFile">The output file.</param>
        // /// <param name="password">The password for encryption.</param>
        // /// <returns>True if encryption is successful.</returns>
        // public static bool EncryptFile(this string inputFile, string outputFile, string password)
        // {
        //     try
        //     {
        //         Cryptography.EncryptFile(inputFile, outputFile, Base64Tool.Encode(password));
        //         return true;
        //     }
        //     catch (Exception ex)
        //     {
        //         Logging.WriteLog(ex.Message, Logging.LogType.Information, Logging.LogCaller.Cryptography);
        //         return false;
        //     }
            

        // }
        // /// <summary>
        // /// Decrypts the file.
        // /// </summary>
        // /// <param name="inputFile">The input file.</param>
        // /// <param name="outputFile">The output file.</param>
        // /// <param name="password">The password for decryption.</param>
        // /// <returns>True if decryption is successful.</returns>
        // public static bool DecryptFile(this string inputFile, string outputFile, string password)
        // {
        //     try
        //     {
        //         Cryptography.DecryptFile(inputFile, outputFile, Base64Tool.Decode(password));
        //         return true;
        //     }
        //     catch (Exception ex)
        //     {
        //         Logging.WriteLog(ex.Message, Logging.LogType.Information, Logging.LogCaller.Cryptography);
        //         return false;
        //     }
        // }

        #region boneyard
        //public static void EncryptFileOld(this string inputFile, string outputFile, string key, int keysize)
        //{
        //    try
        //    {
        //        //string password = @"myKey123"; // Your Key Here
        //        UnicodeEncoding encoding = new UnicodeEncoding();
        //        byte[] keyBytes = encoding.GetBytes(key);
        //        string cryptFile = outputFile;
        //        FileStream fileStreamCrypt = new FileStream(cryptFile, FileMode.Create);
        //        RijndaelManaged rijnManaged = new RijndaelManaged();
        //        CryptoStream cryptoStream = new CryptoStream(fileStreamCrypt, rijnManaged.CreateEncryptor(keyBytes, key), CryptoStreamMode.Write);
        //        FileStream fileStreamInput = new FileStream(inputFile, FileMode.Open);

        //        int data;
        //        while ((data = fileStreamInput.ReadByte()) != -1)
        //            cryptoStream.WriteByte((byte)data);

        //        fileStreamInput.Close();
        //        cryptoStream.Close();
        //        fileStreamCrypt.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.Cryptography);
        //    }
        //}
        //public static void DecryptFileOld(this string inputFile, string outputFile, string key, int keysize)
        //{
        //    byte[] ivBytes = null;
        //    key = key.Base64Decode();
        //    switch (keysize / 8)
        //    {
        //        // Determine which initialization vector to use.
        //        case 8:
        //            ivBytes = Cryptography.Iv8;
        //            break;
        //        case 16:
        //            ivBytes = Cryptography.Iv16;
        //            break;
        //        case 24:
        //            ivBytes = Cryptography.Iv24;
        //            break;
        //        case 32:
        //            ivBytes = Cryptography.Iv32;
        //            break;

        //    }

        //    try
        //    {
        //        //string password = @"myKey123"; // Your Key Here
        //        UnicodeEncoding encoding = new UnicodeEncoding();

        //        byte[] keyBytes = encoding.GetBytes(key);
        //        FileStream fileStreamCrypt = new FileStream(inputFile, FileMode.Open);
        //        RijndaelManaged rijnManaged = new RijndaelManaged();
        //        CryptoStream cryptoStream = new CryptoStream(fileStreamCrypt, rijnManaged.CreateDecryptor(keyBytes, ivBytes), CryptoStreamMode.Read);
        //        FileStream fileStreamOutput = new FileStream(outputFile, FileMode.Create);

        //        int data;
        //        while ((data = cryptoStream.ReadByte()) != -1)
        //            fileStreamOutput.WriteByte((byte)data);

        //        fileStreamOutput.Close();
        //        cryptoStream.Close();
        //        fileStreamCrypt.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.Cryptography);
        //    }

        //}

        #endregion
    }
}
