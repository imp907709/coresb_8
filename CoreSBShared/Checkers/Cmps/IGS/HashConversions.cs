using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text;

namespace InfrastructureCheckers.IGS
{
    public class HashConversionsIGS
    {
        public static void GO()
        {

            var initialPass = 123456;
            var initialAslt = "AgEnTporTAL";
            var dataSalt = initialPass + initialAslt;
            
            var databt = Encoding.UTF8.GetBytes(dataSalt);
            
            var assumedBlitzHash = MD5.HashData(databt);
            var asumedBlitsHex = Convert.ToHexString(assumedBlitzHash);
            // 377D1910A92318860B137E910B2F28B2
            // CORRECT
            
            
            // ACTUAL MIGRATED CODE 
            // correct input for secondary hash 
            var hashSecCorr = "SHURGOTUV@MAIL.RU25.03.2019377d1910a92318860b137e910b2f28b2";
            var hashSecBtCr = Encoding.UTF8.GetBytes(hashSecCorr);
            var hashedSecondaryCr = SHA1.HashData(hashSecBtCr);
            var hsSecResCr = Convert.ToHexString(hashedSecondaryCr);
            // 940C9067866924A50FC43ACCC2F15B4968DBC32D
            // CORRECT
            
            
            
            // 1
            var userLogin = "SHURGOTUV@MAIL.RU";
            // 2 
            var creatediso =  "2019-03-13T16:09:32Z";
           
            
            // 2 A - direct string parse 
            string resultFormatted = creatediso.Substring(8, 2) + "." +
                            creatediso.Substring(5, 2) + "." +
                            creatediso.Substring(0, 4);
            
            // 2 B - convert assuming ISO format 
            var createdDate = DateTimeOffset.Parse(creatediso)
                .ToString("dd.MM.yyyy");

            // scenery01  - to lower 
            var asumedBlitsHexLower = asumedBlitsHex.ToLower();
            var hashToProcess = userLogin + resultFormatted + asumedBlitsHexLower;
            // SHURGOTUV@MAIL.RU13.03.2019377d1910a92318860b137e910b2f28b2
            
            var hashSecBt = Encoding.UTF8.GetBytes(hashToProcess);
            var hashedSecondary = SHA1.HashData(hashSecBt);
            var hsSecRes = Convert.ToHexString(hashedSecondary);
            // B70D83AF26FAB809AEA23DCF1794B20F3D0CB780
            
            
       

            
            
            // scenery02 
            var hsToProcess02 = userLogin + resultFormatted + asumedBlitsHex; 
            // SHURGOTUV@MAIL.RU13.03.2019377D1910A92318860B137E910B2F28B2
            
            var hashSecBt02 = Encoding.UTF8.GetBytes(hsToProcess02);
            var hashedSecondary02 = SHA1.HashData(hashSecBt02);
            var hsSecRes02 = Convert.ToHexString(hashedSecondary02);
            // 29600493DF577C2FBD42EE5BB8B78C76DCD5D96C
            
            
            


            // value in ORACLE
            var orclHash = "940C9067866924A50FC43ACCC2F15B4968DBC32D";
            var orcl64bt = Convert.FromHexString(orclHash);
            var orcle64Str = Convert.ToBase64String(orcl64bt);
            // => value in PG 
            // lAyQZ4ZpJKUPxDrMwvFbSWjbwy0=
            // CORRECT
            // ORACLE to PG path is Clear 

            var bytes = Convert.FromBase64String(orcle64Str);
            var hex = Convert.ToHexString(bytes);
            
            
            
            // value from blitz
            // assumed as 123456 + salt ->
            var blitzHash = "377D1910A92318860B137E910B2F28B2";
            
            // SHA1
            var blitzHashBt = Convert.FromHexString(blitzHash);
            var blitzSha1 = SHA1.HashData(blitzHashBt);

            // to hex string
            var blitzHashExpected = Convert.ToHexString(blitzSha1);
            // 37BD0939E3F881195D62C6AD8A593162055C92EA
            // INCORRECT
            // we expect this value to be as 
            // var orclHash = "940C9067866924A50FC43ACCC2F15B4968DBC32D";
            
            // to 64 string 
            var blitz64bt = Convert.FromHexString(blitzHashExpected);
            var blitz64str = Convert.ToBase64String(blitz64bt);
            // N70JOeP4gRldYsatilkxYgVckuo=
            // INCORRECT
            // this value must be equal to Migrator pg DB 
            // lAyQZ4ZpJKUPxDrMwvFbSWjbwy0=


            
            
            var blitzHashTwo = "377d1910a92318860b137e910b2f28b2";
            
            // SHA1
            var blitzHashTwoBt = Convert.FromHexString(blitzHashTwo);
            var blitzSha1Two = SHA1.HashData(blitzHashTwoBt);

            var blitzHashExpectedTwo = Convert.ToHexString(blitzSha1Two);
            
            
            // alternate flow 
            // first 64 than hex 
            var blitzTo64First = Convert.ToBase64String(blitzSha1);
            var blitzto64Bts = Convert.FromBase64String(blitzTo64First);
            var blitzHex = Convert.ToHexString(blitzto64Bts);
            // 37BD0939E3F881195D62C6AD8A593162055C92EA
            // INCORRECT
            
            var url = ConstantsCheckers.testApiURl2;
        }
    }
}
