namespace PRDownloader.App.Controls;

public partial class FolderPicker : HorizontalStackLayout
{
    public static readonly BindableProperty DownloadLocationProperty = BindableProperty.Create(nameof(DownloadLocation), typeof(string), typeof(FolderPicker), string.Empty, BindingMode.TwoWay);
    public static readonly BindableProperty PickDownloadLocationCommandProperty = BindableProperty.Create(nameof(PickDownloadLocationCommand), typeof(Command), typeof(FolderPicker), null, BindingMode.OneWay);

    public FolderPicker()
    {
        InitializeComponent();
    }

    public string DownloadLocation
    {
        get => (string)GetValue(DownloadLocationProperty);
        set => SetValue(DownloadLocationProperty, value);
    }

    public Command? PickDownloadLocationCommand
    {
        get => (Command?)GetValue(PickDownloadLocationCommandProperty);
        set => SetValue(PickDownloadLocationCommandProperty, value);
    }
}
