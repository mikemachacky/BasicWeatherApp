
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
            Location = await Application.Current.MainPage.DisplayPromptAsync("Location", "Type new location here: ", "OK");
            await RefreshData();
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
            if ( Location == null)
            {
                await DisplayPopUp();
                await SecureStorage.Default.SetAsync("location", Location);

            }
            else
            {
                await RefreshData();
            }
           
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

    private async Task RefreshData()
    {
        var response = await _weatherApiService.GetWeatherInfo(Location);
        LastUpdated = response.Current.last_updated;
        TempC = $"{response.Current.temp_c}°";
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
    }


}