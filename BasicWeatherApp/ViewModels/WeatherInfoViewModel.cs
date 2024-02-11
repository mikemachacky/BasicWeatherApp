
using BasicWeatherApp.ApiModels;
using BasicWeatherApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;


namespace BasicWeatherApp.ViewModels;

internal partial class WeatherInfoViewModel : ObservableObject
{

    private readonly WeatherApiService _weatherApiService;

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
    [ObservableProperty]
    private string location;
    [ObservableProperty]
    public ObservableCollection<Forecastday> forecasts;

    public WeatherInfoViewModel()
    {
        _weatherApiService = new WeatherApiService();
        Forecasts = new ObservableCollection<Forecastday>();
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
            await App.Current.MainPage.DisplayAlert("Error", $"{ex}","OK");
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

    [RelayCommand]
    public async Task DisplayPopUp()
    {
        try {
            string answer = await Application.Current.MainPage.DisplayPromptAsync("Update Your Location", "Let's make sure we have your current location. \nEnter your new location to receive accurate and personalized information.", "Update Location");
            if (answer != null)
            {
                Location = answer;
                await SecureStorage.Default.SetAsync("location", Location);
                await LoadData();
            }
           

        }
        catch(Exception ex) {
           await Application.Current.MainPage.DisplayAlert("Error", $"{ex}", "OK");
            
        }
    }


    [RelayCommand]
    public async Task FetchWeatherInfo()
    {
        IsRefreshing = true;
        

        try
        {
            Location = await SecureStorage.Default.GetAsync("location");
            if ( Location == null)
            {
                Location = await Application.Current.MainPage.DisplayPromptAsync("Set Your Location", "Help us tailor your experience by entering your desired location. \nWe'll show you the most relevant data for your area.", "Set Location");
                await SecureStorage.Default.SetAsync("location", Location);
            }
           
            await LoadData();
            //await Application.Current.MainPage.DisplayAlert("Success", "Service works!", "OK");
        }
        catch (Exception ex)
        {
          
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsRefreshing = false; 
        }
    } 

    private async Task LoadData()
    {
        var response = await _weatherApiService.GetWeatherInfo(Location);
        LastUpdated = response.Current.Last_updated;
        TempC = $"{response.Current.Temp_c}°";
        IsDay = $"{response.Current.Is_day}";
        Text = $"{response.Current.Condition.Text}";
        Icon = $"https:{response.Current.Condition.Icon}";
        Code = $"{response.Current.Condition.Code}";
        WindDir = $"{response.Current.Wind_dir}";
        FeelsLikeC = $"Feels like {response.Current.Feelslike_c}°";
        City = $"{response.Location.Name}";
        Humidity = $"{response.Current.Humidity}";
        Cloud = $"{response.Current.Cloud}";
        UV = $"{response.Current.Uv}";
        var array = response.Forecast.Forecastday;
        foreach( var item in array )
        {
            Forecasts.Add(item);
        }
       
    }


}