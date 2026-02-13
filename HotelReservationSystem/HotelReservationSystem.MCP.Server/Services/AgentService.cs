using HotelReservationSystem.MCP.Server.Tools;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System.Reflection;
using System.Text.Json;

namespace HotelReservationSystem.MCP.Server.Services;

public interface IAgentService
{
    Task<string> ProcessMessageAsync(string message);
}

public class AgentService : IAgentService
{
    private readonly ReceptionTools receptionTools;
    private readonly ChatClient chatClient;
    private readonly string systemPrompt;

    private static string LoadPromptFromYaml()
    {
        Assembly assembly = typeof(ReceptionTools).Assembly;
        string resourceName = assembly.GetManifestResourceNames()
            .First(n => n.EndsWith("AuroraBase.yaml", StringComparison.OrdinalIgnoreCase));

        using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
        using StreamReader reader = new(stream);
        string yaml = reader.ReadToEnd();

        // Parse "instructions: |" block from YAML
        const string marker = "instructions: |";
        int idx = yaml.IndexOf(marker, StringComparison.Ordinal);
        if (idx < 0)
            throw new InvalidOperationException("YAML prompt file missing 'instructions:' key.");

        string block = yaml[(idx + marker.Length)..];

        // Collect all indented lines that belong to the block scalar
        var lines = block.Split('\n')
            .Skip(1) // skip the empty line right after "|"
            .TakeWhile(l => l.Length == 0 || l.StartsWith("    "))
            .Select(l => l.Length >= 4 ? l[4..] : l);

        return string.Join('\n', lines).Trim();
    }

    private static readonly ChatTool NotifyStaffTool = ChatTool.CreateFunctionTool(
        functionName: "notify_staff",
        functionDescription: "Wysyła pilne zgłoszenie do personelu hotelowego. Używać w przypadku próśb gości wymagających interwencji (np. brak ręczników, sprzątanie, usterka techniczna).",
        functionParameters: BinaryData.FromString("""
        {
            "type": "object",
            "properties": {
                "message": {
                    "type": "string",
                    "description": "Treść zgłoszenia do personelu"
                },
                "category": {
                    "type": "string",
                    "enum": ["housekeeping", "reception", "technical"],
                    "description": "Kategoria zgłoszenia"
                }
            },
            "required": ["message", "category"]
        }
        """)
    );

    private static readonly ChatTool BookRoomTool = ChatTool.CreateFunctionTool(
        functionName: "book_room",
        functionDescription: "Tworzy rezerwację pokoju hotelowego dla gościa.",
        functionParameters: BinaryData.FromString("""
        {
            "type": "object",
            "properties": {
                "arrival": {
                    "type": "string",
                    "description": "Data przyjazdu w formacie YYYY-MM-DD"
                },
                "departure": {
                    "type": "string",
                    "description": "Data wyjazdu w formacie YYYY-MM-DD"
                },
                "roomId": {
                    "type": "integer",
                    "description": "ID pokoju do zarezerwowania"
                },
                "guests": {
                    "type": "integer",
                    "description": "Liczba gości"
                }
            },
            "required": ["arrival", "departure", "roomId", "guests"]
        }
        """)
    );

    public AgentService(ReceptionTools receptionTools, IConfiguration configuration)
    {
        this.receptionTools = receptionTools;
        this.systemPrompt = LoadPromptFromYaml();

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
            Tools = { NotifyStaffTool, BookRoomTool },
            Temperature = 0.7f,
        };

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
