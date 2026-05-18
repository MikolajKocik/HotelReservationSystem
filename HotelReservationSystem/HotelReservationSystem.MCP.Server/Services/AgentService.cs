using HotelReservationSystem.MCP.Server.Tools;
using HotelReservationSystem.MCP.Server.Utils;
using OpenAI.Chat;
using System.Text.Json;
using HotelReservationSystem.MCP.Server.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace HotelReservationSystem.MCP.Server.Services;

public record ChatMessageDto(string Role, string Content);

public sealed class AgentService : IAgentService
{
    private readonly ReceptionTools _receptionTools;
    private readonly GuestTools _guestTools;
    private readonly ChatClient _chatClient;
    private readonly string _systemPrompt;
    private readonly List<ChatTool> _tools;
    private readonly IDistributedCache _cache;

    public AgentService(
        ReceptionTools receptionTools,
        GuestTools guestTools,
        ChatClient chatClient,
        IDistributedCache cache
        )
    {
        _guestTools = guestTools;
        _receptionTools = receptionTools;
        _chatClient = chatClient;
        _cache = cache;

        _systemPrompt = McpServerUtils.LoadPromptFromYaml("AuroraBase.yaml");
        _tools = McpServerUtils.GenerateToolsFrom<ReceptionTools>();
        _tools.AddRange(McpServerUtils.GenerateToolsFrom<GuestTools>());
    }

    /// <summary>
    /// Executes the available tools for chatbot by reading the props from Json Document
    /// and implement these parameters to dedicated tool.
    /// </summary>
    /// <param name="toolCall"></param>
    /// <returns>The relevant tool for chat message request</returns>
    private async Task<string> ExecuteToolAsync(ChatToolCall toolCall, CancellationToken cancellationToken)
    {
        try
        {
            using JsonDocument args = JsonDocument.Parse(toolCall.FunctionArguments);
            JsonElement root = args.RootElement;

            return toolCall.FunctionName switch
            {
                "notify_staff" => await _receptionTools.NotifyStaffAsync(
                    root.GetProperty("roomId").GetInt32(),
                    root.GetProperty("message").GetString()!,
                    root.GetProperty("category").GetString()!,
                    cancellationToken),

                "book_room" => await _receptionTools.BookRoomAsync(
                    DateTime.Parse(root.GetProperty("arrival").GetString()!),
                    DateTime.Parse(root.GetProperty("departure").GetString()!),
                    root.GetProperty("roomId").GetInt32(),
                    root.GetProperty("guests").GetInt32(),
                    cancellationToken),

                "search_available_rooms" => await _guestTools.SearchAvailableRoomsAsync(
                    DateTime.Parse(root.GetProperty("arrival").GetString()!),
                    DateTime.Parse(root.GetProperty("departure").GetString()!),
                    root.GetProperty("guests").GetInt32(),
                    cancellationToken),

                "get_my_reservations" => await _guestTools.GetMyReservationsAsync(cancellationToken),

                _ => $"Nieznane narzędzie: {toolCall.FunctionName}"
            };
        }
        catch (Exception ex)
        {
            return $"Błąd wykonania narzędzia {toolCall.FunctionName}: {ex.Message}";
        }
    }

    /// <summary>
    /// Map chat data transfer object to OpenAI chat message class
    /// depends on chat role
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>OpenAI chat message role</returns>
    private ChatMessage MapToOpenAiMessage(ChatMessageDto dto)
    {
        return dto.Role.ToLowerInvariant() switch
        {
            "system" => new SystemChatMessage(dto.Content),
            "assistant" => new AssistantChatMessage(dto.Content),
            "user" => new UserChatMessage(dto.Content),
            _ => new UserChatMessage(dto.Content)
        };
    }
    
    /// <summary>
    /// Process the chat bot answer for user's request
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="message"></param>
    /// <returns>The answer based on user's request</returns>
    public async Task<string> ProcessMessageAsync(Guid sessionId, string message, CancellationToken cancellationToken)
    {
        string cacheKey = $"session_{sessionId}";
        List<ChatMessageDto> historyDto = new();

        string? cachedHistory = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (!string.IsNullOrEmpty(cachedHistory))
        {
            historyDto = JsonSerializer.Deserialize<List<ChatMessageDto>>(cachedHistory) ?? new();
        }
        else
        {
            // new session
            historyDto.Add(new ChatMessageDto("system", _systemPrompt));
        }

        historyDto.Add(new ChatMessageDto("user", message));

        List<ChatMessage> openAiMessages = historyDto.Select(MapToOpenAiMessage).ToList();

        ChatCompletionOptions options = new()
        {
            Temperature = 0.2f,
        };

        foreach (var tool in _tools)
        {
            options.Tools.Add(tool);
        }

        string finalAnswer = "Przepraszam, wystąpił błąd.";
        const int maxToolRounds = 5;

        for (int i = 0; i < maxToolRounds; i++)
        {
            ChatCompletion completion = await _chatClient.CompleteChatAsync(openAiMessages, options, cancellationToken);

            if (completion.FinishReason == ChatFinishReason.ToolCalls)
            {
                openAiMessages.Add(new AssistantChatMessage(completion));

                foreach (ChatToolCall toolCall in completion.ToolCalls)
                {
                    string toolResult = await this.ExecuteToolAsync(toolCall, cancellationToken);
                    openAiMessages.Add(new ToolChatMessage(toolCall.Id, toolResult));
                }
            }
            else
            {
                finalAnswer = completion.Content[0].Text ?? finalAnswer;
                historyDto.Add(new ChatMessageDto("assistant", finalAnswer));
                break; 
            }
        }

        // cutting questions for context window and avoid memory-leak
        if (historyDto.Count > 15)
        {
            historyDto.RemoveRange(1, 2); 
        }

        var cacheOptions = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(60)
        };

        string updatedHistoryJson = JsonSerializer.Serialize(historyDto);
        await _cache.SetStringAsync(cacheKey, updatedHistoryJson, cacheOptions, cancellationToken);

        return finalAnswer;
    }    
}
