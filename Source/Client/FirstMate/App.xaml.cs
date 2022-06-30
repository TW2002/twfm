using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FirstMate;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjQyMDkyQDMyMzAyZTMxMmUzMEMxencxYUt4TUJERnZaN0hoWldyN0dQMndhaVRSc0ExZUxWazRWWlgyNG89");
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        m_window = WindowManager.CreateWindow();
        m_window.Activate();

        m_socket = SocketManager.CreateProxy();
        _ = m_socket.ConnectAsync();
    }

    private Window m_window;
    private Terminal.Client m_socket;
}
