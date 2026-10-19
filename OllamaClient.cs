using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BugTrackerApp
{
    /// <summary>
    /// Helper for calling a local Ollama /api/generate endpoint and processing clean NDJSON streams.
    /// </summary>
    public sealed class OllamaClient : IDisposable
    {
        private readonly HttpClient _http;
        private readonly string _endpoint;
        private readonly bool _disposeHttp;

        public OllamaClient(HttpClient? httpClient = null, string? endpoint = null)
        {
            // Default setup fallback logic
            _endpoint = string.IsNullOrWhiteSpace(endpoint) ? "http://localhost:11434/api/generate" : endpoint!;
            if (httpClient is null)
            {
                _http = new HttpClient();
                _disposeHttp = true;
            }
            else
            {
                _http = httpClient;
                _disposeHttp = false;
            }
        }

        /// <summary>
        /// Send a prompt request to Ollama /api/generate. Reads streaming responses line-by-line,
        /// extracts the clean 'response' tokens safely, and ignores structural JSON errors.
        /// </summary>
        public async Task<string> SendChatAsync(string userQuery, string model = "llama3.2", string? bearerToken = null, CancellationToken cancellationToken = default, IProgress<string>? onMessage = null)
        {
            if (string.IsNullOrWhiteSpace(userQuery))
                return string.Empty;

            // Hardcode the endpoint path to map perfectly to your system UI selection
            var endpoint = "http://localhost:11434/api/generate";

            // Inject engineering system identity instructions seamlessly into the text payload block
            string systemInstruction = "You are an assistant that recommends the first actions a developer or support engineer should take when they encounter a bug. Prioritize only necessary and high-impact steps. Keep responses concise.";
            string structuredPrompt = $"System instruction: {systemInstruction}\nUser request: {userQuery}\nAssistant response:";

            // Build payload using the verified "prompt" shape with streaming true
            var payload = new { model = model, prompt = structuredPrompt, stream = true };
            var json = JsonSerializer.Serialize(payload);

            using var req = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrWhiteSpace(bearerToken))
            {
                try { req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken); } catch { }
            }

            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromSeconds(60));

                using var response = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cts.Token).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync(cts.Token).ConfigureAwait(false);
                    return $"Ollama request failed ({(int)response.StatusCode}): {err}";
                }

                await using var stream = await response.Content.ReadAsStreamAsync(cts.Token).ConfigureAwait(false);
                using var reader = new StreamReader(stream, Encoding.UTF8);

                var sb = new StringBuilder();

                // Bulletproof loop: skips raw parsing anomalies silently to preserve clean UI logs
                while (!reader.EndOfStream && !cts.IsCancellationRequested)
                {
                    var line = await reader.ReadLineAsync().ConfigureAwait(false);
                    if (line is null) break;
                    line = line.Trim();
                    if (line.Length == 0) continue;

                    if (line.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                        line = line.Substring(5).Trim();

                    if (line == "[DONE]" || line.Equals("done", StringComparison.OrdinalIgnoreCase))
                        break;

                    try
                    {
                        using var doc = JsonDocument.Parse(line);
                        var root = doc.RootElement;

                        // Extract target root-level string property explicitly sent by /api/generate
                        if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("response", out var resp) && resp.ValueKind == JsonValueKind.String)
                        {
                            var piece = resp.GetString();
                            if (!string.IsNullOrEmpty(piece))
                            {
                                try { onMessage?.Report(piece); } catch { }
                                sb.Append(piece);
                            }
                        }
                    }
                    catch (JsonException)
                    {
                        // Completely skip malformed or fractional data packets instead of polluting text boxes
                        continue;
                    }
                }

                return sb.ToString();
            }
            catch (OperationCanceledException)
            {
                return "Ollama request timed out or was cancelled.";
            }
            catch (Exception ex)
            {
                return $"OllamaClient error: {ex.Message}";
            }
        }

        public void Dispose()
        {
            if (_disposeHttp) _http.Dispose();
        }
    }
}
