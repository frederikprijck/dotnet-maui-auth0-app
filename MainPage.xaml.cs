﻿using MauiAuth0App.Auth0;

namespace MauiAuth0App;

public partial class MainPage : ContentPage
{
	int count = 0;
  private readonly Auth0Client auth0Client;

  public MainPage(Auth0Client client)
	{
		InitializeComponent();
    auth0Client = client;

#if WINDOWS
    auth0Client.Browser = new WebViewBrowserAuthenticator(WebViewInstance);
#endif
  }

  private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

  private async void OnLoginClicked(object sender, EventArgs e)
  {
    var loginResult = await auth0Client.LoginAsync();

    if (!loginResult.IsError)
    {
      UsernameLbl.Text = loginResult.User.Identity.Name;
      UserPictureImg.Source = loginResult.User
        .Claims.FirstOrDefault(c => c.Type == "picture")?.Value;

      LoginView.IsVisible = false;
      HomeView.IsVisible = true;
    }
    else
    {
      await DisplayAlert("Error", loginResult.ErrorDescription, "OK");
    }
  }

  private async void OnLogoutClicked(object sender, EventArgs e)
  {
    var logoutResult = await auth0Client.LogoutAsync();

    if (!logoutResult.IsError)
    {
      HomeView.IsVisible = false;
      LoginView.IsVisible = true;
    }
    else
    {
      await DisplayAlert("Error", logoutResult.ErrorDescription, "OK");
    }
  }
}

