namespace CoreSBShared.Registrations
{
    /// <summary>
    ///     App settings sections and keys string constants
    /// </summary>
    public class RegistrationStrings
    {
        public static string ConnectionsSectionName => "Connections";

        public static string MSSQL => "MSSQL";
        public static string MSSQLLOCAL => "MSSQLLOCAL";
        public static string DOCKERMSSQL => "DOCKERMSSQL";
        public static string AZUREMSSQL => "AZUREMSSQL";


        public static string MongoSectionName => "Mongo";
        public static string MongoConnetionsString => "ConnectionString";
        public static string DatabaseName => "DatabaseName";


        public static string ElasticSectionName => "Elastic";
        public static string ElasticConnetionsString => "ConnectionString";
        
        public static string RabbitSectionName => "Rabbit";
    }

    public class DefaultConfigurationValues
    {
        public static string DefaultElasticConnStr => "http://localhost:9200";
        public static string DefaultElasticIndex => "logging";
    }
}
