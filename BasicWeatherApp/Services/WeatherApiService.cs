using BasicWeatherApp.ApiModels;
using System;
using System.Net.Http.Json;

namespace BasicWeatherApp.Services
{
    internal class WeatherApiService
    {
        private readonly HttpClient _httpClient;

        public WeatherApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Constants.API_BASE_URL);
           
        }

        public async Task<WeatherApiResponse> GetWeatherInfo(string location) {
            var response = await _httpClient.GetFromJsonAsync<WeatherApiResponse>($"v1/current.json?key=07701956d1824222a4a80257240902&q=Tułowice&aqi=no");
            //App.Current.MainPage.DisplayAlert("GetWeatherInfo", $"{response.Location.country}", "OK");
            return response;
        }
    }
}
