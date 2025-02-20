using System;
using System.Windows;

namespace PRDownloader;

public partial class OptionsWindow : Window
{
    public OptionsWindow(OptionsWindowViewModel viewmodel)
    {
        DataContext = viewmodel;
        viewmodel.Close += OnViewModelClose;
        InitializeComponent();
    }

    public void OnViewModelClose(object? sender, EventArgs args)
    {
        Close();
    }
}
