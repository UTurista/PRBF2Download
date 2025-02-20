using System;
using System.Windows;

namespace PRDownloader;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewmodel)
    {
        DataContext = viewmodel;
        InitializeComponent();
    }

    protected override void OnActivated(EventArgs e)
    {
        base.OnActivated(e);
        if (DataContext is MainWindowViewModel vm)
        {
            try
            {
                vm.OnActivated();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    protected override void OnDeactivated(EventArgs e)
    {
        base.OnDeactivated(e);
        if (DataContext is MainWindowViewModel vm)
        {
            vm.OnDeactivated();
        }
    }

}
