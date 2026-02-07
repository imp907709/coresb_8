using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using CoreSBShared.Universal.Checkers.Threading;

using InfrastructureCheckers.IGS;

namespace Live
{
    public class LiveCheck
    {
        public static async Task GO()
        {
            LINQcheck.GO();
            
            HashConversionsIGS.GO();
        }
    }
}
