using CoreSBShared.Registrations;
using Google.Cloud.SecretManager.V1;
using Microsoft.Extensions.Options;

namespace CoreSBShared.Universal.Infrastructure.Clouds
{
    public class GoogleCloud
    {
        private readonly string? projectId;
        private readonly string? secretId;

        public GoogleCloud(IOptions<GoogleCloudOptions> options)
        {
            var o = options.Value;
            projectId = o.ProjectId;
            secretId = o.SecretId;
        }

        /// <summary>Uses ProjectId and SecretId from configuration (IOptions).</summary>
        public string GetApiKey()
        {
            if (string.IsNullOrWhiteSpace(projectId) || string.IsNullOrWhiteSpace(secretId))
                throw new InvalidOperationException(
                    "Google Cloud ProjectId and SecretId must be set under configuration section Clouds:Google.");

            return GetApiKey(projectId, secretId);
        }

        public static string GetApiKey(string projectId, string secretId)
        {
            var client = SecretManagerServiceClient.Create();
            var secretVersionName = new SecretVersionName(projectId, secretId, "latest");

            var result = client.AccessSecretVersion(secretVersionName);
            string apiKey = result.Payload.Data.ToStringUtf8();
            return apiKey;
        }
    }
}
