using System;
using CoreSBBL.Logging.Infrastructure.EF;
using CoreSBBL.Logging.Infrastructure.Generic;
using CoreSBBL.Logging.Infrastructure.GN;
using CoreSBBL.Logging.Infrastructure.TS;
using CoreSBBL.Logging.Infrastructure.Mongo;
using CoreSBBL.Logging.Services;
using CoreSBShared.Registrations;
using CoreSBShared.Universal.Infrastructure.EF;
using CoreSBShared.Universal.Infrastructure.EF.Store;
using CoreSBShared.Universal.Infrastructure.EF.Stores;
using CoreSBShared.Universal.Infrastructure.HTTP;
using CoreSBShared.Universal.Infrastructure.HTTP.MyApp.Services.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoreSBBL
{
    public static class RegistrationsBL
    {
        /// <summary>
        /// Register contexts for database
        /// </summary>
        public static void RegisterContextsBL(this WebApplicationBuilder builder)
        {
            
            RegisterEFContextsGC(builder);
            RegisterEFContextsTC(builder);
            RegisterEFContextsGenric(builder);
            
            RegisterMongoContexts(builder);
            RegisterElasticContexts(builder);
        }

        /// <summary>
        /// Register stores, services
        /// </summary>
        public static void RegisterServicesBL(this WebApplicationBuilder builder)
        {
            RegisterEFStoresBL(builder);
            RegisterMongoStores(builder);
            RegisterServices(builder);
        }
        
        /// <summary>
        ///     Register db contexts
        /// </summary>
        internal static void RegisterEFContextsTC(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<LogsContextTC>(options =>

                //options.UseSqlServer(ConnectionsRegister.Connections.MSSQL));
                options.UseSqlServer(ConnectionsRegister.Connections.MSSQL));
            builder.Services.AddScoped<DbContext, LogsContextTC>();

        }
        internal static void RegisterEFContextsGC(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<LogsContextGN>(options =>

                options.UseSqlServer(ConnectionsRegister.Connections.MSSQL));
                // options.UseSqlServer(ConnectionsRegister.Connections.MSSQL));
            builder.Services.AddScoped<DbContext, LogsContextGN>();

        }
        internal static void RegisterEFContextsGenric(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<LogsContextGeneric>(options =>
                options.UseSqlServer(ConnectionsRegister.Connections.MSSQL));
            
            //options.UseSqlServer(ConnectionsRegister.Connections.DOCKERMSSQL));
            builder.Services.AddScoped<IEFStoreGK<LogsContextGeneric>, EFStoreGK<LogsContextGeneric>>();

            builder.Services.AddDbContext<LogsContextGeneric2>(options =>
            options.UseSqlServer(ConnectionsRegister.Connections.MSSQL));
            
            //options.UseSqlServer(ConnectionsRegister.Connections.DOCKERMSSQL));
            builder.Services.AddScoped<IEFStoreGK<LogsContextGeneric2>, EFStoreGK<LogsContextGeneric2>>();
            
            // test context
            builder.Services.AddDbContext<TestContext>(o=>
                o.UseSqlServer(ConnectionsRegister.Connections.MSSQL));
            builder.Services.AddScoped<IEFStoreGeneric<TestContext>, EFStoreGeneric<TestContext>>();
        }
        
        

        internal static void RegisterEFStoresBL(this WebApplicationBuilder builder)
        {
            // Interface for LogsEFStore
            builder.Services.AddScoped<ILogsEFStore, LogsEFStore>();

            // Interface for LogsEFStoreG<T, K>
            builder.Services.AddScoped(typeof(ILogsEFStoreG<,>), typeof(LogsEFStoreG<,>));
            
            builder.Services.AddScoped<ITestStore, TestStore>();

            // Interface for LogsEFStoreGInt
            builder.Services.AddScoped<ILogsEFStoreGInt, LogsEFStoreGInt>();

            // Interface for LogsEFStoreG
            builder.Services.AddScoped<ILogsEFStoreG, LogsEFStoreG>();
        }

        internal static void RegisterMongoStores(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ILoggingMongoStore>(s =>
                new LoggingMongoStore(ConnectionsRegister.MongoConnection.ConnectionString,
                    ConnectionsRegister.MongoConnection.DatabaseName));
        }

        internal static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ILoggingServiceNew, LoggingService>();
            
            builder.Services.AddScoped<ILogsServiceGeneric, LogsServiceGeneric>();
            

            builder.Services.AddHttpClient<IHttpService, HttpService>();
            builder.Services.AddScoped<IHttpService, HttpService>();
        }

        internal static void RegisterMongoContexts(this WebApplicationBuilder builder)
        {
        }

        /// Register db contexts
        /// </summary>
        internal static void RegisterElasticContexts(this WebApplicationBuilder builder)
        {
        }

     
    }
}
