using System.Diagnostics.CodeAnalysis;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Shouldly;

namespace AISemanticKernel;

[TestFixture]
public class OllamaWithAzureOpenAILibraryTester
{
    [Test]
    public void ShouldCallOllamaButUseOpenAIChatCompletion()
    {
        var handler = new CustomHttpMessageHandler();
        handler.CustomLlmUrl = "http://localhost:11434";
        var client = new HttpClient(handler);

        var endpoint = "http://localhost:11434";
        var model = "llama3.2";
        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion(model, "api-key", httpClient:client);
        var kernel = builder.Build();

        var result = kernel.InvokePromptAsync("what color is the sky during the day when there are no clouds? Single word, no punctuation");
        Console.WriteLine(result.Result);
        result.Result.ToString().ToLower().ShouldBe("blue");
    }

    public class CustomHttpMessageHandler : HttpClientHandler
    {
        public string CustomLlmUrl { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string[] urls = { "api.openai.com", "openai.azure.com" };

            // validate if request.RequestUri is not null and request.RequestUri.Host is in urls
            if (request.RequestUri != null && urls.Contains(request.RequestUri.Host))
            {
                // set request.RequestUri to a new Uri with the LLMUrl and request.RequestUri.PathAndQuery
                request.RequestUri = new Uri($"{CustomLlmUrl}{request.RequestUri.PathAndQuery}");
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}