using Android.App;
using Android.Content;
using Microsoft.Identity.Client;

namespace MicrosoftEntraIdDemoApp.Platforms.Android
{
    /// <summary>
    /// MSAL redirect activity used to receive the authentication response
    /// from the system browser and return control back to the .NET MAUI app.
    ///
    /// This activity is required for Android authentication flows using
    /// Microsoft Authentication Library (MSAL).
    ///
    /// Azure Portal:
    /// Entra ID -> App registrations -> Your Application -> Authentication
    ///
    /// Redirect URI should match the DataScheme below:
    /// msal{ApplicationClientId}://auth
    ///
    /// Example:
    /// msalfb631f30-6681-478c-9b4b-48100cce3://auth
    /// </summary>
    [Activity(Exported = true)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[]
        {
        Intent.CategoryDefault,
        Intent.CategoryBrowsable
        },

        // Must match the host part of the Redirect URI
        // configured in Microsoft Entra ID.
        //
        // Example:
        // msalfb631f30-6681-478c-9b4b-48100cce3://auth
        //                                         ^^^^
        DataHost = "auth",

        // Must match the scheme part of the Redirect URI
        // configured in Microsoft Entra ID.
        //
        // Example:
        // msalfb631f30-6681-478c-9b4b-48100cce3://auth
        // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        //
        // Recommended format:
        // msal{ApplicationClientId}
        //
        // Example:
        // msalfb631f30-6681-478c-9b4b-48100cce3
        DataScheme = "msalfb631f30-6681-478c-9b4b-48100cce3fea")]
    public class MsalActivity : BrowserTabActivity
    {
    }
}