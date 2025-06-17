using Globalping;
using System;
using System.Net.Http;
using System.Net;

namespace Globalping.Mobile;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        TypePicker.SelectedIndex = 0;
    }

    private async void OnRunClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(HostEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a host", "OK");
            return;
        }

        var measurementType = TypePicker.SelectedIndex == 0 ? MeasurementType.Ping : MeasurementType.Http;

        var request = new MeasurementRequestBuilder()
            .WithType(measurementType)
            .WithTarget(HostEntry.Text)
            .Build();

        using var httpClient = new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.All
        });

        var probeService = new ProbeService(httpClient);
        var measurement = await probeService.CreateMeasurementAsync(request);

        var client = new MeasurementClient(httpClient);
        var result = await client.GetMeasurementByIdAsync(measurement.Id);

        ResultLabel.Text = result != null
            ? System.Text.Json.JsonSerializer.Serialize(result, new System.Text.Json.JsonSerializerOptions { WriteIndented = true })
            : "No result";
    }
}
