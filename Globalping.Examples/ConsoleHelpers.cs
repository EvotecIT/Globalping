using System;
using System.Collections;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Spectre.Console;
using Spectre.Console.Json;

namespace Globalping.Examples;

public static class ConsoleHelpers
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static void WriteTable(object data, string? title = null)
    {
        if (title != null)
        {
            AnsiConsole.MarkupLine($"[yellow]{title}[/]");
        }

        var table = new Table().RoundedBorder();
        table.AddColumn("Property");
        table.AddColumn("Value");

        foreach (var prop in data.GetType().GetProperties())
        {
            var value = prop.GetValue(data);
            if (value == null)
            {
                continue;
            }

            var formatted = Markup.Escape(FormatValue(value));
            table.AddRow(Markup.Escape(prop.Name), formatted);
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    private static string FormatValue(object value)
    {
        if (value is string s)
        {
            return s;
        }
        if (value is IEnumerable enumerable && value is not IDictionary)
        {
            var items = enumerable.Cast<object>().Select(FormatValue);
            return string.Join(", ", items);
        }
        if (value.GetType().IsClass)
        {
            return JsonSerializer.Serialize(value, JsonOptions);
        }
        return value.ToString() ?? string.Empty;
    }

    public static void WriteJson(object data, string? title = null)
    {
        var json = JsonSerializer.Serialize(data, JsonOptions);
        if (title != null)
        {
            AnsiConsole.MarkupLine($"[yellow]{title}[/]");
        }
        AnsiConsole.Write(new JsonText(json));
        AnsiConsole.WriteLine();
    }
}
