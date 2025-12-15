using System.Text;
using System.Text.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace sedatodev1.Services
{
    public class GeminiAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiAiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"];
        }

        public async Task<string> HaftalikPlanOlusturAsync(
            int boy,
            int kilo,
            string vucutTipi,
            string hedef)
        {
            var url =
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

            // API KEY HEADER
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);

            var prompt = $@"
Boyu {boy} cm, kilosu {kilo} kg olan,
vücut tipi {vucutTipi} ve hedefi {hedef} olan bir kişi için:

1) 7 günlük haftalık spor programı oluştur.
2) 7 gün için her gün Kahvaltı, Öğle, Akşam olacak şekilde beslenme planı oluştur.
3) Sadece metin üret, başlıkları belirgin yaz.
";

            // ✅ GEMINI 2.5 İÇİN DOĞRU BODY
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(url, jsonContent);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            // ✅ GÜVENLİ PARSE (TEXT VARSA AL)
            using var doc = JsonDocument.Parse(responseJson);

            var parts = doc
                .RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts");

            var sb = new StringBuilder();

            foreach (var part in parts.EnumerateArray())
            {
                if (part.TryGetProperty("text", out var textElement))
                {
                    var text = textElement.GetString();
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        sb.AppendLine(text);
                    }
                }
            }

            var sonuc = sb.ToString().Trim();

            return string.IsNullOrEmpty(sonuc)
                ? "Yapay zekâ yanıt verdi ancak metin bulunamadı."
                : sonuc;
        }

        // MODEL LİSTELEME
        public async Task<string> ModelListeleAsync()
        {
            var url = "https://generativelanguage.googleapis.com/v1beta/models";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);

            var res = await _httpClient.GetAsync(url);
            var body = await res.Content.ReadAsStringAsync();

            return $"STATUS={(int)res.StatusCode} {res.ReasonPhrase}\n\n{body}";
        }
    }
}
