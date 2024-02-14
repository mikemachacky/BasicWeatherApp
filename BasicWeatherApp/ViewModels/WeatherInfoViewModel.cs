
using BasicWeatherApp.ApiModels;
using BasicWeatherApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BasicWeatherApp.ViewModels;

internal partial class WeatherInfoViewModel : ObservableObject
{

    private readonly WeatherApiService _weatherApiService;
    public ObservableCollection<Forecastday> ForecastApi { get; set; }
    public ObservableCollection<Hour> HourApi { get; set; }
    public ObservableCollection<string> AstroInfo { get; set; }
  
    
    [ObservableProperty]
    string icon;
    [ObservableProperty]
    string location;
    [ObservableProperty]
    Current currentApi;
    [ObservableProperty]
    string lastUpdated;
    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    ApiModels.Location locationApi;



    public WeatherInfoViewModel()
    {
        _weatherApiService = new WeatherApiService();
        ForecastApi = new ObservableCollection<Forecastday>();
        HourApi = new ObservableCollection<Hour>();
        AstroInfo = new ObservableCollection<string>();
        LoadLocalStorageAsync();
        Initialize();
    }

    private async void Initialize()
    {
        try
        {
         
            await FetchWeatherInfoAsync();
            
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Error", $"{ex}","OK");
        }
    }




    [RelayCommand]
    public async Task Refresh()
    {
        try
        {
            IsRefreshing = true;
            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error occured", $"{ex}", "OK");
        }
        finally
        {
            IsRefreshing = false;
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

    public async Task FetchWeatherInfoAsync()
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
            if(response != null) {
                CurrentApi = response.Current;
                await SecureStorage.SetAsync("CurrentApi", JsonSerializer.Serialize(CurrentApi));
                LastUpdated = $"{DateTime.Parse(CurrentApi.Last_updated).ToString("dddd")} {DateTime.Parse(CurrentApi.Last_updated).ToString("HH:mm")}";
                await SecureStorage.SetAsync("LastUpdated", LastUpdated);
                LocationApi = response.Location;
                await SecureStorage.SetAsync("LocationApi", JsonSerializer.Serialize(LocationApi));
                Icon = $"https:{response.Current.Condition.Icon}";
                await SecureStorage.SetAsync("Icon", Icon);
                ForecastApi.Clear();
                HourApi.Clear();
                AstroInfo.Clear();

                foreach (var item in response.Forecast.Forecastday)
                {
                    if (DateTime.Parse(item.Date).DayOfWeek == DateTime.Now.DayOfWeek)
                    {
                        item.Date = "Today";
                        AstroInfo.Add($"Sunrise: {item.Astro.Sunrise} \nSunset: {item.Astro.Sunset}");
                        AstroInfo.Add($"Moonrise: {item.Astro.Moonrise} \nMoonset: {item.Astro.Moonset}");
                        AstroInfo.Add($"Phase: {item.Astro.Moon_phase}\nIllumination:{item.Astro.Moon_illumination}%");
                    }
                    else
                    {
                        item.Date = DateTime.Parse(item.Date).DayOfWeek.ToString();
                    }
                    DateTime now = DateTime.Now;
                    foreach (var hour in item.Hour)
                    {
                        DateTime hourTime = DateTime.Parse(hour.Time);
                        TimeSpan difference = hourTime - now;

                        if ((hourTime.Date == now.Date && difference.TotalHours > 0 && difference.TotalHours <= 12) ||
                            (hourTime.Date != now.Date && hourTime > now && hourTime <= now.AddHours(12)))
                        {
                            hour.Time = hourTime.ToString("HH:mm");
                            HourApi.Add(hour);
                        }
                    }
                    ForecastApi.Add(item);
                }
            }
           
            

          
        }
        catch(Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error Occured", $"{ex}", "OK");
        }
       
       
    }
    public async void LoadLocalStorageAsync()
    {
        CurrentApi = JsonSerializer.Deserialize<Current>(await SecureStorage.GetAsync( "CurrentApi"));
        LastUpdated = await SecureStorage.GetAsync("LastUpdated");
        LocationApi = JsonSerializer.Deserialize<ApiModels.Location>( await SecureStorage.GetAsync("LocationApi"));
        Icon = await SecureStorage.GetAsync("Icon");
    }

}