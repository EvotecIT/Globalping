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

    public MeasurementRequestBuilder ReuseLocationsFromId(string measurementId)
    {
        _request.ReuseLocationsFromId = measurementId;
        _request.Locations = null;
        return this;
    }

    public MeasurementRequestBuilder AddCountry(string country, int? limit = null)
    {
        var loc = new LocationRequest { Country = country, Limit = limit };
        return AddLocation(loc);
    }

    public MeasurementRequestBuilder AddContinent(string continent, int? limit = null)
    {
        var loc = new LocationRequest { Continent = continent, Limit = limit };
        return AddLocation(loc);
    }

    public MeasurementRequestBuilder AddRegion(string region, int? limit = null)
    {
        var loc = new LocationRequest { Region = region, Limit = limit };
        return AddLocation(loc);
    }

    public MeasurementRequestBuilder AddState(string state, int? limit = null)
    {
        var loc = new LocationRequest { State = state, Limit = limit };
        return AddLocation(loc);
    }

    public MeasurementRequestBuilder AddCity(string city, int? limit = null)
    {
        var loc = new LocationRequest { City = city, Limit = limit };
        return AddLocation(loc);
    }

    public MeasurementRequestBuilder AddAsn(int asn, int? limit = null)
    {
        var loc = new LocationRequest { Asn = asn, Limit = limit };
        return AddLocation(loc);
    }

    public MeasurementRequestBuilder AddNetwork(string network, int? limit = null)
    {
        var loc = new LocationRequest { Network = network, Limit = limit };
        return AddLocation(loc);
    }

    public MeasurementRequestBuilder AddTags(IEnumerable<string> tags, int? limit = null)
    {
        var loc = new LocationRequest { Tags = new List<string>(tags), Limit = limit };
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
        _request.ReuseLocationsFromId = null;
        return this;
    }

    public MeasurementRequestBuilder AddLocation(LocationRequest location)
    {
        _request.Locations ??= new List<LocationRequest>();
        _request.ReuseLocationsFromId = null;
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
