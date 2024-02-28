namespace PRDownloader.App.Pages.Requirements;

public partial class RequirementsPage : ContentPage
{
    public RequirementsPage(RequirementsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if(BindingContext is RequirementsPageViewModel viewModel)
        {
            viewModel.StartRequirementsCheck();
        }
    }
}
