using System.Management.Automation;
using System.Net.Http;
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
    [ValidateRange(1, int.MaxValue)]
    public int Limit { get; set; } = 1;

    [Parameter]
    public IMeasurementOptions? MeasurementOptions { get; set; }

    [Parameter]
    public SwitchParameter InProgressUpdates { get; set; }

    protected override void ProcessRecord()
    {
        using var httpClient = new HttpClient();
        var service = new ProbeService(httpClient);

        var builder = new MeasurementRequestBuilder()
            .WithType(Type)
            .WithTarget(Target)
            .WithLimit(Limit);

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

        var id = service.CreateMeasurementAsync(request).GetAwaiter().GetResult();
        if (string.IsNullOrEmpty(id))
        {
            WriteError(new ErrorRecord(new InvalidOperationException("Measurement creation failed"), "CreateFailed", ErrorCategory.InvalidOperation, Target));
            return;
        }

        var client = new MeasurementClient(httpClient);
        var result = client.GetMeasurementByIdAsync(id).GetAwaiter().GetResult();
        WriteObject(result);
    }
}
