using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Elastic.Clients.Elasticsearch.Cluster;

namespace InfrastructureCheckers.Review
{
    
    public class ReportService
    {
        public async Task<string> GenerateReport(int reportId)
        {
            var data = await LoadData(reportId);
            var formatted = FormatData(data);
            var pdf = await GeneratePdf(formatted);
            SaveToDisk(pdf, $"report_{reportId}.pdf");
            return $"report_{reportId}.pdf";
        }

        private async Task<List<string>> LoadData(int id)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://external.api/reports/{id}");
            return JsonSerializer.Deserialize<List<string>>(await response.Content.ReadAsStringAsync());
        }

        private string FormatData(List<string> data)
        {
            return string.Join(",", data);
        }

        private async Task<byte[]> GeneratePdf(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        private void SaveToDisk(byte[] bytes, string fileName)
        {
            File.WriteAllBytes(fileName, bytes);
        }
    }
    
    public class ReportingServiceRev {
        private string _mainUrl = $"https://external.api/reports";
        private string _getUrl(int id) => $"https://external.api/reports/{id}";

        public DataService ds = new DataService();
        public UtilityService us = new UtilityService();
        public IOservice ioS = new IOservice();
        
        public async Task<string> GenerateReport(int reportId)
        {
            var ct = new CancellationToken();
            using var _client = new HttpClient();
            var data = await ds.LoadData(_getUrl(reportId), _client, ct);
            var formatted = us.FormatData(data);
            var pdf = await us.GeneratePdfAsync(formatted);
            await ioS.SaveToDiskAsync($"report_{reportId}.pdf", pdf);
            return $"report_{reportId}.pdf";
        }
    }

    public class DataService
    {
        public async Task<List<string>> LoadData(string url, HttpClient _client, CancellationToken ct)
        {
            var resp = await _client.GetAsync(url, ct);
            if (resp == null || resp?.Content == null)
                return new List<string>();

            var result = await resp?.Content?.ReadAsStringAsync();
            var ret = JsonSerializer.Deserialize<List<string>>(result);
            return ret;
        }
    }
    public class UtilityService
    {
        public string FormatData(List<string> data)
        {
            return string.Join(",", data);
        }
        public Task<byte[]> GeneratePdfAsync(string data)
        {
            return Task.FromResult(Encoding.UTF8.GetBytes(data));
        }
    }
    public class IOservice
    {
        public async Task SaveToDiskAsync(string filename, byte[] bytes)
        {
            await File.WriteAllBytesAsync(filename, bytes);
        }
    }

}
