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
using InfrastructureCheckers;
using InfrastructureCheckers.Collections;
using LINQtoObjectsCheck;
using LiveCodingPrep;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualBasic;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Linq;
using Nest;
using Id = Elastic.Clients.Elasticsearch.Id;
using System.Collections.Concurrent;

namespace Live
{
    public class LiveCheck
    {
        public static void GO()
        {
            var url = ConstantsCheckers.testApiURl2;
        }

        public void Blocked()
        {
            var item = SomeAsync().Result;
            var fix = SomeAsync().GetAwaiter().GetResult();
        }
        public async Task<int> SomeAsync()
        {
            await Task.Delay(1);
            return 1;
        }

    }

}
