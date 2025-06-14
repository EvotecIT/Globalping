using System.Collections.Generic;

namespace Globalping;

public class MeasurementRequestBuilder
{
    private readonly MeasurementRequest _request = new();

    public MeasurementRequestBuilder WithType(MeasurementType type)
    {
        _request.Type = type;
        return this;
    }

    public MeasurementRequestBuilder WithTarget(string target)
    {
        _request.Target = target;
        return this;
    }

    public MeasurementRequestBuilder AddCountry(string country, int? limit = null)
    {
        var loc = new LocationRequest { Country = country, Limit = limit };
        return AddLocation(loc);
    }

    public MeasurementRequestBuilder AddMagic(string magic)
    {
        var loc = new LocationRequest { Magic = magic };
        return AddLocation(loc);
    }

    public MeasurementRequestBuilder WithLocations(IEnumerable<LocationRequest> locations)
    {
        _request.Locations = new List<LocationRequest>(locations);
        return this;
    }

    public MeasurementRequestBuilder AddLocation(LocationRequest location)
    {
        _request.Locations ??= new List<LocationRequest>();
        _request.Locations.Add(location);
        return this;
    }

    public MeasurementRequestBuilder WithMeasurementOptions(IMeasurementOptions options)
    {
        _request.MeasurementOptions = options;
        return this;
    }

    public MeasurementRequestBuilder WithLimit(int? limit)
    {
        _request.Limit = limit;
        return this;
    }

    public MeasurementRequest Build() => _request;
}
