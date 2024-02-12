
using BasicWeatherApp.ApiModels;
using BasicWeatherApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;




namespace BasicWeatherApp.ViewModels;

internal partial class WeatherInfoViewModel : ObservableObject
{

    private readonly WeatherApiService _weatherApiService;
    public ObservableCollection<Forecastday> ForecastApi { get; set; }
    
    [ObservableProperty]
    string icon;
    [ObservableProperty]
    string location;
    [ObservableProperty]
    Current currentApi;
    [ObservableProperty]
    ApiModels.Location locationApi;

    public WeatherInfoViewModel()
    {
        _weatherApiService = new WeatherApiService();
        ForecastApi = new ObservableCollection<Forecastday>();
        Initialize();
    }

    private async void Initialize()
    {
        try
        {
            await FetchWeatherInfo();
            
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
                await LoadDataAsync();
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
           
            await LoadDataAsync();
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

    private async Task LoadDataAsync()
    {
        try
        {
            var response = await _weatherApiService.GetWeatherInfo(Location);
            CurrentApi = response.Current;
            LocationApi = response.Location;
            ForecastApi.Clear();
            foreach (var item in response.Forecast.Forecastday)
            {
                ForecastApi.Add(item);
            }
            Icon = $"https:{response.Current.Condition.Icon}";
          
        }
        catch(Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error Occured", $"{ex}", "OK");
        }
       
       
    }


}