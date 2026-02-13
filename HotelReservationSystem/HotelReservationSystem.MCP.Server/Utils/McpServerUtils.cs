using HotelReservationSystem.MCP.Server.Tools;
using System.Reflection;
using System.ComponentModel;
using OpenAI.Chat;
using ModelContextProtocol.Server;
using System.Text.Json;

namespace HotelReservationSystem.MCP.Server.Utils;

public static class McpServerUtils
{
    public static List<ChatTool> GenerateToolsFrom<T>()
    {
        var tools = new List<ChatTool>();
        MethodInfo[] methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance);
    
        foreach (var method in methods)
        {
            var mcpAttr = method.GetCustomAttribute<McpServerToolAttribute>();
            if (mcpAttr == null) continue;

            var descAttr = method.GetCustomAttribute<DescriptionAttribute>();
            string description = descAttr?.Description ?? "Brak opisu";

            var properties = new Dictionary<string, object>();
            var required = new List<string>();

            foreach (var param in method.GetParameters())
            {
                var paramDescAttr = param.GetCustomAttribute<DescriptionAttribute>();
                string paramDesc = paramDescAttr?.Description ?? "";

                string jsonType = param.ParameterType == typeof(int) ? "integer" : "string";

                var propDef = new Dictionary<string, object>
                {
                    { "type", jsonType },
                    { "description", paramDesc }
                };

                if (param.ParameterType.IsEnum)
                {
                    propDef["enum"] = Enum.GetNames(param.ParameterType);
                }

                properties.Add(param.Name!, propDef);

                if (!param.IsOptional)
                {
                    required.Add(param.Name!);
                }
            }

            var parametersSchema = new
            {
                type = "object",
                properties = properties,
                required = required
            };

            // definition of OpenAI tools
            tools.Add(ChatTool.CreateFunctionTool(
                functionName: mcpAttr.Name,
                functionDescription: description,
                functionParameters: BinaryData.FromObjectAsJson(
                    parametersSchema,
                    new JsonSerializerOptions {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    })
            ));
        }

        return tools;
    }

    public static string LoadPromptFromYaml(string yamlName)
    {
        Assembly assembly = typeof(ReceptionTools).Assembly;
        string resourceName = assembly.GetManifestResourceNames()
            .First(n => n.EndsWith(yamlName, StringComparison.OrdinalIgnoreCase));

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
}