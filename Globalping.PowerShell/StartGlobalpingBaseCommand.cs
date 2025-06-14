using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Globalping;

namespace Globalping.PowerShell;

/// <summary>Base class for Globalping measurement cmdlets.</summary>
/// <para>Derived cmdlets send measurement requests to the Globalping service
/// and return structured results.</para>
/// <para>The common parameters defined here control target host, probe
/// selection and authentication.</para>
/// <seealso href="https://github.com/EvotecIT/Globalping/tree/main/Globalping.Examples">Repository examples</seealso>
/// <example>
///   <summary>Ping from Germany</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingPing -Target "example.com" -SimpleLocations "DE"</code>
///   <para>Runs a ping measurement from German probes.</para>
/// </example>
/// <example>
///   <summary>HTTP request with live updates</summary>
///   <prefix>PS> </prefix>
///   <code>Start-GlobalpingHttp -Target "https://example.com" -Limit 3 -InProgressUpdates</code>
///   <para>Requests three probes and streams intermediate results.</para>
/// </example>
public abstract class StartGlobalpingBaseCommand : PSCmdlet
{
    protected abstract MeasurementType Type { get; }

    /// <para>Target host name, IP address or URL to test.</para>
    /// <para>The target string is passed verbatim to the underlying
    /// measurement API.</para>
    [Parameter(Mandatory = true)]
    public string Target { get; set; } = string.Empty;

    /// <para>Detailed location definitions for probes.</para>
    /// <para>Each <see cref="LocationRequest"/> may specify city, country,
    /// ASN or provider details.</para>
    [Parameter]
    public LocationRequest[]? Locations { get; set; }

    /// <summary>Reuse probe locations from a previous measurement.</summary>
    [Parameter]
    public string? ReuseLocationsFromId { get; set; }

    /// <para>Short location identifiers such as city or country codes.</para>
    /// <para>Two-letter strings are treated as ISO country codes. Longer
    /// values map to the "magic" location syntax used by the API.</para>
    [Parameter]
    public string[]? SimpleLocations { get; set; }

    /// <para>Maximum number of probes to use.</para>
    /// <para>If omitted the cmdlet estimates a value based on provided
    /// locations.</para>
    [Parameter]
    public int? Limit { get; set; }

    /// <para>Request progress updates while the measurement runs.</para>
    /// <para>When set the API streams partial results that are written as they
    /// arrive.</para>
    [Parameter]
    public SwitchParameter InProgressUpdates { get; set; }

    /// <para>API key used to authenticate with Globalping.</para>
    /// <para>Anonymous requests may be rate limited.</para>
    [Parameter]
    [Alias("Token")]
    public string? ApiKey { get; set; }

    protected virtual IMeasurementOptions? MeasurementOptions => null;

    protected override void ProcessRecord()
    {
        using var httpClient = new HttpClient();
        var service = new ProbeService(httpClient, ApiKey);

        int? limit = Limit;
        var calculateLimit = !MyInvocation.BoundParameters.ContainsKey(nameof(Limit));
        var hasLocationLimits = ReuseLocationsFromId is null &&
            Locations is not null && Locations.Any(l => l.Limit.HasValue);

        if (ReuseLocationsFromId is null && calculateLimit && !hasLocationLimits)
        {
            limit = 0;

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
            .WithTarget(Target);

        if (ReuseLocationsFromId is not null)
        {
            builder.ReuseLocationsFromId(ReuseLocationsFromId);
        }

        if (limit.HasValue)
        {
            builder.WithLimit(limit);
        }

        if (ReuseLocationsFromId is null && SimpleLocations is not null)
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

        if (ReuseLocationsFromId is null && Locations is not null)
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

        HandleOutput(result);
    }

    protected virtual void HandleOutput(MeasurementResponse? result)
    {
        WriteObject(result);
    }
}
