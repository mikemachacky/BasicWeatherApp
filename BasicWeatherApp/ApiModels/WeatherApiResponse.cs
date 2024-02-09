using System;
using System.Text.Json.Serialization;

namespace BasicWeatherApp.ApiModels
{
    public class WeatherApiResponse
    {
        [JsonPropertyName("location")]
        public WeatherApiResponseLocation Location { get; set; }
        [JsonPropertyName("current")]
        public WeatherApiResponseCurrent Current { get; set; }
    }

    public class WeatherApiResponseLocation
    {
        public string name { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public string tz_id { get; set; }
        public int localtime_epoch { get; set; }
        public string localtime { get; set; }
    }

    public class WeatherApiResponseCurrent
    {
        public int last_updated_epoch { get; set; }
        public string last_updated { get; set; }
        public float temp_c { get; set; }
        public float temp_f { get; set; }
        public int is_day { get; set; }
        public WeatherApiResponseCondition condition { get; set; }
        public float wind_mph { get; set; }
        public float wind_kph { get; set; }
        public int wind_degree { get; set; }
        public string wind_dir { get; set; }
        public float pressure_mb { get; set; }
        public float pressure_in { get; set; }
        public float precip_mm { get; set; }
        public float precip_in { get; set; }
        public int humidity { get; set; }
        public int cloud { get; set; }
        public float feelslike_c { get; set; }
        public float feelslike_f { get; set; }
        public float vis_km { get; set; }
        public float vis_miles { get; set; }
        public float uv { get; set; }
        public float gust_mph { get; set; }
        public float gust_kph { get; set; }
    }

    public class WeatherApiResponseCondition
    {
        public string text { get; set; }
        public string icon { get; set; }
        public int code { get; set; }
    }
}
