// Domain Logging specific strings 

using CoreSBBL.Logging.Models.DAL.TS;

namespace CoreSBBL.Logging
{
    public class DefaultConfigurationValues
    {
        public static string DefaultElasticIndex => "logging";
    }

    public class DefaultModelValues
    {
        /// <summary>
        ///     Strings for labels and tags
        /// </summary>
        public class Logging
        {
            public static string LoggingLabelDefault => "Default";
            public static string LoggingLabelError => "Error";

            public static string LoggingSystemTag => "System";

            public static string MessageEmpty => "empty";
        }

        /// <summary>
        ///     Tags themselves
        /// </summary>
        public class LoggingTags
        {
            public static LogsTagDALEfTc Default => new() {Index = 1, Text = Logging.LoggingLabelDefault};
            public static LogsTagDALEfTc System => new() {Index = 2, Text = Logging.LoggingSystemTag};
        }
    }
}
