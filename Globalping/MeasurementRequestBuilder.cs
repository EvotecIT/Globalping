using System.Collections.Generic;

namespace Globalping;

/// <summary>
/// Fluent helper used to construct <see cref="MeasurementRequest"/> instances.
/// </summary>
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

    public MeasurementRequestBuilder AddCountry(CountryCode country, int? limit = null)
    {
        var loc = new LocationRequest { Country = country, Limit = limit };
        return AddLocation(loc);
    }

    public MeasurementRequestBuilder AddContinent(ContinentCode continent, int? limit = null)
    {
        var loc = new LocationRequest { Continent = continent, Limit = limit };
        return AddLocation(loc);
    }

    public MeasurementRequestBuilder AddRegion(RegionName region, int? limit = null)
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

    public MeasurementRequestBuilder AddMagic(string magic, int? limit = null)
    {
        var loc = new LocationRequest { Magic = magic, Limit = limit };
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

    public MeasurementRequestBuilder WithOptions<TOptions>(TOptions options)
        where TOptions : IMeasurementOptions
    {
        _request.MeasurementOptions = options;
        return this;
    }

    public MeasurementRequestBuilder WithMeasurementOptions(IMeasurementOptions options)
        => WithOptions(options);

    public MeasurementRequestBuilder WithInProgressUpdates(bool value = true)
    {
        _request.InProgressUpdates = value;
        return this;
    }

    public MeasurementRequestBuilder WithLimit(int? limit)
    {
        _request.Limit = limit;
        return this;
    }

    /// <summary>
    /// Finalizes the builder and returns the configured request instance.
    /// </summary>
    /// <returns>The constructed <see cref="MeasurementRequest"/>.</returns>
    public MeasurementRequest Build() => _request;
}
