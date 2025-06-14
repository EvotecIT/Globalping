using System.Management.Automation;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Globalping;

namespace Globalping.PowerShell;

[Cmdlet(VerbsLifecycle.Start, "Globalping")]
[OutputType(typeof(MeasurementResponse))]
public class StartGlobalpingCommand : PSCmdlet
{
    [Parameter(Mandatory = true)]
    public MeasurementType Type { get; set; }

    [Parameter(Mandatory = true)]
    public string Target { get; set; } = string.Empty;

    [Parameter]
    public LocationRequest[]? Locations { get; set; }

    [Parameter]
    public string[]? SimpleLocations { get; set; }

    [Parameter]
    public int? Limit { get; set; }

    [Parameter]
    public IMeasurementOptions? MeasurementOptions { get; set; }

    [Parameter]
    public SwitchParameter InProgressUpdates { get; set; }

    [Parameter]
    [Alias("Token")]
    public string? ApiKey { get; set; }

    protected override void ProcessRecord()
    {
        using var httpClient = new HttpClient();
        var service = new ProbeService(httpClient, ApiKey);

        var limit = Limit ?? 0;
        if (!MyInvocation.BoundParameters.ContainsKey(nameof(Limit)))
        {
            if (SimpleLocations is not null)
            {
                limit += SimpleLocations.Length;
            }

            if (Locations is not null)
            {
                foreach (var loc in Locations)
                {
                    limit += loc.Limit ?? 1;
                }
            }

            if (limit == 0)
            {
                limit = 1;
            }
        }
        var builder = new MeasurementRequestBuilder()
            .WithType(Type)
            .WithTarget(Target)
            .WithLimit(limit);

        if (SimpleLocations is not null)
        {
            foreach (var loc in SimpleLocations)
            {
                if (loc.Length == 2)
                {
                    builder.AddCountry(loc);
                }
                else
                {
                    builder.AddMagic(loc);
                }
            }
        }

        if (Locations is not null)
        {
            builder.WithLocations(Locations);
        }

        if (MeasurementOptions is not null)
        {
            builder.WithMeasurementOptions(MeasurementOptions);
        }

        var request = builder.Build();
        request.InProgressUpdates = InProgressUpdates.IsPresent;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
        jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        WriteVerbose($"Request: {JsonSerializer.Serialize(request, jsonOptions)}");

        string id;
        try
        {
            id = service.CreateMeasurementAsync(request).GetAwaiter().GetResult();
            WriteVerbose($"Measurement id: {id}");
        }
        catch (HttpRequestException ex)
        {
            WriteVerbose($"Request failed: {ex.Message}");
            throw;
        }
        if (string.IsNullOrEmpty(id))
        {
            WriteError(new ErrorRecord(new InvalidOperationException("Measurement creation failed"), "CreateFailed", ErrorCategory.InvalidOperation, Target));
            return;
        }

        var client = new MeasurementClient(httpClient, ApiKey);
        var result = client.GetMeasurementByIdAsync(id).GetAwaiter().GetResult();
        WriteVerbose($"Response: {JsonSerializer.Serialize(result, jsonOptions)}");
        WriteObject(result);
    }
}
