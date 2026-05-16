using HotelReservationSystem.MCP.Server.Tools;
using HotelReservationSystem.MCP.Server.Utils;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System.Text.Json;
using HotelReservationSystem.MCP.Server.Interfaces;
using System.Collections.Concurrent;
using OpenAI.Assistants;

namespace HotelReservationSystem.MCP.Server.Services;

public class AgentService : IAgentService
{
    private static readonly ConcurrentDictionary<Guid, List<ChatMessage>> _activeSessions = new();
    private readonly ReceptionTools receptionTools;
    private readonly ChatClient chatClient;
    private readonly string systemPrompt;
    private readonly List<ChatTool> tools;

    public AgentService(ReceptionTools receptionTools, IConfiguration configuration)
    {
        this.receptionTools = receptionTools;
        this.systemPrompt = McpServerUtils.LoadPromptFromYaml("AuroraBase.yaml");

        this.tools = McpServerUtils.GenerateToolsFrom<ReceptionTools>();

        string apiKey = configuration["OpenAI:ApiKey"] 
            ?? throw new InvalidOperationException("OpenAI Key not found");
        string model = configuration["OpenAI:Model"] ?? "gpt-4o-mini";

        this.chatClient = new ChatClient(model, apiKey);
    }
    
    /// <summary>
    /// Process the chat bot answer for user's request
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="message"></param>
    /// <returns>The answer based on user's request</returns>
    public async Task<string> ProcessMessageAsync(Guid sessionId, string message)
    {
        if (!_activeSessions.TryGetValue(sessionId, out List<ChatMessage>? messages))
        {
            messages = new List<ChatMessage>
            {
                new SystemChatMessage(this.systemPrompt)
            };
            _activeSessions[sessionId] = messages;
        }

        if (messages.Count > 15)
        {
            messages.RemoveRange(1, 2); 
        }

        messages.Add(new UserChatMessage(message));

        ChatCompletionOptions options = new()
        {
            Temperature = 0.2f,
        };

        foreach (var tool in this.tools)
        {
            options.Tools.Add(tool);
        }

        const int maxToolRounds = 5;

        for (int i = 0; i < maxToolRounds; i++)
        {
            ChatCompletion completion = await this.chatClient.CompleteChatAsync(messages, options);

            if (completion.FinishReason == ChatFinishReason.ToolCalls)
            {
                messages.Add(new AssistantChatMessage(completion));

                foreach (ChatToolCall toolCall in completion.ToolCalls)
                {
                    string toolResult = await this.ExecuteToolAsync(toolCall);
                    messages.Add(new ToolChatMessage(toolCall.Id, toolResult));
                }
            }
            else
            {
                string finalAnswer = completion.Content[0].Text ?? "Przepraszam, błąd przetwarzania.";
                messages.Add(new AssistantChatMessage(finalAnswer));

                return finalAnswer;
            }
        }

        return "Przepraszam, wystąpił problem z przetwarzaniem zapytania. Spróbuj ponownie.";
    }

    /// <summary>
    /// Executes the available tools for chatbot by reading the props from Json Document
    /// and implement these parameters to dedicated tool.
    /// </summary>
    /// <param name="toolCall"></param>
    /// <returns>The relevant tool for chat message request</returns>
    private async Task<string> ExecuteToolAsync(ChatToolCall toolCall)
    {
        try
        {
            using JsonDocument args = JsonDocument.Parse(toolCall.FunctionArguments);
            JsonElement root = args.RootElement;

            return toolCall.FunctionName switch
            {
                "notify_staff" => await this.receptionTools.NotifyStaffAsync(
                    root.GetProperty("message").GetString()!,
                    root.GetProperty("category").GetString()!),

                "book_room" => await this.receptionTools.BookRoomAsync(
                    DateTime.Parse(root.GetProperty("arrival").GetString()!),
                    DateTime.Parse(root.GetProperty("departure").GetString()!),
                    root.GetProperty("roomId").GetInt32(),
                    root.GetProperty("guests").GetInt32()),

                _ => $"Nieznane narzędzie: {toolCall.FunctionName}"
            };
        }
        catch (Exception ex)
        {
            return $"Błąd wykonania narzędzia {toolCall.FunctionName}: {ex.Message}";
        }
    }
}
