namespace CoreSBShared.Registrations
{
    /// <summary>
    ///     Models for settings with option pattern registration
    /// </summary>
    public class Connections
    {
        public static string SectionName => RegistrationStrings.ConnectionsSectionName;

        public string? MSSQL { get; set; }
        public string? MSSQLLOCAL { get; set; }
        public string? DOCKERMSSQL { get; set; }
        public string? AZUREMSSQL { get; set; }
    }

    public class MongoConnection
    {
        public static string SectionName => RegistrationStrings.MongoSectionName;
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public class ElasticConenction
    {
        public static string SectionName => RegistrationStrings.ElasticSectionName;
        public string? ConnectionString { get; set; }
        public string? DefaultIndex { get; set; }
    }

    public class RabbitConfig
    {
        public static string SectionName => RegistrationStrings.RabbitSectionName;
        
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
    
    /// <summary>
    ///     Connections and vars from app settings register for option pattern
    /// </summary>
    public static class ConnectionsRegister
    {
        public static Connections Connections { get; set; } = new();
        public static MongoConnection MongoConnection { get; set; } = new();
        public static ElasticConenction ElasticConenction { get; set; } = new();
        public static RabbitConfig RabbitConfig { get; set; } = new();
    }

    public class GoogleCloudOptions
    {
        public static string SectionName => RegistrationStrings.GoogleCloudSectionName;

        public string? ProjectId { get; set; }
        public string? SecretId { get; set; }
    }

    public class GoogleGeoApiOptions
    {
        public static string SectionName => RegistrationStrings.GoogleGeoApiSectionName;

        /// <summary>Geocoding API JSON endpoint; appsettings overrides this default.</summary>
        public string GeocodeEndpoint { get; set; } =
            "https://maps.googleapis.com/maps/api/geocode/json";

        /// <summary>Address for the smoke-test call; appsettings overrides this default.</summary>
        public string TestAddress { get; set; } =
            "1600 Amphitheatre Parkway, Mountain View, CA";
    }
}
