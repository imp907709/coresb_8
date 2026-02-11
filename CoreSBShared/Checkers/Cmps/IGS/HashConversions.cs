using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BCrypt.Net;

namespace InfrastructureCheckers.IGS
{

    public class HashConversionsIGS
    {
        public static string HashAis(string login, string dateDDMMYYY, string password)
        {
            var hash1 = Hash1(password);
            var hash2 = Hash2(login, dateDDMMYYY, hash1);
            return hash2;
        }

        public static void Blitz()
        {
            var username  = "testhash004";
            var password  = "123456";
            var date      = "05.09.2029";
            var hashSalt  = "AgEnTporTAL";
            var hashAis = HashAis(username, date, password);
            
            // ais hash 
            // B786F71DB055D6E05843A7BFE7436CE2638932B9
            // b786f71db055d6e05843a7bfe7436ce2638932b9
            var ml = "kovina.73@mail.ru";
            var hs = "99963D783C3523FBBB31F2184C8F3C9E97AB0B32";
            
            var dt = "25.08.2016";
            // MD5(password || salt) -> hex lowercase
            var md5Bytes = MD5.HashData(Encoding.UTF8.GetBytes(password + hashSalt));
            var hash1 = Convert.ToHexString(md5Bytes).ToLowerInvariant();

            // SHA1(username || date || hash1) -> hex lowercase
            var sha1Input = Encoding.UTF8.GetBytes(username + date + hash1);
            var sha1Bytes = SHA1.HashData(sha1Input);
            var hash2 = Convert.ToHexString(sha1Bytes).ToLowerInvariant();
        }

        public static string Hash1(string password)
        {
            var hashSalt  = "AgEnTporTAL";

            // MD5(password || salt) -> hex lowercase
            var md5Bytes = MD5.HashData(Encoding.UTF8.GetBytes(password + hashSalt));
            var hash1 = Convert.ToHexString(md5Bytes).ToLowerInvariant();
            return hash1;
        }

        public static string Hash2(string UserName, string dateDDMMYYY, string hash1)
        {
            var sha1Input = Encoding.UTF8.GetBytes(UserName + dateDDMMYYY + hash1);
            var sha1Bytes = SHA1.HashData(sha1Input);
            var hash2 = Convert.ToHexString(sha1Bytes).ToLowerInvariant();
            return hash2;
        }
        
        public static void GO()
        {
            Blitz();
            
            GenMd5ForTesting();
        }



        public static void GenMd5ForTesting()
        {
            var passwords = new List<string>();

            passwords.Add(("123456"));
            passwords.Add(("ZzXx123."));

            var withHashes = passwords.Select(s => 
                new {pass = s, md5 = Hash1(s), md5Up = Hash1(s).ToUpper()});
            var str = JsonSerializer.Serialize(withHashes, new JsonSerializerOptions { WriteIndented = true });
            var dir = Environment.CurrentDirectory;
            File.WriteAllText("hashes.json", str);
        }
    }
}
