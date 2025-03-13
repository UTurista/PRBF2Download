using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRDownloader.Entities;
using PRDownloader.Services;
using PRDownloader.Validators;

namespace PRDownloader;

public partial class OptionsWindowViewModel : ObservableValidator
{
    private readonly OptionsService _optionsService;
    public event EventHandler? Close;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [DirectoryMustExist]
    private string _downloadPath;
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [DirectoryMustExist]
    private string _cachePath;
    [ObservableProperty]
    private bool _isLimitDownloadEnabled;
    [ObservableProperty]
    private bool _isLimitUploadEnabled;
    [ObservableProperty]
    private bool _isAllowDhtEnabled;
    [ObservableProperty]
    private bool _isAllowPeerExchangeEnabled;
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(OptionsWindowViewModel), nameof(PositiveNumber))]
    private string _limitDownloadSpeed;
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(OptionsWindowViewModel), nameof(PositiveNumber))]
    private string _limitUploadSpeed;

    public OptionsWindowViewModel(OptionsService optionsService)
    {
        _optionsService = optionsService;
        ReadOptions();
    }

    private void ReadOptions()
    {
        DownloadPath = _optionsService.State.DownloadPath;
        CachePath = _optionsService.State.CachePath;
        IsLimitDownloadEnabled = _optionsService.State.LimitDownloadSpeed is not null;
        IsLimitUploadEnabled = _optionsService.State.LimitUploadSpeed is not null;
        IsAllowDhtEnabled = _optionsService.State.AllowDHT;
        IsAllowPeerExchangeEnabled = _optionsService.State.AllowPeerExchange;
    }

    private TorrentOptions ToOptions()
    {
        return new()
        {
            DownloadPath = DownloadPath,
            CachePath = CachePath,
            LimitDownloadSpeed = null,
            LimitUploadSpeed = null,
            AllowDHT = IsAllowDhtEnabled,
            AllowPeerExchange = IsAllowPeerExchangeEnabled,
        };
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == "HasErrors")
        {
            ApplyAndCloseCommand?.NotifyCanExecuteChanged();
        }
    }

    [RelayCommand]
    private void ResetOptions()
    {
        _optionsService.Update(OptionsService.DefaultState);
        ReadOptions();
    }


    private bool CanApplyAndClose()
    {
        return !this.HasErrors;
    }

    [RelayCommand(CanExecute = nameof(CanApplyAndClose))]
    private void ApplyAndClose()
    {
        var options = ToOptions();
        _optionsService.Update(options);
        Close?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void CancelAndClose()
    {
        Close?.Invoke(this, EventArgs.Empty);
    }

    public static ValidationResult PositiveNumber(string @value, ValidationContext _)
    {
        if (int.TryParse(@value as string, out int number))
        {
            return number >= 0 ? ValidationResult.Success! : new ValidationResult("Value must be positive.");
        }

        return new ValidationResult("Invalid number.");
    }
}
