using BasicWeatherApp.ViewModels;

namespace BasicWeatherApp.Views;

public partial class WeatherInfoView : ContentPage
{
	public WeatherInfoView()
	{
		InitializeComponent();
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            Application.Current.MainPage.DisplayAlert("Error", $"No internet connection", "OK");
          
        }
        BindingContext = new WeatherInfoViewModel();

    }

   
}