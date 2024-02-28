namespace PRDownloader.App.Services;

public sealed class ShellNavigationService : IShellNavigationService
{
    public Task GoToAsync(ShellNavigationState state, bool animate, IDictionary<string, object> parameters )
    {
        return Shell.Current.GoToAsync(state, animate, parameters);
    }

    public Task GoToAsync(ShellNavigationState state)
    {
        return Shell.Current.GoToAsync(state);
    }
}

public interface IShellNavigationService
{
    Task GoToAsync(ShellNavigationState state, bool animate, IDictionary<string, object> parameters);
    Task GoToAsync(ShellNavigationState state);
}
