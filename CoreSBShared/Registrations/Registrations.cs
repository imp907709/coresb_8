using CoreSBShared.Universal.Infrastructure.EF;
using CoreSBShared.Universal.Infrastructure.Elastic;
using CoreSBShared.Universal.Infrastructure.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreSBShared.Registrations
{
    /// <summary>
    ///     Startup registrations, builder, app
    /// </summary>
    public static class Registrations
    {
        /// <summary>
        ///     Register connections from app settings to register with option pattern
        /// </summary>
        public static void RegisterConnections(this WebApplicationBuilder builder)
        {
            builder.Configuration.GetSection(Connections.SectionName).Bind(ConnectionsRegister.Connections);
            builder.Configuration.GetSection(MongoConnection.SectionName).Bind(ConnectionsRegister.MongoConnection);
            builder.Configuration.GetSection(ElasticConenction.SectionName).Bind(ConnectionsRegister.ElasticConenction);
        }



        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            //RegisterEFStores(builder);
        }

        public static void RegisterContexts(this WebApplicationBuilder builder)
        {
            //RegisterEFContexts(builder);
            RegisterMongoContexts(builder);
            RegisterElasticContexts(builder);
        }

        /// <summary>
        ///     Register db contexts
        /// </summary>
        internal static void RegisterEFContexts(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IEFStore, EFStore>();
        }

        internal static void RegisterMongoContexts(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IMongoStore>(s =>
                new MongoStore(ConnectionsRegister.MongoConnection.ConnectionString,
                    ConnectionsRegister.MongoConnection.DatabaseName));
        }

        /// Register db contexts
        /// </summary>
        internal static void RegisterElasticContexts(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IElasticStoreNest>(p =>
            {
                return new ElasticStoreNest(null, null);
            });
        }

        internal static void RegisterEFStores(this WebApplicationBuilder builder)
        {
                // Method level store, type containing id
                builder.Services.AddScoped<IEFStore, EFStore>();

                // Class level store generic id
                builder.Services.AddScoped(typeof(IEFStore<,>), typeof(EFStoreG<,>));

                // Class level store generic id int
                builder.Services.AddScoped<IEFStoreInt, EFStoreGInt>();

                // Method level store generic id int
                builder.Services.AddScoped<IEFStoreG, EFStoreG>();
        }

        public static void FrameworkRegistrations(this WebApplicationBuilder builder)
        {
            // Add services to the container.

            builder.Services.AddCors();
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
        
        public static void Registration(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
