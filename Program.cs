using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    private static readonly string apiKey = "YOUR_API_KEY";
    private static readonly string apiUrl = "https://api.openai.com/v1/chat/completions";

    static async Task Main(string[] args)
    {
        

        Console.WriteLine("Hello, my name is Patty, I can answer anything that relates to plastics recycling and ocean conscious consumption!\n\nWhat can I answer for you?");
        var question = Console.ReadLine();
        //var response = await GetCompletionAsync(prompt);
        Console.Clear()
        Console.WriteLine("What city would be easiest to find a recycling center in for you?");
        var city = Console.ReadLine();

        prompt = question + " and find recycling centers located in " + city + ", USA."
        
        var response = await GetCompletionAsync(prompt);

        Console.Write("Hmm");
        await Task.Delay(800);
        Console.Write(".");
        await Task.Delay(800);
        Console.Write(".");
        await Task.Delay(800);
        Console.Write(".");
        await Task.Delay(800);
        Console.Write(".");
        await Task.Delay(800);
        Console.Write(".");
        Console.WriteLine();

        

        Console.WriteLine(response);
    }

    // Actual request handler. Token limit set to 350.
    static async Task<string> GetCompletionAsync(string prompt)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant named Patty. You are knowledgeable about plastics, recycling, ocean conscious consumerism, eco-consumerism, and crafts and improvised tools made from repurposed single use plastics. You are encouraged to search the internet and supply the user with supplementary links to additional information. If you feel the user is asking questions outside the scope of what is described, you should refrain from answering and remind them of your area of expertise." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 350
            };

            var jsonRequestBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonDocument.Parse(jsonResponse);
                return parsedResponse.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();
            }
            else
            {
                return $"Error: {response.StatusCode}";
            }
        }
    }
}
