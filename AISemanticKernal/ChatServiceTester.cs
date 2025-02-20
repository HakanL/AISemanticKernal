using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAI;
using Shouldly;

namespace AISemanticKernel;

public class ChatServiceTester
{
    private readonly string _endpoint = EnvironmentVariable.AI_OpenAI_Url.Get();
    private readonly string _apiKey = EnvironmentVariable.AI_OpenAI_ApiKey.Get();
    private readonly string _model = EnvironmentVariable.AI_OpenAI_Model.Get();

    [Test]
    public void ShouldCallAzureChatService()
    {
        IChatCompletionService chatService = new AzureOpenAIChatCompletionService(
            _model, _endpoint, _apiKey);
        var result = chatService.GetChatMessageContentAsync(
            "what color is the sky during the day when there are no clouds? Single word, no punctuation");
        Console.WriteLine(result.Result);
        result.Result.ToString().ToLower().ShouldBe("blue");
    }

    [Test]
    public void ShouldCallOpenAIUsingSemanticKernel()
    {
        var client = new AzureOpenAIClient(new Uri(_endpoint),
            new ApiKeyCredential(_apiKey));
        var chatClient = client.GetChatClient(_model);
        var result = chatClient.CompleteChat("What are your first two capabilities? Answer in JSON format");
        foreach (var part in result.Value.Content)
        {
            Console.Write(part.Text);
        }
    }

    [Test]
    public void ShouldCallOpenAIWithShortAnswer()
    {
        var client = new AzureOpenAIClient(new Uri(_endpoint),
            new ApiKeyCredential(_apiKey));
        var chatClient = client.GetChatClient(_model);
        var result = chatClient.CompleteChat("What are your first two capabilities? Answer in JSON format");
        foreach (var part in result.Value.Content)
        {
            Console.Write(part.Text);
        }
    }

    [Test]
    public async Task AzureCodeTest()
    {

        async Task RunAsync()
        {
            // Retrieve the OpenAI endpoint from environment variables
            var endpoint = _endpoint;
            if (string.IsNullOrEmpty(endpoint))
            {
                Console.WriteLine("Please set the AI_OpenAI_Url environment variable.");
                return;
            }

            var key = _apiKey;
            if (string.IsNullOrEmpty(key))
            {
                Console.WriteLine("Please set the AI_OpenAI_ApiKey environment variable.");
                return;
            }

            AzureKeyCredential credential = new AzureKeyCredential(key);

            // Initialize the AzureOpenAIClient
            AzureOpenAIClient azureClient = new(new Uri(endpoint), credential);

            // Initialize the ChatClient with the specified deployment name
            ChatClient chatClient = azureClient.GetChatClient(_model);

            // Create a list of chat messages
            var messages = new List<ChatMessage>
          {
              new SystemChatMessage("You are an AI assistant that helps people find information."),
              new UserChatMessage("What are 3 things to visit in Seattle?")
          };


            // Create chat completion options
            var options = new ChatCompletionOptions
            {
                Temperature = (float)0.7,
                MaxOutputTokenCount = 800,

                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };

            
                // Create the chat completion request
                ChatCompletion completion = await chatClient.CompleteChatAsync(messages, options);

                // Print the response
                if (completion.Content != null && completion.Content.Count > 0)
                {
                    Console.WriteLine($"{completion.Content[0].Kind}: {completion.Content[0].Text}");
                }
                else
                {
                    Console.WriteLine("No response received.");
                }
            
        }

        await RunAsync();
    }
}