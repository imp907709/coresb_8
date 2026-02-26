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
        public static string GenerateApiKey()
        {
            var bytes = RandomNumberGenerator.GetBytes(32); // 256 bits
            return Convert.ToBase64String(bytes);
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

        public static string Hash3(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
            return hash;
        }

        public static bool Hash3Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        public static string Hash3Stable(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password, "$2a$12$22charactersOfBase64Salt");
            return hash;
        }

        
        
        
        
        public static string HashAis(string login, string dateDDMMYYY, string password)
        {
            var hash1 = Hash1(password);
            var hash2 = Hash2(login, dateDDMMYYY, hash1);
            return hash2;
        }

        public static void CheckNewHashes()
        {
            var rng = Enumerable.Range(0, 5).Select(s=> new 
            {
                k = GenerateApiKey()
            }).Select(s=>s.k).ToList();
          
            
            var hashSent = Hash3Stable("123456");
            var hashStored = Hash3(hashSent);
            
            var hashSentTwo = Hash3Stable("123456");
            var hashChck = Hash3Verify(hashSentTwo, hashStored);
            
            var hashSentNew = Hash3("123456");
            
                
            var hash1lvlv1= "$2a$12$Ei9SzH6mEHSjEWVoQIuacOGbkrd8TOG/rINxz/d/YU8EQ0Tv9AtCq";
            var hash1lvlv2 = "$2a$12$sTfoyspkJ7DvqgUcnYZQhevAa6sKkDrXYFTGmmVka02MtNlYyzNqK";
            
            
            //hash sent 
            var hashSentBlitz = "$2a$12$10hpKkLps/8xGOODEFhWUuenEX24b18geQIGPMe8TP6XoOzgfM./2";
            var hashed = Hash3(hash1lvlv1); 

            // double hash 1
            var doubleHashOne = "$2a$12$Uqs6XbrMFCki1cbOYXcE5OFlXLtywi/ahqvw9JAoOe1EpQebcDEWG";
            // double hash 2
            var doubleHashTwo = "$2a$12$h6ejkVIjss/XiFHSr1P9leMxUzWRYCA4oCZDOWj7N2xQ0KD68YEka";
            
            var doubledHashVerify = Hash3Verify(hash1lvlv2, doubleHashOne);

            // "$2a$12$qNEvACIvx0e/PM7RnYKQ9.wzEQuUItFscwugqfLZpDcSO2ucD4FjK";
            
            // test hash stored
            var testStored = "$2a$12$O5m727zoD8QAUBPfjfKgg.RV2jUeA7QZ4FEIhUsIM98gdwb1luiBe";
            

        }
        
        public static void Blitz()
        {
            var username  = "testTwoZero";
            var password  = "123456";
            var date      = "28.01.2026";
            var hashSalt  = "AgEnTporTAL";
            var hashAis = HashAis(username, date, password);
            
            var hs3 = Hash3("1234567");
            
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
        
        
        
        public static void GO()
        {
            HashExperiments.AisExperiment();
            
            CheckNewHashes();
            
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
