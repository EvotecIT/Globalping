using System;
using System.Linq;
using System.Management.Automation;
using System.Net;
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
///   <code>Start-GlobalpingHttp -Target "https://example.com" -Limit 3 -InProgressUpdates -WaitTime 60</code>
///   <para>Requests three probes and streams intermediate results for up to one minute.</para>
/// </example>
public abstract class StartGlobalpingBaseCommand : PSCmdlet
{
    /// <summary>
    /// Gets the measurement type implemented by the derived cmdlet.
    /// </summary>
    protected abstract MeasurementType Type { get; }

    /// <para>Target host names, IP addresses or URLs to test.</para>
    /// <para>Each value is passed verbatim to the underlying
    /// measurement API.</para>
    [Parameter(Mandatory = true)]
    public string[] Target { get; set; } = Array.Empty<string>();

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
    [ValidateRange(1, 500)]
    public int? Limit { get; set; }

    /// <para>Request progress updates while the measurement runs.</para>
    /// <para>When set the API streams partial results that are written as they
    /// arrive.</para>
    [Parameter]
    public SwitchParameter InProgressUpdates { get; set; }

    /// <para>Time in seconds to wait for progress updates.</para>
    /// <para>Only applies when <see cref="InProgressUpdates"/> is specified.</para>
    [Parameter]
    [ValidateRange(1, int.MaxValue)]
    public int WaitTime { get; set; } = 150;

    /// <para>API key used to authenticate with Globalping.</para>
    /// <para>Anonymous requests may be rate limited.</para>
    [Parameter]
    [Alias("Token")]
    public string? ApiKey { get; set; }

    /// <summary>
    /// Gets additional measurement options used when building the request.
    /// </summary>
    protected virtual IMeasurementOptions? MeasurementOptions => null;

    /// <summary>
    /// Creates the measurement request and writes the resulting objects to the pipeline.
    /// </summary>
    protected override void ProcessRecord()
    {
        using var httpClient = new HttpClient(new HttpClientHandler
        {
#if NET6_0_OR_GREATER
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
#else
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
#endif
        });
        var service = new ProbeService(httpClient, ApiKey);
        var client = new MeasurementClient(httpClient, ApiKey);

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
        jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        foreach (var target in Target)
        {
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
                .WithTarget(target);

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
                    if (CountryCodeExtensions.TryParse(loc, out var code))
                    {
                        builder.AddCountry(code);
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

            WriteVerbose($"Request: {JsonSerializer.Serialize(request, jsonOptions)}");

            CreateMeasurementResponse createResponse;
            try
            {
                createResponse = service.CreateMeasurementAsync(request, WaitTime).GetAwaiter().GetResult();
                WriteVerbose($"Measurement id: {createResponse.Id}");
            }
            catch (HttpRequestException ex)
            {
                WriteVerbose($"Request failed: {ex.Message}");
                throw;
            }
            if (string.IsNullOrEmpty(createResponse.Id))
            {
                WriteError(new ErrorRecord(new InvalidOperationException("Measurement creation failed"), "CreateFailed", ErrorCategory.InvalidOperation, target));
                continue;
            }

            var result = client.GetMeasurementByIdAsync(createResponse.Id).GetAwaiter().GetResult();
            WriteVerbose($"Response: {JsonSerializer.Serialize(result, jsonOptions)}");

            HandleOutput(result);
        }
    }

    /// <summary>
    /// Handles the measurement response produced by <see cref="ProcessRecord"/>.
    /// </summary>
    /// <param name="result">Measurement result returned by the service.</param>
    protected virtual void HandleOutput(MeasurementResponse? result)
    {
        WriteObject(result);
    }
}
