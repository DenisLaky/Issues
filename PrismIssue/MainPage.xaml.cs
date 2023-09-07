namespace PrismIssue;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnButtonClicked(object sender, EventArgs e)
	{
		var navigationService = ContainerLocator.Container.Resolve<INavigationService>();
		if (navigationService != null)
		{
			_ = navigationService.NavigateAsync(new Uri($"/{nameof(SecondPage)}"));
		}
	}
}


