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
            var response = await _httpClient.GetFromJsonAsync<WeatherApiResponse>($"v1/forecast.json?key={Constants.API_KEY}&q={location}&days=7&aqi=no&alerts=no");
            //App.Current.MainPage.DisplayAlert("GetWeatherInfo", $"{response.Location.country}", "OK");
            return response;
        }
    }
}
