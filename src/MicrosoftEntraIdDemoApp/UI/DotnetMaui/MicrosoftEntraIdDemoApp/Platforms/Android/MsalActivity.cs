using Android.App;
using Android.Content;
using Android.OS;
using Microsoft.Identity.Client;

namespace MicrosoftEntraIdDemoApp.Platforms.Android
{
    //    /// <summary>
    //    /// MsalActivity - OAuth 2.0 Redirect Handler for Microsoft Entra ID (MSAL)
    //    /// 
    //    /// ✅ PURPOSE:
    //    /// This Activity intercepts the OAuth 2.0 redirect callback from the Microsoft login page
    //    /// and routes the authentication result back to your .NET MAUI app. Without this,
    //    /// Android will close your app after the user logs in because there's nowhere to return to.
    //    /// 
    //    /// ✅ HOW IT WORKS:
    //    /// 1. User clicks "Login" button in your app → LoginHttpService.LoginAsync() is called
    //    /// 2. MSAL opens the device browser with the Microsoft login page
    //    /// 3. User enters credentials and logs in successfully
    //    /// 4. Microsoft redirects to: msauth://com.kmaraszkiewicz86.microsoftentraiddemoapp/MgO7Vtlx%2FRlYeNiSbxjhy9NRht4%3D
    //    /// 5. Android sees this special "msauth://" scheme and opens THIS activity (MsalActivity)
    //    /// 6. The IntentFilter below tells Android to route this specific URI scheme to this activity
    //    /// 7. MsalActivity (which extends BrowserTabActivity from MSAL library) receives the redirect
    //    /// 8. MSAL extracts the authorization code from the redirect and passes it back to LoginAsync()
    //    /// 9. Your app continues execution and receives the token
    //    /// 
    //    /// ✅ WHERE IT'S USED:
    //    /// This class doesn't need to be explicitly called from your code. It's used automatically by:
    //    /// - The MSAL library (Microsoft.Identity.Client)
    //    /// - Android's Intent system (via the IntentFilter attributes) this class is config in <see cref="Logic.Extensions"/> at line 23
    //    /// - Called automatically when the redirect URI is triggered
    //    /// 
    //    /// ✅ WHY THE ATTRIBUTES MATTER:
    //    /// [Activity] - Declares this as an Android Activity (must be in AndroidManifest.xml or here)
    //    ///   - Exported = true: Allows other apps (the browser) to launch this activity
    //    ///   - Name = "com.kmaraszkiewicz86.microsoftentraiddemoapp.MsalActivity": Unique identifier
    //    /// 
    //    /// [IntentFilter] - Tells Android which URLs should open this activity
    //    ///   - ActionView: Open URLs (not send/receive intents)
    //    ///   - DataScheme = "msauth": Only intercept msauth:// URLs (not http:// or https://)
    //    ///   - DataHost = "com.kmaraszkiewicz86.microsoftentraiddemoapp": Your app's identifier
    //    ///   - DataPath = "/MgO7Vtlx/RlYeNiSbxjhy9NRht4=": Your app's hash (from Azure App Registration)
    //    /// 
    //    /// ⚠️ IMPORTANT: This path MUST match exactly what you configured in:
    //    /// - Your Azure App Registration > Mobile app platform > Redirect URI
    //    /// - MauiProgram.cs line 20: new AzureEntraIdConfig(..., "msauth://...", ...)
    //    /// - If they don't match, the redirect will fail and your app will close
    //    /// </summary>
    [Activity(Exported = true)]
    [IntentFilter(
            new[] { Intent.ActionView },
            Categories = new[]
            {
                Intent.CategoryDefault,
                Intent.CategoryBrowsable
            },
            DataHost = "auth",
            DataScheme = "msal_tenant_id")]
    public class MsalActivity : BrowserTabActivity
    {
    }
}