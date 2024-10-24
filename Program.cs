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
        
        Console.Clear();
        Console.WriteLine("Hello, my name is Patty, I can answer anything that relates to plastics recycling and ocean conscious consumption!\n\nWhat kind of question can I answer for you?");

        Console.WriteLine("\t1.\tRepurosing / Recycling Plastics\n\t2.\tGeneral Question");
        Console.Write("> ");

        int choice;

        choice = int.Parse(Console.ReadLine());

        while(choice!=1&&choice!=2){
            Console.Clear();
            Console.WriteLine("Invalid Input!");
            Console.WriteLine("Select a Question Type:\n\t1.\tRepurposing / Recycling Plastics\n\t2.\tGeneral Question");
            
            choice = int.Parse(Console.ReadLine());
        }

        string question;
        string prompt = "Tell me about the Ocean Health sustainable development goal";
        string city;

        switch(choice){
            case 1:
                Console.Clear();
                Console.WriteLine("Please describe what sort of plastic materials you want to try and repurpose:\n");
                string materials = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("What city would be easiest to find a recycling center in for you?");
                city = Console.ReadLine();
                prompt = "Describe ways with links to examples on how to repurpose " + materials + " and find recycling centers and their operating hours located in " + city + ", USA.";
                Console.Clear();
                break;
            case 2:
                Console.Clear();
                Console.WriteLine("Enter your plastic or ocean health realted question below:\n");
                question = Console.ReadLine();
                prompt = "Answer and find relevant supplementary information to answer the following question: "  + question;
                Console.Clear();
                break;
        }

        
        
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
