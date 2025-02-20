using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.ChatCompletion;
using OllamaSharp;
using OllamaSharp.Models.Chat;
using Shouldly;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.Connectors.Ollama;
using ChatRole = OllamaSharp.Models.Chat.ChatRole;

namespace AISemanticKernel
{
    public class OllamaApiClientTester
    {
        private readonly string _endpoint = EnvironmentVariable.AI_Ollama_Url.Get();
        private readonly string _model = EnvironmentVariable.AI_Ollama_Model.Get();

        [Test]
        [Experimental("SKEXP0001")]
        public void SemanticKernelToOllama()
        {
            IChatCompletionService chatService =
                new OllamaApiClient(new Uri(_endpoint), _model).AsChatCompletionService();

            var result = chatService.GetChatMessageContentAsync(
                "what color is the sky during a sunny day? One word answer with no punctuation");
            Console.WriteLine(result.Result);
            result.Result.ToString().ToLower().ShouldBe("blue");
        }

        [Test]
        public void ShouldCallLocalLlm()
        {
            var ollamaApiClient = new OllamaApiClient(_endpoint);
            var chatRequest = new ChatRequest
            {
                Model = _model,
                Messages =
                [
                    new Message(){Content = "What are your capabilities? Only list 3? use json response only", Role = ChatRole.User}
                ]
            };


            var chatResponse = ollamaApiClient.ChatAsync(chatRequest);

            foreach (var message in chatResponse.ToBlockingEnumerable())
            {
                Console.Write(message!.Message.Content);
            }
        }

        [Test, Repeat(5), CancelAfter(20000)]
        [Experimental("SKEXP0001")]
        public void SemanticKernelToOllamaMultiTest()
        {
            IChatCompletionService chatService =
                new OllamaApiClient(new Uri(_endpoint), _model).AsChatCompletionService();

            var result = chatService.GetChatMessageContentAsync(
                "what color is the sky during a sunny day? One word answer with no punctuation");
            Console.WriteLine(result.Result);
            result.Result.ToString().ToLower().ShouldBe("blue");
        }
    }
}
