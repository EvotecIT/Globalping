using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Spectre.Console;
using Spectre.Console.Json;
using Spectre.Console.Rendering;

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

        var table = CreateTable(data);
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    private static Table CreateTable(object data)
    {
        var table = new Table().RoundedBorder();
        table.AddColumn("Property");
        table.AddColumn("Value");

        IEnumerable<KeyValuePair<string, object?>> entries;
        if (data is IDictionary<string, object?> genericDict)
        {
            entries = genericDict;
        }
        else if (data is IDictionary dict)
        {
            entries = dict.Cast<DictionaryEntry>()
                .Select(e => new KeyValuePair<string, object?>(e.Key?.ToString() ?? string.Empty, e.Value));
        }
        else
        {
            entries = data.GetType().GetProperties()
                .Select(p => new KeyValuePair<string, object?>(p.Name, p.GetValue(data)));
        }

        foreach (var entry in entries)
        {
            if (entry.Value == null)
            {
                continue;
            }

            table.AddRow(new Markup(Markup.Escape(entry.Key)), RenderValue(entry.Value));
        }

        return table;
    }

    private static IRenderable RenderValue(object value)
    {
        switch (value)
        {
            case string s:
                return new Markup(Markup.Escape(s));
            case JsonElement je:
                return RenderJsonElement(je);
            case IEnumerable enumerable when value is not IDictionary:
                var list = enumerable.Cast<object>().ToList();
                return CreateListTable(list);
            default:
                if (value.GetType().IsClass)
                {
                    return CreateTable(value);
                }
                return new Markup(Markup.Escape(value.ToString() ?? string.Empty));
        }
    }

    private static IRenderable RenderJsonElement(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(element.GetRawText(), JsonOptions) ?? new();
                return CreateTable(dict);
            case JsonValueKind.Array:
                var list = JsonSerializer.Deserialize<List<object>>(element.GetRawText(), JsonOptions) ?? new();
                return CreateListTable(list);
            default:
                return new Markup(Markup.Escape(element.ToString()));
        }
    }

    private static Table CreateListTable(IEnumerable<object> items)
    {
        var table = new Table().RoundedBorder();
        table.AddColumn("Value");
        foreach (var item in items)
        {
            table.AddRow(RenderValue(item));
        }
        return table;
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
