
using BasicWeatherApp.ApiModels;
using BasicWeatherApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BasicWeatherApp.ViewModels;

internal partial class WeatherInfoViewModel : ObservableObject
{
    private readonly WeatherApiService _weatherApiService;
    public WeatherInfoViewModel()
    {
        _weatherApiService = new WeatherApiService();
        //App.Current.MainPage.DisplayAlert("Constructor", "Works", "Cancel");
        FetchWeatherInfo();
    }
    bool isRefreshing;

    public bool IsRefreshing
    {
        get { return isRefreshing; }
        set
        {
            isRefreshing = value;
            OnPropertyChanged();
        }
    }
    public ICommand RefreshCommand => new Command(async () => await FetchWeatherInfo());
    [ObservableProperty]
    private string lastUpdated;
    [ObservableProperty]
    private string tempC;
    [ObservableProperty]
    private string tempF;
    [ObservableProperty]
    private string isDay;
    [ObservableProperty]
    private string text;
    [ObservableProperty]
    private string icon;
    [ObservableProperty]
    private string code;
    [ObservableProperty]
    private string windMPH;
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
    private string feelsLikeF;
    [ObservableProperty]
    private string uV;
    [ObservableProperty]
    private string location;

  

 
    private async Task<WeatherApiResponse> FetchWeatherInfo()
    {
        var response = await _weatherApiService.GetWeatherInfo("Opole");
      
        await Application.Current.MainPage.DisplayAlert("Success", "Service works!", "OK");

        return response;
           
        
    }
  
}