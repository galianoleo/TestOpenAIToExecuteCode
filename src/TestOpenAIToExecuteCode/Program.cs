#pragma warning disable SKEXP0001, SKEXP0003, SKEXP0010, SKEXP0011, SKEXP0050, SKEXP0052


using Microsoft.SemanticKernel;

using Microsoft.SemanticKernel.ChatCompletion;

using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Embeddings;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using TestOpenAIToExecuteCode;

var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json", optional: false, reloadOnChange: true)
            .AddJsonFile("config.Development.json", optional: true)
            .Build();


var builder = Kernel.CreateBuilder();

var endpoint = configuration["endpoint"];
var apiKey = configuration["apiKey"];
builder.AddAzureOpenAIChatCompletion(
deploymentName: "gpt-4o",
endpoint: endpoint,
apiKey: apiKey);
builder.Plugins.AddFromType<UserService>("user_info");
var kernel = builder.Build();

var chatService = kernel.GetRequiredService<IChatCompletionService>();

ChatHistory history = new ChatHistory();


history.AddUserMessage(@"Hola, mi nombre es Leonardo y mi apellido es Galiano, mi email es leonardo@galiano.com y nací el 31/01/1987");

var result = await chatService.GetChatMessageContentAsync(
    history,
    executionSettings: new OpenAIPromptExecutionSettings()
    {
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
        //ToolCallBehavior = ToolCallBehavior.EnableKernelFunctions,
    },
    kernel: kernel);

Console.WriteLine(result.Items[0].ToString());

// Add the message from the agent to the chat history

history.AddAssistantMessage(result.Items[0].ToString());
Console.ReadLine();


