
using BasicWeatherApp.ApiModels;
using BasicWeatherApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows.Input;

namespace BasicWeatherApp.ViewModels;

internal partial class WeatherInfoViewModel : ObservableObject
{

    private readonly WeatherApiService _weatherApiService;
    public WeatherInfoViewModel()
    {
        _weatherApiService = new WeatherApiService();
        Initialize();
    }

    private async void Initialize()
    {
        try
        {
            await FetchWeatherInfo();
            // Now, you can show an alert or perform other actions after data is fetched
            //await Application.Current.MainPage.DisplayAlert("Constructor", "Works", "Cancel");
        }
        catch (Exception ex)
        {
            // Handle exceptions if any
        }
    }

    [ObservableProperty]
    private bool isRefreshing;


    [RelayCommand]
    public async Task Refresh()
    {
        if (!IsRefreshing)
        {
            IsRefreshing = true;
            try
            {
                await FetchWeatherInfo();
                await App.Current.MainPage.DisplayAlert("Refresh", "Works", "OK");
            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Refresh", "Doesn't work", "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }

    [ObservableProperty]
    private string lastUpdated;
    [ObservableProperty]
    private string tempC;
    [ObservableProperty]
    private string isDay;
    [ObservableProperty]
    private string text;
    [ObservableProperty]
    private string icon;
    [ObservableProperty]
    private string code;
    [ObservableProperty]
    private string windKPH;
    [ObservableProperty]
    private string windDir;
    [ObservableProperty]
    private string pressure_mb;
    [ObservableProperty]
    private string humidity;
    [ObservableProperty]
    private string cloud;
    [ObservableProperty]
    private string feelsLikeC;
    [ObservableProperty]
    private string uV;
    [ObservableProperty]
    private string city;
  


    [RelayCommand]
    public async Task FetchWeatherInfo()
    {
        IsRefreshing = true; // Set to true before fetching data

        try
        {
            var response = await _weatherApiService.GetWeatherInfo("Opole");
            LastUpdated = response.Current.last_updated;
            TempC = $"{response.Current.temp_c}°" ;
            IsDay = $"{response.Current.is_day}";
            Text = $"{response.Current.condition.text}";
            Icon = $"https:{response.Current.condition.icon}";
            Code = $"{response.Current.condition.code}";
            WindDir = $"{response.Current.wind_dir}";
            FeelsLikeC = $"Feels like {response.Current.feelslike_c}°";
            City = $"{response.Location.name}";
            Humidity = $"{response.Current.humidity}";
            Cloud = $"{response.Current.cloud}";
            UV = $"{response.Current.uv}";
           

            // Update other properties similarly
            //await Application.Current.MainPage.DisplayAlert("Success", "Service works!", "OK");
        }
        catch (Exception ex)
        {
            // Handle exceptions
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsRefreshing = false; // Set to false after data is fetched
        }
    } 

}