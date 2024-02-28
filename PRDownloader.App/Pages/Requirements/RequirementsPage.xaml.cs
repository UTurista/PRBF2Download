namespace PRDownloader.App.Pages.Requirements;

public partial class RequirementsPage : ContentPage
{
    public RequirementsPage(RequirementsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is RequirementsPageViewModel viewModel)
        {
            _ = viewModel.StartRequirementsCheck();
        }
    }
}
