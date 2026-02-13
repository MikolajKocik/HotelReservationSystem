using HotelReservationSystem.MCP.Server.Tools;
using HotelReservationSystem.MCP.Server.Utils;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System.Text.Json;
using HotelReservationSystem.MCP.Server.Interfaces;

namespace HotelReservationSystem.MCP.Server.Services;

public class AgentService : IAgentService
{
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

    public async Task<string> ProcessMessageAsync(string message)
    {
        List<ChatMessage> messages = new()
        {
            new SystemChatMessage(this.systemPrompt),
            new UserChatMessage(message)
        };

        ChatCompletionOptions options = new()
        {
            Temperature = 0.7f,
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
                return completion.Content[0].Text
                    ?? "Przepraszam, nie udało mi się przetworzyć odpowiedzi.";
            }
        }

        return "Przepraszam, wystąpił problem z przetwarzaniem zapytania. Spróbuj ponownie.";
    }

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
